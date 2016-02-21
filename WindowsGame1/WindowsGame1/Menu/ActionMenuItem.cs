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
	public class ActionMenuItem : PortalMenuItem, IMenuItem
	{
		String function;

		public ActionMenuItem(Texture2D texture, IMenu goesTo, String function)
            :base(texture, goesTo)
		{
			this.function = function;
		}

        public ActionMenuItem(Texture2D texture, IMenu goesTo, String function, int anchorX, int anchorY)
            : base(texture, goesTo, anchorX, anchorY)
        {
            this.function = function;
        }

        public String GetFunction()
        {
            return this.function;
        }

		public override void ActivateItem(Game1 game, IMenu from)
		{
            from.Disable();
			game.callMenuFunction(function, from);
			if (GetGoesTo() == null)
			{
				game.currentMenu = null;
			}
			else
			{
				GetGoesTo().OpenMenu(game);
			}
		}
	}
}
