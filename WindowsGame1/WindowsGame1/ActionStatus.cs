using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spaceman
{
	public class ActionStatus
	{
		public ActionStates state;
		public int duration;

		public ActionStatus(ActionStates state, int duration)
		{
			this.state = state;
			this.duration = duration;
		}
	}
}
