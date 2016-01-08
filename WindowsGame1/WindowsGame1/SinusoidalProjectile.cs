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
        double xVel;
        double yVel;
        int lifeSpan;
        int damage;

        public SinusoidalProjectile(Texture2D texture, double bulletVel, int lifeSpan, int damage)
            :base(texture,bulletVel, lifeSpan, damage)
        {
            this.texture = texture;
            this.bulletVel = bulletVel;
            this.lifeSpan = lifeSpan;
            this.damage = damage;
        }
        public new void UpdateProjectile(Projectile projectile, Game1 game)
        {
            switch (projectile.GetDirection())
            {
                case Directions.upLeft:
                    yVel = -Math.Sin(0.5 * projectile.GetLife()) - Math.Sin(Math.PI / 4) * bulletVel;
                    xVel = -Math.Sin(0.5 * projectile.GetLife()) - Math.Sin(Math.PI / 4) * bulletVel;
                    break;
                case Directions.upRight:
                    yVel = -Math.Sin(0.5 * projectile.GetLife()) - Math.Sin(Math.PI / 4) * bulletVel;
                    xVel = Math.Sin(0.5 * projectile.GetLife()) + Math.Sin(Math.PI / 4) * bulletVel;
                    break;
                case Directions.downLeft:
                    yVel = Math.Sin(0.5 * projectile.GetLife()) + Math.Sin(Math.PI / 4) * bulletVel;
                    xVel = -Math.Sin(0.5 * projectile.GetLife()) - Math.Sin(Math.PI / 4) * bulletVel;
                    break;
                case Directions.downRight:
                    yVel = Math.Sin(0.5 * projectile.GetLife()) + Math.Sin(Math.PI / 4) * bulletVel;
                    xVel = Math.Sin(0.5 * projectile.GetLife()) + Math.Sin(Math.PI / 4) * bulletVel;
                    break;
                case Directions.up:
                    yVel = -bulletVel;
                    xVel = Math.Sin(projectile.GetLife());
                    break;
                case Directions.down:
                    yVel = bulletVel;
                    xVel = Math.Sin(projectile.GetLife());
                    break;
                case Directions.right:
                    yVel = Math.Sin(projectile.GetLife());
                    xVel = bulletVel;
                    break;
                case Directions.left:
                    yVel = Math.Sin(projectile.GetLife());
                    xVel = -bulletVel;
                    break;
            }
        }
    }
}
