﻿using Microsoft.Xna.Framework;
using SignsOfLife;
using SignsOfLife.Entities.Items;
using SignsOfLife.UI;
using SignsOfLife.UI.Gumps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autostyle.GUMPs
{
    public class GUMP_CombustionGenerator : PrefabGump
    {

        private static readonly string GUMP_SHEET = "SpriteSheets/hudSpriteSheet";
        private static readonly string GUMP_SHEET_DICT = "SpriteSheets/hudSpriteSheetMap";

        private Color _textColor = new Color(255, 102, 0);

        private SubContainer _fuelSC;

        protected List<SubContainer> _itemSubContainers = new List<SubContainer>();

        public GUMP_CombustionGenerator()
            : base(0, 0, ContentHandler.GetTexture("Sprites/Gumps/gump_refinement_smelt"))
        {

            _itemBounds = new Rectangle(0, 0, this._spriteBounds.Width, this._spriteBounds.Height);

            _fuelSC = new SubContainer(new Rectangle(183, 131, 50, 50));

            _itemSubContainers.Add(_fuelSC);

            this._minimizeButton = new GraphicalButton(new Vector2(227f, 27f), GUMP_SHEET, GUMP_SHEET_DICT, null, "menu_min_press", "menu_min_hover", null, "menu_min_press", "menu_min_hover");
            this._minimizeButton.Pressed += new ButtonPressedHandler(base.MinimizeButtonPressed);
            this._tooltipRegions.Add(new TooltipRegion(new Rectangle((int)this._minimizeButton.X, (int)this._minimizeButton.Y, this._minimizeButton.Width, this._minimizeButton.Height), TooltipRegionType.MENU_CLOSE));

        }


       // public override void moveItem(InventoryItem item, int localX, int localY)
       // {
       //     base.moveItem(item, localX, localY);
       //     if (CanDropAtLocal(localX, localY))
       //     {
       //         HandleItemAddedToSubContainer(item,localX,localY);
       //     }
       // }

        public override void HandleItemAddedToSubContainer(InventoryItem item, int localX, int localY)
		{
			item.LocalX = localX;
			item.LocalY = localY;
			foreach (SubContainer current in this._itemSubContainers)
			{
				if (IsWithinBounds((float)(item.LocalX + item.Width / 2), (float)(item.LocalY + item.Height / 2), new Rectangle(current.Bounds.X - item.Width / 2, current.Bounds.Y - item.Height / 2, current.Bounds.Width + item.Width / 2, current.Bounds.Height + item.Height / 2)))
				{
					base.MoveWithinBounds(item, current.Bounds, current);
					break;
				}
			}
		}
        public bool CanDropAtLocal(float localX, float localY)
        {
            foreach (SubContainer current in this._itemSubContainers)
            {
                if (IsWithinBounds(localX, localY, current.Bounds))
                {
                    return true;
                }
            }
            return false;
        }

        public override bool CanDropAt(InventoryItem item, float mouseX, float mouseY)
        {
            float mouseX2 = mouseX - base.X;
            float mouseY2 = mouseY - base.Y;
            foreach (SubContainer current in this._itemSubContainers)
            {
                if (IsWithinBounds(mouseX2, mouseY2, current.Bounds))
                {
                    return true;
                }
            }
            return false;            
        }

        private bool IsWithinBounds(float mouseX, float mouseY, Rectangle bounds)
        {
            return mouseX > (float)bounds.X && mouseX < (float)(bounds.X + bounds.Width) && mouseY > (float)bounds.Y && mouseY < (float)(bounds.Y + bounds.Height);
        }

        public override void GrabbedItem(InventoryItem item)
        {
            foreach (SubContainer current in this._itemSubContainers)
            {
                current.RemoveItem(item);
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, InventoryItem item, bool selected)
        {
            base.Draw(spriteBatch, item, selected);
            spriteBatch.Begin();

            Graphics.DrawString(spriteBatch, "Fuel", new Vector2(base.X + 196f, base.Y + 130f), _textColor);

            spriteBatch.End();

        }

    }
}
