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
    class Megaman : Human
    {
        public const int RUNSPEED = 3;

        private Animation runForwardAnim, runBackAnim, runShootingForwarAnim, runShootingBackAnim;
        private Animation idleForwardAnim, idleBackAnim, idleShootingForwardAnim, idleShootingBackAnim;
        private Animation dickForwardAnim, dickBackAnim;
        private Animation flyForwardAnim, flyBackAnim, flyShootingForwardAnim, flyShootingBackAnim;
        private Animation landingForwardAnim, landingBackAnim;

        private Animation climWallForward, climWallBack;

        private long lastShootingTime;
        private bool isShooting = false;

        WindowsMediaPlayer hurtingSound;
        WindowsMediaPlayer shooting1;

        public Megaman(float x, float y, GameWorld gameWorld) : base(x, y, 70, 90, 1, 100, gameWorld)
        {
            shooting1 = CacheDataLoader.getInstance().getSound("bluefireshooting");
            hurtingSound = CacheDataLoader.getInstance().getSound("megamanhurt");

            setTeamType(LEAGUE_TEAM);

            setTimeForNoBehurt(2000 * 1000000);

            runForwardAnim = CacheDataLoader.getInstance().getAnimation("run");
            runBackAnim = CacheDataLoader.getInstance().getAnimation("run");
            runBackAnim.flipAllImage();

            idleForwardAnim = CacheDataLoader.getInstance().getAnimation("idle");
            idleBackAnim = CacheDataLoader.getInstance().getAnimation("idle");
            idleBackAnim.flipAllImage();

            dickForwardAnim = CacheDataLoader.getInstance().getAnimation("dick");
            dickBackAnim = CacheDataLoader.getInstance().getAnimation("dick");
            dickBackAnim.flipAllImage();

            flyForwardAnim = CacheDataLoader.getInstance().getAnimation("flyingup");
            flyForwardAnim.setIsRepeated(false);
            flyBackAnim = CacheDataLoader.getInstance().getAnimation("flyingup");
            flyBackAnim.setIsRepeated(false);
            flyBackAnim.flipAllImage();

            landingForwardAnim = CacheDataLoader.getInstance().getAnimation("landing");
            landingBackAnim = CacheDataLoader.getInstance().getAnimation("landing");
            landingBackAnim.flipAllImage();

            climWallBack = CacheDataLoader.getInstance().getAnimation("clim_wall");
            climWallForward = CacheDataLoader.getInstance().getAnimation("clim_wall");
            climWallForward.flipAllImage();

            behurtForwardAnim = CacheDataLoader.getInstance().getAnimation("hurting");
            behurtBackAnim = CacheDataLoader.getInstance().getAnimation("hurting");
            behurtBackAnim.flipAllImage();

            idleShootingForwardAnim = CacheDataLoader.getInstance().getAnimation("idleshoot");
            idleShootingBackAnim = CacheDataLoader.getInstance().getAnimation("idleshoot");
            idleShootingBackAnim.flipAllImage();

            runShootingForwarAnim = CacheDataLoader.getInstance().getAnimation("runshoot");
            runShootingBackAnim = CacheDataLoader.getInstance().getAnimation("runshoot");
            runShootingBackAnim.flipAllImage();

            flyShootingForwardAnim = CacheDataLoader.getInstance().getAnimation("flyingupshoot");
            flyShootingBackAnim = CacheDataLoader.getInstance().getAnimation("flyingupshoot");
            flyShootingBackAnim.flipAllImage();

        }
        public override void Update()
        {
            base.Update();

            if (isShooting)
            {
                if (nanoTime() - lastShootingTime > 500 * 1000000)
                {
                    isShooting = false;
                }
            }

            if (getIsLanding())
            {
                landingBackAnim.Update(nanoTime());
                if (landingBackAnim.isLastFrame())
                {
                    setIsLanding(false);
                    landingBackAnim.reset();
                    runForwardAnim.reset();
                    runBackAnim.reset();
                }
            }

        }
        public override Rectangle getBoundForCollisionWithEnemy()
        {
            Rectangle rect = getBoundForCollisionWithMap();

            if (getIsDicking())
            {
                rect.X = (int)getPosX() - 22;
                rect.Y = (int)getPosY() - 20;
                rect.Width = 44;
                rect.Height = 65;
            }
            else
            {
                rect.X = (int)getPosX() - 22;
                rect.Y = (int)getPosY() - 40;
                rect.Width = 44;
                rect.Height = 80;
            }
            return rect;
        }

        public override void draw(Graphics g)
        {
            switch (getState())
            {
                case ALIVE:
                case NOBEHURT:
                    if (getState() == NOBEHURT && (nanoTime() / 10000000) % 2 != 1)
                    {
                        //Console.WriteLine("Plash...");
                    }
                    else
                    {

                        if (getIsLanding())
                        {

                            if (getDirection() == RIGHT_DIR)
                            {
                                landingForwardAnim.setCurrentFrame(landingBackAnim.getCurrentFrame());
                                landingForwardAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()),
                                        (int)getPosY() - (int)getGameWorld().camera.getPosY() + (getBoundForCollisionWithMap().Height / 2 - landingForwardAnim.getCurrentImage().Height / 2),
                                        g);
                            }
                            else
                            {
                                landingBackAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()),
                                        (int)getPosY() - (int)getGameWorld().camera.getPosY() + (getBoundForCollisionWithMap().Height / 2 - landingBackAnim.getCurrentImage().Height / 2),
                                        g);
                            }

                        }
                        else if (getIsJumping())
                        {

                            if (getDirection() == RIGHT_DIR)
                            {
                                flyForwardAnim.Update(nanoTime());
                                if (isShooting)
                                {
                                    flyShootingForwardAnim.setCurrentFrame(flyForwardAnim.getCurrentFrame());
                                    flyShootingForwardAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()) + 10, (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
                                }
                                else
                                    flyForwardAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
                            }
                            else
                            {
                                flyBackAnim.Update(nanoTime());
                                if (isShooting)
                                {
                                    flyShootingBackAnim.setCurrentFrame(flyBackAnim.getCurrentFrame());
                                    flyShootingBackAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()) - 10, (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
                                }
                                else
                                    flyBackAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
                            }

                        }
                        else if (getIsDicking())
                        {

                            if (getDirection() == RIGHT_DIR)
                            {
                                dickForwardAnim.Update(nanoTime());
                                dickForwardAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()),
                                        (int)getPosY() - (int)getGameWorld().camera.getPosY() + (getBoundForCollisionWithMap().Height / 2 - dickForwardAnim.getCurrentImage().Height / 2),
                                        g);
                            }
                            else
                            {
                                dickBackAnim.Update(nanoTime());
                                dickBackAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()),
                                        (int)getPosY() - (int)getGameWorld().camera.getPosY() + (getBoundForCollisionWithMap().Height / 2 - dickBackAnim.getCurrentImage().Height / 2),
                                        g);
                            }

                        }
                        else
                        {
                            if (getSpeedX() > 0)
                            {
                                runForwardAnim.Update(nanoTime());
                                if (isShooting)
                                {
                                    runShootingForwarAnim.setCurrentFrame(runForwardAnim.getCurrentFrame() - 1);
                                    runShootingForwarAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
                                }
                                else
                                    runForwardAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
                                if (runForwardAnim.getCurrentFrame() == 1) runForwardAnim.setIgnoreFrame(0);
                            }
                            else if (getSpeedX() < 0)
                            {
                                runBackAnim.Update(nanoTime());
                                if (isShooting)
                                {
                                    runShootingBackAnim.setCurrentFrame(runBackAnim.getCurrentFrame() - 1);
                                    runShootingBackAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
                                }
                                else
                                    runBackAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
                                if (runBackAnim.getCurrentFrame() == 1) runBackAnim.setIgnoreFrame(0);
                            }
                            else
                            {
                                if (getDirection() == RIGHT_DIR)
                                {
                                    if (isShooting)
                                    {
                                        idleShootingForwardAnim.Update(nanoTime());
                                        idleShootingForwardAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
                                    }
                                    else
                                    {
                                        idleForwardAnim.Update(nanoTime());
                                        idleForwardAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
                                    }
                                }
                                else
                                {
                                    if (isShooting)
                                    {
                                        idleShootingBackAnim.Update(nanoTime());
                                        idleShootingBackAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
                                    }
                                    else
                                    {
                                        idleBackAnim.Update(nanoTime());
                                        idleBackAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
                                    }
                                }
                            }
                        }
                    }
                    break;

                case BEHURT:
                    if (getDirection() == RIGHT_DIR)
                    {
                        behurtForwardAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
                    }
                    else
                    {
                        behurtBackAnim.setCurrentFrame(behurtForwardAnim.getCurrentFrame());
                        behurtBackAnim.draw((int)(getPosX() - getGameWorld().camera.getPosX()), (int)getPosY() - (int)getGameWorld().camera.getPosY(), g);
                    }
                    break;

                case FEY:

                    break;

            }

            //drawBoundForCollisionWithMap(g);
            //drawBoundForCollisionWithEnemy(g);
        }
        public override void run()
        {
            if (getDirection() == LEFT_DIR)
                setSpeedX(-13.0f);
            else setSpeedX(13.0f);
        }
        public override void jump()
        {
            if (!getIsJumping())
            {
                setIsJumping(true);
                setSpeedY(-20.0f);
                flyBackAnim.reset();
                flyForwardAnim.reset();
            }
            // for clim wall
            else
            {
                Rectangle rectRightWall = getBoundForCollisionWithMap();
                rectRightWall.X += 1;
                Rectangle rectLeftWall = getBoundForCollisionWithMap();
                rectLeftWall.X -= 1;

                if (getGameWorld().physicalMap.haveCollisionWithRightWall(rectRightWall).Width != 0 && getSpeedX() > 0)
                {
                    setSpeedY(-20.0f);
                    //setSpeedX(-1);
                    flyBackAnim.reset();
                    flyForwardAnim.reset();
                    //setDirection(LEFT_DIR);
                }
                else if (getGameWorld().physicalMap.haveCollisionWithLeftWall(rectLeftWall).Width != 0 && getSpeedX() < 0)
                {
                    setSpeedY(-20.0f);
                    //setSpeedX(1);
                    flyBackAnim.reset();
                    flyForwardAnim.reset();
                    //setDirection(RIGHT_DIR);
                }
            }
        }
        public override void dick()
        {
            if (!getIsJumping())
                setIsDicking(true);
        }
        public override void standUp()
        {
            setIsDicking(false);
            idleForwardAnim.reset();
            idleBackAnim.reset();
            dickForwardAnim.reset();
            dickBackAnim.reset();
        }
        public override void stopRun()
        {
            setSpeedX(0);
            runForwardAnim.reset();
            runBackAnim.reset();
            runForwardAnim.unIgnoreFrame(0);
            runBackAnim.unIgnoreFrame(0);
        }
        public override void attack()
        {
            if (!isShooting && !getIsDicking())
            {
                shooting1.controls.play();

                Bullet bullet = new BlueFire(getPosX(), getPosY(), getGameWorld());
                if (getDirection() == LEFT_DIR)
                {
                    bullet.setSpeedX(-50);
                    bullet.setPosX(bullet.getPosX() - 40);
                    if (getSpeedX() != 0 && getSpeedY() == 0)
                    {
                        bullet.setPosX(bullet.getPosX() - 10);
                        bullet.setPosY(bullet.getPosY() - 5);
                    }
                }
                else
                {
                    bullet.setSpeedX(50);
                    bullet.setPosX(bullet.getPosX() + 40);
                    if (getSpeedX() != 0 && getSpeedY() == 0)
                    {
                        bullet.setPosX(bullet.getPosX() + 10);
                        bullet.setPosY(bullet.getPosY() - 5);
                    }
                }
                if (getIsJumping())
                    bullet.setPosY(bullet.getPosY() - 20);

                bullet.setTeamType(getTeamType());
                getGameWorld().bulletManager.addObject(bullet);

                lastShootingTime = nanoTime();
                isShooting = true;
            }
        }
        public override void hurtingCallback()
        {
            hurtingSound.controls.play();
        }
        private static long nanoTime()
        {
            double timestamp = Stopwatch.GetTimestamp();
            double nanoseconds = 1_000_000_000.0 * timestamp / Stopwatch.Frequency;

            return (long)nanoseconds;
        }
    }
}