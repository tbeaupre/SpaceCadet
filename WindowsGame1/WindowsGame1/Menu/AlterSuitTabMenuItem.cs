using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Spaceman
{
    public class AlterSuitTabMenuItem : PortalMenuItem
    {
        String key;

        public AlterSuitTabMenuItem(Texture2D texture, int anchorX, int anchorY, String key)
            : base(texture, null)
        {
            SetAnchorRect(new Rectangle(anchorX, anchorY, texture.Width, texture.Height/2));
            this.key = key;
        }

        public override void ActivateItem(Game1 game, IMenu from)
        {
            if (from is PowerUpMenu) ActivateItem(game, (PowerUpMenu)from);
            else base.ActivateItem(game, from);
        }

        public void ActivateItem(Game1 game, PowerUpMenu from)
        {
            if (from.GetKey() != this.key)
            {
                from.Disable();
                game.OpenMenu(key);
            }
        }
    }
}
