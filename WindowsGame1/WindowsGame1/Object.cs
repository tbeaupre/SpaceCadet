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
	public abstract class Object : Sprite, IObject
    {
		public double worldX;
		public double worldY;
		public bool onScreen;
		public bool nearScreen;
		private Texture2D hitbox;

        public override Texture2D GetHitbox()
        {
            return this.hitbox;
        }

        public void SetHitbox(Texture2D hitbox)
        {
            this.hitbox = hitbox;
        }

        public Object(double worldX, double worldY, Texture2D texture, Vector2 destCoords, int numFrames, int frameNum, bool mirrorX)
			: base(texture, destCoords, numFrames, frameNum, mirrorX)
		{
			this.worldX = worldX;
			this.worldY = worldY;
			this.hitbox = texture;
		}

		public Object(double worldX, double worldY, Texture2D texture, Vector2 destCoords, int numFrames, int frameNum, bool mirrorX, Texture2D hitbox)
			: base(texture, destCoords, numFrames, frameNum, mirrorX)
		{
			this.worldX = worldX;
			this.worldY = worldY;
			this.hitbox = hitbox;
		}

		public bool IsOnScreen(Map map)
		{
            return (this.worldX + this.spriteWidth + map.offset.X > map.mapCoordinates.X
                && this.worldX + map.offset.X < map.mapCoordinates.X + Game1.screenWidth
                && this.worldY + this.spriteHeight + map.offset.Y > map.mapCoordinates.Y
                && this.worldY + map.offset.Y < map.mapCoordinates.Y + Game1.screenHeight);
		}

		public bool IsNearScreen(Map map)
		{
			return ((this.worldX + this.spriteWidth) > (map.mapCoordinates.X - Game1.screenWidth) && this.worldX < (map.mapCoordinates.X + (2 * Game1.screenWidth)) &&
				(this.worldY + this.spriteHeight) > (map.mapCoordinates.Y - Game1.screenHeight) && this.worldY < map.mapCoordinates.Y + (2 * Game1.screenHeight));
		}

        public void UpdateCoords(Map map)
        {
            this.destRect.X = (int)(this.worldX - map.mapCoordinates.X + map.offset.X);
            this.destRect.Y = (int)(this.worldY - map.mapCoordinates.Y + map.offset.Y);
        }

        public virtual void UpdateSprite(Map map)
        {
            if (status.state.Equals("hit"))
            {
                if (status.duration > 0) status.duration--;
            }
            UpdateCoords(map);
            this.onScreen = IsOnScreen(map);
            this.nearScreen = IsNearScreen(map);
            this.sourceRect = new Rectangle(this.spriteWidth * this.frameNum, 0, this.texture.Width / numFrames, this.texture.Height);
        }
    }
}
