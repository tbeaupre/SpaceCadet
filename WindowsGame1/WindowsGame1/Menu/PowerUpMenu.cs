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
    public class PowerUpMenu : MenuGrid
    {
        PowerUpMenuItem selected;
        String key;

        public PowerUpMenu (Texture2D background, IMenuItem[,] items, Rectangle itemZone, String key)
            :base(background, items, itemZone)
        {
            selected = (PowerUpMenuItem)items[0, 1];
            selected.SetSelected(true);
            this.key = key;
        }

        public String GetKey()
        {
            return this.key;
        }

        public override void ActivateItem(Game1 game)
        {
            if (this.items[GetCurrentItemX(), GetCurrentItemY()] is PowerUpMenuItem)
            {
                selected.SetSelected(false);
                selected = (PowerUpMenuItem)items[GetCurrentItemX(), GetCurrentItemY()];
                selected.SetSelected(true);
            }
            base.ActivateItem(game);
        }

        public int GetIndex()
        {
            int index = 0;
            switch (key)
            {
                case "AlterSuitTab1Menu":
                    index = 0;
                    break;
                case "AlterSuitTab2Menu":
                    index = 1;
                    break;
                case "AlterSuitTab3Menu":
                    index = 2;
                    break;
            }
            return index;
        }

        public override void OpenMenu(Game1 game)
        {
            SetCurrentItem(0, 0); // Default
            this.items[GetCurrentItemX(), GetCurrentItemY()].SetIsHighlighted(false);

            switch (key)
            {
                case "AlterSuitTab1Menu":
                    SetCurrentItem(0, 2);
                    break;
                case "AlterSuitTab2Menu":
                    SetCurrentItem(1, 2);
                    break;
                case "AlterSuitTab3Menu":
                    SetCurrentItem(2, 2);
                    break;
            }

            this.items[GetCurrentItemX(), GetCurrentItemY()].SetIsHighlighted(true);
            game.lastMenu = game.currentMenu;
            game.currentMenu = this;
        }
    }
}
