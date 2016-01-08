using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spaceman
{
    public interface IObject : ISprite
    {
        bool IsOnScreen(Map map);
        bool IsNearScreen(Map map);
        void UpdateSprite(Map map);
        void Hit();
    }
}
