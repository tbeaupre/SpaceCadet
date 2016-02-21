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
    public class MenuGrid : IMenu
    {
        public Texture2D background;
        public IMenuItem[,] items;
        private int currentItemX;
        int currentItemY;
        int itemsHeight;
        int itemsWidth;
        Rectangle itemZone;
        private string click1 = "click2";
        private string click2 = "click1";
        private SoundFX sound;

        public IMenuItem GetMenuItem(int x, int y)
        {
            return this.items[x, y];
        }

        public int GetItemsHeight()
        {
            return this.itemsHeight;
        }

        public int GetItemsWidth()
        {
            return this.itemsWidth;
        }

        public Rectangle GetItemZone()
        {
            return this.itemZone;
        }

        public Texture2D GetBackground()
        {
            return this.background;
        }

        public SoundFX GetSound()
        {
            return this.sound;
        }

        public string GetClick1()
        {
            return this.click1;
        }

        public string GetClick2()
        {
            return this.click2;
        }

        public int GetCurrentItemX()
        {
            return this.currentItemX;
        }

        public int GetCurrentItemY()
        {
            return this.currentItemY;
        }

        public void SetItemX(int x)
        {
            this.currentItemX = x;
        }

        public void SetItemY(int y)
        {
            this.currentItemY = y;
        }

        public MenuGrid(Texture2D background, IMenuItem[,] items, Rectangle itemZone)
        {
            this.background = background;
            this.items = items;
            this.currentItemX = 0;
            this.currentItemY = 0;
            this.itemsWidth = items.GetUpperBound(0) + 1;
            this.itemsHeight = items.GetUpperBound(1) + 1;
            this.itemZone = itemZone;
            this.items[0, 0].SetIsHighlighted(true);
            sound = new SoundFX();
        }

        public void ChangeCurrentItem(int changeXBy, int changeYBy)
        {
            this.currentItemX += changeXBy;
            if (this.currentItemX < 0) this.currentItemX = this.itemsWidth - 1;
            if (this.currentItemX >= this.itemsWidth) this.currentItemX = 0;

            this.currentItemY += changeYBy;
            if (this.currentItemY < 0) this.currentItemY = this.itemsHeight - 1;
            if (this.currentItemY >= this.itemsHeight) this.currentItemY = 0;
        }

        public void SetCurrentItem(int x, int y)
        {
            this.currentItemX = x;
            this.currentItemY = y;
        }

        public virtual void OpenMenu(Game1 game)
        {
            this.currentItemX = 0;
            this.currentItemY = 0;
            this.items[currentItemX, currentItemY].SetIsHighlighted(true);
            game.lastMenu = game.currentMenu;
            game.currentMenu = this;
        }

        public virtual void UpdateMenu(Game1 game)
        {
            if (game.newkeys.IsKeyDown(Keys.Down) && game.oldkeys.IsKeyUp(Keys.Down))
            {
                items[currentItemX, currentItemY].SetIsHighlighted(false);
                ChangeCurrentItem(0,1);
                items[currentItemX, currentItemY].SetIsHighlighted(true);
                sound.Play(click1);
            }
            if (game.newkeys.IsKeyDown(Keys.Up) && game.oldkeys.IsKeyUp(Keys.Up))
            {
                items[currentItemX, currentItemY].SetIsHighlighted(false);
                ChangeCurrentItem(0,-1);
                items[currentItemX, currentItemY].SetIsHighlighted(true);
                sound.Play(click1);
            }
            if (game.newkeys.IsKeyDown(Keys.Left) && game.oldkeys.IsKeyUp(Keys.Left))
            {
                items[currentItemX, currentItemY].SetIsHighlighted(false);
                ChangeCurrentItem(-1, 0);
                items[currentItemX, currentItemY].SetIsHighlighted(true);
                sound.Play(click1);
            }
            if (game.newkeys.IsKeyDown(Keys.Right) && game.oldkeys.IsKeyUp(Keys.Right))
            {
                items[currentItemX, currentItemY].SetIsHighlighted(false);
                ChangeCurrentItem(1, 0);
                items[currentItemX, currentItemY].SetIsHighlighted(true);
                sound.Play(click1);
            }
            if (game.newkeys.IsKeyDown(Game1.fire) && game.oldkeys.IsKeyUp(Game1.fire))
            {
                ActivateItem(game);
            }
        }

        public virtual void ActivateItem(Game1 game)
        {
            items[currentItemX, currentItemY].ActivateItem(game, this);
            sound.Play(click2);
        }

        public void Disable()
        {
            foreach (IMenuItem i in items) {
                i.SetIsHighlighted(false);
            }
        }
    }
}
