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
    public class BoostJumpPickUp : PickUp
    {
        public BoostJumpPickUp(Vector2 mapCoordinates, double worldX, double worldY, Texture2D texture)
			: base(0, worldX, worldY, texture, new Vector2((float)worldX - mapCoordinates.X, (float)worldY - mapCoordinates.Y), 1, 0, 1, false, null) { }

        public override void PickUpObj(Game1 game)
        {
            game.UnlockPowerUp(PowerUps.BoostJump);
        }
    }
}
