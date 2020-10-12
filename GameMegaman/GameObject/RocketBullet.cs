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
    class RocketBullet : Bullet
    {
        private Animation forwardBulletAnimUp, forwardBulletAnimDown, forwardBulletAnim;
        private Animation backBulletAnimUp, backBulletAnimDown, backBulletAnim;

        private long startTimeForChangeSpeedY;

        public RocketBullet(float x, float y, GameWorld gameWorld) : base(x, y, 30, 30, 1.0f, 10, gameWorld)
        {
            backBulletAnimUp = CacheDataLoader.getInstance().getAnimation("rocketUp");
            backBulletAnimDown = CacheDataLoader.getInstance().getAnimation("rocketDown");
            backBulletAnim = CacheDataLoader.getInstance().getAnimation("rocket");

            forwardBulletAnimUp = CacheDataLoader.getInstance().getAnimation("rocketUp");
            forwardBulletAnimUp.flipAllImage();
            forwardBulletAnimDown = CacheDataLoader.getInstance().getAnimation("rocketDown");
            forwardBulletAnimDown.flipAllImage();
            forwardBulletAnim = CacheDataLoader.getInstance().getAnimation("rocket");
            forwardBulletAnim.flipAllImage();
        }

        public override Rectangle getBoundForCollisionWithEnemy()
        {
            return getBoundForCollisionWithMap();
        }

        public override void draw(Graphics g)
        {
            if (getSpeedX() > 0)
            {
                if (getSpeedY() > 0)
                {
                    forwardBulletAnimDown.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
                }
                else if (getSpeedY() < 0)
                {
                    forwardBulletAnimUp.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
                }
                else
                    forwardBulletAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
            }
            else
            {
                if (getSpeedY() > 0)
                {
                    backBulletAnimDown.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
                }
                else if (getSpeedY() < 0)
                {
                    backBulletAnimUp.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
                }
                else
                    backBulletAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
            }
            //drawBoundForCollisionWithEnemy(g);
        }

        private void changeSpeedY()
        {
            if (CurrentTimeMillis() % 3 == 0)
            {
                setSpeedY(getSpeedX());
            }
            else if (CurrentTimeMillis() % 3 == 1)
            {
                setSpeedY(-getSpeedX());
            }
            else
            {
                setSpeedY(0);

            }
        }

        public override void Update()
        {
            base.Update();

            if (nanoTime() - startTimeForChangeSpeedY > 500 * 1000000)
            {
                startTimeForChangeSpeedY = nanoTime();
                changeSpeedY();
            }
        }

        public override void attack() { }

        private static long nanoTime()
        {
            double timestamp = Stopwatch.GetTimestamp();
            double nanoseconds = 1_000_000_000.0 * timestamp / Stopwatch.Frequency;

            return (long)nanoseconds;
        }
        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }
    }
}
