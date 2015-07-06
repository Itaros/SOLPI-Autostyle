using Microsoft.Xna.Framework;
using SignsOfLife;
using SignsOfLife.Entities.Items;
using SignsOfLife.Entities.Items.Containers;
using SignsOfLife.Maps;
using SignsOfLife.Particles;
using SignsOfLife.UI.Containers;
using SOLPolymorph.SignsOfLife.Polymorph.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autostyle.Items.Robots
{
    public class TechBlowbot : ContainerItem
    {

        public static readonly int ID = 50002;
        private int _texid = TextureLoader.Instance.LoadTextureAsPNG("vacubot.png");

        public static readonly float MaxSpeed = 0.5F;

        public TechBlowbot()
            : base(0, 0, new TechCrateLargeContainer(100, 100), new Rectangle(1, 1, 30, 26))
        {
            base.NewInventoryItemType = (InventoryItemType)ID;
            base.Texture = TextureLoader.Instance.GetTexture(_texid);
            base.GetContainerUI().SetPhysicalItem(this);

            base.Name = "VacuBot(tm)";
            base.Description = "The best of engineering for your luxurious life!\nDisclaimer: Warranty void if used against big stuff. Batteries are not included too.";
            base.Category = Commons.CathegoryRobot;
            base.IsFlippable = true;

            SetMovingStart();
        }

        private void SetMovingStart()
        {
            isMoving = true;
            Direction = IsFlipped?1:-1;
            //Speed = .25F;
        }
        private void SetMovingStop()
        {
            isMoving = false;
            Speed = 0;
        }

        public override void Collided(SignsOfLife.BoxSide side, Map map)
        {
            base.Collided(side, map);
            if (IsOnGround())
            {
                SetMovingStart();
            }

            if (side == BoxSide.RIGHT || side == BoxSide.LEFT)
            {
                //IsFlipped = !IsFlipped;
                Speed = -Speed/2F;//Collided
                _hasCollided = true;
                EvaluateMovingState();
            }
        }

        private bool _hasCollided;

        public override void Update(GameTime gameTime, Map map)
        {

            EvaluateMovingState();
            if (isMoving && Speed<MaxSpeed)
            {
                Speed += 0.5F * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_hasCollided)
                {
                    if (Speed > 0F)
                    {
                        IsFlipped = !IsFlipped;
                        _hasCollided = false;
                    }
                }
            }
            TryCollectInfront(map);

            base.Update(gameTime, map);
        }

        private void TryCollectInfront(Map map)
        {
            Vector2 mypos = this.Pos;
            int range = 25 * this.Direction;

            var chunk = this._curChunk;
            if (chunk != null)
            {
                foreach (InventoryItem nearby in chunk.NearbyItems)
                {
                    if (nearby == this) { continue; }
                    //if(nearby.Category)
                    if (CathegoryWhitelisted(nearby))
                    {
                        //if (nearby.Height * nearby.Width > 500) { continue; }
                        //Vertical cutoff
                        if (Math.Abs((this.Y+(this.Height/2F)) - (nearby.Y+(nearby.Height/2F))) < 10)
                        {
                            //Horizontal cutoff
                            float diff = nearby.Pos.X - mypos.X;
                            if (this.Direction == -1 && (diff > range && diff < 0))
                            {
                                PickItem(nearby);
                            }
                            else if (this.Direction == 1 && (diff < range && diff > 0))
                            {
                                PickItem(nearby);
                            }
                        }
                    }
                }
            }
        }

        private bool CathegoryWhitelisted(InventoryItem nearby)
        {
            string cat = nearby.Category.ToUpperInvariant();
            if (string.IsNullOrEmpty(cat)) { return false; }

            return WhiteListCathegories.Contains(cat);
        }

        private String[] WhiteListCathegories = new String[]{
            "Consumable",
            "Materials",
            "Throwable"
        }.Select(i=>i.ToUpperInvariant()).ToArray();

        private void PickItem(InventoryItem item)
        {
            var map = item._map;
            this._map.RemoveItemFromWorld(item);
            this.GetContainerUI().AddItemAutoStackRandom(item);


            Vector2 partpos = item.DisplayPos;
            partpos.X += item.Width/2F;
            partpos.Y += item.Height/2F;
            ManualParticle pewEffect = new ManualParticle(partpos, Vector2.Zero, "spark_circle", 200);
            pewEffect.Scale = new Vector2(0.2F,0.2F);
            pewEffect.EndScale = Vector2.Zero;
            pewEffect.DoLerpScale = true;
            pewEffect.StartAlpha = 0.5F;
            pewEffect.EndAlpha = 0f;
            pewEffect._fadeOutTime = 100;
            pewEffect._fadeOut = true;
            pewEffect.AffectedByGravity = false;
            pewEffect.Color = Color.AliceBlue;
            //pewEffect.;
            map.AboveLightingManualParticles.Add(pewEffect);
        }

        private void EvaluateMovingState()
        {
            if (IsOnGround())
            {
                if (!isMoving) { SetMovingStart(); }
            }
            else
            {
                if (isMoving) { SetMovingStop(); }
            }
        }

    }
}
