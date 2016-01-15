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
	public class Sprite : ISprite
	{
		public Texture2D texture;
		public Rectangle sourceRect;
		public DRectangle destRect;
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
			this.destRect = new DRectangle(destCoords.X, destCoords.Y, this.spriteWidth, this.spriteHeight);
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

		public Texture2D MirrorTexture(ISprite sprite, GraphicsDeviceManager graphics, Texture2D texture)
		{
			Texture2D mirroredProjectile = new Texture2D(graphics.GraphicsDevice, sprite.GetSpriteWidth(), sprite.GetSpriteHeight());
			Color[] projectileTextureHelper = new Color[sprite.GetSpriteWidth() * sprite.GetSpriteHeight()];
			Color[] newTextureData = new Color[sprite.GetSpriteWidth() * sprite.GetSpriteHeight()];
			texture.GetData<Color>(
				0, sprite.GetSourceRect(), projectileTextureHelper, 0, sprite.GetSpriteWidth() * sprite.GetSpriteHeight());

			for (int x = 0; x < sprite.GetSpriteWidth(); x++)
			{
				for (int y = 0; y < sprite.GetSpriteHeight(); y++)
				{
					newTextureData[y * sprite.GetSpriteWidth() + x] = projectileTextureHelper[(y + 1) * sprite.GetSpriteWidth() - 1 - x];
					newTextureData[(y + 1) * sprite.GetSpriteWidth() - 1 - x] = projectileTextureHelper[y * sprite.GetSpriteWidth() + x];
				}
			}

			mirroredProjectile.SetData<Color>(0, new Rectangle(0, 0, sprite.GetSpriteWidth(), sprite.GetSpriteHeight()), newTextureData, 0, sprite.GetSpriteWidth() * sprite.GetSpriteHeight());
			return mirroredProjectile;
		}

        public int GetHitDuration()
        {
            return HIT_DURATION;
        }

        public Texture2D GetTexture()
        {
            return this.texture;
        }

        public Status GetStatus()
        {
            return this.status;
        }

        public bool GetMirrorX()
        {
            return this.mirrorX;
        }

        public DRectangle GetDestRect()
        {
            return this.destRect;
        }

        public Rectangle GetSourceRect()
        {
            return this.sourceRect;
        }

        public int GetSpriteWidth()
        {
            return this.spriteWidth;
        }

        public int GetSpriteHeight()
        {
            return this.spriteHeight;
        }

        public int GetFrameNum()
        {
            return this.frameNum;
        }

        public void SetFrameNum(int frameNum)
        {
            this.frameNum = frameNum;
        }

        public virtual Texture2D GetHitbox()
        {
            return this.texture;
        }

        public virtual Texture2D GetVulnerable()
        {
            return this.texture;
        }
    }
}
