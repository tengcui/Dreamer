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
using dreamer.AI;
using dreamer.Level;

namespace dreamer
{
    public class Monster
    {
        #region Variable
        GraphicsDevice gd;

        /// <summary>
        /// ActionList
        /// TKey is Action Name,
        /// TValue is Picture Rectangle
        /// </summary>s
        private Dictionary<string, Dictionary<int, Rectangle>> actionList;

        /// <summary>
        /// Texture
        /// </summary>
        private Texture2D texture2d;

        /// <summary>
        /// Sprite Batch
        /// </summary>
        private SpriteBatch spBatch;

        /// <summary>
        /// Current Action List
        /// </summary>
        string nowActionName = string.Empty;

        /// <summary>
        /// Current Action Index
        /// </summary>
        int nowActionIndex = 0;

        /// <summary>
        /// Action interval
        /// </summary>
        public int actionTimer = 10;

        /// <summary>
        /// Current time passed
        /// </summary>
        int nowTimer = 0;

        /// <summary>
        /// Blood value
        /// </summary>
        int monsterBloodNum = 100;
        public int MonsterBloodNum
        {
            get
            {
                return monsterBloodNum; ;
            }
            set
            {
                monsterBloodNum = value;
                
                if (monsterBloodNum <= 0)
                {
                    
                    if (this.OnMonsterDead != null)
                    {
                        this.OnMonsterDead(this, null);
                    }
                }
            }
        }
        /// <summary>
        /// Monster dead event
        /// </summary>
        public event EventHandler OnMonsterDead;
        /// <summary>
        /// Attack object
        /// </summary>
        public Attack attack;

        /// <summary>
        /// Monster image size
        /// </summary>
        public Rectangle rect;

        /// <summary>
        /// Health Background Texture
        /// </summary>
        Texture2D bloodBg;

        /// <summary>
        /// Blood Texture
        /// </summary>
        Texture2D bloodValue;

        /// <summary>
        /// Face Texture
        /// </summary>
        Texture2D face;

        /// <summary>
        /// Monster X Position
        /// </summary>
        public int monsterX = 350;

        /// <summary>
        /// Monster Y position
        /// </summary>
        public int monsterY = 500;

        /// <summary>
        /// Action Name
        /// </summary>
        public string actionName;

        /// <summary>
        /// Monster Image Color array
        /// </summary>
        Color[] colorMonster;

        /// <summary>
        /// Current monster direction
        /// </summary>
        public Direction currentDirection = Direction.Left;

        /// <summary>
        /// Current monster behavior
        /// </summary>
        public ChaseBehavior currentBehavior;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        public Monster(GraphicsDevice gd, ContentManager content, SpriteBatch spriteBatch, Texture2D monsterAction, string actionListfileName, Texture2D faceFile)
        {
            //Set Graphic device
            this.gd = gd;

            //set SpriteBatch
            spBatch = spriteBatch;

            //Instantiate Action List
            actionList = new Dictionary<string, Dictionary<int, Rectangle>>();

            //Set Monster Action
            this.texture2d = monsterAction;

            //Set Blood background texture
            this.bloodBg = Resource.AddTexture2D(gd, "Graphics\\B2");

            //Set Blood Texture
            this.bloodValue = Resource.AddTexture2D(gd, "Graphics\\B1");

            //Set Monster face texture
            this.face = faceFile;

            //Load Action List
            LoadActionList(actionListfileName);

            //Set Monster X position
            this.monsterX = new Random((int)DateTime.Now.Ticks).Next(20, Map.mapTexture.Width * Map.mapNum - 20);

            //Set Monster Image color array
            colorMonster = new Color[texture2d.Width * texture2d.Height];

            //Get color array assign to monster image color array
            texture2d.GetData(colorMonster);
        }

        #endregion

        #region Method

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
        public virtual void Draw()
        {
            try
            {
                rect = (actionList[actionName])[nowActionIndex + 1];
                nowTimer++;
                if (actionTimer == nowTimer)
                {
                    nowTimer = 0;
                    if (nowActionIndex == actionList[actionName].Count - 1)
                        nowActionIndex = 0;
                    else
                        nowActionIndex++;
                }
                SpriteEffects se = SpriteEffects.None;
                if (currentDirection == Direction.Left)
                {
                    se = SpriteEffects.FlipHorizontally;
                }
                spBatch.Begin();
                spBatch.Draw(texture2d, new Rectangle(monsterX, monsterY, (int)(rect.Width * 1.2), (int)(rect.Height * 1.2)), new Rectangle(rect.X, rect.Y, rect.Width, rect.Height), Color.White, 0, new Vector2(), se, 0);
                spBatch.End();
            }
            catch (Exception e)
            {
                nowActionIndex = 0;
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Draw Blood
        /// </summary>
        public virtual void DrawArticleBlood()
        {
            spBatch.Begin();
            spBatch.Draw(bloodBg, new Rectangle(410, 0, bloodBg.Width, bloodBg.Height),  new Rectangle(0,0,bloodBg.Width, bloodBg.Height), Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            spBatch.Draw(bloodValue, new Rectangle(460, 30, (int)(bloodValue.Width * (MonsterBloodNum * 0.01)), bloodValue.Height), Color.White);
            spBatch.End();
        }

        /// <summary>
        /// Draw Face
        /// </summary>
        public virtual void DrawFace()
        {
            spBatch.Begin();
            spBatch.Draw(face, new Rectangle(753, 17, face.Width, face.Height), Color.White);
            spBatch.End();
        }

        /// <summary>
        /// Move
        /// </summary>
        public virtual void Move(KeyboardState ks)
        {
            for (int i = 0; i <= Map.mapNum; i++)
            {
            Check:
            //Monster cannot get down  Bug
            //if (Physics.IntersectPixels(new Rectangle(monsterX, monsterY, rect.Width, rect.Height), colorMonster, new Rectangle(Map.startX[i], Map.startY[i], gd.Viewport.Width, gd.Viewport.Height), Map.colorMap))
            //{
            //    monsterY++;
            //    goto Check;
            //}
            CheckOne:
                if (Physics.IntersectPixels(new Rectangle(monsterX, monsterY, rect.Width, rect.Height), colorMonster, new Rectangle(Map.startX[i], Map.startY[i], Map.mapTexture.Width, Map.mapTexture.Height), Map.colorMap))
                {
                    monsterY--;
                    goto CheckOne;
                }
                else
                {
                    monsterY++;
                }
            }
            //monsterX += new Random().Next(-100, 100);
            if (monsterX <= 0)
                monsterX = new Random().Next(1, Map.mapTexture.Width);
            if (monsterX >= Map.mapTexture.Width * Map.mapNum)
                monsterX = Map.mapTexture.Width * Map.mapNum - new Random().Next(Map.mapTexture.Width, Map.mapTexture.Width * 2);
        }

        public virtual void Move(KeyboardState ks, TimeSpan gameTime)
        {
            //Behavior object update
            this.currentBehavior.Update(gameTime);
            this.Move(ks);

            //Attack object update
            if (this.attack != null)
            {
                this.attack.Update(gameTime);
            }
        }


        #endregion
    }
}
