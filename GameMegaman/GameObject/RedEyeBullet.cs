using GameMegaman.Effect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMegaman.GameObject
{
    class RedEyeBullet : Bullet
    {
        private Animation forwardBulletAnim, backBulletAnim;

        public RedEyeBullet(float x, float y, GameWorld gameWorld) : base(x, y, 30, 30, 1.0f, 10, gameWorld)
        {
            forwardBulletAnim = CacheDataLoader.getInstance().getAnimation("redeyebullet");
            backBulletAnim = CacheDataLoader.getInstance().getAnimation("redeyebullet");
            backBulletAnim.flipAllImage();
        }

        public override Rectangle getBoundForCollisionWithEnemy()
        {
            return getBoundForCollisionWithMap();
        }

        public override void draw(Graphics g)
        {
            if (getSpeedX() > 0)
            {
                forwardBulletAnim.Update(nanoTime());
                forwardBulletAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
            }
            else
            {
                backBulletAnim.Update(nanoTime());
                backBulletAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
            }
            //drawBoundForCollisionWithEnemy(g2);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void attack() { }

        private static long nanoTime()
        {
            double timestamp = Stopwatch.GetTimestamp();
            double nanoseconds = 1_000_000_000.0 * timestamp / Stopwatch.Frequency;

            return (long)nanoseconds;
        }
    }
}
