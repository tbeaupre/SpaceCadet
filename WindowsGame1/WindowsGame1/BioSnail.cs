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
	class BioSnail : Enemy
	{
		public BioSnail(Spawn spawn, Vector2 mapCoordinates, double worldX, double worldY, EnemyTextureSet texture, ProjectileData projectileData)
			: base(spawn, projectileData,100,20,worldX, worldY, texture.vulnerable,texture.hitBox,texture.overlay, new Vector2((float)worldX - mapCoordinates.X, (float)worldY - mapCoordinates.Y), 14, 0, 1, false, null) 
			{
				this.FRAME_OFFSET = 6;
				this.die = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
				this.attack = new int[] { 2, 2, 0, 1, 1, 999, 2, 0};
			}
	}
}
