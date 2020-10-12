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
    abstract class ParticularObject : GameObject
    {
        public const int LEAGUE_TEAM = 1;
        public const int ENEMY_TEAM = 2;

        public const int LEFT_DIR = 0;
        public const int RIGHT_DIR = 1;

        public const int ALIVE = 0;
        public const int BEHURT = 1;
        public const int FEY = 2;
        public const int DEATH = 3;
        public const int NOBEHURT = 4;
        private int state = ALIVE;

        private float width;
        private float height;
        private float mass;
        private float speedX;
        private float speedY;
        private int blood;

        private int damage;

        private int direction;

        protected Animation behurtForwardAnim, behurtBackAnim;

        private int teamType;

        private long startTimeNoBeHurt;
        private long timeForNoBeHurt;

        public ParticularObject(float x, float y, float width, float height, float mass, int blood, GameWorld gameWorld) : base(x, y, gameWorld)
        {
            setWidth(width);
            setHeight(height);
            setMass(mass);
            setBlood(blood);

            direction = RIGHT_DIR;
        }

        public void setTimeForNoBehurt(long time)
        {
            timeForNoBeHurt = time;
        }

        public long getTimeForNoBeHurt()
        {
            return timeForNoBeHurt;
        }

        public void setState(int state)
        {
            this.state = state;
        }

        public int getState()
        {
            return state;
        }

        public void setDamage(int damage)
        {
            this.damage = damage;
        }

        public int getDamage()
        {
            return damage;
        }

        public void setTeamType(int team)
        {
            teamType = team;
        }

        public int getTeamType()
        {
            return teamType;
        }

        public void setMass(float mass)
        {
            this.mass = mass;
        }

        public float getMass()
        {
            return mass;
        }

        public void setSpeedX(float speedX)
        {
            this.speedX = speedX;
        }

        public float getSpeedX()
        {
            return speedX;
        }

        public void setSpeedY(float speedY)
        {
            this.speedY = speedY;
        }

        public float getSpeedY()
        {
            return speedY;
        }

        public void setBlood(int blood)
        {
            if (blood >= 0)
                this.blood = blood;
            else this.blood = 0;
        }

        public int getBlood()
        {
            return blood;
        }

        public void setWidth(float width)
        {
            this.width = width;
        }

        public float getWidth()
        {
            return width;
        }

        public void setHeight(float height)
        {
            this.height = height;
        }

        public float getHeight()
        {
            return height;
        }

        public void setDirection(int dir)
        {
            direction = dir;
        }

        public int getDirection()
        {
            return direction;
        }

        public abstract void attack();


        public bool isObjectOutOfCameraView()
        {
            if (getPosX() - getGameWorld().camera.getPosX() > getGameWorld().camera.getWidthView() ||
                    getPosX() - getGameWorld().camera.getPosX() < -50
                || getPosY() - getGameWorld().camera.getPosY() > getGameWorld().camera.getHeightView()
                        || getPosY() - getGameWorld().camera.getPosY() < -50)
                return true;
            else return false;
        }

        public Rectangle getBoundForCollisionWithMap()
        {
            Rectangle bound = new Rectangle();
            bound.X = (int)(getPosX() - (getWidth() / 2));
            bound.Y = (int)(getPosY() - (getHeight() / 2));
            bound.Width = (int)getWidth();
            bound.Height = (int)getHeight();
            return bound;
        }

        public void beHurt(int damgeEat)
        {
            setBlood(getBlood() - damgeEat);
            state = BEHURT;
            hurtingCallback();
        }
        public override void Update()
        {
            switch (state)
            {
                case ALIVE:
                    ParticularObject Object = getGameWorld().particularObjectManager.getCollisionWidthEnemyObject(this);
                    if (Object != null)
                    {
                        if (Object.getDamage() > 0)
                        {
                            beHurt(Object.getDamage());
                        }

                    }
                    break;

                case BEHURT:
                    if (behurtBackAnim == null)
                    {
                        state = NOBEHURT;
                        startTimeNoBeHurt = nanoTime();
                        if (getBlood() == 0)
                            state = FEY;

                    }
                    else
                    {
                        behurtForwardAnim.Update(nanoTime());
                        if (behurtForwardAnim.isLastFrame())
                        {
                            behurtForwardAnim.reset();
                            state = NOBEHURT;
                            if (getBlood() == 0)
                                state = FEY;
                            startTimeNoBeHurt = nanoTime();
                        }
                    }

                    break;

                case FEY:

                    state = DEATH;

                    break;

                case DEATH:


                    break;

                case NOBEHURT:
                    if (nanoTime() - startTimeNoBeHurt > timeForNoBeHurt)
                        state = ALIVE;
                    break;
            }

        }

        public void drawBoundForCollisionWithMap(Graphics g)
        {
            Rectangle rect = getBoundForCollisionWithMap();
            g.DrawRectangle(new Pen(Color.Blue, 1), rect.X - getGameWorld().camera.getPosX(), rect.Y - (int)getGameWorld().camera.getPosY(), rect.Width, rect.Height);
        }

        public void drawBoundForCollisionWithEnemy(Graphics g)
        {
            Rectangle rect = getBoundForCollisionWithEnemy();
            g.DrawRectangle(new Pen(Color.Red, 1),rect.X - (int)getGameWorld().camera.getPosX(), rect.Y - (int)getGameWorld().camera.getPosY(), rect.Width, rect.Height);
        }

        public abstract Rectangle getBoundForCollisionWithEnemy();

        public abstract void draw(Graphics g);

        public virtual void hurtingCallback() { }
        private static long nanoTime()
        {
            long nano = 10000L * Stopwatch.GetTimestamp();
            nano /= TimeSpan.TicksPerMillisecond;
            nano *= 100L;
            return nano;
        }
    }
}
