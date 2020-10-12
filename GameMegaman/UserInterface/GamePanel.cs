using GameMegaman.Effect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameMegaman.GameObject;

namespace GameMegaman.UserInterface
{
    class GamePanel : Panel
    {
        Thread thread;
        bool isRunning;
        InputManager inputManager;
        public GameWorld gameWorld;

        public GamePanel()
        {
            gameWorld = new GameWorld(this);
            inputManager = new InputManager(this);
        }

        void Repaint()
        {
            try
            {
                Graphics g = this.CreateGraphics();
                g.DrawImageUnscaled(gameWorld.getBufferedImage(), 0, 0);
            }
            catch
            {
                return;
            }
        }

        public void startGame()
        {
            if (thread == null)
            {
                isRunning = true;
                thread = new Thread(Run);
                thread.IsBackground = true;
                thread.Start();
            }
        }

        public void Run()
        {
            long FPS = 1000;
            long period = 1000 * 1000000 / FPS;
            long beginTime;
            long sleepTime;
            long deltaTime;

            beginTime = nanoTime();
            while (isRunning)
            {
                gameWorld.Update();
                gameWorld.Render();
                Repaint();

                deltaTime = nanoTime() - beginTime;
                sleepTime = period - deltaTime;

                try
                {
                    if (sleepTime > 0)
                        Thread.Sleep((int)sleepTime / 1000000);
                    else Thread.Sleep((int)period / 2000000);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                beginTime = nanoTime();
            }
        }
        private static long nanoTime()
        {
            double timestamp = Stopwatch.GetTimestamp();
            double nanoseconds = 1000000000.0 * timestamp / Stopwatch.Frequency;

            return (long)nanoseconds;
        }

        public void KeyPressed(Keys e)
        {
            inputManager.processKeyPressed(e);
        }
        public void KeyReleased(Keys e)
        {
            inputManager.processKeyReleased(e);
        }
    }
}
