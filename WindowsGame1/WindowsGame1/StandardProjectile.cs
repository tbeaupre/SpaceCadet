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

        public Projectile CreateProjectile(Directions direction, ISprite origin, Vector2 mapCoords, double worldX, double worldY, bool mirrorX)
        {
            int frameNum;

            switch (direction)
            {
                case Directions.down:
                    frameNum = 0;
                    break;
                case Directions.downLeft:
                    frameNum = 1;
                    break;
                case Directions.downRight:
                    frameNum = 1;
                    break;
                case Directions.left:
                    frameNum = 0;
                    break;
                case Directions.right:
                    frameNum = 0;
                    break;
                case Directions.up:
                    frameNum = 0;
                    break;
                case Directions.upLeft:
                    frameNum = 1;
                    break;
                case Directions.upRight:
                    frameNum = 1;
                    break;
                default:
                    frameNum = 0;
                    break;
            }

            return new Projectile(this, direction, origin, mapCoords, worldX, worldY, frameNum, mirrorX);
        }

        public void DestroyProjectile(Projectile projectile)
        {
            projectile.Delete();
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

        public virtual double GetXVel(Projectile projectile)
        {
            switch (projectile.GetDirection())
            {
                case Directions.left:
                    return -this.bulletVel;
                case Directions.right:
                    return this.bulletVel;
                case Directions.up:
                    return 0;
                case Directions.down:
                    return 0;
                case Directions.downLeft:
                    return -this.bulletVel / 2;
                case Directions.downRight:
                    return this.bulletVel / 2;
                case Directions.upLeft:
                    return -this.bulletVel / 2;
                case Directions.upRight:
                    return this.bulletVel / 2;
            }
            return this.bulletVel;
        }

        public double GetYAcc()
        {
            return this.yAcc;
        }

        public virtual double GetYVel(Projectile projectile)
        {
            switch (projectile.GetDirection())
            {
                case Directions.left:
                    return 0;
                case Directions.right:
                    return 0;
                case Directions.up:
                    return -this.bulletVel;
                case Directions.down:
                    return this.bulletVel;
                case Directions.downLeft:
                    return this.bulletVel / 2;
                case Directions.downRight:
                    return this.bulletVel / 2;
                case Directions.upLeft:
                    return -this.bulletVel / 2;
                case Directions.upRight:
                    return -this.bulletVel / 2;
            }
            return this.bulletVel;
        }

        public void UpdateProjectile(Projectile projectile, Game1 game)
        {
            projectile.life++;
            if (projectile.life == GetLifeSpan())
            {
                DestroyProjectile(projectile);
            }

            projectile.worldX += projectile.GetData().GetXVel(projectile);
            projectile.worldY += projectile.GetData().GetYVel(projectile);
            projectile.UpdateSprite(game.worldMap[game.currentRoom]);
            if (projectile.PerPixelCollisionDetect(game) && game.worldMap[game.currentRoom].enemyProjectiles.Contains(projectile))
            {
                game.player.TakeDamage(projectile);
                DestroyProjectile(projectile);
            }
            if (game.CheckMapCollision(0, 0, projectile) == false)
            {
                if (projectile.onScreen && projectile.nearScreen) game.AddObjectToDraw(projectile);
                else projectile.Delete();
            }
            else
            {
                DestroyProjectile(projectile);
            }
        }
    }
}
