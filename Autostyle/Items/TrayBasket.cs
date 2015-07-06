using Microsoft.Xna.Framework;
using SignsOfLife.Entities;
using SignsOfLife.Entities.Items;
using SignsOfLife.Entities.Items.Containers;
using SignsOfLife.UI.Containers;
using SignsOfLife.Utils;
using SOLPolymorph.SignsOfLife.Polymorph.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autostyle.Items
{
    public class TrayBasket : ContainerItem
    {

        public static readonly int ID = 50001;
        private int _texid = TextureLoader.Instance.LoadTextureAsPNG("traybasket.png");

        public TrayBasket()
            : base(0, 0, new TechCrateLargeContainer(100, 100), new Rectangle(0, 35, 64, 29))
        {
            base.NewInventoryItemType = (InventoryItemType)ID;
            
            base.Texture = TextureLoader.Instance.GetTexture(_texid);

            base.GetContainerUI().SetPhysicalItem(this);

            base.Name = "Tray Basket";
            base.Description = "From now on your stupid trash is collected in one place. This handy 'device' collects all stuff automatically if it falls in from top.";
            base.Category=Commons.CathegoryCrate;

            base.StandOn = true;
            base.StandOnOptional = true;
        }

        public Vector2 TopCenter { get { return new Vector2(base.Pos.X+(base.Width*0.5F),base.Pos.Y); } }

        public override void Update(GameTime gameTime, SignsOfLife.Maps.Map map)
        {
            if (base.IsOnGround())
            {
                var item = TrySelectFallenItem();

                if (item != null)
                {
                    //base.GetContainerUI().AddItemAutoStackRandom(InventoryItemHandler.CopyInventoryItem(item));
                    //item.RemoveSelf();
                    item._map.RemoveItemFromWorld(item);
                    //base.GetContainerUI().AddItemAutoStack(item);
                }
            }

            base.Update(gameTime, map);
        }

        private InventoryItem TrySelectFallenItem()
        {
            var chunk = _curChunk;
            if (chunk != null)
            {
                foreach (var sel in chunk.InventoryItems)
                {
                    if (sel == this) { continue; }
                    //if (sel is SignsOfLife.Entities.Items.RiggedItems.TurretSteel) { continue; }
                   // base.Pos
                    //if (GeomUtils.isWithin(TopCenter.X, TopCenter.Y, sel.CurGlobalCenter.X, sel.Pos.Y, 20))
                    //{
                    if (sel.CurGlobalCenter.X > this.Pos.X && sel.CurGlobalCenter.X < this.Pos.X + this.Width)
                    {
                        float diff = this.TopCenter.Y - (sel.Pos.Y+(sel.Height));
                        if (diff>=0 & diff<5)
                        {
                            return sel;
                        }
                    }
                    //}
                }
            }
            return null;
        }


    }
}
