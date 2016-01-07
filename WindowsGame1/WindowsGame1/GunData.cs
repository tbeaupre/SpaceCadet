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
	public class GunData
	{
		public string name;
		public bool unlocked;
        public IProjectileData projectileData;
		public int cooldown;
		public bool automatic;
		public int barrelX;
		public int barrelY;
		public int angledBarrelX;
		public int angledBarrelY;

		public GunData(string name, bool unlocked, int cooldown, IProjectileData projectileData,bool automatic, int barrelX, int barrelY,int angledBarrelX,int angledBarrelY)
		{
			this.name = name;
			this.unlocked = unlocked;
            this.projectileData = projectileData;
			this.cooldown = cooldown;
			this.automatic = automatic;
			this.barrelX = barrelX;
			this.barrelY = barrelY;
			this.angledBarrelX = angledBarrelX;
			this.angledBarrelY = angledBarrelY;
		}

        public Projectile CreateProjectile(Spaceman origin, Vector2 mapCoords, double worldX, double worldY)
        {
            return projectileData.CreateProjectile(origin.direction, origin, mapCoords, worldX, worldY, origin.GetMirrorX());
        }

	}
}
