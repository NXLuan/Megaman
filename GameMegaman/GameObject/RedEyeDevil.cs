using GameMegaman.Effect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMPLib;

namespace GameMegaman.GameObject
{
    class RedEyeDevil : ParticularObject
    {
        private Animation forwardAnim, backAnim;

        private long startTimeToShoot;

        WindowsMediaPlayer shooting;

        public RedEyeDevil(float x, float y, GameWorld gameWorld) : base(x, y, 127, 89, 0, 100, gameWorld)
        {
            backAnim = CacheDataLoader.getInstance().getAnimation("redeye");
            forwardAnim = CacheDataLoader.getInstance().getAnimation("redeye");
            forwardAnim.flipAllImage();
            startTimeToShoot = 0;
            setDamage(10);
            setTimeForNoBehurt(300000000);
            shooting = CacheDataLoader.getInstance().getSound("redeyeshooting");
        }

        public override void attack()
        {
            shooting.controls.play();
            Bullet bullet = new RedEyeBullet(getPosX(), getPosY(), getGameWorld());
            if (getDirection() == LEFT_DIR) bullet.setSpeedX(-8);
            else bullet.setSpeedX(8);
            bullet.setTeamType(getTeamType());
            getGameWorld().bulletManager.addObject(bullet);
        }


        public override void Update()
        {
            base.Update();
            if (nanoTime() - startTimeToShoot > 1000 * 1000000)
            {
                attack();
                startTimeToShoot = nanoTime();
            }
        }
        public override Rectangle getBoundForCollisionWithEnemy()
        {
            Rectangle rect = getBoundForCollisionWithMap();
            rect.X += 20;
            rect.Width -= 40;

            return rect;
        }

        public override void draw(Graphics g)
        {
            if (!isObjectOutOfCameraView())
            {
                if (getState() == NOBEHURT && (nanoTime() / 10000000) % 2 != 1)
                {
                    // plash...
                }
                else
                {
                    if (getDirection() == LEFT_DIR)
                    {
                        backAnim.Update(nanoTime());
                        backAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()),
                                (int)(getPosY() - getGameWorld().camera.getPosY()), g);
                    }
                    else
                    {
                        forwardAnim.Update(nanoTime());
                        forwardAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()),
                                (int)(getPosY() - getGameWorld().camera.getPosY()), g);
                    }
                }
            }
            //drawBoundForCollisionWithEnemy(g2);
        }
        private static long nanoTime()
        {
            double timestamp = Stopwatch.GetTimestamp();
            double nanoseconds = 1_000_000_000.0 * timestamp / Stopwatch.Frequency;

            return (long)nanoseconds;
        }
    }
}
