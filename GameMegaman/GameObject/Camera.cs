using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMegaman.GameObject
{
    class Camera : GameObject
    {
        private float widthView;
        private float heightView;

        private bool isLocked = false;

        public Camera(float x, float y, float widthView, float heightView, GameWorld gameWorld) : base(x, y, gameWorld)
        {
            this.widthView = widthView;
            this.heightView = heightView;
        }

        public void Lock()
        {
            isLocked = true;
        }

        public void unLock()
        {
            isLocked = false;
        }
        public override void Update()
        {
            if (!isLocked)
            {
                Megaman mainCharacter = getGameWorld().megaman;

                if (mainCharacter.getPosX() - getPosX() > 400) setPosX(mainCharacter.getPosX() - 400);
                if (mainCharacter.getPosX() - getPosX() < 200) setPosX(mainCharacter.getPosX() - 200);

                if (mainCharacter.getPosY() - getPosY() > 400) setPosY(mainCharacter.getPosY() - 400); 
                else if (mainCharacter.getPosY() - getPosY() < 250) setPosY(mainCharacter.getPosY() - 250);
            }
        }

        public float getWidthView()
        {
            return widthView;
        }

        public void setWidthView(float widthView)
        {
            this.widthView = widthView;
        }

        public float getHeightView()
        {
            return heightView;
        }

        public void setHeightView(float heightView)
        {
            this.heightView = heightView;
        }

    }
}
