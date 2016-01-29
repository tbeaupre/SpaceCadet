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
    class SinusoidalProjectile : StandardProjectile
    {
        private double bulletVel;
        private double amplitude;
        private double frequency;
        private double shift = 1;
        private bool random;

        public SinusoidalProjectile(Texture2D texture, double bulletVel, int lifeSpan, int damage, double amplitude, double frequency, bool random, string sound)
            : base(texture, bulletVel, lifeSpan, damage, sound)
        {
            this.bulletVel = bulletVel;
            this.amplitude = amplitude / 10;
            this.frequency = frequency / amplitude;
            this.random = random;
        }

        public override double GetXVel(Projectile projectile)
        {
            if (random)
                if (projectile.GetRandom() % 2 == 0)
                {
                    shift = Math.PI;
                }
                else
                {
                    shift = Math.PI / 4;
                }
            else
            {
                shift = 0;
            }

            int life = projectile.GetLife();
            switch (projectile.GetDirection())
            {
                case Directions.left:
                    return -this.bulletVel;
                case Directions.right:
                    return this.bulletVel;
                case Directions.up:
                    return amplitude * Math.Sin(frequency * life + shift);
                case Directions.down:
                    return amplitude * Math.Sin(frequency * life + shift);
                case Directions.downLeft:
                    return -(amplitude * Math.Sin(frequency / life + shift) + 0.7 * bulletVel);
                case Directions.downRight:
                    return  (amplitude * Math.Sin(frequency / life + shift) + 0.7 * bulletVel);
                case Directions.upLeft:
                    return -(amplitude * Math.Sin(frequency / life + shift) + 0.7 * bulletVel);
                case Directions.upRight:
                    return  (amplitude * Math.Sin(frequency / life + shift) + 0.7 * bulletVel);
            }
            return bulletVel;
        }

        public override double GetYVel(Projectile projectile)
        {
            if (random)
            {
                if(projectile.GetRandom() % 2 == 0)
                {
                    shift = Math.PI;
                }
                else
                {
                    shift = Math.PI / 4;
                }
            }
            else
            {
                shift = 0;
            }

            int life = projectile.GetLife();
            switch (projectile.GetDirection())
            {
                case Directions.left:
                    return amplitude * Math.Sin(frequency * life + shift);
                case Directions.right:
                    return amplitude * Math.Sin(frequency * life + shift);
                case Directions.up:
                    return -this.bulletVel;
                case Directions.down:
                    return this.bulletVel;
                case Directions.downLeft:
                    return  (amplitude * Math.Sin(frequency * life + shift) + 0.7 * bulletVel);
                case Directions.downRight:
                    return  (amplitude * Math.Sin(frequency * life + shift) + 0.7 * bulletVel);
                case Directions.upLeft:
                    return -(amplitude * Math.Sin(frequency * life + shift) + 0.7 * bulletVel);
                case Directions.upRight:
                    return -(amplitude * Math.Sin(frequency * life + shift) + 0.7 * bulletVel);
            }
            return this.bulletVel;
        }
    }
}