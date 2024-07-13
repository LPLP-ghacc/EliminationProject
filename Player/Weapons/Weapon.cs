using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Player.Weapons
{
    public class Weapon
    {
        public int BulletMaxCount { get; set; }
        public float FireCycle { get; set; }
        public float newBulletTime = 0;
        public bool canShoot { get; private set; }
        private int actualBulletCount;

        public Weapon(int bulletMaxCount, float fireCycle, int actualBulletCount)
        {
            BulletMaxCount = bulletMaxCount;
            FireCycle = fireCycle;

            this.actualBulletCount = actualBulletCount < BulletMaxCount ? actualBulletCount : BulletMaxCount;

            if (actualBulletCount > 0)
                canShoot = true;

            if (actualBulletCount == 0)
                canShoot = false;
        }

        public int GetActualBullets(Action onGetAction)
        {
            onGetAction?.Invoke();

            return actualBulletCount;
        }

        public void Shoot(Action onShootAction)
        {
            if(newBulletTime <= 0)
            {
                if(actualBulletCount > 0)
                {
                    onShootAction?.Invoke();
                    actualBulletCount--;
                    if(actualBulletCount == 0)
                    {
                        canShoot = false;
                    }
                }
            }
        }

        public void Reload()
        {
            actualBulletCount = BulletMaxCount;
        }
    }
}
