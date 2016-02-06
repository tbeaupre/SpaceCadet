using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spaceman
{
    public interface IMenu
    {
        void OpenMenu(Game1 game);
        void UpdateMenu(Game1 game);
        IMenuItem GetMenuItem(int x, int y);
        Texture2D GetBackground();
        Rectangle GetItemZone();
        int GetItemsHeight();
        int GetItemsWidth();
        void Disable();
    }
}
