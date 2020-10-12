using GameMegaman.Effect;
using GameMegaman.UserInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMPLib;

namespace GameMegaman.GameObject
{
    class GameWorld
    {
        private Image bufferedImage;
        public int lastState;


        public Megaman megaman;
        public BackgroundMap backgroundMap;
        public PhysicalMap physicalMap;
        public BulletManager bulletManager;

        public ParticularObjectManager particularObjectManager;
        public Camera camera;

        public const int finalBossX = 3600;
        public const int INIT_GAME = 0;
        public const int TUTORIAL = 1;
        public const int GAMEPLAY = 2;
        public const int GAMEOVER = 3;
        public const int GAMEWIN = 4;
        public const int PAUSEGAME = 5;


        public const int INTROGAME = 0;
        public const int MEETFINALBOSS = 1;


        public int openIntroGameY = 0;
        public static int state = INIT_GAME;
        public int previousState = state;
        public int tutorialState = INTROGAME;


        public int storyTutorial = 0;
        public string[] texts1 = new string[4];

        public string textTutorial;
        public int currentSize = 1;

        private bool finalbossTrigger = true;
        ParticularObject boss;

        FrameImage avatar = CacheDataLoader.getInstance().getFrameImage("avatar");


        private int numberOfLife = 3;


        public WindowsMediaPlayer bgMusic;

        public GameWorld(GamePanel gamePanel)
        {
            texts1[0] = "Tôi là Nguyễn Xuân Luân\nMSSV 18521066....";
            texts1[1] = "Ms.T đã đàn áp chúng ta hơn 10 năm qua\n"
                    + "và chúng ta đã phải học trong môi trường đáng sợ đó....";
            texts1[2] = "Bây giờ là thời gian của chúng ta giành lại tự do!....";
            texts1[3] = "      LET'S GO!.....";
            textTutorial = texts1[0];

            bufferedImage = new Bitmap(GameFrame.SCREEN_WIDTH, GameFrame.SCREEN_HEIGHT);
            megaman = new Megaman(400, 400, this);
            megaman.setTeamType(ParticularObject.LEAGUE_TEAM);
            backgroundMap = new BackgroundMap(0, 0, this);
            physicalMap = new PhysicalMap(0, 0, this);
            camera = new Camera(0, 0, GameFrame.SCREEN_WIDTH, GameFrame.SCREEN_HEIGHT, this);
            bulletManager = new BulletManager(this);

            particularObjectManager = new ParticularObjectManager(this);
            particularObjectManager.addObject(megaman);

            initEnemies();

            bgMusic = CacheDataLoader.getInstance().getSound("bgmusic");
        }

        private void initEnemies()
        {
            ParticularObject redeye = new RedEyeDevil(1250, 410, this);
            redeye.setDirection(ParticularObject.LEFT_DIR);
            redeye.setTeamType(ParticularObject.ENEMY_TEAM);
            particularObjectManager.addObject(redeye);

            ParticularObject redeye2 = new RedEyeDevil(2500, 500, this);
            redeye2.setDirection(ParticularObject.LEFT_DIR);
            redeye2.setTeamType(ParticularObject.ENEMY_TEAM);
            particularObjectManager.addObject(redeye2);

            ParticularObject redeye3 = new RedEyeDevil(3450, 500, this);
            redeye3.setDirection(ParticularObject.LEFT_DIR);
            redeye3.setTeamType(ParticularObject.ENEMY_TEAM);
            particularObjectManager.addObject(redeye3);

            ParticularObject redeye4 = new RedEyeDevil(500, 1190, this);
            redeye4.setDirection(ParticularObject.RIGHT_DIR);
            redeye4.setTeamType(ParticularObject.ENEMY_TEAM);
            particularObjectManager.addObject(redeye4);
        }

        public void switchState(int st)
        {
            previousState = state;
            state = st;
        }

        private void TutorialUpdate()
        {
            switch (tutorialState)
            {
                case INTROGAME:
                    if (storyTutorial == 0)
                    {
                        if (openIntroGameY < 450)
                        {
                            openIntroGameY += 10;
                        }
                        else storyTutorial++;
                    }
                    else
                    {
                        if (currentSize < textTutorial.Length) currentSize++;
                    }
                    break;
                case MEETFINALBOSS:
                    if (storyTutorial == 0)
                    {
                        if (openIntroGameY >= 450)
                        {
                            openIntroGameY -= 5;
                        }
                        if (camera.getPosX() < finalBossX)
                        {
                            camera.setPosX(camera.getPosX() + 5);
                        }

                        if (megaman.getPosX() < finalBossX + 150)
                        {
                            megaman.setDirection(ParticularObject.RIGHT_DIR);
                            megaman.run();
                            megaman.Update();
                        }
                        else
                        {
                            megaman.stopRun();
                        }

                        if (openIntroGameY < 450 && camera.getPosX() >= finalBossX && megaman.getPosX() >= finalBossX + 150)
                        {
                            camera.Lock();
                            storyTutorial++;
                            megaman.stopRun();
                            physicalMap.phys_map[14, 120] = 1;
                            physicalMap.phys_map[15, 120] = 1;
                            physicalMap.phys_map[16, 120] = 1;
                            physicalMap.phys_map[17, 120] = 1;

                            backgroundMap.map[14, 120] = 17;
                            backgroundMap.map[15, 120] = 17;
                            backgroundMap.map[16, 120] = 17;
                            backgroundMap.map[17, 120] = 17;
                        }

                    }
                    else
                    {
                        if (currentSize < textTutorial.Length) currentSize++;
                    }
                    break;
            }
        }

        private void drawString(Graphics g, string text, int x, int y)
        {
            foreach (string str in text.Split('\n'))
                g.DrawString(str, new Font("Arial", 8), Brushes.White, x, y);
        }

        private void TutorialRender(Graphics g)
        {
            switch (tutorialState)
            {
                case INTROGAME:
                    int yMid = GameFrame.SCREEN_HEIGHT / 2 - 15;
                    int y1 = yMid - GameFrame.SCREEN_HEIGHT / 2 - openIntroGameY / 2;
                    int y2 = yMid + openIntroGameY / 2;

                    g.FillRectangle(Brushes.Black, 0, y1, GameFrame.SCREEN_WIDTH, GameFrame.SCREEN_HEIGHT / 2);
                    g.FillRectangle(Brushes.Black, 0, y2, GameFrame.SCREEN_WIDTH, GameFrame.SCREEN_HEIGHT / 2);

                    if (storyTutorial >= 1)
                    {
                        g.DrawImage(avatar.getImage(), 600, 350);
                        g.FillRectangle(Brushes.Blue, 280, 450, 350, 80);
                        string text = textTutorial.Substring(0, currentSize - 1);
                        g.DrawString(text, new Font("Arial", 8), Brushes.White, 290, 480);
                    }

                    break;
                case MEETFINALBOSS:
                    yMid = GameFrame.SCREEN_HEIGHT / 2 - 15;
                    y1 = yMid - GameFrame.SCREEN_HEIGHT / 2 - openIntroGameY / 2;
                    y2 = yMid + openIntroGameY / 2;

                    g.FillRectangle(Brushes.Black, 0, y1, GameFrame.SCREEN_WIDTH, GameFrame.SCREEN_HEIGHT / 2);
                    g.FillRectangle(Brushes.Black, 0, y2, GameFrame.SCREEN_WIDTH, GameFrame.SCREEN_HEIGHT / 2);
                    break;
            }
        }

        public void Update()
        {
            switch (state)
            {
                case INIT_GAME:

                    break;
                case TUTORIAL:
                    TutorialUpdate();

                    break;
                case GAMEPLAY:
                    particularObjectManager.UpdateObjects();
                    bulletManager.UpdateObjects();

                    physicalMap.Update();
                    camera.Update();


                    if (megaman.getPosX() > finalBossX && finalbossTrigger)
                    {
                        finalbossTrigger = false;
                        switchState(TUTORIAL);
                        tutorialState = MEETFINALBOSS;
                        storyTutorial = 0;
                        openIntroGameY = 550;

                        boss = new FinalBoss(finalBossX + 700, 460, this);
                        boss.setTeamType(ParticularObject.ENEMY_TEAM);
                        boss.setDirection(ParticularObject.LEFT_DIR);
                        particularObjectManager.addObject(boss);

                    }

                    if (megaman.getState() == ParticularObject.DEATH)
                    {
                        numberOfLife--;
                        if (numberOfLife >= 0)
                        {
                            megaman.setBlood(100);
                            megaman.setPosY(megaman.getPosY() - 50);
                            megaman.setState(ParticularObject.NOBEHURT);
                            particularObjectManager.addObject(megaman);
                        }
                        else
                        {
                            switchState(GAMEOVER);
                            bgMusic.controls.stop();
                        }
                    }
                    if (!finalbossTrigger && boss.getState() == ParticularObject.DEATH)
                        switchState(GAMEWIN);

                    break;
                case GAMEOVER:

                    break;
                case GAMEWIN:

                    break;
            }
        }
        public void Render()
        {
            Graphics g = Graphics.FromImage(bufferedImage);

            if (g != null)
            {
                switch (state)
                {
                    case INIT_GAME:
                        g.FillRectangle(Brushes.Black, 0, 0, GameFrame.SCREEN_WIDTH, GameFrame.SCREEN_HEIGHT);
                        g.DrawString("PRESS ENTER TO CONTINUE", new Font("Arial", 8), Brushes.White, 400, 290);
                        break;
                    case PAUSEGAME:
                        g.FillRectangle(Brushes.Black, 300, 260, 400, 70);
                        g.DrawString("PRESS ENTER TO CONTINUE", new Font("Arial", 8), Brushes.White, 415, 290);
                        break;
                    case TUTORIAL:
                        backgroundMap.draw(g);
                        if (tutorialState == MEETFINALBOSS)
                        {
                            particularObjectManager.draw(g);
                        }
                        TutorialRender(g);

                        break;
                    case GAMEWIN:
                    case GAMEPLAY:
                        backgroundMap.draw(g);
                        particularObjectManager.draw(g);
                        bulletManager.draw(g);

                        if (tutorialState == MEETFINALBOSS)
                        {
                            g.FillRectangle(Brushes.Gray, 599, 59, 372, 22);
                            g.FillRectangle(Brushes.Blue, 600, 60, boss.getBlood(), 20);
                        }

                        g.FillRectangle(Brushes.Gray, 19, 59, 102, 22);
                        g.FillRectangle(Brushes.Red, 20, 60, megaman.getBlood(), 20);

                        for (int i = 0; i < numberOfLife; i++)
                        {
                            g.DrawImage(CacheDataLoader.getInstance().getFrameImage("hearth").getImage(), 20 + i * 40, 18);
                        }


                        if (state == GAMEWIN)
                        {
                            g.DrawImage(CacheDataLoader.getInstance().getFrameImage("gamewin").getImage(), 300, 300);
                        }

                        break;
                    case GAMEOVER:
                        g.FillRectangle(Brushes.Black, 0, 0, GameFrame.SCREEN_WIDTH, GameFrame.SCREEN_HEIGHT);
                        g.DrawString("GAME OVER!", new Font("Arial", 8), Brushes.White, 450, 300);
                        break;
                }
            }
        }

        public Image getBufferedImage()
        {
            return bufferedImage;
        }
    }
}
