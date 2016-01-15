using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spaceman
{
	public class Portal
	{
		Map side1;
		Map side2;
		public Door door1;
		public Door door2;

		public Portal(Map side1, Map side2, Door door1, Door door2)
		{
			this.side1 = side1;
			this.side2 = side2;
			this.door1 = door1;
			this.door2 = door2;
		}

		public void Initialize()
		{
			side1.AddPortal(this);
			side2.AddPortal(this);
		}

		public void UpdatePortal(Game1 game)
		{
			if (side1.active)
			{
				door1.UpdateDoor(game.worldMap[game.currentRoom]);
                CollisionState result = CollisionDetector.PerPixelSprite(door1, game.player, game.graphics);
                if (result == CollisionState.Hurtbox)
				{
                    if (!side1.GetWasJustActivated())
                    {
                        game.RemoveObjectToDraw(door1);
                        game.ActivateMap(side2, door2);
                    }
				}
				else
				{
					if (door1.onScreen)
					{
						game.AddObjectToDraw(door1);
					}
					else
					{
						game.RemoveObjectToDraw(door1);
					}
				}
			}
			else if (side2.active)
			{
				door2.UpdateDoor(game.worldMap[game.currentRoom]);
                CollisionState result = CollisionDetector.PerPixelSprite(door2, game.player, game.graphics);
                if (result == CollisionState.Hurtbox)
                {
                    if (!side2.GetWasJustActivated())
                    {
                        game.RemoveObjectToDraw(door2);
                        game.ActivateMap(side1, door1);
                    }
				}
				else
				{
					if (door2.onScreen)
					{
						game.AddObjectToDraw(door2);
					}
					else
					{
						game.RemoveObjectToDraw(door2);
					}
				}
			}
		}
	}
}
