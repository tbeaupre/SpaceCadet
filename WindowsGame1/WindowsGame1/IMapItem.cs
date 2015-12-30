using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spaceman
{
	public interface IMapItem
	{
		void UpdateSprite(Game1 game);

		double GetX();

		double GetY();

		Boolean GetIsOnScreen();

		String GetType();
	}
}
