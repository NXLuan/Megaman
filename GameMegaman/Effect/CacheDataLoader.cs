using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace GameMegaman.Effect
{
    class CacheDataLoader
    {
        private static CacheDataLoader instance;

        private static string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\";
        private string framefile = projectDirectory + @"data\frame.txt";
        private string animationfile = projectDirectory + @"data\animation.txt";
        private string physmapfile = projectDirectory + @"data\phys_map.txt";
        private string backgroundmapfile = projectDirectory + @"data\background_map.txt";
        private string soundfile = projectDirectory + @"data\sounds.txt";


        private Hashtable frameImages;
        private Hashtable animations;
        private Hashtable sounds;

        private int[,] phys_map;
        private int[,] background_map;

        private CacheDataLoader() { }
        public static CacheDataLoader getInstance()
        {
            if (instance == null)
                instance = new CacheDataLoader();
            return instance;
        }

        public void LoadSounds()
        {
            sounds = new Hashtable();

            StreamReader reader = new StreamReader(soundfile);

            string line = null;

            if (reader.ReadLine() == null)
            {
                return;
            }
            else
            {
                reader = new StreamReader(soundfile);

                while ((line = reader.ReadLine()).Equals("")) ;

                int n = int.Parse(line);

                for (int i = 0; i < n; i++)
                {
                    WindowsMediaPlayer sound = new WindowsMediaPlayer();
                    while ((line = reader.ReadLine()).Equals("")) ;

                    string[] str = line.Split(' ');
                    string name = str[0];

                    sound.URL = projectDirectory + str[1];

                    instance.sounds.Add(name, sound);
                }
            }
            reader.Close();
        }


        public void LoadFrame()
        {
            frameImages = new Hashtable();

            StreamReader reader = new StreamReader(framefile);

            string line = null;

            if (reader.ReadLine() == null)
            {
                Console.WriteLine("No data");
                return;
            }
            else
            {
                reader = new StreamReader(framefile);

                while ((line = reader.ReadLine()).Equals("")) ;

                int n = int.Parse(line);

                for (int i = 0; i < n; i++)
                {

                    FrameImage frame = new FrameImage();
                    while ((line = reader.ReadLine()).Equals("")) ;
                    frame.setName(line);

                    while ((line = reader.ReadLine()).Equals("")) ;
                    string[] str = line.Split(' ');
                    string path = str[1];

                    while ((line = reader.ReadLine()).Equals("")) ;
                    str = line.Split(' ');
                    int x = int.Parse(str[1]);

                    while ((line = reader.ReadLine()).Equals("")) ;
                    str = line.Split(' ');
                    int y = int.Parse(str[1]);

                    while ((line = reader.ReadLine()).Equals("")) ;
                    str = line.Split(' ');
                    int w = int.Parse(str[1]);

                    while ((line = reader.ReadLine()).Equals("")) ;
                    str = line.Split(' ');
                    int h = int.Parse(str[1]);

                    Image imageData = Image.FromFile(projectDirectory + path);
                    Image image = getSubimage(imageData, x, y, w, h);
                    imageData.Dispose();
                    frame.setImage(image);

                    instance.frameImages.Add(frame.getName(), frame);
                }
            }
            reader.Close();
        }
        public Bitmap getSubimage(Image imageData, int x, int y, int w, int h)
        {
            imageData = new Bitmap(imageData, imageData.Width, imageData.Height);
            Bitmap bm = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(bm);
            g.DrawImage(imageData, 0, 0, new Rectangle(x, y, w, h), GraphicsUnit.Pixel);
            g.Dispose();
            imageData.Dispose();
            return bm;
        }

        public void LoadAnimation()
        {
            animations = new Hashtable();

            StreamReader reader = new StreamReader(animationfile);

            string line = null;

            if (reader.ReadLine() == null)
            {
                Console.WriteLine("No data");
                return;
            }
            else
            {
                reader = new StreamReader(animationfile);

                while ((line = reader.ReadLine()).Equals("")) ;
                int n = int.Parse(line);

                for (int i = 0; i < n; i++)
                {
                    Animation animation = new Animation();

                    while ((line = reader.ReadLine()).Equals("")) ;
                    animation.setName(line);

                    while ((line = reader.ReadLine()).Equals("")) ;
                    string[] str = line.Split(' ');

                    for (int j = 0; j < str.Length; j += 2)
                        animation.add(getFrameImage(str[j]), double.Parse(str[j + 1]));

                    instance.animations.Add(animation.getName(), animation);
                }
            }
            reader.Close();
        }

        public void LoadPhysMap()
        {
            StreamReader reader = new StreamReader(physmapfile);

            string line = null;

            line = reader.ReadLine();
            int numberOfRows = int.Parse(line);
            line = reader.ReadLine();
            int numberOfColumns = int.Parse(line);

            instance.phys_map = new int[numberOfRows, numberOfColumns];

            for (int i = 0; i < numberOfRows; i++)
            {
                line = reader.ReadLine();
                String[] str = line.Split(' ');
                for (int j = 0; j < numberOfColumns; j++)
                    instance.phys_map[i, j] = int.Parse(str[j]);
            }
            reader.Close();
        }

        public void LoadBackgroundMap()
        {
            StreamReader reader = new StreamReader(backgroundmapfile);

            String line = null;

            line = reader.ReadLine();
            int numberOfRows = int.Parse(line);
            line = reader.ReadLine();
            int numberOfColumns = int.Parse(line);


            instance.background_map = new int[numberOfRows, numberOfColumns];

            for (int i = 0; i < numberOfRows; i++)
            {
                line = reader.ReadLine();
                String[] str = line.Split(' ');
                for (int j = 0; j < numberOfColumns; j++)
                    instance.background_map[i, j] = int.Parse(str[j]);
            }
            reader.Close();
        }


        public int[,] getBackgroundMap()
        {
            return instance.background_map;
        }

        public FrameImage getFrameImage(string name)
        {
            FrameImage frameImage = new FrameImage((FrameImage)instance.frameImages[name]);
            return frameImage;
        }
        public Animation getAnimation(string name)
        {
            Animation animation = new Animation((Animation)instance.animations[name]);
            return animation;
        }
        public int[,] getPhysicalMap()
        {
            return instance.phys_map;
        }

        public WindowsMediaPlayer getSound(string name)
        {
            return (WindowsMediaPlayer)instance.sounds[name];
        }

        public void LoadData()
        {
            LoadFrame();
            LoadAnimation();
            LoadPhysMap();
            LoadBackgroundMap();
            LoadSounds();
        }
    }
}
