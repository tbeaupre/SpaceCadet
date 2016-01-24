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
	public abstract class PickUp : Object
	{
		public int idleTimer;
		public PickUp(int idleTimer, double worldX, double worldY, Texture2D texture, Vector2 destCoords, int numFrames, int frameNum, int sizeMultiplier, bool mirrorX, Nullable<int> cycleStart)
			: base(worldX, worldY, texture, destCoords, numFrames, frameNum, mirrorX)
		{
			this.idleTimer = idleTimer;
		}

		public override void UpdateSprite(Map map)
		{
			this.onScreen = IsOnScreen(map);
			this.sourceRect = new Rectangle(this.spriteWidth * this.frameNum, 0, this.texture.Width / numFrames, this.texture.Height);

			if (onScreen)
			{
                UpdateCoords(map);
				this.NextFrame(idleTimer);
			}
		}

		public virtual void PickUpObj(Game1 game)
		{
		}
	}
}
