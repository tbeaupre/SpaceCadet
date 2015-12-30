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
	public class Door: MapAsset
	{
		int level;
		public bool isLeft;

		public Door(double worldX, double worldY, Texture2D texture,Texture2D hitbox, Vector2 mapCoordinates, int level, bool isLeft)
			: base(worldX, worldY, texture, mapCoordinates, 1, 0, !isLeft)
		//	: base(worldX, worldY, texture, new Vector2((float)worldX - mapCoordinates.X, (float)worldY - mapCoordinates.Y), 2, 0, false, hitbox)
		{
			this.hitbox = hitbox;
			this.level = level;
			this.isLeft = isLeft;
		}

		public virtual void UpdateDoor(Map map)
		{
			this.onScreen = IsOnScreen(map);
            UpdateCoords(map);
		}
	}
}
