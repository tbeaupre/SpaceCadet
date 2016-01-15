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
	public class BoostJump : CharOverlay
	{
		private int xOffset;
		private int yOffset;
		private Texture2D texture;
		private int numFrames = 7;
		private int spriteWidth;
		private int currentFrame = 0;
		private int timer = 0;
		private const int FRAME_OFFSET = 3;
		private bool complete = false;

		public BoostJump(Texture2D texture)
		{
			this.xOffset = -2;
			this.yOffset = 13;
			this.texture = texture;
			this.spriteWidth = texture.Width / numFrames;
		}

		public void incrementTimer()
		{
			if (!complete)
			{
				this.timer++;
				if (this.timer == FRAME_OFFSET)
				{
					this.timer = 0;
					this.currentFrame++;
					if (this.currentFrame >= numFrames - 1)
					{
						this.currentFrame = 0;
						complete = true;
					}
				}
			}
		}

		public int getSpacemanFrame()
		{
			if (complete) return numFrames + 3;
			if (currentFrame == numFrames) return numFrames + 3;
			else return currentFrame + 5;
		}

		public int getCurrentFrame()
		{
			return this.currentFrame;
		}

		public int getXOffset()
		{
			return this.xOffset;
		}

		public int getYOffset()
		{
			return this.yOffset;
		}

		public Texture2D getTexture()
		{
			return this.texture;
		}

		public Rectangle getDestRect(Game1 game)
		{
			return new Rectangle(game.player.destRect.X, game.player.destRect.Y, spriteWidth, texture.Height);
		}

		public Rectangle getSourceRect(Game1 game)
		{
			return new Rectangle(spriteWidth * currentFrame, 0, spriteWidth, texture.Height);
		}

		public bool getMirrorX(Game1 game)
		{
			return game.player.mirrorX;
		}

		public void reset()
		{
			this.currentFrame = 0;
			this.timer = 0;
			this.complete = false;
		}
	}
}
