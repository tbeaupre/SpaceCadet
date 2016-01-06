using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spaceman
{
    interface IProjectileData
    {
        Projectile CreateProjectile();
        void UpdateProjectile();
        void DestroyProjectile();
    }
}
