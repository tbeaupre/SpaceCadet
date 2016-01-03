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
	public class MapAsset : Object
	{
		public MapAsset(double worldX, double worldY, Texture2D texture, Vector2 mapCoordinates, int numFrames, int frameNum, bool mirrorX)
			: base(worldX, worldY, texture, new Vector2((float)worldX - mapCoordinates.X, (float)worldY - mapCoordinates.Y), numFrames, frameNum, mirrorX)
		{
			this.sourceRect = new Rectangle(this.spriteWidth * this.frameNum, 0, this.texture.Width / numFrames, this.texture.Height);
		}

		public Spaceman offsetSpaceMan(Spaceman s, int xOffset, int yOffset)
		{
			Spaceman result = new Spaceman(s.body, s.head,new Vector2(s.destRect.X, s.destRect.Y),s.numFrames, s.frameNum, s.mirrorX);
			result.destRect.X += xOffset;
			result.destRect.Y += yOffset;
			return result;
		}

		public void UpdateSprite(Game1 game)
		{
			if (status.state.Equals("hit"))
			{
				if (status.duration > 0) status.duration--;
			}
			this.onScreen = IsOnScreen(game.worldMap[game.currentRoom]);
			this.nearScreen = IsNearScreen(game.worldMap[game.currentRoom].mapCoordinates);
			UpdateCoords(game.worldMap[game.currentRoom]);
		}

	}
}
