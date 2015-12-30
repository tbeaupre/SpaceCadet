using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spaceman
{
	public class HealthPickupData
	{
		public int x;
		public int y;
		public int level;

		public HealthPickupData(int x, int y, int level)
		{
			this.x = x;
			this.y = y;
			this.level = level;
		}
	}
}
