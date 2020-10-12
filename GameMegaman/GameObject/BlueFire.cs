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
    class BlueFire : Bullet
    {
        private Animation forwardBulletAnim, backBulletAnim;

        public BlueFire(float x, float y, GameWorld gameWorld) : base(x, y, 60, 30, 1.0f, 10, gameWorld)
        {
            forwardBulletAnim = CacheDataLoader.getInstance().getAnimation("bluefire");
            backBulletAnim = CacheDataLoader.getInstance().getAnimation("bluefire");
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
                if (!forwardBulletAnim.isIgnoreFrame(0) && forwardBulletAnim.getCurrentFrame() == 3)
                {
                    forwardBulletAnim.setIgnoreFrame(0);
                    forwardBulletAnim.setIgnoreFrame(1);
                    forwardBulletAnim.setIgnoreFrame(2);
                }

                forwardBulletAnim.Update(nanoTime());
                forwardBulletAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
            }
            else
            {
                if (!backBulletAnim.isIgnoreFrame(0) && backBulletAnim.getCurrentFrame() == 3)
                {
                    backBulletAnim.setIgnoreFrame(0);
                    backBulletAnim.setIgnoreFrame(1);
                    backBulletAnim.setIgnoreFrame(2);
                }
                backBulletAnim.Update(nanoTime());
                backBulletAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
            }
            //drawBoundForCollisionWithEnemy(g2);
        }

        public override void Update()
        {
            if (forwardBulletAnim.isIgnoreFrame(0) || backBulletAnim.isIgnoreFrame(0))
                setPosX(getPosX() + getSpeedX());
            ParticularObject Object = getGameWorld().particularObjectManager.getCollisionWidthEnemyObject(this);
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
