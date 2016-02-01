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
    class MenuGrid : IMenu
    {
        public Texture2D background;
        public IMenuItem[,] items;
        int currentItemX;
        int currentItemY;
        int numRows;
        int numColumns;
        public Vector2 itemZone; // X is the Y value and Y is the Height of the item zone;
        private SoundFX click1;
        private SoundFX click2;

        public MenuGrid(Texture2D background, IMenuItem[,] items, Vector2 itemZone)
        {
            this.background = background;
            this.items = items;
            this.currentItemX = 0;
            this.currentItemY = 0;
            this.numRows = items.GetUpperBound(0);
            this.numColumns = items.GetUpperBound(1);
            this.itemZone = itemZone;
            this.items[0,0].SetIsHighlighted(true);

            click1 = new SoundFX("click1");
            click2 = new SoundFX("click2");
        }

        public void ChangeCurrentItem(int changeXBy, int changeYBy)
        {
            this.currentItemX += changeXBy;
            if (this.currentItemX < 0) this.currentItemX = this.numColumns - 1;
            if (this.currentItemX >= this.numColumns) this.currentItemX = 0;

            this.currentItemY += changeYBy;
            if (this.currentItemY < 0) this.currentItemY = this.numRows - 1;
            if (this.currentItemY >= this.numRows) this.currentItemY = 0;
        }

        public void OpenMenu(Game1 game)
        {
            this.currentItemX = 0;
            this.currentItemY = 0;
            game.lastMenu = game.currentMenu;
            game.currentMenu = this;
        }

        public void UpdateMenu(Game1 game)
        {
            if (game.newkeys.IsKeyDown(Keys.Down) && game.oldkeys.IsKeyUp(Keys.Down))
            {
                items[currentItemX, currentItemY].SetIsHighlighted(false);
                ChangeCurrentItem(0,1);
                items[currentItemX, currentItemY].SetIsHighlighted(true);
                click1.Play();
            }
            if (game.newkeys.IsKeyDown(Keys.Up) && game.oldkeys.IsKeyUp(Keys.Up))
            {
                items[currentItemX, currentItemY].SetIsHighlighted(false);
                ChangeCurrentItem(0,-1);
                items[currentItemX, currentItemY].SetIsHighlighted(true);
                click1.Play();
            }
            if (game.newkeys.IsKeyDown(Keys.Left) && game.oldkeys.IsKeyUp(Keys.Left))
            {
                items[currentItemX, currentItemY].SetIsHighlighted(false);
                ChangeCurrentItem(-1, 0);
                items[currentItemX, currentItemY].SetIsHighlighted(true);
                click1.Play();
            }
            if (game.newkeys.IsKeyDown(Keys.Right) && game.oldkeys.IsKeyUp(Keys.Right))
            {
                items[currentItemX, currentItemY].SetIsHighlighted(false);
                ChangeCurrentItem(1, 0);
                items[currentItemX, currentItemY].SetIsHighlighted(true);
                click1.Play();
            }
            if (game.newkeys.IsKeyDown(Game1.fire) && game.oldkeys.IsKeyUp(Game1.fire))
            {
                items[currentItemX, currentItemY].ActivateItem(game);
                click2.Play();
            }
        }
    }
}
