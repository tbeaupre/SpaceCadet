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
        double bulletVel;
        double amplitude;
        double frequency;
        bool randomized;
        Random rnd= new Random();

        public SinusoidalProjectile(Texture2D texture, double bulletVel, int lifeSpan, int damage, double amplitude, double frequency, bool randomized)
            : base(texture, bulletVel, lifeSpan, damage)
        {
            this.bulletVel = bulletVel;
            this.amplitude = amplitude / 10;
            this.frequency = frequency / amplitude;
            this.randomized = randomized;
        }

        public override double GetXVel(Projectile projectile)
        {
            int randy;

            if (randomized)
            {
                randy = rnd.Next(0,4);
            }
            else
            {
                randy = 0;
            }

            switch (projectile.GetDirection())
            {
                case Directions.left:
                    return -this.bulletVel;
                case Directions.right:
                    return this.bulletVel;
                case Directions.up:
                    return amplitude * Math.Sin((frequency * projectile.GetLife())+randy);
                case Directions.down:
                    return amplitude * Math.Sin((frequency * projectile.GetLife())+randy);
                case Directions.downLeft:
                    return -(amplitude * Math.Sin((frequency / projectile.GetLife())+randy) + 0.7 * bulletVel);
                case Directions.downRight:
                    return  (amplitude * Math.Sin((frequency / projectile.GetLife())+randy) + 0.7 * bulletVel);
                case Directions.upLeft:
                    return -(amplitude * Math.Sin((frequency / projectile.GetLife())+randy) + 0.7 * bulletVel);
                case Directions.upRight:
                    return  (amplitude * Math.Sin((frequency / projectile.GetLife())+randy) + 0.7 * bulletVel);
            }
            return this.bulletVel;
        }

        public override double GetYVel(Projectile projectile)
        {
            int randy;

            if (randomized)
            {
                randy = rnd.Next(0, 4);
            }
            else
            {
                randy = 0;
            }

            switch (projectile.GetDirection())
            {
                case Directions.left:
                    return amplitude * Math.Sin((frequency * projectile.GetLife())+randy);
                case Directions.right:
                    return amplitude * Math.Sin((frequency * projectile.GetLife())+randy);
                case Directions.up:
                    return -this.bulletVel;
                case Directions.down:
                    return this.bulletVel;
                case Directions.downLeft:
                    return  (amplitude * Math.Sin((frequency * projectile.GetLife())+randy) + 0.7 * bulletVel);
                case Directions.downRight:
                    return  (amplitude * Math.Sin((frequency * projectile.GetLife())+randy) + 0.7 * bulletVel);
                case Directions.upLeft:
                    return -(amplitude * Math.Sin((frequency * projectile.GetLife())+randy) + 0.7 * bulletVel);
                case Directions.upRight:
                    return -(amplitude * Math.Sin((frequency * projectile.GetLife())+randy) + 0.7 * bulletVel);
            }
            return this.bulletVel;
        }
        //private double angledXVel(double angle)
        //{

        //}
        //private double angledYVel(double angle)
        //{

        //}
    }
}