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
	public class Menu
	{
		public Texture2D background;
		public List<IMenuItem> items;
		int currentItem;
		public int numItems;
		public Vector2 itemZone; // X is the Y value and Y is the Height of the item zone;

		public Menu(Texture2D background, List<IMenuItem> items, Vector2 itemZone)
		{
			this.background = background;
			this.items = items;
			this.currentItem = 0;
			this.numItems = items.Count;
			this.itemZone = itemZone;
			this.items[0].SetIsHighlighted(true);
		}

		public void ChangeCurrentItem(int changeBy)
		{
			this.currentItem += changeBy;
			if (this.currentItem < 0) this.currentItem = this.numItems - 1;
			if (this.currentItem >= this.numItems) this.currentItem = 0;
		}

		public void OpenMenu(Game1 game)
		{
			this.currentItem = 0;
			game.lastMenu = game.currentMenu;
			game.currentMenu = this;
		}

		public void UpdateMenu(Game1 game)
		{
			if (game.newkeys.IsKeyDown(Keys.Down) && game.oldkeys.IsKeyUp(Keys.Down))
			{
				items[currentItem].SetIsHighlighted(false);
				ChangeCurrentItem(1);
				items[currentItem].SetIsHighlighted(true);
			}
			if (game.newkeys.IsKeyDown(Keys.Up) && game.oldkeys.IsKeyUp(Keys.Up))
			{
				items[currentItem].SetIsHighlighted(false);
				ChangeCurrentItem(-1);
				items[currentItem].SetIsHighlighted(true);
			}
			if (game.newkeys.IsKeyDown(Game1.fire) && game.oldkeys.IsKeyUp(Game1.fire))
			{
				items[currentItem].ActivateItem(game);
			}
		}
	}
}
