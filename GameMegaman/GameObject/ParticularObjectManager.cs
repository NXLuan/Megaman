using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMegaman.GameObject
{
    class ParticularObjectManager
    {
        protected SynchronizedCollection<ParticularObject> particularObjects;

        private GameWorld gameWorld;

        public ParticularObjectManager(GameWorld gameWorld)
        {
            particularObjects = new SynchronizedCollection<ParticularObject>();
            this.gameWorld = gameWorld;

        }

        public GameWorld getGameWorld()
        {
            return gameWorld;
        }

        public void addObject(ParticularObject particularObject)
        {
            particularObjects.Add(particularObject);
        }

        public void RemoveObject(ParticularObject particularObject)
        {
            for (int id = 0; id < particularObjects.Count; id++)
            {
                ParticularObject Object = particularObjects[id];
                if (Object == particularObject)
                    particularObjects.RemoveAt(id);
            }
        }

        public ParticularObject getCollisionWidthEnemyObject(ParticularObject Object)
        {
            for (int id = 0; id < particularObjects.Count; id++)
            {
                ParticularObject objectInList = particularObjects[id];

                if (Object.getTeamType() != objectInList.getTeamType() &&
                        Object.getBoundForCollisionWithEnemy().IntersectsWith(objectInList.getBoundForCollisionWithEnemy()))
                {
                    return objectInList;
                }
            }
            return null;
        }

        public virtual void UpdateObjects()
        {
            for (int id = 0; id < particularObjects.Count; id++)
            {
                ParticularObject Object = particularObjects[id];

                if (!Object.isObjectOutOfCameraView()) Object.Update();

                if (Object.getState() == ParticularObject.DEATH)
                {
                    particularObjects.RemoveAt(id);
                }
            }
        }

        public void draw(Graphics g)
        {
            foreach (ParticularObject Object in particularObjects)
                if (!Object.isObjectOutOfCameraView()) Object.draw(g);
        }
    }
}
