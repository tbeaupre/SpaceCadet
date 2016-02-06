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
	public class MenuList : MenuGrid, IMenu 
	{
		public MenuList(Texture2D background, IMenuItem[,] items, Vector2 itemZone)
            : base(background, items, new Rectangle(0, (int)itemZone.X, 0, (int)itemZone.Y))
		{
            
        }

		public override void UpdateMenu(Game1 game)
		{
            if (game.newkeys.IsKeyDown(Keys.Down) && game.oldkeys.IsKeyUp(Keys.Down))
            {
                items[GetCurrentItemX(), GetCurrentItemY()].SetIsHighlighted(false);
                ChangeCurrentItem(0, 1);
                items[GetCurrentItemX(), GetCurrentItemY()].SetIsHighlighted(true);
                GetSound().Play(GetClick1());
            }
            if (game.newkeys.IsKeyDown(Keys.Up) && game.oldkeys.IsKeyUp(Keys.Up))
            {
                items[GetCurrentItemX(), GetCurrentItemY()].SetIsHighlighted(false);
                ChangeCurrentItem(0, -1);
                items[GetCurrentItemX(), GetCurrentItemY()].SetIsHighlighted(true);
                GetSound().Play(GetClick1());
            }
            if (game.newkeys.IsKeyDown(Game1.fire) && game.oldkeys.IsKeyUp(Game1.fire))
            {
                items[GetCurrentItemX(), GetCurrentItemY()].ActivateItem(game, this);
                GetSound().Play(GetClick2());
            }
        }
	}
}
