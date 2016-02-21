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
        private Vector2 destOffsetCoord;
        Vector2 origin;
        private bool cornered;
        public Droplet(Texture2D pixel, Vector2 destCoordOffset, int numFrames, int frameNum, Vector2 origin)
            : base(pixel, destCoordOffset, numFrames, frameNum, false)
        {
            this.destOffsetCoord = destCoordOffset - origin;
            this.origin = origin;
        }
        public void SetCornered(bool cornered)
        {
            this.cornered = cornered;
        }
        public new Vector2 GetDestRect()
        {
            return origin + destOffsetCoord;
        }
        public void OffsetDestCoord(Vector2 coords)
        {
            destOffsetCoord.X += coords.X;
            destOffsetCoord.Y += coords.Y;
        }

        public void UpdateCoords(Vector2 offset)
        {
            destRect.X = origin.X + destOffsetCoord.X + offset.X;
            destRect.Y = origin.Y + destOffsetCoord.Y + offset.Y;
        }

        public bool MapCollide(Game1 game)
        {
            if (CollisionDetector.CheckMapCollision((int)destRect.X,(int) destRect.Y+1, this, game.worldMap[game.currentRoom])) return true;
            else return false;
        }
    }
}
