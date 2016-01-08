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
        private int life = 0;
        public int damage;
        public ISprite origin;
        public Game1.Directions direction;
        private IProjectileData data;

        public Projectile(IProjectileData data, Game1.Directions direction, ISprite origin, Vector2 mapCoordinates, double worldX, double worldY, int frameNum, bool mirrorX)
            : base(worldX, worldY, data.GetTexture(), new Vector2((float)worldX - mapCoordinates.X, (float)worldY - mapCoordinates.Y), data.GetNumFrames(), frameNum, mirrorX)
        {
            this.origin = origin;
            this.life++;
            this.damage = data.GetDamage();
            this.direction = direction;
        }

        public IProjectileData GetData()
        {
            return this.data;
        }

        public Game1.Directions GetDirection()
        {
            return this.direction;
        }
        public int GetLife()
        {
            return life;
        }
    }
}
