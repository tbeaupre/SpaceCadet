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
	class PortalMenuItem : IMenuItem
	{
		Texture2D texture;
		IMenu goesTo;
		bool isHighlighted = false;

		public PortalMenuItem(Texture2D texture, IMenu goesTo)
		{
			this.texture = texture;
			this.goesTo = goesTo;
		}

		Texture2D IMenuItem.GetTexture()
		{
			return this.texture;
		}

		void IMenuItem.SetIsHighlighted(bool val)
		{
			this.isHighlighted = val;
		}

		bool IMenuItem.GetIsHighlighted()
		{
			return this.isHighlighted;
		}

		void IMenuItem.ActivateItem(Game1 game, IMenu from)
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
	}
}
