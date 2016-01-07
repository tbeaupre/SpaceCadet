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

namespace Spaceman
{
    public class StandardProjectile : IProjectileData
    {
        Texture2D texture;
        double bulletVel;
        double yAcc;
        int lifeSpan;
        int damage;
        const int NUM_FRAMES = 2; // one for straight and one for angled.

        public StandardProjectile(Texture2D texture, double bulletVel, int lifeSpan, int damage)
        {
            this.texture = texture;
            this.bulletVel = bulletVel;
            this.yAcc = 0;
            this.lifeSpan = lifeSpan;
            this.damage = damage;
        }

        public Projectile CreateProjectile(Game1.Directions direction, ISprite origin, Vector2 mapCoords, double worldX, double worldY, bool mirrorX)
        {
            int frameNum;
            if (direction == Game1.Directions.left) frameNum = 0; // example frameNum logic. Need to add actual cases.
            else frameNum = 1;

            return new Projectile(this, direction, origin, mapCoords, worldX, worldY, frameNum, mirrorX);
        }

        public void DestroyProjectile()
        {
            // Nothing for the everyday projectile.
        }

        public int GetDamage()
        {
            return this.damage;
        }

        public int GetLifeSpan()
        {
            return this.lifeSpan;
        }

        public int GetNumFrames()
        {
            return NUM_FRAMES;
        }

        public Texture2D GetTexture()
        {
            return this.texture;
        }

        public double GetXVel(Game1.Directions dir)
        {
            return this.bulletVel;
        }

        public double GetYAcc()
        {
            return this.yAcc;
        }

        public double GetYVel(Game1.Directions dir)
        {
            return this.bulletVel;
        }

        public void UpdateProjectile(Projectile projectile)
        {
            
        }
    }
}
