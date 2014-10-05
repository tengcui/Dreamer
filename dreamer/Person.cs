using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using dreamer.Level;

namespace dreamer
{
    public enum Direction
    {
        Up, Down, Left, Right
    }

    public class Person
    {
        #region Variable
        /// <summary>
        /// Graphics Device
        /// </summary>
        GraphicsDevice gd;

        /// <summary>
        /// Action List
        /// </summary>
        private Dictionary<string, Dictionary<int, Rectangle>> actionList;

        /// <summary>
        /// Person Texture
        /// </summary>
        private Texture2D personTexture;

        /// <summary>
        /// SpriteBatch
        /// </summary>
        private SpriteBatch spBatch;

        /// <summary>
        /// Current Action name
        /// </summary>
        string nowActionName = string.Empty;

        /// <summary>
        /// Current Action Index
        /// </summary>
        int nowActionIndex = 0;

        /// <summary>
        /// Action time span
        /// </summary>
        public int actionTimer = 10;

        /// <summary>
        /// current time passed
        /// </summary>
        int nowTimer = 0;

        /// <summary>
        /// Blood number
        /// </summary>
        int bloodNum = 100;
        public int BloodNum
        {
            get
            {
                return bloodNum;
            }
            set
            {
                bloodNum = value;
                if (bloodNum <= 0)
                {
                    //Active Ninja Died event
                    if (this.OnNinjaDead != null)
                    {
                        this.OnNinjaDead(this, null);
                    }
                }
            }
        }
        /// <summary>
        /// Ninja Died Event
        /// </summary>
        public event EventHandler OnNinjaDead;
        /// <summary>
        /// Attack Object
        /// </summary>
        public Attack attack;

        ///<summary>
        ///Ninja Get Hit
        /// </summary>
        public bool canGetHit = true;
        /// <summary>
        /// Person Size
        /// </summary>
        public Rectangle rect;

        /// <summary>
        /// blood Background
        /// </summary>
        Texture2D bloodBg;

        /// <summary>
        /// Blood
        /// </summary>
        Texture2D bloodValue;

        /// <summary>
        /// Face
        /// </summary>
        Texture2D face;

        /// <summary>
        /// X coordinate of Person's position
        /// </summary>
        public int personX = 350;

        /// <summary>
        /// Y coordinate of Person's position
        /// </summary>
        public int personY = 500;

        /// <summary>
        /// Action Name
        /// </summary>
        public string actionName;

        /// <summary>
        /// Person Color Array
        /// </summary>
        Color[] colorPerson;

        /// <summary>
        /// If is Jumping
        /// </summary>
        public static bool isJump = false;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        public Person(GraphicsDevice gd, SpriteBatch spriteBatch, Texture2D personAction, string actionListfileName, Texture2D faceFile)
        {
             
            this.gd = gd;

             
            spBatch = spriteBatch;

             
            this.personTexture = personAction;

           
            this.bloodBg = Resource.AddTexture2D(gd, "Graphics\\B2");

          
            this.bloodValue = Resource.AddTexture2D(gd, "Graphics\\B1");

           
            this.face = faceFile;

            //Create Action list
            actionList = new Dictionary<string, Dictionary<int, Rectangle>>();

            //Load action list file
            LoadActionList(actionListfileName);

            //Set person's color array size
            colorPerson = new Color[personTexture.Width * personTexture.Height];

            //Get People texture color to color array
            personTexture.GetData(colorPerson);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load Action List
        /// </summary>
        private void LoadActionList(string fileName)
        {
            //Action Name
            string actionName = string.Empty;

            //Read Stream
            StreamReader sr = new StreamReader(fileName, Encoding.UTF8);

            //Series Action Image List
            Dictionary<int, Rectangle> indexPicture = null;

            //Infinite Loop
            while (true)
            {

            //Begin Read mark
            BeginRead:

                //Content
                string content = string.Empty;

                //Try
                try
                {

                    //If Read Stream finish
                    if (sr.EndOfStream)

                        //Jump to end read mark
                        goto EndRead;

                    //else
                    else
                    {

                        // If Image list not null and the data in image list is not 0
                        if (indexPicture != null && indexPicture.Count != 0)
                            //If has not added this action then add it
                            if (!actionList.ContainsKey(actionName))

                                //Add image list and action name to action list
                                actionList.Add(actionName, indexPicture);

                    }

                    //Read one line data and save to content variable
                    content = sr.ReadLine().Trim();

                    //If content is '<'
                    if (content == "<")
                    {

                        //Create Image List
                        indexPicture = new Dictionary<int, Rectangle>();

                        //Image index default is 1
                        int picIndex = 1;

                        //Infinite Loop
                        while (true)
                        {

                            //Read one line data and save to content variable
                            content = sr.ReadLine().Trim();

                            //If the first symbol in content string is % or null
                            if (content == "" || content[0] == '%')

                                //Go back
                                continue;

                            //If the first symbol in the content string is !
                            if (content[0] == '!')
                            {

                                //Save every charactor in to action name except !
                                actionName = content.Trim().Substring(1, content.Trim().Length - 1);

                                //Global Action name save local action name
                                this.actionName = actionName;

                                //Go back
                                continue;

                            }

                            //If content is '>'
                            if (content == ">")

                                //Jump to begin read mark to read next action series
                                goto BeginRead;

                            //else
                            else
                            {
                                //Rectangle array
                                int[] actionNum = new int[4];

                                //coordinate has been read, so assign X value
                                actionNum[0] = int.Parse(content);

                                //next line of Y coordinate Read stream 
                                actionNum[1] = int.Parse(sr.ReadLine().Trim());

                                //Width's read stream next line
                                actionNum[2] = int.Parse(sr.ReadLine().Trim());

                                //Height's read stream next line
                                actionNum[3] = int.Parse(sr.ReadLine().Trim());

                                //Add action image rectangle to action image list
                                indexPicture.Add(picIndex, new Rectangle(actionNum[0], actionNum[1], actionNum[2], actionNum[3]));

                                //Action Image index++
                                picIndex++;

                            }
                        }
                    }
                }

                //If have error
                catch
                {

                    //Jump to end mark
                    goto EndRead;

                }
            }
        //read stream end mark
        EndRead:

            //close read stream
            sr.Close();

            //Collect read stream resources
            sr.Dispose();
        }

        /// <summary>
        /// Draw
        /// </summary>
        public virtual void Draw(Direction direction)
        {
            
            try
            {
                //Current Person Action image size
                rect = (actionList[actionName])[nowActionIndex + 1];

                //Action interval ++
                nowTimer++;

                //If action interval = current time interval
                if (actionTimer == nowTimer)
                {

                    //Current time interval is 0
                    nowTimer = 0;

                    //If current image index equal to the last image of this action series
                    if (nowActionIndex == actionList[actionName].Count - 1)

                        //Clear current action index
                        nowActionIndex = 0;

                    
                    else

                        //Current action index++
                        nowActionIndex++;

                }

                SpriteEffects se = SpriteEffects.None;

                //If forward direction is left
                if (direction == Direction.Left)
                {

                    //sprite effects
                    se = SpriteEffects.FlipHorizontally;

                }

          
                spBatch.Begin();

                //Draw person
                spBatch.Draw(personTexture, new Rectangle(personX, personY, (int)(rect.Width * 1.2), (int)(rect.Height * 1.2)), new Rectangle(rect.X, rect.Y, rect.Width, rect.Height), Color.White, 0, new Vector2(), se, 0);

           
                spBatch.End();
            }
            //error
            catch
            {
                //Clear current action index
                nowActionIndex = 0;
            }
        }

        /// <summary>
        /// Draw Blood
        /// </summary>
        public virtual void DrawArticleBlood()
        {

         
            spBatch.Begin();

            //blood background
            spBatch.Draw(bloodBg, new Rectangle(0, 0, bloodBg.Width, bloodBg.Height), Color.White);

            //Draw Blood
            spBatch.Draw(bloodValue, new Rectangle(65, 30, (int)(bloodValue.Width * (BloodNum * 0.01)), bloodValue.Height), Color.White);

          
            spBatch.End();

        }

        /// <summary>
        /// Draw person's face
        /// </summary>
        public virtual void DrawFace()
        {

         
            spBatch.Begin();

            //Draw person's face
            spBatch.Draw(face, new Rectangle(2, 20, face.Width, face.Height), Color.White);

            
            spBatch.End();

        }

        /// <summary>
        /// Move
        /// </summary>
        public virtual void Move(KeyboardState ks)
        {
            if (personY < 0)
            {
                personY = 0;
                BloodNum -= 5;
            }
            for (int i = 0; i <= Map.mapNum; i++)
            {
            Check:
                if (Physics.IntersectPixels(new Rectangle(personX, personY, rect.Width, rect.Height), colorPerson, new Rectangle(Map.startX[i], Map.startY[i], gd.Viewport.Width, gd.Viewport.Height), Map.colorMap))
                {
                    Person.isJump = false;
                    personY++;
                    goto Check;
                }
            CheckOne:
                if (Physics.IntersectPixels(new Rectangle(personX, personY, rect.Width, rect.Height), colorPerson, new Rectangle(Map.startX[i], Map.startY[i], Map.mapTexture.Width, Map.mapTexture.Height), Map.colorMap))
                {
                    Person.isJump = false;
                    personY--;
                    goto CheckOne;
                }
            }
            if (personY >= gd.Viewport.Height)
                personY = 0;
        }

        public virtual void Move(KeyboardState ks, TimeSpan gameTime)
        {
            #region Other update
            if (attack != null)
            {
                attack.Update(gameTime);
            }
            #endregion
            Move(ks);
        }

        #endregion
    }
}
