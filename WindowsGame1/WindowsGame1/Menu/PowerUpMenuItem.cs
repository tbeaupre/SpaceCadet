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
    class PowerUpMenuItem : ActionMenuItem
    {
        bool selected;
        PowerUps powerUp;

        public PowerUpMenuItem(Texture2D texture, String function, PowerUps powerUp)
                :base (texture, null, function)
        {
            this.powerUp = powerUp;
        }

        public override Rectangle GetSourceRect()
        {
            return new Rectangle((this.selected? GetTexture().Width / 2: 0), (GetIsHighlighted() ? GetTexture().Height / 2 : 0), GetTexture().Width / 2, GetTexture().Height / 2);
        }

        public void SetSelected(bool val)
        {
            this.selected = val;
        }

        public override void ActivateItem(Game1 game, IMenu from)
        {
            this.selected = true;
            game.callMenuFunction(GetFunction(), from);
            PowerUpMenu menu = (PowerUpMenu)from;
            game.GetPowerUpManager().UpdateAbility(powerUp, menu.GetIndex());
            game.UpdateAttributes(game.GetPowerUpManager().currentPowerUps);
        }
    }
}
