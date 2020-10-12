﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMegaman.GameObject
{
    abstract class GameObject
    {
        private float posX;
        private float posY;
        private GameWorld gameWorld;

        public GameObject(float posX, float posY, GameWorld gameWorld)
        {
            this.posX = posX;
            this.posY = posY;
            this.gameWorld = gameWorld;
        }
        public abstract void Update();
        public float getPosX()
        {
            return posX;
        }
        public void setPosX(float posX)
        {
            this.posX = posX;
        }
        public float getPosY()
        {
            return posY;
        }
        public void setPosY(float posY)
        {
            this.posY = posY;
        }
        public GameWorld getGameWorld()
        {
            return gameWorld;
        }
        public void setGameWorld(GameWorld gameWorld)
        {
            this.gameWorld = gameWorld;
        }
    }
}