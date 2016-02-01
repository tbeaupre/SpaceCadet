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
	class ActionMenuItem : IMenuItem
	{
		Texture2D texture;
		MenuList goesTo;
		bool isHighlighted = false;
		String function;

		public ActionMenuItem(Texture2D texture, MenuList goesTo, String function)
		{
			this.texture = texture;
			this.goesTo = goesTo;
			this.function = function;
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

		void IMenuItem.ActivateItem(Game1 game)
		{
			game.callMenuFunction(function);
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
