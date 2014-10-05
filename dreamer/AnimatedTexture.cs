using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace dreamer
{
    public class AnimatedTexture
    {
        public int framecount;//number of activeFrame for animation
        public int rowCount;
        public Texture2D gridImage;
        private float timePerFrame;// ammount of time for displaying each activeFrame
        public int activeFrame;// the current activeFrame of the texture to be  display
        private float totalElapsed;
        private bool Paused;
        public Rectangle currentFrame;
        int activeRow;
        int totalPlayOnceFrames;
        bool isStaticImage;
        bool currentlyPlayOnce;//determine if currently in playOnce mode
        //size:
        int frameWidth;
        int frameHeight;

        //    public float Rotation, Scale, Depth;
        //   public Vector2 Origin;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="gridIm">the texture2D object that contains frames for the animation</param>
        /// <param name="frameCount">number of frames in the texture2D</param>
        /// <param name="framesPerSec">frame rate - fps</param>
        public AnimatedTexture(ref Texture2D gridIm, int frameCount, int nrows, int framesPerSec)
        {


            totalPlayOnceFrames = 0;
            activeRow = 0;//set the active row to the first one (row numbered from 0 to nrows-1)
            currentlyPlayOnce = false;
            if (framesPerSec == -1)
                isStaticImage = true;
            else
                isStaticImage = false;
            gridImage = gridIm;

            framecount = frameCount;
            rowCount = nrows;

            timePerFrame = (float)1 / framesPerSec * 1000;
            activeFrame = 0;
            totalElapsed = 0;
            Paused = false;
            //initialize Size;

            frameWidth = gridImage.Width / framecount;
            frameHeight = gridIm.Height / rowCount;
            currentFrame = new Rectangle(0, 0, frameWidth, frameHeight);

        }
        /// <summary>
        /// Change the current frame of the animation to a certain frame on the gridImage
        /// </summary>
        /// <param name="rowNumber">row number( starting with 0)</param>
        /// <param name="columnNumber">column number (starting with 0)</param>
        public void ChangeCurrentFrame(int rowNumber, int columnNumber)
        {
            currentFrame = new Rectangle(frameWidth * rowNumber , frameHeight * columnNumber ,
    frameWidth, frameHeight);

        }
        /// <summary>
        /// change current speed of playing to new fps
        /// </summary>
        /// <param name="fps"></param>
        public void ChangeSpeedTo(int fps)
        {
            timePerFrame = (float)1 / fps * 1000;

        }
        // update the activeFrame, should be called every gameUpdate like other components

        public void PlayOnce(int row, int nFrames)
        {
            currentlyPlayOnce = true;
            activeRow = row;
            //set active frame back to 0 
            activeFrame = 0;
            totalPlayOnceFrames = nFrames;
            //test
            totalElapsed = timePerFrame + 1;
        }
        public void Update(GameTime gameTime)
        {
            if (Paused || isStaticImage)
                return;

            //play once mode:


            totalElapsed += gameTime.ElapsedGameTime.Milliseconds;
            //change the current active frame
            if (totalElapsed > timePerFrame)
            {
                activeFrame++;
                //check right after change frame
                if (currentlyPlayOnce && activeFrame > totalPlayOnceFrames - 1)//check to end the playOnce mode
                {
                    //this mean the playonce series has been done playing

                    //end of playonce mode: move the current Frame back to normal
                    activeRow = 0;
                    activeFrame = 0;
                    currentlyPlayOnce = false;

                }
                // Keep the activeFrame between 0 and the total frames, minus one.
                activeFrame = activeFrame % framecount;
                totalElapsed = 0;
                //update currentFrame:

                currentFrame = new Rectangle(frameWidth * activeFrame, frameHeight * activeRow,
                    frameWidth, frameHeight);

            }

        }



        public bool IsPaused
        {
            get { return Paused; }
        }
        public void Reset()
        {
            activeFrame = 0;
            totalElapsed = 0f;
        }
        public void Stop()
        {
            Pause();
            Reset();
        }
        public void Play()
        {
            Paused = false;
        }
        public void Pause()
        {
            Paused = true;
        }

    }
}
