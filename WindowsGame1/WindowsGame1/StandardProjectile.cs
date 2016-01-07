using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spaceman
{
    class StandardProjectile// : IProjectileData
    {
        public double xVel;
        public double yVel;
        public double yAcc;
        public int lifeSpan;
        public int damage;

        public StandardProjectile(double xVel, double yVel, double yAcc, int lifeSpan, int damage)

        {
            this.xVel = xVel;
            this.yVel = yVel;
            this.yAcc = yAcc;
            this.lifeSpan = lifeSpan;
            this.damage = damage;
        }
        
        Projectile CreateProjectile()
        {
            return null;
        }
        void UpdateProjectile()
        {

        }
        void DestroyProjectile()
        {

        }
        
    }
}
