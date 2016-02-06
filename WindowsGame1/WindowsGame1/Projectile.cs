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
        public int life;
        public int damage;
        public ISprite origin;
        public Directions direction;
        private IProjectileData data;
        private bool delete = false;
        private int random;


        public Projectile(IProjectileData data, Directions direction, ISprite origin, Vector2 mapCoordinates, double worldX, double worldY, int frameNum, bool mirrorX, string cue, SoundFX effect)
            : base(worldX, worldY, data.GetTexture(), new Vector2((float)worldX - mapCoordinates.X, (float)worldY - mapCoordinates.Y), data.GetNumFrames(), frameNum, mirrorX)
        {
            Random rnd = new Random();
            this.random = rnd.Next();
            this.data = data;
            this.origin = origin;
            this.life = 0;
            this.damage = data.GetDamage();
            this.direction = direction;
            effect.Play(StringConverter(cue));
        }

        public void Delete()
        {
            this.delete = true;
        }

        public string StringConverter(string s)
        {
            int number;
            switch (s)
            {
                case "bees":
                    number = (random % 3) + 1;
                    return (s + number);
                default:
                    return s;
            }

        }

        public bool GetDelete()
        {
            return this.delete;
        }

        public IProjectileData GetData()
        {
            return this.data;
        }

        public Directions GetDirection()
        {
            return this.direction;
        }

        public int GetLife()
        {
            return this.life;
        }

        public void SetLife(int life)
        {
            this.life = life;
        }
        public int GetRandom()
        {
            return this.random;
        }
    }
}
