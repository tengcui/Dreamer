using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace dreamer
{

    class ScrollingBackground
    {
        #region Fields
        //The sprites that make up the images to be scrolled 
        //across the screen.
        List<Sprite> mBackgroundSprites;

        //The sprite at the right end of the chain of images
        Sprite mRightMostSprite;
        //The sprite at the left end of the cahin of images
        Sprite mLeftMostSprite;

        //The viewing area for drawing the scrolling background images within
        Viewport mViewport;

        //The direction to scroll the background images at their given speed
        public enum HorizontalScrollDirection
        {
            Left,
            Right,
            sLeft,
            sRight,
            mLeft,
            mRight,
            fLeft,
            fRight,
        }
        #endregion

        #region Initialization
        public ScrollingBackground(Viewport theViewport)
        {
            mBackgroundSprites = new List<Sprite>();
            mRightMostSprite = null;
            mLeftMostSprite = null;
            mViewport = theViewport;
        }

        //Loading the images
        public void LoadContent(ContentManager theContentManager)
        {
            //Clear the sprites currently stored as the left and right ends of the chains
            mRightMostSprite = null;
            mLeftMostSprite = null;

            //The total width of all the sprites in the chain
            float aWidth = 0;

            //Cycle through all of the background sprites that have been added
            //and then load their content and position them.
            foreach (Sprite aBackgroundSprite in mBackgroundSprites)
            {
                //Load the sprite's content and apply it's scale, the scale is calculated by figuring
                //out how far the sprite need to be strecthed to make it fill the height of the viewport
                aBackgroundSprite.LoadContent(theContentManager, aBackgroundSprite.AssetName);
                aBackgroundSprite.Scale = mViewport.Height / aBackgroundSprite.Size.Height;

                //If the background sprite is the first in line, then the mLastInLine will be null
                if (mRightMostSprite == null)
                {
                    //Position the first background sprite in lina at the (0,0) position
                    aBackgroundSprite.Position = new Vector2(mViewport.X, mViewport.Y);
                    mLeftMostSprite = aBackgroundSprite;
                }
                else
                {
                    //Position the sprite after the last sprite in the line
                    aBackgroundSprite.Position = new Vector2(mRightMostSprite.Position.X + mRightMostSprite.Size.Width, mViewport.Y);
                }

                //Set the sprite as the last one in the line
                mRightMostSprite = aBackgroundSprite;

                //Increment the width of all the sprites combined in the chain
                aWidth += aBackgroundSprite.Size.Width;
            }

            //If thw width of all the sprites inthe chain does not fill the twice the viewport width
            // then we need to cycle through the images over and over until we have added
            //enough background images to fill the twice the width.
            int aIndex = 0;
            if (mBackgroundSprites.Count > 0 && aWidth < mViewport.Width * 2)
            {
                do
                {
                    //Add another background image to the chain
                    Sprite aBackgroundSprite = new Sprite();
                    aBackgroundSprite.AssetName = mBackgroundSprites[aIndex].AssetName;
                    aBackgroundSprite.LoadContent(theContentManager, aBackgroundSprite.AssetName);
                    aBackgroundSprite.Scale = mViewport.Height / aBackgroundSprite.Size.Height;
                    aBackgroundSprite.Position = new Vector2(mRightMostSprite.Position.X + mRightMostSprite.Size.Width, mViewport.Y);
                    mBackgroundSprites.Add(aBackgroundSprite);
                    mRightMostSprite = aBackgroundSprite;

                    //Add the new background image's width to the total width of the chain
                    aWidth += aBackgroundSprite.Size.Width;

                    //Move to the next image in the background images
                    //If we have moved to the end of the indexes, start over
                    aIndex += 1;
                    if (aIndex > mBackgroundSprites.Count - 1)
                    {
                        aIndex = 0;
                    }
                }
                while (aWidth < mViewport.Width * 2);
            }
        }
        #endregion


        //Add a background sprite to be scrolled through the screen
        public void AddBackground(string theAssetName)
        {
            Sprite aBackgroundSprite = new Sprite();
            aBackgroundSprite.AssetName = theAssetName;

            mBackgroundSprites.Add(aBackgroundSprite);
        }

        #region Update and Draw
        //Update the position of the background images
        public void Update(GameTime theGameTime, int theSpeed, HorizontalScrollDirection theDirection)
        {           

            if (theDirection == HorizontalScrollDirection.Left)
            {
                //Check to see if any of the background sprites have moved off the screen
                //If they did, then move them to the right of the chain of scrolling backgrounds
                foreach (Sprite aBackgroundSprite in mBackgroundSprites)
                {
                    if (aBackgroundSprite.Position.X < mViewport.X - aBackgroundSprite.Size.Width)
                    {
                        aBackgroundSprite.Position = new Vector2(mRightMostSprite.Position.X + mRightMostSprite.Size.Width, mViewport.Y);
                        mLeftMostSprite = aBackgroundSprite;
                    }
                }
            }
            else if (theDirection == HorizontalScrollDirection.Right)
            {
                //Check to see if any of the background images have moved off the screen
                //if they have, then move them to the left of the chain of scrolling backgrounds
                foreach (Sprite aBackgroundSprite in mBackgroundSprites)
                {
                    if (aBackgroundSprite.Position.X > mViewport.X + mViewport.Width)
                    {
                        aBackgroundSprite.Position = new Vector2(mLeftMostSprite.Position.X - mLeftMostSprite.Size.Width, mViewport.Y);
                        mLeftMostSprite = aBackgroundSprite;
                    }
                }
            }
            if (theDirection == HorizontalScrollDirection.sLeft)
            {
                foreach (Sprite aBackgroundSprite in mBackgroundSprites)
                {
                    if (aBackgroundSprite.Position.X < mViewport.X - aBackgroundSprite.Size.Width)
                    {
                        aBackgroundSprite.Position = new Vector2(mRightMostSprite.Position.X + mRightMostSprite.Size.Width, mViewport.Y);
                        mRightMostSprite = aBackgroundSprite;
                    }
                }
            }
            else if (theDirection == HorizontalScrollDirection.sRight)
            {
                foreach (Sprite aBackgroundSprite in mBackgroundSprites)
                {
                    if (aBackgroundSprite.Position.X > mViewport.X + mViewport.Width)
                    {
                        aBackgroundSprite.Position = new Vector2(mLeftMostSprite.Position.X - mLeftMostSprite.Size.Width, mViewport.Y);
                        mLeftMostSprite = aBackgroundSprite;
                    }
                }
            }
            if (theDirection == HorizontalScrollDirection.mLeft)
            {
                foreach (Sprite aBackgroundSprite in mBackgroundSprites)
                {
                    if (aBackgroundSprite.Position.X < mViewport.X - aBackgroundSprite.Size.Width)
                    {
                        aBackgroundSprite.Position = new Vector2(mRightMostSprite.Position.X + mRightMostSprite.Size.Width, mViewport.Y);
                        mRightMostSprite = aBackgroundSprite;
                    }
                }
            }
            else if (theDirection == HorizontalScrollDirection.mRight)
            {
                foreach (Sprite aBackgroundSprite in mBackgroundSprites)
                {
                    if (aBackgroundSprite.Position.X > mViewport.X + mViewport.Width)
                    {
                        aBackgroundSprite.Position = new Vector2(mLeftMostSprite.Position.X - mLeftMostSprite.Size.Width, mViewport.Y);
                        mLeftMostSprite = aBackgroundSprite;
                    }
                }
            }
            if (theDirection == HorizontalScrollDirection.fLeft)
            {
                foreach (Sprite aBackgroundSprite in mBackgroundSprites)
                {
                    if (aBackgroundSprite.Position.X < mViewport.X - aBackgroundSprite.Size.Width)
                    {
                        aBackgroundSprite.Position = new Vector2(mRightMostSprite.Position.X + mRightMostSprite.Size.Width, mViewport.Y);
                        mRightMostSprite = aBackgroundSprite;
                    }
                }
            }
            else if (theDirection == HorizontalScrollDirection.fRight)
            {

                foreach (Sprite aBackgroundSprite in mBackgroundSprites)
                {
                    if (aBackgroundSprite.Position.X > mViewport.X + mViewport.Width)
                    {
                        aBackgroundSprite.Position = new Vector2(mLeftMostSprite.Position.X - mLeftMostSprite.Size.Width, mViewport.Y);
                        mLeftMostSprite = aBackgroundSprite;
                    }
                }
            }

            //Set the direction based on movement to the left or right that was passed in
            Vector2 aDirection = Vector2.Zero;
            if (theDirection == HorizontalScrollDirection.Left)
            {
                aDirection.X = -1f;
            }
            else if (theDirection == HorizontalScrollDirection.Right)
            {
                aDirection.X = 1f;
            }
            if (theDirection == HorizontalScrollDirection.sLeft)
            {
                aDirection.X = -.25f;
            }
            else if (theDirection == HorizontalScrollDirection.sRight)
            {
                aDirection.X = .25f;
            }
            if (theDirection == HorizontalScrollDirection.mLeft)
            {
                aDirection.X = -.75f;
            }
            else if (theDirection == HorizontalScrollDirection.mRight)
            {
                aDirection.X = .75f;
            }
            if (theDirection == HorizontalScrollDirection.fLeft)
            {
                aDirection.X = -2f;
            }
            else if (theDirection == HorizontalScrollDirection.fRight)
            {
                aDirection.X = 2f;
            }

            //Update the positions of each of the background sprites
            foreach (Sprite aBackgroundSprite in mBackgroundSprites)
            {
                aBackgroundSprite.Update(theGameTime, new Vector2(theSpeed, 0), aDirection);
            }
        }

        //Draw the background images to the screen
        public void Draw(SpriteBatch theSpriteBatch)
        {            
            foreach (Sprite aBackgroundSprite in mBackgroundSprites)
            {
                aBackgroundSprite.Draw(theSpriteBatch);
            }
        }
        #endregion
    }
}
