using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMegaman.Effect
{
    class FrameImage
    {
        private string name;
        private Image image;

        public FrameImage()
        {
            this.name = null;
            this.image = null;
        }
        public FrameImage(string name, Image image)
        { 
            this.name = name;
            this.image = image;
        }
        public FrameImage(FrameImage frameImage)
        {
            image = new Bitmap(frameImage.getImageWidth(), frameImage.getImageHeight());
            Graphics g = Graphics.FromImage(image);
            g.DrawImage(frameImage.getImage(), 0, 0);
        }
        public void draw(Graphics g, int x, int y)
        {
            g.DrawImage(image, x - image.Width / 2, y - image.Height / 2);
        }
        public int getImageWidth()
        {
            return image.Width;
        }
        public int getImageHeight()
        {
            return image.Height;
        }
        public string getName()
        {
            return name;
        }
        public void setName(string name)
        {
            this.name = name;
        }
        public Image getImage()
        {
            return image;
        }
        public void setImage(Image image)
        {
            this.image = image;
        }
    }
}
