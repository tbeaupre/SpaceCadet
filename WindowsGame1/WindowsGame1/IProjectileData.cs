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
        double GetXVel();
        double GetYVel();
        double GetYAcc();
        Game1.Directions GetDirection();
        Projectile CreateProjectile();
        void UpdateProjectile();
        void DestroyProjectile();
    }
}
