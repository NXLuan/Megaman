using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMegaman.GameObject
{
    abstract class Bullet : ParticularObject
    {
        public Bullet(float x, float y, float width, float height, float mass, int damage, GameWorld gameWorld) : base(x, y, width, height, mass, 1, gameWorld)
        {
            setDamage(damage);
        }

        public override void Update()
        {
            base.Update();
            setPosX(getPosX() + getSpeedX());
            setPosY(getPosY() + getSpeedY());
            ParticularObject Object = getGameWorld().particularObjectManager.getCollisionWidthEnemyObject(this);
            if (Object != null && Object.getState() == ALIVE)
            {
                setBlood(0);
                Object.beHurt(getDamage());
            }
        }
    }
}
