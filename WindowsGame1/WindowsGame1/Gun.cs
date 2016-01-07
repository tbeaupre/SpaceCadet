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
	public class Gun
	{
		public string name;
		public bool unlocked;
		public int bulletVel;
		private int damage;
		public int cooldown;
        private int bulletLifeSpan;
		public bool automatic;
		public int barrelX;
		public int barrelY;
		public int angledBarrelX;
		public int angledBarrelY;
        private bool sinusoidal;

		public Gun(string name, bool unlocked, int bulletVel, int damage, int cooldown, int bulletLifeSpan,bool automatic, int barrelX, int barrelY,int angledBarrelX,int angledBarrelY, bool sinusoidal)
		{
			this.name = name;
			this.unlocked = unlocked;
			this.bulletVel = bulletVel;
			this.damage = damage;
			this.cooldown = cooldown;
            this.bulletLifeSpan = bulletLifeSpan;
			this.automatic = automatic;
			this.barrelX = barrelX;
			this.barrelY = barrelY;
			this.angledBarrelX = angledBarrelX;
			this.angledBarrelY = angledBarrelY;
            this.sinusoidal = sinusoidal;
		}

        public bool isSinusoidal()
        {
            return sinusoidal;
        }

        public int GetDamage()
        {
            return damage;
        }

        public int GetBulletLifeSpan()
        {
            return bulletLifeSpan;
        }
    }
}
