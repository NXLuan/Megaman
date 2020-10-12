using GameMegaman.Effect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMegaman.GameObject
{
    class FinalBoss : Human
    {


        private Animation idleforward, idleback;
        private Animation shootingforward, shootingback;
        private Animation slideforward, slideback;

        private long startTimeForAttacked;

        private Hashtable timeAttack = new Hashtable();
        private string[] attackType = new string[4];
        private int attackIndex = 0;
        private long lastAttackTime;

        public FinalBoss(float x, float y, GameWorld gameWorld) : base(x, y, 110, 150, 0.1f, 370, gameWorld)
        {
            idleback = CacheDataLoader.getInstance().getAnimation("boss_idle");
            idleforward = CacheDataLoader.getInstance().getAnimation("boss_idle");
            idleforward.flipAllImage();

            shootingback = CacheDataLoader.getInstance().getAnimation("boss_shooting");
            shootingforward = CacheDataLoader.getInstance().getAnimation("boss_shooting");
            shootingforward.flipAllImage();

            slideback = CacheDataLoader.getInstance().getAnimation("boss_slide");
            slideforward = CacheDataLoader.getInstance().getAnimation("boss_slide");
            slideforward.flipAllImage();

            setTimeForNoBehurt(500 * 1000000);
            setDamage(10);

            attackType[0] = "NONE";
            attackType[1] = "shooting";
            attackType[2] = "NONE";
            attackType[3] = "slide";

            timeAttack.Add("NONE", 2000);
            timeAttack.Add("shooting", 500);
            timeAttack.Add("slide", 5000);

        }

        public override void Update()
        {
            base.Update();

            if (getGameWorld().megaman.getPosX() > getPosX())
                setDirection(RIGHT_DIR);
            else setDirection(LEFT_DIR);

            if (startTimeForAttacked == 0)
                startTimeForAttacked = CurrentTimeMillis();
            else if (CurrentTimeMillis() - startTimeForAttacked > 1000)
            {
                attack();
                startTimeForAttacked = CurrentTimeMillis();
            }

            if (!attackType[attackIndex].Equals("NONE"))
            {
                if (attackType[attackIndex].Equals("shooting"))
                {
                    Bullet bullet = new RocketBullet(getPosX(), getPosY() - 50, getGameWorld());
                    if (getDirection() == LEFT_DIR) bullet.setSpeedX(-10);
                    else bullet.setSpeedX(10);
                    bullet.setTeamType(getTeamType());
                    getGameWorld().bulletManager.addObject(bullet);
                }
                else if (attackType[attackIndex].Equals("slide"))
                {

                    if (getGameWorld().physicalMap.haveCollisionWithLeftWall(getBoundForCollisionWithMap()).Width != 0)
                        setSpeedX(20);
                    if (getGameWorld().physicalMap.haveCollisionWithRightWall(getBoundForCollisionWithMap()).Width != 0)
                        setSpeedX(-20);


                    setPosX(getPosX() + getSpeedX());
                }
            }
            else
            {
                // stop attack
                setSpeedX(0);
            }

        }

        public override void run()
        {

        }

        public override void jump()
        {
            setSpeedY(-10);
        }

        public override void dick()
        {


        }

        public override void standUp()
        {


        }

        public override void stopRun()
        {


        }

        public override void attack()
        {
            if (CurrentTimeMillis() - lastAttackTime > (int)timeAttack[attackType[attackIndex]])
            {
                lastAttackTime = CurrentTimeMillis(); 

                attackIndex++;
                if (attackIndex >= attackType.GetLength(0)) attackIndex = 0;

                if (attackType[attackIndex].Equals("slide"))
                {
                    if (getPosX() < getGameWorld().megaman.getPosX()) setSpeedX(20);
                    else setSpeedX(-20);
                }
            }

        }

        public override Rectangle getBoundForCollisionWithEnemy()
        {
            if (attackType[attackIndex].Equals("slide"))
            {
                Rectangle rect = getBoundForCollisionWithMap();
                rect.Y += 100;
                rect.Height -= 100;
                return rect;
            }
            else
                return getBoundForCollisionWithMap();
        }

        public override void draw(Graphics g)
        {

            if (getState() == NOBEHURT && (nanoTime() / 10000000) % 2 != 1)
            {
            }
            else
            {

                if (attackType[attackIndex].Equals("NONE"))
                {
                    if (getDirection() == RIGHT_DIR)
                    {
                        idleforward.Update(nanoTime());
                        idleforward.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
                    }
                    else
                    {
                        idleback.Update(nanoTime());
                        idleback.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
                    }
                }
                else if (attackType[attackIndex].Equals("shooting"))
                {

                    if (getDirection() == RIGHT_DIR)
                    {
                        shootingforward.Update(nanoTime());
                        shootingforward.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
                    }
                    else
                    {
                        shootingback.Update(nanoTime());
                        shootingback.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
                    }

                }
                else if (attackType[attackIndex].Equals("slide"))
                {
                    if (getSpeedX() > 0)
                    {
                        slideforward.Update(nanoTime());
                        slideforward.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY() + 50, g);
                    }
                    else
                    {
                        slideback.Update(nanoTime());
                        slideback.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY() + 50, g);
                    }
                }
            }
            // drawBoundForCollisionWithEnemy(g2);
        }
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
