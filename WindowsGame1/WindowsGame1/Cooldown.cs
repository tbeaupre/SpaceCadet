using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spaceman
{
    public class Cooldown
    {
        int current = 0;
        int MAX;

        public Cooldown(int MAX)
        {
            this.MAX = MAX;
        }

        public void SetCooldown()
        {
            this.current = MAX;
        }

        public bool Iterate()
        {
            current--;
            if (current <= 0)
            {
                current = 0;
                return true;
            }
            return false;
        }

        public int GetCurrent()
        {
            return this.current;
        }

        public int GetMax()
        {
            return this.MAX;
        }
    }
}
