using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spaceman
{
    public interface IMenu
    {
        void OpenMenu(Game1 game);
        void UpdateMenu(Game1 game);
    }
}
