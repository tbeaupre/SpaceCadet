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
    public interface IProjectileData
    {
        Texture2D GetTexture();
        int GetNumFrames();
        int GetLifeSpan();
        int GetDamage();
        double GetXVel(Game1.Directions dir);
        double GetYVel(Game1.Directions dir);
        double GetYAcc();
        Projectile CreateProjectile(Game1.Directions direction, ISprite origin, Vector2 mapCoords, double worldX, double worldY, bool mirrorX);
        void UpdateProjectile(Projectile projectile);
        void DestroyProjectile();
    }
}
