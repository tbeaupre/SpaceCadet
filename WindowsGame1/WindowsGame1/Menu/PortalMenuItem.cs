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
	public class PortalMenuItem : IMenuItem
	{
		Texture2D texture;
		IMenu goesTo;
		bool isHighlighted = false;
        Nullable<Rectangle> anchorRect;

        public PortalMenuItem(Texture2D texture, IMenu goesTo)
		{
			this.texture = texture;
			this.goesTo = goesTo;
            this.anchorRect = null;
		}

        public PortalMenuItem(Texture2D texture, IMenu goesTo, int anchorX, int anchorY)
        {
            this.texture = texture;
            this.goesTo = goesTo;
            this.anchorRect = new Rectangle(anchorX, anchorY, texture.Width, texture.Height / 2);
        }

        public IMenu GetGoesTo()
        {
            return this.goesTo;
        }

		public Texture2D GetTexture()
		{
			return this.texture;
		}

		public void SetIsHighlighted(bool val)
		{
			this.isHighlighted = val;
		}

		public bool GetIsHighlighted()
		{
			return this.isHighlighted;
		}

        public virtual Nullable<Rectangle> GetAnchorRect()
        {
            return anchorRect;
        }

        public void SetAnchorRect(Nullable<Rectangle> rect)
        {
            this.anchorRect = rect;
        }

        public virtual void ActivateItem(Game1 game, IMenu from)
		{
            from.Disable();
			if (goesTo == null)
			{
				game.currentMenu = null;
			}
			else
			{
				goesTo.OpenMenu(game);
			}
		}

        public virtual Rectangle GetSourceRect()
        {
            return new Rectangle(0, (GetIsHighlighted()? GetTexture().Height / 2 : 0), GetTexture().Width, GetTexture().Height / 2);
        }
    }
}
