using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SignsOfLife.Entities.Items;
using SignsOfLife.Prefabs.StaticPrefabs;
using SignsOfLife.Tiles;
using SignsOfLife.Utils;
using SOLPolymorph.SignsOfLife.Polymorph.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autostyle.Projects
{
    public class IndustrialDrill : SpriteStaticPrefab
    {

        private int _texid = TextureLoader.Instance.LoadTextureAsPNG("industrialdrill.png");

        public static readonly int TypeID = 5000;
        public static readonly int[,] TypeTileData;
        public static readonly string TypeName = "Industrial Drill";

        public static readonly Rectangle BoundsHead = new Rectangle(4,166,25,25);
        public static readonly Vector2 HeadDefaultPosition = new Vector2(52,41);
        public static readonly float HeadBottomPosition = 138-HeadDefaultPosition.Y;
        public static readonly float Speed = 100F;

        static IndustrialDrill()
        {
            TypeTileData = new int[5, 4];
            for (int x = 0; x < 4; x++)
            {
                TypeTileData[4,x]=999;
            }
        }

        public IndustrialDrill()
            : base(TypeName, TypeTileData, (StaticPrefabType)TypeID, "SpriteSheets/InventorySpriteSheet")
        {
            this.State = DrillState.INITIAL;
            this.HeadDepth = 0F;

            this._cutOutBounds = true;
            //this._currentBounds = SignsOfLife.ContentHandler.GetItemTextureBounds("tech_gaspipe");
            this._currentBounds = new Rectangle(2,0,127,162);
            this._texture = TextureLoader.Instance.GetTexture(_texid);
            this._requiresFloor = true;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, float screenX, float screenY, Microsoft.Xna.Framework.Color color, Microsoft.Xna.Framework.Matrix transformMatrix)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, transformMatrix);
            spriteBatch.Draw(this._texture, new Vector2(screenX, screenY), new Rectangle?(this._currentBounds), Color.White);
            spriteBatch.End();
        }

        public DrillState State { get; private set; }
        public float HeadDepth { get; private set; }
        public int CurrentMiningLevel { get; private set; }

        public enum DrillState
        {
            INITIAL,
            GETTING_INTO_POSITION,
            STEP_DOWN,
            DRILLING,
            RETRACTING
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.Vector2 screenGameFieldPosition, Microsoft.Xna.Framework.Matrix transformMatrix)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, StaticPrefab._scissorRasterizerState, null, transformMatrix);
            Rectangle scissorRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;
            if (this._cutOutBounds && State==DrillState.INITIAL)
            {
                base.HandleScissorRect(spriteBatch, screenGameFieldPosition);
            }
            Vector2 locpos = new Vector2(base.Position.X, base.Position.Y);
            //Body
            spriteBatch.Draw(this._texture, locpos - screenGameFieldPosition, new Rectangle?(this._currentBounds), Color.White);
            //Head
            Vector2 headpos = locpos + HeadDefaultPosition + new Vector2(0, (float)Math.Ceiling(HeadDepth));
            spriteBatch.Draw(this._texture, headpos - screenGameFieldPosition, BoundsHead, Color.White);
            if (this._cutOutBounds && State == DrillState.INITIAL)
            {
                spriteBatch.GraphicsDevice.ScissorRectangle = scissorRectangle;
            }
            spriteBatch.End();
        }

        private IntPoint[] _mineTargets = new IntPoint[2];
        private bool _isMineTargetSet = false;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (State == DrillState.INITIAL)
            {
                State = DrillState.GETTING_INTO_POSITION;
            }
            else if (State == DrillState.GETTING_INTO_POSITION)
            {
                if (HeadDepth < HeadBottomPosition + (CurrentMiningLevel*25))
                {
                    HeadDepth += (float)gameTime.ElapsedGameTime.TotalSeconds * Speed;
                }
                else
                {
                    State = DrillState.DRILLING;
                }
            }
            else if (State == DrillState.DRILLING)
            {
                var map = this.Map;
                if (!_isMineTargetSet)
                {
                    var p1 = map.GetTileLocAtPoint(this.Position + new Vector2(83, 169));
                    var p2 = map.GetTileLocAtPoint(this.Position + new Vector2(47, 169));
                    p1.Y += CurrentMiningLevel;
                    p2.Y += CurrentMiningLevel;

                    //var tile1 = map.GetTileAtPoint(this.Position + new Vector2(83, 169));
                    //var tile2 = map.GetTileAtPoint(this.Position + new Vector2(47, 169));
                    _mineTargets[0] = p1; _mineTargets[1] = p2;
                    _isMineTargetSet = true;
                }
                else
                {
                    ITile t1 = map.GetTileSafeLoopedHorizontally(_mineTargets[0].X, _mineTargets[0].Y);
                    ITile t2 = map.GetTileSafeLoopedHorizontally(_mineTargets[1].X, _mineTargets[1].Y);
                    SelectAndDropTile(map, t1);
                    SelectAndDropTile(map, t2);
                    map.SetTile(_mineTargets[0].X, _mineTargets[0].Y, null, false);
                    map.SetTile(_mineTargets[1].X, _mineTargets[1].Y, null, false);
                    _isMineTargetSet = false;
                    State = DrillState.STEP_DOWN;
                    CurrentMiningLevel++;
                }
            }
            else if (State == DrillState.STEP_DOWN)
            {
                if (HeadDepth < HeadBottomPosition + (CurrentMiningLevel * 32))
                {
                    HeadDepth += (float)gameTime.ElapsedGameTime.TotalSeconds * Speed;
                }
                else
                {
                    State = DrillState.DRILLING;
                }
            }
        }

        private void SelectAndDropTile(SignsOfLife.Maps.Map map, ITile t1)
        {
            if (t1 != null)
            {
                int dropid = t1.TileData.DropId;
                if (dropid > 0)
                {
                    if (t1.TileData.CanPickUp) { }
                    InventoryItem item = new BlockItem(t1.TileData);//InventoryItemHandler.GetNewItemByItemTypeID(dropid);
                    item.Pos = new Vector2(this.X, this.Y);
                    //map.AddDynamicEntity(item);
                    map.AddItemFromServer(item);
                    //map.AddNewItem(this.X, this.Y, item);
                }
            }
        }

    }
}
