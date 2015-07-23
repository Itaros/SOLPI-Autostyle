using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SignsOfLife.Entities.Items;
using SignsOfLife.UI.Containers;
using SOLPolymorph.SignsOfLife.Polymorph.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autostyle.Items
{
    public class ChargableBattery : InventoryItem
    {

        public static readonly int ID = 50003;
        private int _texid = TextureLoader.Instance.LoadTextureAsPNG("battery.png");

        private static readonly Rectangle _rectSourceNoPower = new Rectangle(0, 0, 15, 18);
        private static readonly Rectangle _rectSourcePowered = new Rectangle(15, 0, 15, 18);

        public float Charge { get; set; }
        public static readonly float MaxCharge = 1000F;

        public ChargableBattery()
            : base(0, 0, null, _rectSourceNoPower)
        {
            Charge = 0f;
            Name = "Rechargable Battery";
            Description = "This is kinda bipolar.";

            NewInventoryItemType = (InventoryItemType)ID;

            _stackable = false;

            base.Texture = TextureLoader.Instance.GetTexture(_texid);
        }

        public override void DrawInContainer(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, SignsOfLife.UI.Containers.Container container, Color tintColor, Color behindTint)
        {
            Vector2 position = new Vector2((float)this._localPosX * Container.CONTAINER_SCALE_VECTOR.X + container.X, (float)this._localPosY * Container.CONTAINER_SCALE_VECTOR.Y + container.Y);
            Rectangle noChargedOverlay = new Rectangle((int)position.X,(int)position.Y,_rectSourceNoPower.Width,_rectSourceNoPower.Height);
            Rectangle chargedOverlay = noChargedOverlay;
            Rectangle chargedSource = _rectSourcePowered;
            int diff = (int)((float)chargedOverlay.Height*(Charge/MaxCharge));
            chargedOverlay.Y += chargedOverlay.Height-diff;
            chargedOverlay.Height = diff;
            chargedSource.Y += chargedSource.Height-diff;
            chargedSource.Height = diff;
            //

            SpriteEffects effects = SpriteEffects.None;
            if (base.IsFlipped)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            if (this._amount == 1)
            {
                spriteBatch.Draw(this._texture, noChargedOverlay, new Rectangle?(_rectSourceNoPower), tintColor, 0f, Vector2.Zero, effects, 0f);
                spriteBatch.Draw(this._texture, chargedOverlay, new Rectangle?(chargedSource), tintColor, 0f, Vector2.Zero, effects, 0f);
                return;
            }
            spriteBatch.Draw(this._texture, position+new Vector2(this.MULTIPLE_QUANTITY_SHIFT,0), new Rectangle?(this._spriteBounds), behindTint, 0f, Vector2.Zero, Container.CONTAINER_SCALE_VECTOR, effects, 0f);
            spriteBatch.Draw(this._texture, position, new Rectangle?(this._spriteBounds), tintColor, 0f, Vector2.Zero, Container.CONTAINER_SCALE_VECTOR, effects, 0f);           
        }

    }
}
