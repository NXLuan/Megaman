using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMegaman.GameObject
{
    abstract class Human : ParticularObject
    {
        private bool isJumping;
        private bool isDicking;

        private bool isLanding;

        public Human(float x, float y, float width, float height, float mass, int blood, GameWorld gameWorld) : base(x, y, width, height, mass, blood, gameWorld)
        {
            setState(ALIVE);
        }

        public abstract void run();

        public abstract void jump();

        public abstract void dick();

        public abstract void standUp();

        public abstract void stopRun();

        public bool getIsJumping()
        {
            return isJumping;
        }

        public void setIsLanding(bool b)
        {
            isLanding = b;
        }

        public bool getIsLanding()
        {
            return isLanding;
        }

        public void setIsJumping(bool isJumping)
        {
            this.isJumping = isJumping;
        }

        public bool getIsDicking()
        {
            return isDicking;
        }

        public void setIsDicking(bool isDicking)
        {
            this.isDicking = isDicking;
        }

        public override void Update()
        {
            base.Update();

            if (getState() == ALIVE || getState() == NOBEHURT)
            {

                if (!isLanding)
                {
                    setPosX(getPosX() + getSpeedX());


                    if (getDirection() == LEFT_DIR &&
                            getGameWorld().physicalMap.haveCollisionWithLeftWall(getBoundForCollisionWithMap()).Width != 0)
                    {

                        Rectangle rectLeftWall = getGameWorld().physicalMap.haveCollisionWithLeftWall(getBoundForCollisionWithMap());
                        setPosX(rectLeftWall.X + rectLeftWall.Width + getWidth() / 2);

                    }
                    if (getDirection() == RIGHT_DIR &&
                            getGameWorld().physicalMap.haveCollisionWithRightWall(getBoundForCollisionWithMap()).Width != 0)
                    {

                        Rectangle rectRightWall = getGameWorld().physicalMap.haveCollisionWithRightWall(getBoundForCollisionWithMap());
                        setPosX(rectRightWall.X - getWidth() / 2);

                    }

                    Rectangle boundForCollisionWithMapFuture = getBoundForCollisionWithMap();
                    boundForCollisionWithMapFuture.Y += (int)(getSpeedY() != 0 ? getSpeedY() : 2);
                    Rectangle rectLand = getGameWorld().physicalMap.haveCollisionWithLand(boundForCollisionWithMapFuture);

                    Rectangle rectTop = getGameWorld().physicalMap.haveCollisionWithTop(boundForCollisionWithMapFuture);

                    if (rectTop.Width != 0)
                    {
                        setSpeedY(0);
                        setPosY(rectTop.Y + getGameWorld().physicalMap.getTileSize() + getHeight() / 2);

                    }
                    else if (rectLand.Width != 0)
                    {
                        setIsJumping(false);
                        if (getSpeedY() > 0) setIsLanding(true);
                        setSpeedY(0);
                        setPosY(rectLand.Y - getHeight() / 2 - 1);
                    }
                    else
                    {
                        setIsJumping(true);
                        setSpeedY(getSpeedY() + getMass());
                        setPosY(getPosY() + getSpeedY());
                    }
                }
            }
        }

    }
}
