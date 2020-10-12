using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameMegaman.GameObject;

namespace GameMegaman.UserInterface
{
    class InputManager
    {
        private GamePanel gamePanel;
        public InputManager(GamePanel gamePanel)
        {
            this.gamePanel = gamePanel;
        }
        public void processKeyPressed(Keys keyCode)
        {
            switch (keyCode)
            {
                case Keys.Down:
                    gamePanel.gameWorld.megaman.dick();
                    break;
                case Keys.Left:
                    gamePanel.gameWorld.megaman.setDirection(0);
                    gamePanel.gameWorld.megaman.run();
                    break;
                case Keys.Right:
                    gamePanel.gameWorld.megaman.setDirection(1);
                    gamePanel.gameWorld.megaman.run();
                    break;
                case Keys.Enter:

                    if (GameWorld.state == GameWorld.INIT_GAME)
                    {
                        if (gamePanel.gameWorld.previousState == GameWorld.GAMEPLAY)
                            gamePanel.gameWorld.switchState(GameWorld.GAMEPLAY);
                        else gamePanel.gameWorld.switchState(GameWorld.TUTORIAL);

                        gamePanel.gameWorld.bgMusic.controls.play();
                    }
                    if (GameWorld.state == GameWorld.TUTORIAL && gamePanel.gameWorld.storyTutorial >= 1)
                    {
                        if (gamePanel.gameWorld.storyTutorial <= 3)
                        {
                            gamePanel.gameWorld.storyTutorial++;
                            gamePanel.gameWorld.currentSize = 1;
                            gamePanel.gameWorld.textTutorial = gamePanel.gameWorld.texts1[gamePanel.gameWorld.storyTutorial - 1];
                        }
                        else
                        {
                            gamePanel.gameWorld.switchState(GameWorld.GAMEPLAY);
                        }

                        // for meeting boss tutorial
                        if (gamePanel.gameWorld.tutorialState == GameWorld.MEETFINALBOSS)
                        {
                            gamePanel.gameWorld.switchState(GameWorld.GAMEPLAY);
                        }
                    }
                    break;

                case Keys.Space:
                    gamePanel.gameWorld.megaman.jump();
                    break;

                case Keys.A:
                    gamePanel.gameWorld.megaman.attack();
                    break;

            }
        }
        public void processKeyReleased(Keys keyCode)
        {
            switch (keyCode)
            {
                case Keys.Down:
                    gamePanel.gameWorld.megaman.standUp();
                    break;
                case Keys.Left:
                    if (gamePanel.gameWorld.megaman.getSpeedX() < 0)
                        gamePanel.gameWorld.megaman.stopRun();
                    break;
                case Keys.Right:
                    if (gamePanel.gameWorld.megaman.getSpeedX() > 0)
                        gamePanel.gameWorld.megaman.stopRun();
                    break;

                case Keys.Enter:
                    if (GameWorld.state == GameWorld.GAMEOVER || GameWorld.state == GameWorld.GAMEWIN)
                    {
                    }
                    else if (GameWorld.state == GameWorld.PAUSEGAME)
                    {
                        GameWorld.state = gamePanel.gameWorld.lastState;
                    }
                    break;

                case Keys.Escape:
                    gamePanel.gameWorld.lastState = GameWorld.state;
                    GameWorld.state = GameWorld.PAUSEGAME;
                    break;
            }
        }
    }
}
