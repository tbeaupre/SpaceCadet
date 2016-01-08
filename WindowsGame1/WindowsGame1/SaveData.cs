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
	public class SaveData
	{
		// Map Resources
		public int mapIndex;

		//Power Ups information.
		public List<PowerUps> unlockedPowerUps;
		public List<PowerUps> currentPowerUps;

		// For Figuring out which guns the player owns
		public List<Guns> guns;
		public int currentGun;

		// Coordinates
		public Vector2 coordinates;

		public SaveData(int mapIndex, List<PowerUps> unlockedPowerUps, List<PowerUps> currentPowerUps, List<Guns> guns, int currentGun, Vector2 coordinates)
		{
			this.mapIndex = mapIndex;
			this.unlockedPowerUps = unlockedPowerUps;
			this.currentPowerUps = currentPowerUps;
			this.guns = guns;
			this.currentGun = currentGun;
			this.coordinates = coordinates;
		}

		public SaveData() { }
	}
}
