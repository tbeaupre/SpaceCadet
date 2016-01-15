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
    public class ProjectileData
    {
        private int damage;
        public double xVel;
        public double yVel;
        public double yAcc;
        public Texture2D texture;
        public int numFrames;
        public int frameNum;
        public int xOffset;
        public int yOffset;

        public ProjectileData(int damage,
            double xVel,
            double yVel,
            double yAcc,
            Texture2D texture,
            int numFrames,
            int frameNum,
            int xOffset,
            int yOffset)
        {
            this.damage = damage;
            this.xVel = xVel;
            this.yVel = yVel;
            this.yAcc = yAcc;
            this.texture = texture;
            this.numFrames = numFrames;
            this.frameNum = frameNum;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
        }

        public int GetDamage()
        {
            return damage;
        }
    }
}