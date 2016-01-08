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
        Texture2D texture;
        double bulletVel;
        double yVel;
        int lifeSpan;
        int damage;
        const int NUM_FRAMES = 2; // one for straight and one for angled.

        public SinusoidalProjectile(Texture2D texture, double bulletVel, int lifeSpan, int damage)
            :base(texture,bulletVel, lifeSpan, damage)
        {
            this.texture = texture;
            this.bulletVel = bulletVel;
            this.lifeSpan = lifeSpan;
            this.damage = damage;
        }
        public new void UpdateProjectile(Projectile projectile)
        {
            switch (projectile.GetDirection())
            {
                case Game1.Directions.upLeft:

                    break;
                case Game1.Directions.upRight:

                    break;
                case Game1.Directions.downLeft:

                    break;
                case Game1.Directions.up:

                    break;
                case Game1.Directions.downRight:

                    break;
                case Game1.Directions.down:

                    break;
                default:
                    yVel = Math.Sin(projectile.GetLife());
                    break;
            }
        }
    }
}
