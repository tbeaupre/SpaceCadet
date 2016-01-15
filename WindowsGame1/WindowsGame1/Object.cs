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
		public Texture2D hitbox;

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

		public override bool PerPixelCollisionDetect(Game1 game)
		{
			Rectangle rect = new Rectangle(this.destRect.X - game.player.destRect.X + 2, this.destRect.Y - game.player.destRect.Y + 1, this.spriteWidth, this.spriteHeight);

			Texture2D projTexture = game.player.GetTexture();
			Texture2D hitBoxTexture = this.hitbox;

			Color[] objectPixels;
			Color[] projectilePixels;
			Rectangle objRect = rect;
			Rectangle projRect = new Rectangle(0, 0, rect.Width, rect.Height);

			//initial tests to see if the box is even applicable to the object texure being checked
			if (rect.X + rect.Width <= 0 || rect.Y + rect.Height <= 0) return false;
			if (rect.X >= game.player.spriteWidth - 4 || rect.Y >= game.player.spriteHeight - 1) return false;

			if (rect.X < 0)
			{
				objRect.Width += rect.X;
				objRect.X = 0;
				projRect.X -= rect.X;
				projRect.Width = objRect.Width;
			}

			if (rect.Y < 0)
			{
				objRect.Height += rect.Y;
				objRect.Y = 0;
				projRect.Y -= rect.Y;
				projRect.Height = objRect.Height;
			}

			for (int i = 0; i <= objRect.Width; i++)
			{
				if (objRect.X + i == game.player.spriteWidth - 4)
				{
					objRect.Width = i;
					projRect.Width = objRect.Width;
					break;
				}
			}
			for (int i = 0; i <= objRect.Height; i++)
			{
				if (objRect.Y + i == game.player.spriteHeight - 1)
				{
					objRect.Height = i;
					projRect.Height = objRect.Height;
					break;
				}
			}

			objectPixels = new Color[objRect.Width * objRect.Height];
			projectilePixels = new Color[objRect.Width * objRect.Height];

			if (this.mirrorX)
			{
				hitBoxTexture = MirrorTexture(this, game, hitBoxTexture);
			}

			projTexture.GetData<Color>(
				0, objRect, objectPixels, 0, objRect.Width * objRect.Height
				);

			hitBoxTexture.GetData<Color>(
				0, projRect, projectilePixels, 0, objRect.Width * objRect.Height
				);

			for (int y = 0; y < objRect.Height; y++)
			{
				for (int x = 0; x < objRect.Width; x++)
				{
					Color colorA = objectPixels[y * objRect.Width + x];
					Color colorB = projectilePixels[y * objRect.Width + x];
					if (colorA.A != 0 && colorB.A != 0)
					{
						return true;
					}
				}
			}
			return false;
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
