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
	public class GunOverlay : Sprite
	{
		Spaceman baseSprite;
		public float angle = 0;
		public int xOffset = 0;
		public int yOffset = 0;
		Texture2D angleUpTexture;
		Texture2D angleDownTexture;
		Texture2D flatTexture;

		public GunOverlay(Spaceman baseSprite, Texture2D angleUpTexture, Texture2D angleDownTexture, Texture2D flatTexture,  Vector2 destCoords, int numFrames, int frameNum,
			int sizeMultiplier, bool mirrorX, Nullable<int> cycleStart)
			: base(flatTexture, destCoords, numFrames, frameNum, mirrorX)
		{
			this.baseSprite = baseSprite;
			this.angleUpTexture = angleUpTexture;
			this.angleDownTexture = angleDownTexture;
			this.flatTexture = flatTexture;
		}

		public override void UpdateSprite()
		{
			this.mirrorX = baseSprite.mirrorX;
			switch (baseSprite.direction) // 1 = left, 2 = upLeft, 3 = up, 4 = upRight, 5 = right, 6 = down
			{
				case Game1.Directions.upLeft:
					texture = angleUpTexture;
					angle = 0;
					xOffset = -3;
					yOffset = -7;
					break;
				case Game1.Directions.up:
					angle = (float)Math.PI / 2;
					texture = flatTexture;
					if (mirrorX)
					{
						xOffset = 23;
						yOffset = -7;
					}
					else
					{
						xOffset = -4;
						yOffset = 12;
					}
					break;
				case Game1.Directions.upRight:
					texture = angleUpTexture;
					angle = 0;
					xOffset = 3;
					yOffset = -7;
					break;
				case Game1.Directions.down:
					angle = -(float)Math.PI / 2;
					texture = flatTexture;
					if (mirrorX)
					{
						xOffset = 6;
						yOffset = 23;
					}
					else
					{
						xOffset = 13;
						yOffset = 4;
					}
					break;
				case Game1.Directions.downLeft:
					angle = -(float)Math.PI / 2;
					texture = angleDownTexture;
					xOffset = -1;
					yOffset = 26;
					break;
				case Game1.Directions.downRight:
					angle = -(float)Math.PI / 2;
					texture = angleDownTexture;
					xOffset = 20;
					yOffset = 7;
					break;
				default:
					texture = flatTexture;
					angle = 0;
					xOffset = 0;
					yOffset = 0;
					break;
			}

			this.status = this.baseSprite.status;
			int[] array = new int[4] { 4, 5, 9, 10 };
			this.sourceRect = new Rectangle(this.spriteWidth * this.frameNum, 0, this.texture.Width / numFrames, this.texture.Height);
			if (array.Contains(baseSprite.currentBodyFrame))
			{
				this.destRect.Y = baseSprite.destRect.Y - 1;
			}
			else
			{
				if (baseSprite.currentBodyFrame == 0)
				{
					this.destRect.Y = baseSprite.destRect.Y + 3;
				}
				else
				{
					this.destRect.Y = baseSprite.destRect.Y;
				}
			}

			if (this.spriteWidth > this.baseSprite.spriteWidth)
			{
				if (mirrorX == false) this.destRect.X = this.baseSprite.destRect.X;
				else this.destRect.X = this.baseSprite.destRect.X - (this.spriteWidth - this.baseSprite.spriteWidth);
			}
		}
	}
}
