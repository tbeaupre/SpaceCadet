using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spaceman
{
	class PowerUpManager
	{
		public List<PowerUps> unlockedPowerUps = new List<PowerUps>();
		public List<PowerUps> currentPowerUps = new List<PowerUps>();

		public PowerUpManager() { }
		public PowerUpManager(List<PowerUps> unlocked, List<PowerUps> current)
		{
			this.unlockedPowerUps = unlocked;
			this.currentPowerUps = current;
		}

		public void UpdateAbilities (PowerUps pu1, PowerUps pu2, PowerUps pu3)
		{
			currentPowerUps.Clear();
			if (pu1 != PowerUps.NULL && unlockedPowerUps.Contains(pu1))
				currentPowerUps.Add(pu1);
			if (pu2 != PowerUps.NULL && unlockedPowerUps.Contains(pu2))
				currentPowerUps.Add(pu2);
			if (pu3 != PowerUps.NULL && unlockedPowerUps.Contains(pu3))
				currentPowerUps.Add(pu3);
		}

		public void UnlockPowerUp(PowerUps pu)
		{
			if (!unlockedPowerUps.Contains(pu) && pu != PowerUps.NULL)
				unlockedPowerUps.Add(pu);
		}

		public List<PowerUps> GetUnlockedPowerUps()
		{
			return this.unlockedPowerUps;
		}

		public List<PowerUps> GetCurrentPowerUps()
		{
			return this.currentPowerUps;
		}
	}
}
