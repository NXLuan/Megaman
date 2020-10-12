using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMegaman.Effect
{
    class Animation
    {
        private string name;
        private bool isRepeated;
        private List<FrameImage> frameImages;
        private int currentFrame;
        private List<bool> ignoreFrames;
        private List<double> delayFrames;
        private long beginTime;
        private bool drawRectFrame;
        public Animation()
        {
            isRepeated = true;
            frameImages = new List<FrameImage>();
            currentFrame = 0;
            ignoreFrames = new List<bool>();
            delayFrames = new List<double>();
            beginTime = 0;
            drawRectFrame = false;
        }
        public Animation(Animation animation)
        {
            beginTime = animation.beginTime;
            currentFrame = animation.currentFrame;
            drawRectFrame = animation.drawRectFrame;
            isRepeated = animation.isRepeated;

            delayFrames = new List<double>();
            foreach (double d in animation.delayFrames)
            {
                delayFrames.Add(d);
            }

            ignoreFrames = new List<bool>();
            foreach (bool b in animation.ignoreFrames)
            {
                ignoreFrames.Add(b);
            }

            frameImages = new List<FrameImage>();
            foreach (FrameImage f in animation.frameImages)
            {
                frameImages.Add(new FrameImage(f));
            }
        }
        public void setIsRepeated(bool isRepeated)
        {
            this.isRepeated = isRepeated;
        }

        public bool getIsRepeated()
        {
            return isRepeated;
        }

        public bool isIgnoreFrame(int id)
        {
            return ignoreFrames[id];
        }

        public void setIgnoreFrame(int id)
        {
            if (id >= 0 && id < ignoreFrames.Count)
                ignoreFrames[id] = true;
        }

        public void unIgnoreFrame(int id)
        {
            if (id >= 0 && id < ignoreFrames.Count)
                ignoreFrames[id] = false;
        }

        public void setName(string name)
        {
            this.name = name;
        }
        public string getName()
        {
            return name;
        }

        public void setCurrentFrame(int currentFrame)
        {
            if (currentFrame >= 0 && currentFrame < frameImages.Count)
                this.currentFrame = currentFrame;
            else this.currentFrame = 0;
        }
        public int getCurrentFrame()
        {
            return this.currentFrame;
        }
        public void reset()
        {
            currentFrame = 0;
            beginTime = 0;

            for (int i = 0; i < ignoreFrames.Count; i++)
                ignoreFrames[i] = false;
        }
        public void add(FrameImage frameImage, double timeToNextFrame)
        {
            ignoreFrames.Add(false);
            frameImages.Add(frameImage);
            delayFrames.Add(timeToNextFrame);
        }
        public void setDrawRectFrame(bool b)
        {
            drawRectFrame = b;
        }

        public Image getCurrentImage()
        {
            return frameImages[currentFrame].getImage();
        }
        public void Update(long currentTime)
        {
            if (beginTime == 0) beginTime = currentTime;
            else
            {

                if (currentTime - beginTime > delayFrames[currentFrame])
                {
                    nextFrame();
                    beginTime = currentTime;
                }
            }
        }
        private void nextFrame()
        {
            if (currentFrame >= frameImages.Count - 1)
            {
                if (isRepeated) currentFrame = 0;
            }
            else currentFrame++;

            if (ignoreFrames[currentFrame]) nextFrame();
        }
        public bool isLastFrame()
        {
            if (currentFrame == frameImages.Count - 1)
                return true;
            else return false;
        }
        public void flipAllImage()
        {
            for (int i = 0; i < frameImages.Count; i++)
            {
                Image image = frameImages[i].getImage();
                image.RotateFlip(RotateFlipType.Rotate180FlipY);
                frameImages[i].setImage(image);
            }
        }
        public void draw(int x, int y, Graphics g)
        {
            Image image = getCurrentImage();

            g.DrawImage(image, x - image.Width / 2, y - image.Height / 2);
            if (drawRectFrame)
                g.DrawRectangle(new Pen(Brushes.Red), x - image.Width / 2, x - image.Width / 2, image.Width, image.Height);
        }
    }
}
