﻿using System;
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
	public class Sprite
	{
		public Texture2D texture;
		public Rectangle sourceRect;
		public Rectangle destRect;
		public int spriteWidth;
		public int spriteHeight;
		public int numFrames;
		public int frameNum;
		public bool mirrorX;
		public int timer;
		public Nullable<int> cycleStart;
		public int HIT_DURATION = 5;
		public Status status;
		public int FRAME_OFFSET;

		public Sprite(Texture2D texture, Vector2 destCoords, int numFrames, int frameNum, bool mirrorX)
		{
			this.texture = texture;
			this.spriteWidth = (texture.Width / numFrames);
			this.spriteHeight = texture.Height;
			this.numFrames = numFrames;
			this.frameNum = frameNum;
			this.sourceRect = new Rectangle(this.spriteWidth * frameNum, 0, spriteWidth, texture.Height);
			this.destRect = new Rectangle((int)destCoords.X, (int)destCoords.Y, this.spriteWidth, this.spriteHeight);
			this.mirrorX = mirrorX;
			this.timer = 0;
			this.FRAME_OFFSET = 5;
			this.status = new Status("ok", 0);
		}

		public void Hit()
		{
			this.status = new Status("hit", this.HIT_DURATION * this.FRAME_OFFSET);
		}

		public void NextFrame(int offset)
		{
			FrameTimer(offset);
		}

		public void FrameTimer(int offset)
		{
			this.timer++;
			if (this.timer == offset)
			{
				this.FrameUpdate();
				timer = 0;
			}
		}

		public void FrameUpdate()
		{
			if (this.frameNum == (numFrames - 1))
			{
				if (cycleStart == null)
				{
					this.frameNum = 0;
				}
				else
				{
					this.frameNum = ((int)cycleStart - 1);
				}
			}
			else this.frameNum++;
		}

		public virtual void UpdateSprite()
		{
			this.sourceRect = new Rectangle(this.spriteWidth * this.frameNum, 0, this.texture.Width / numFrames, this.texture.Height);
			if (status.state.Equals("hit"))
			{
				if (status.duration > 0) status.duration--;
			}
		}

		public virtual bool PerPixelCollisionDetect(Sprite sprite, Game1 game)
		{
			Rectangle rect = new Rectangle(sprite.destRect.X - this.destRect.X, sprite.destRect.Y - this.destRect.Y, sprite.spriteWidth, sprite.spriteHeight);
			// sets the coordinates relative to (0,0) being the top left corner of this.
			Texture2D projTexture = sprite.texture;
			Texture2D hitBoxTexture = this.texture;

			Color[] hitBoxPixels;
			Color[] projectilePixels;
			Rectangle objRect = rect;
			Rectangle projRect = new Rectangle(0, 0, sprite.spriteWidth, sprite.spriteHeight);

			//initial tests to see if the box is even applicable to the object texure being checked
			if (rect.X + rect.Width <= 0 || rect.Y + rect.Height <= 0) return false;
			if (rect.X >= this.spriteWidth || rect.Y >= this.spriteHeight) return false;

			if (rect.X < 0)
			{
				objRect.X = 0;
				objRect.Width += rect.X;
				projRect.X -= rect.X;
				projRect.Width += rect.X;
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
				if (objRect.X + i == this.spriteWidth)
				{
					objRect.Width = i;
					projRect.Width = objRect.Width;
					break;
				}
			}
			for (int i = 0; i <= objRect.Height; i++)
			{
				if (objRect.Y + i == this.spriteHeight)
				{
					objRect.Height = i;
					projRect.Height = objRect.Height;
					break;
				}
			}

			if (objRect.Width == 0 || objRect.Height == 0) return false;

			hitBoxPixels = new Color[objRect.Width * objRect.Height];
			projectilePixels = new Color[objRect.Width * objRect.Height];

			if (sprite.mirrorX)
			{
				projTexture = MirrorTexture(sprite, game, sprite.texture);
			}

			if (this.mirrorX)
			{
				hitBoxTexture = MirrorTexture(this, game, this.texture);
			}

			projTexture.GetData<Color>(
				0, projRect, projectilePixels, 0, objRect.Width * objRect.Height
				);

			hitBoxTexture.GetData<Color>(
				0, objRect, hitBoxPixels, 0, objRect.Width * objRect.Height
				);

			for (int y = 0; y < objRect.Height; y++)
			{
				for (int x = 0; x < objRect.Width; x++)
				{
					Color colorA = hitBoxPixels[y * objRect.Width + x];
					Color colorB = projectilePixels[y * objRect.Width + x];
					if (colorA.A != 0 && colorB.A != 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		public virtual bool PerPixelCollisionDetect(Game1 game)
		{
			Rectangle rect = new Rectangle(this.destRect.X - game.spaceMan.destRect.X + 2, this.destRect.Y - game.spaceMan.destRect.Y + 1, this.spriteWidth, this.spriteHeight);

			Texture2D projTexture = game.spaceMan.texture;
			Texture2D hitBoxTexture = this.texture;

			Color[] objectPixels;
			Color[] projectilePixels;
			Rectangle objRect = rect;
			Rectangle projRect = new Rectangle(0, 0, rect.Width, rect.Height);

			//initial tests to see if the box is even applicable to the object texure being checked
			if (rect.X + rect.Width <= 0 || rect.Y + rect.Height <= 0) return false;
			if (rect.X >= game.spaceMan.spriteWidth - 4 || rect.Y >= game.spaceMan.spriteHeight - 1) return false;

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
				if (objRect.X + i == game.spaceMan.spriteWidth - 4)
				{
					objRect.Width = i;
					projRect.Width = objRect.Width;
					break;
				}
			}
			for (int i = 0; i <= objRect.Height; i++)
			{
				if (objRect.Y + i == game.spaceMan.spriteHeight - 1)
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
				hitBoxTexture = MirrorTexture(this, game, this.texture);
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

		public bool RectCollisionDetect(Sprite sprite)
		{
			Color[] pixels;

			Rectangle rect = new Rectangle(sprite.destRect.X - this.destRect.X, sprite.destRect.Y - this.destRect.Y, sprite.spriteWidth, sprite.spriteHeight);  // this rectangle represents the space the sprite takes up relative to "this"'s top left corner
			Rectangle newRect = rect; // newRect is the actual rectangle to check

			//initial tests to see if the box is even applicable to the object texure being checked
			if (rect.X + rect.Width <= 0 || rect.Y + rect.Height <= 0) return false;
			if (rect.X >= this.spriteWidth || rect.Y >= this.spriteHeight) return false;

			// Removes the space on the rectangle that is outside of the bounds of "this"'s texture
			if (rect.X < 0)
			{
				newRect.X = 0;
				newRect.Width += rect.X;
			}
			if (rect.Y < 0)
			{
				newRect.Y = 0;
				newRect.Height += rect.Y;
			}

			if (newRect.X + newRect.Width > this.spriteWidth)
			{
				for (int i = 0; i <= this.spriteWidth; i++)
				{
					if (newRect.X + i == this.spriteWidth)
					{
						newRect.Width = i;
						break;
					}
				}
			}
			if (newRect.Y + newRect.Height > this.spriteHeight)
			{
				for (int i = 0; i <= this.spriteHeight; i++)
				{
					if (newRect.Y + i == this.spriteHeight)
					{
						newRect.Height = i;
						break;
					}
				}
			}

			if (newRect.Width == 0 || newRect.Height == 0) return false;

			pixels = new Color[newRect.Width * newRect.Height];

			this.texture.GetData<Color>(
				0, newRect, pixels, 0, newRect.Width * newRect.Height
				);

			for (int y = 0; y < newRect.Height; y++)
			{
				for (int x = 0; x < newRect.Width; x++)
				{
					Color colorA = pixels[y * newRect.Width + x];
					if (colorA.A != 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool RectCollisionDetect(Spaceman sprite)
		{
			Color[] pixels;
			int spriteWidth = sprite.spriteWidth - 4; // because the hitbox on the spaceman should be slightly smaller than it is.
			int spriteHeight = sprite.spriteHeight - 1;

			Rectangle rect = new Rectangle(sprite.destRect.X - this.destRect.X + 2, sprite.destRect.Y - this.destRect.Y + 1, spriteWidth, spriteHeight);  // this rectangle represents the space the sprite takes up relative to "this"'s top left corner
			Rectangle newRect = rect; // newRect is the actual rectangle to check

			//initial tests to see if the box is even applicable to the object texure being checked
			if (rect.X + rect.Width <= 0 || rect.Y + rect.Height <= 0) return false;
			if (rect.X >= this.spriteWidth || rect.Y >= this.spriteHeight) return false;

			// Removes the space on the rectangle that is outside of the bounds of "this"'s texture
			if (rect.X < 0)
			{
				newRect.X = 0;
				newRect.Width += rect.X;
			}
			if (rect.Y < 0)
			{
				newRect.Y = 0;
				newRect.Height += rect.Y;
			}

			if (newRect.X + newRect.Width > this.spriteWidth)
			{
				for (int i = 0; i <= this.spriteWidth; i++)
				{
					if (newRect.X + i == this.spriteWidth)
					{
						newRect.Width = i;
						break;
					}
				}
			}
			if (newRect.Y + newRect.Height > this.spriteHeight)
			{
				for (int i = 0; i <= this.spriteHeight; i++)
				{
					if (newRect.Y + i == this.spriteHeight)
					{
						newRect.Height = i;
						break;
					}
				}
			}

			if (newRect.Width == 0 || newRect.Height == 0) return false;

			pixels = new Color[newRect.Width * newRect.Height];

			this.texture.GetData<Color>(
				0, newRect, pixels, 0, newRect.Width * newRect.Height
				);

			for (int y = 0; y < newRect.Height; y++)
			{
				for (int x = 0; x < newRect.Width; x++)
				{
					Color colorA = pixels[y * newRect.Width + x];
					if (colorA.A != 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		public Texture2D MirrorTexture(Sprite sprite, Game1 game, Texture2D texture)
		{
			Texture2D mirroredProjectile = new Texture2D(game.graphics.GraphicsDevice, sprite.spriteWidth, sprite.spriteHeight);
			Color[] projectileTextureHelper = new Color[sprite.spriteWidth * sprite.spriteHeight];
			Color[] newTextureData = new Color[sprite.spriteWidth * sprite.spriteHeight];
			texture.GetData<Color>(
				0, sprite.sourceRect, projectileTextureHelper, 0, sprite.spriteWidth * sprite.spriteHeight);

			for (int x = 0; x < sprite.spriteWidth; x++)
			{
				for (int y = 0; y < sprite.spriteHeight; y++)
				{
					newTextureData[y * sprite.spriteWidth + x] = projectileTextureHelper[(y + 1) * sprite.spriteWidth - 1 - x];
					newTextureData[(y + 1) * sprite.spriteWidth - 1 - x] = projectileTextureHelper[y * sprite.spriteWidth + x];
				}
			}

			mirroredProjectile.SetData<Color>(0, new Rectangle(0, 0, sprite.spriteWidth, sprite.spriteHeight), newTextureData, 0, sprite.spriteWidth * sprite.spriteHeight);
			return mirroredProjectile;
		}
	}
}