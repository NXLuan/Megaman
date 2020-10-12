using GameMegaman.Effect;
using GameMegaman.UserInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMegaman.GameObject
{
    class BackgroundMap : GameObject
    {
        public int[,] map;
        private int tileSize;

        public BackgroundMap(float x, float y, GameWorld gameWorld) : base(x, y, gameWorld)
        {
            map = CacheDataLoader.getInstance().getBackgroundMap();
            tileSize = 30;
        }

        public override void Update() { }

        public void draw(Graphics g)
        {
            Camera camera = getGameWorld().camera;

            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    if (map[i, j] != 0 && j * tileSize - camera.getPosX() > -30 && j * tileSize - camera.getPosX() < GameFrame.SCREEN_WIDTH
                            && i * tileSize - camera.getPosY() > -30 && i * tileSize - camera.getPosY() < GameFrame.SCREEN_HEIGHT)
                    {
                        g.DrawImage(CacheDataLoader.getInstance().getFrameImage("tiled" + map[i, j]).getImage(), (int)getPosX() + j * tileSize - (int)camera.getPosX(),
                            (int)getPosY() + i * tileSize - (int)camera.getPosY());
                    }

        }

    }
}
