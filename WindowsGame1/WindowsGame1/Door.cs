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
        Texture2D vulnerable;

		public Door(double worldX, double worldY, Texture2D texture,Texture2D vulnerable, Vector2 mapCoordinates, int level, bool isLeft)
			: base(worldX, worldY, texture, mapCoordinates, 1, 0, !isLeft)
		{
			this.level = level;
			this.isLeft = isLeft;
            this.vulnerable = vulnerable;
        }

        public override Texture2D GetHitbox()
        {
            return this.texture;
        }

        public override Texture2D GetVulnerable()
        {
            return this.vulnerable;
        }

		public virtual void UpdateDoor(Map map)
		{
			this.onScreen = IsOnScreen(map);
            UpdateCoords(map);
		}
	}
}
