using System;
using System.Collections.Generic;
using System.Linq;
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
    
    public class Game_Main : Microsoft.Xna.Framework.Game
    {
        #region Fields

        const int BLOOD_TIME = 1000; //blood time 
           
        GraphicsDeviceManager graphics;
        ContentManager cm;
        SpriteBatch spriteBatch;       
        Person person;
        Song song;
        SoundEffect sounds;
        SoundEffect defense;
        SoundEffect sAttack;
        SoundEffect sCrouch;
        SoundEffect sLarge;
        SoundEffect sStandAtt;
        SpriteFont scoreChicsa;
        Menu menu;
        MonsterManager monsterManager;
        //Added Sprite class and scrolling background
        Sprite bMain;
        ScrollingBackground mParallaxone;
        ScrollingBackground mParallaxtwo;
        ScrollingBackground mParallaxthree;

        Texture2D dragonText;
        Texture2D bloodText;
        List<blood> bloodList;
        Boss boss1;

        private static Direction personDirection;
        public static Direction PersonDirection
        {
            get { return personDirection; }
        }
        #endregion

        public Game_Main()
        {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
        }

        #region Initialization
        protected override void Initialize()
        {
            bMain = new Sprite();
            base.Initialize();
        }

       
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            dragonText = Content.Load<Texture2D>("Graphics//dragonwarriorb");
            bloodText = Content.Load<Texture2D>("Graphics//blood");
            bloodList = new List<blood>();
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //BaseSupport 
            BaseSupport.Instance.Initialize(this.graphics, this.Content, this.spriteBatch);
           
            menu = new Menu(Content, spriteBatch, graphics, MenuType.MainMenu);
            
            
            bMain.LoadContent(this.Content, "Graphics//BackgroundImages//Main1");
            bMain.Position = new Vector2(0, 0);

            Map.SetMapTexture(Resource.AddTexture2D(GraphicsDevice, "Graphics\\BackgroundImages\\NewB2"));

            defense = Resource.AddSounds(this.Content, "swordecho");
            for (int i = 0; i < 1; i++) Map.AddMap();

            mParallaxthree = new ScrollingBackground(this.GraphicsDevice.Viewport);
            mParallaxtwo = new ScrollingBackground(this.GraphicsDevice.Viewport);
            mParallaxone = new ScrollingBackground(this.GraphicsDevice.Viewport);
            mParallaxthree.AddBackground("Graphics//BackgroundImages//pSlow");
            mParallaxtwo.AddBackground("Graphics//BackgroundImages//pMid");
            mParallaxone.AddBackground("Graphics//BackgroundImages//pFast");
            //Song change to Impact         
            song = Resource.AddSong(Content, "Impact");
            sAttack = Resource.AddSounds(Content, "sAttack");
            sCrouch = Resource.AddSounds(Content, "sCrouch");
            sLarge = Resource.AddSounds(Content, "sLarge");
            sStandAtt = Resource.AddSounds(Content, "sStandAtt");
           //load some player actions
            person = new Person(GraphicsDevice, spriteBatch, Resource.AddTexture2D(GraphicsDevice, "Graphics\\NormalNinja"), @"Content\Data\Main.txt", Resource.AddTexture2D(GraphicsDevice, "Graphics\\rz"))
            {
                actionName = "Attack",
                attack = new Attack(Level.MonsterManagerConfig.attack_TimeSpan_Ninja)
            };
            person.OnNinjaDead += (object sender, EventArgs e) =>
                {
                    //Added music to when you die
                    MediaPlayer.Stop();
                    song = Resource.AddSong(Content, "Testament");
                    MediaPlayer.Play(song);
                    //Changed text to reset
                    System.Windows.Forms.MessageBox.Show("You have died. Press 'OK' to go to the main menu!", "Death!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    System.Windows.Forms.Application.Restart();
                };

            scoreChicsa = Content.Load<SpriteFont>("Font\\ScoreChicsa");
            personDirection = Direction.Right;
            boss1 = new Boss(this, dragonText, spriteBatch, person);
            
            monsterManager = new MonsterManager(person);
            monsterManager.OnAllMonstersDead += (object sender, EventArgs e) =>
                {
                    
                    //Added music to when you win
                    MediaPlayer.Stop();
                    song = Resource.AddSong(Content, "Finish");
                    MediaPlayer.Play(song);
                    //Changed text to the demo stuff 
                    System.Windows.Forms.MessageBox.Show("You destoyed the main force and completed the demo! Please purchase game for the full version.", "Good Work!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    System.Windows.Forms.Application.Restart();
                };
            mParallaxthree.LoadContent(this.Content);
            mParallaxtwo.LoadContent(this.Content);
            mParallaxone.LoadContent(this.Content);
        }
        protected override void UnloadContent()
        {
        }
        #endregion

        #region Update and Draw

                protected override void Update(GameTime gameTime)
        {
            //check boss:
            if (boss1.death)
            {
                if (System.Environment.TickCount - boss1.deathStartTime >= 3000)
                { 
                    boss1.death = false; boss1.health = 50;
                    boss1.position.X = 100;
                    boss1.position.Y = 50;
                }
            
            }
            //update blood:
            for (int i = 0; i <= bloodList.Count - 1; i++)
            {
                if (System.Environment.TickCount - bloodList[i].startingTick >= BLOOD_TIME)
                {
                    bloodList[i].Dispose();
                    bloodList[i] = null;
                    bloodList.RemoveAt(i);
                }
            }

           if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Menu.isMenuDisplay = true;
            #region ϵͳ
            if ((Keyboard.GetState().IsKeyDown(Keys.LeftAlt) || Keyboard.GetState().IsKeyDown(Keys.RightAlt)) && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                graphics.IsFullScreen = !graphics.IsFullScreen;

                if (graphics.IsFullScreen)
                {
                    graphics.PreferredBackBufferWidth = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
                    graphics.PreferredBackBufferHeight = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
                    for (int i = 0; i <= Map.mapNum; i++) Map.startY[i] = graphics.PreferredBackBufferHeight - Map.mapTexture.Height;
                }
                else
                {
                    graphics.PreferredBackBufferWidth = 800;
                    graphics.PreferredBackBufferHeight = 600;
                    for (int i = 0; i <= Map.mapNum; i++) Map.startY[i] = 0;
                }
                graphics.ApplyChanges();
            }
            #endregion
            if (Menu.isMenuDisplay)
            {
                menu.GetKeyboerdState(Keyboard.GetState());
                return;
            }
            //the music change
            #region Media            
            if (person.BloodNum <= 50 && song.Name == @"Sound\Impact")
            {
                MediaPlayer.Stop();             
                song = Resource.AddSong(Content, "NearDeath");
            }

           
            if (MediaPlayer.State == MediaState.Stopped)                
                MediaPlayer.Play(song);
            #endregion

            #region Person

            person.personY += 2;
            person.Move(Keyboard.GetState(), gameTime.ElapsedGameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && Map.startX[Map.mapNum] > -Map.mapTexture.Width + graphics.PreferredBackBufferWidth + person.actionTimer)
            {
                if (person.personX != graphics.PreferredBackBufferWidth / 2 - person.rect.Width / 2)
                {
                    if (person.personX > graphics.PreferredBackBufferWidth / 2 - person.rect.Width / 2)
                    {
                        person.personX--;
                        boss1.position.X -= 2;
                    }
                    if (person.personX < graphics.PreferredBackBufferWidth / 2 - person.rect.Width / 2)
                    {
                        person.personX++;
                        boss1.position.X -= 2;
                    }
                }
                //Add the update to when you press left or right
                mParallaxthree.Update(gameTime, 160, ScrollingBackground.HorizontalScrollDirection.sLeft);
                mParallaxtwo.Update(gameTime, 160, ScrollingBackground.HorizontalScrollDirection.mLeft);
                mParallaxone.Update(gameTime, 160, ScrollingBackground.HorizontalScrollDirection.fLeft);

                for (int i = 0; i <= Map.mapNum; i++) Map.startX[i] -= person.actionTimer;
                monsterManager.MoveMonsterByBackground(-person.actionTimer);
                personDirection = Direction.Right;
                person.actionName = "Run";                
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) && Map.startX[0] < 0 - person.actionTimer)
            {
                if (person.personX != graphics.PreferredBackBufferWidth / 2 - person.rect.Width / 2)
                {
                    if (person.personX > graphics.PreferredBackBufferWidth / 2 - person.rect.Width / 2)
                    {
                        person.personX--;
                        boss1.position.X -= 2;

                    }
                    if (person.personX < graphics.PreferredBackBufferWidth / 2 - person.rect.Width / 2)
                    {
                        person.personX++;
                        boss1.position.X -= 2;

                    }
                }
                mParallaxthree.Update(gameTime, 160, ScrollingBackground.HorizontalScrollDirection.sRight);
                mParallaxtwo.Update(gameTime, 160, ScrollingBackground.HorizontalScrollDirection.mRight);
                mParallaxone.Update(gameTime, 160, ScrollingBackground.HorizontalScrollDirection.fRight);

                for (int i = 0; i <= Map.mapNum; i++) Map.startX[i] += person.actionTimer;
                monsterManager.MoveMonsterByBackground(person.actionTimer);
                personDirection = Direction.Left;
                person.actionName = "Run";
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (person.personX > 0)
                    person.personX -= 5;
                else
                    person.personX = 0;
                personDirection = Direction.Left;
                person.actionName = "Run";
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (person.personX < graphics.PreferredBackBufferWidth - person.rect.Width)
                    person.personX += 5;
                else
                    person.personX = graphics.PreferredBackBufferWidth - person.rect.Width;
                personDirection = Direction.Right;
                person.actionName = "Run";
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                if (Person.isJump)
                {                  
                  return;
                }
                else
                {
                    sounds = Resource.AddSounds(Content, "Phase");
                    sounds.Play();
                    person.actionName = "Jump";
                    person.personY -= 200;
                    Person.isJump = true;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                person.actionName = "Crouch";
                sCrouch.Play();
                
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                person.actionName = "Defense";
                
                person.canGetHit = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.S))
            {
                person.canGetHit = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                person.actionName = "Attack";
                // person attack boss, creating blood
                if (Vector2.Distance(boss1.position, new Vector2(person.personX, person.personY)) < 100)
                {
                    boss1.health -= 10;
                    bloodList.Add(new blood(this, boss1.position, ref bloodText, ref  spriteBatch));
                }
                sAttack.Play();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                person.actionName = "StandAtt";
                sStandAtt.Play();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                person.actionName = "RunAtt";
                sLarge.Play();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                person.actionName = "CrouchAtt";
                sCrouch.Play();
            }
            if (Keyboard.GetState().GetPressedKeys().Length == 0)
                person.actionName = "Stand";
            #endregion

            #region Monster
            this.monsterManager.Update(gameTime.ElapsedGameTime);
            //comeHere.Play();

            
            this.boss1.Update(gameTime);
            #endregion
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw Content
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            if (Menu.isMenuDisplay)
            {
                menu.DrawMenu();
                return;
            }

            
            GraphicsDevice.Clear(Color.CornflowerBlue);

          
            bMain.Draw(this.spriteBatch);            
            mParallaxthree.Draw(this.spriteBatch);
            mParallaxtwo.Draw(this.spriteBatch);
            Map.Draw(spriteBatch, graphics);
            
            person.Draw(personDirection);            
            person.DrawFace();            
            person.DrawArticleBlood();
            monsterManager.Draw();
            mParallaxone.Draw(this.spriteBatch);
            DrawHud();
            this.boss1.Draw();
            for (int i = 0; i <= bloodList.Count - 1; i++)
                bloodList[i].Draw();
            base.Draw(gameTime);
                       
        }

        private void DrawHud()
        {
            //Rectangle titleSafeArea = GraphicsDevice.Viewport.TitleSafeArea;
            //Vector2 hudLocation = new Vector2(titleSafeArea.X, titleSafeArea.Y);
            //Vector2 center = new Vector2(titleSafeArea.X + titleSafeArea.Width / 2.0f,
            //                             titleSafeArea.Y + titleSafeArea.Height / 2.0f);

            //// Draw time remaining. Uses modulo division to cause blinking when the
            //// player is running out of time.
            //string timeString = "TIME: " + level.TimeRemaining.Minutes.ToString("00") + ":" + level.TimeRemaining.Seconds.ToString("00");
            spriteBatch.Begin();
            spriteBatch.DrawString(scoreChicsa, "Score:" + monsterManager.MonsterKilledNumber.ToString(), new Vector2(0, 60), Color.PowderBlue);
            spriteBatch.End();
        }
        #endregion
    }
}
