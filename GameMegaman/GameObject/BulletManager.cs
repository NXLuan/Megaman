using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMegaman.GameObject
{
    class BulletManager : ParticularObjectManager
    {
        public BulletManager(GameWorld gameWorld) : base(gameWorld)
        { }
        public override void UpdateObjects()
        {
            base.UpdateObjects();
            for (int id = 0; id < particularObjects.Count; id++)
            {
                ParticularObject Object = particularObjects[id];

                if (Object.isObjectOutOfCameraView() || Object.getState() == ParticularObject.DEATH)
                {
                    particularObjects.RemoveAt(id);
                }
            }
        }
    }
}
