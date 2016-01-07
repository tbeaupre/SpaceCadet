using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Spaceman;

namespace Spaceman
{
	public class Projectile : Object
	{
		public double xVel;
		public double yVel;
		public double yAcc;
        public Nullable<int> lifeSpan;
		public int damage;
		public Sprite origin;
        public Directions direction;

        public Projectile(Directions direction, Sprite origin, Vector2 mapCoordinates, int damage, double worldX, double worldY, double xVel, double yVel, double yAcc, Nullable<int> lifeSpan, Texture2D texture, int numFrames, int frameNum, bool mirrorX, bool sinusoidal)
            : base(worldX, worldY, texture, new Vector2((float)worldX - mapCoordinates.X, (float)worldY - mapCoordinates.Y), numFrames, frameNum, mirrorX)
        {
            this.origin = origin;
            this.lifeSpan = lifeSpan;
            this.xVel = xVel;
            if (sinusoidal)
            {
                this.yVel = Math.Sin((double)lifeSpan);
            }
            else
            {
                this.yVel = yVel;
            }
            this.yAcc = yAcc;
			this.damage = damage;
			this.direction = direction;
		}

	}
}
