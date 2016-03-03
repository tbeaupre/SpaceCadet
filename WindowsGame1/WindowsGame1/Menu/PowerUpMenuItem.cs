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
        bool abilityUsed;

        public PowerUpMenuItem(Texture2D texture, String function, PowerUps powerUp)
                :base (texture, null, function)
        {
            this.powerUp = powerUp;
            this.abilityUsed = false;
        }

        public void SetAbilityUsed(bool val)
        {
            this.abilityUsed = val;
        }

        public void SetIsSelected(bool val)
        {
            this.selected = val;
        }

        public bool GetIsSelected()
        {
            return this.selected;
        }

        public PowerUps GetPowerUp()
        {
            return this.powerUp;
        }

        public override Rectangle GetSourceRect()
        {
            int sourceX = 0;
            int sourceY = 0;
            if (powerUp != PowerUps.NULL)
            {
                if (powerUp == PowerUps.None)
                {
                    if (selected) sourceX = GetTexture().Width / 2;
                }
                else
                {
                    if (selected) sourceX = GetTexture().Width / 3;
                    else
                    {
                        if (abilityUsed) sourceX = 2 * GetTexture().Width / 3;
                        else sourceX = 0;
                    }
                }
            }
            if (GetIsHighlighted()) sourceY = GetTexture().Height / 2;
            if (powerUp == PowerUps.NULL || powerUp == PowerUps.None) return new Rectangle(sourceX, sourceY, GetTexture().Width / 2, GetTexture().Height / 2);
            return new Rectangle(sourceX, sourceY, GetTexture().Width / 3, GetTexture().Height / 2);
        }

        public override void ActivateItem(Game1 game, IMenu from)
        {
            if (!game.GetPowerUpManager().AbilityUsed(powerUp) && !selected && game.GetPowerUpManager().GetUnlockedPowerUps().Contains(powerUp))
            {
                this.selected = true;
                game.callMenuFunction(GetFunction(), from);
                PowerUpMenu menu = (PowerUpMenu)from;
                game.GetPowerUpManager().UpdateAbility(powerUp, menu.GetIndex());
                game.UpdateAttributes(game.GetPowerUpManager().currentPowerUps);
            }
        }
    }
}
