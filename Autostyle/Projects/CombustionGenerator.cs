using Autostyle.GUMPs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SignsOfLife;
using SignsOfLife.Entities.Items;
using SignsOfLife.Entities.Items.Containers;
using SignsOfLife.Prefabs.StaticPrefabs;
using SOLPolymorph.SignsOfLife.Polymorph.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autostyle.Projects
{
    public class CombustionGenerator : SpriteStaticPrefab
    {

        private int _texid = TextureLoader.Instance.LoadTextureAsPNG("combustion.png");

        public static readonly int TypeID = 5001;
        public static readonly int[,] TypeTileData;
        public static readonly string TypeName = "Combustion Generator";

        public static readonly Rectangle SourceWheel = new Rectangle(98,3,49,47);
        public static readonly Vector2 OffsetWheel = new Vector2(46.5F,30.5F);

        static CombustionGenerator()
        {
            TypeTileData = new int[3, 3];
        }

        private GUMP_CombustionGenerator _gump;
        private Satchel _hackySatchel;

        public CombustionGenerator()
            : base(TypeName, TypeTileData, (StaticPrefabType)TypeID, "SpriteSheets/InventorySpriteSheet")
        {

            this._cutOutBounds = true;
            //this._currentBounds = SignsOfLife.ContentHandler.GetItemTextureBounds("tech_gaspipe");
            this._currentBounds = new Rectangle(1,2,96,82);
            this._texture = TextureLoader.Instance.GetTexture(_texid);
            this._requiresFloor = true;

            _gump = new GUMP_CombustionGenerator();
            _hackySatchel = new Satchel();
            //_hackySatchel.SetContainer(_gump);
            _gump.SetPhysicalItem(_hackySatchel);//Damn it...
            _gump.SetPhysicalStaticPrefab(this);
        }


        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, float screenX, float screenY, Microsoft.Xna.Framework.Color color, Microsoft.Xna.Framework.Matrix transformMatrix)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, transformMatrix);
            spriteBatch.Draw(this._texture, new Vector2(screenX, screenY), new Rectangle?(this._currentBounds), Color.White);
            spriteBatch.End();
        }

        private float animationTick = 0F;

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.Vector2 screenGameFieldPosition, Microsoft.Xna.Framework.Matrix transformMatrix)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, StaticPrefab._scissorRasterizerState, null, transformMatrix);
            Rectangle scissorRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;
            if (this._cutOutBounds)
            {
                base.HandleScissorRect(spriteBatch, screenGameFieldPosition);
            }
            Vector2 locpos = new Vector2(base.Position.X, base.Position.Y);

            //spriteBatch.Draw(this._texture, locpos - screenGameFieldPosition, new Rectangle?(SourceWheel), Color.White);
            Vector2 locCorrected = locpos - screenGameFieldPosition;
            Vector2 wheelOrigin = new Vector2(SourceWheel.Width,SourceWheel.Height)/2;
            spriteBatch.Draw(this._texture,locCorrected+OffsetWheel,SourceWheel,Color.White,animationTick,wheelOrigin,1F,SpriteEffects.None,0);

            spriteBatch.Draw(this._texture, locpos - screenGameFieldPosition, new Rectangle?(this._currentBounds), Color.White);

            spriteBatch.End();

        }

        public float EngineSpeed { get; private set; }
        public float FuelLast { get; private set; }

        public override void Update(GameTime gameTime)
        {
            float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (FuelLast <= 0F)
            {
                if (EngineSpeed > 0)
                {
                    EngineSpeed -= seconds * 1F;
                }
                else
                {
                    EngineSpeed = 0F;
                }
                FuelLast = _gump.TryPickFuel() ? 100F : 0F;
            }
            else
            {
                FuelLast -= seconds * 5F;
                EngineSpeed += seconds * 0.1F;
                _gump.TryCharge(seconds);
            }

            animationTick += EngineSpeed*4F*(float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        public override void RightClicked(float globalMouseX, float globalMouseY)
        {
            GameInput.AddContainerUIToScreen(SpaceGame._hud, SpaceGame._playerEntity, _gump);
            return;
        }

    }
}
