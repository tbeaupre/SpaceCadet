using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spaceman
{
	class PowerUpManager
	{
		public List<Game1.PowerUps> unlockedPowerUps = new List<Game1.PowerUps>();
		public List<Game1.PowerUps> currentPowerUps = new List<Game1.PowerUps>();

		public PowerUpManager() { }
		public PowerUpManager(List<Game1.PowerUps> unlocked, List<Game1.PowerUps> current)
		{
			this.unlockedPowerUps = unlocked;
			this.currentPowerUps = current;
		}

		public void UpdateAbilities (Game1.PowerUps pu1, Game1.PowerUps pu2, Game1.PowerUps pu3)
		{
			currentPowerUps.Clear();
			if (pu1 != Game1.PowerUps.NULL && unlockedPowerUps.Contains(pu1))
				currentPowerUps.Add(pu1);
			if (pu2 != Game1.PowerUps.NULL && unlockedPowerUps.Contains(pu2))
				currentPowerUps.Add(pu2);
			if (pu3 != Game1.PowerUps.NULL && unlockedPowerUps.Contains(pu3))
				currentPowerUps.Add(pu3);
		}

		public void UnlockPowerUp(Game1.PowerUps pu)
		{
			if (!unlockedPowerUps.Contains(pu) && pu != Game1.PowerUps.NULL)
				unlockedPowerUps.Add(pu);
		}

		public List<Game1.PowerUps> GetUnlockedPowerUps()
		{
			return this.unlockedPowerUps;
		}

		public List<Game1.PowerUps> GetCurrentPowerUps()
		{
			return this.currentPowerUps;
		}
	}
}
