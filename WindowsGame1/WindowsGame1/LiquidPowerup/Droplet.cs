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
    public class Droplet : Sprite
    {
        private Vector2 destCoord;
        private bool cornered;
        private bool mapCollide = true;
        public Droplet(Texture2D pixel, Vector2 destCoord, int numFrames, int frameNum)
            : base(pixel, destCoord, numFrames, frameNum, false)
        {
            this.destCoord = destCoord;
        }
        public void setCornered(bool cornered)
        {
            this.cornered = cornered;
        }
        public new Vector2 GetDestRect()
        {
            return new Vector2((float)destRect.X, (float)destRect.Y);
        }
        public void SetDestCoord(Vector2 coords)
        {
            destRect.X += coords.X;
            destRect.Y += coords.Y;
        }
        public void SetMapCollide(bool m) {
            mapCollide = m;
        }
        public bool GetMapCollide()
        {
            return mapCollide;
        }

    }
}
