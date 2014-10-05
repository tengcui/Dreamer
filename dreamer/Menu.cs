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

namespace dreamer
{
    /// <summary>
    /// Menu Type
    /// </summary>
    public enum MenuType
    {
        /// <summary>
        /// Main menu
        /// </summary>
        MainMenu,
        /// <summary>
        /// Option Menu
        /// </summary>
        OptionsMenu,
        /// <summary>
        /// Producer Menu
        /// </summary>
        ProducerMenu,
        /// <summary>
        /// Pause Menu
        /// </summary>
        PauseMenu
    }

    /// <summary>
    /// Menu class
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// If inside of Menu
        /// </summary>
        public static bool isMenuDisplay = true;

        /// <summary>
        /// Content Manager
        /// </summary>
        ContentManager cm = null;

        /// <summary>
        /// Sprite Batch
        /// </summary>
        SpriteBatch sp = null;

        /// <summary>
        /// Graphics Device Manager
        /// </summary>
        GraphicsDeviceManager gdm = null;

        /// <summary>
        /// Menu Type
        /// </summary>
        MenuType mt;

        ///<summary>
        ///Menu selection sounds effect
        ///</summary>
        SoundEffect menuSelect;
        SoundEffect gameStart;
        
        /// <summary>
        /// Key value to judge time span
        /// </summary>
        int keyTimer = 5;

        /// <summary>
        /// Current time passed(for key value judge)
        /// </summary>
        int nowKeyTimer = 0;

        /// <summary>
        /// Menu Item Index
        /// </summary>
        int menuItemIndex = 1;

        //Set all sounds that will be use to null        
        Song mainSong = null;
        Song altSong = null;
        Song selectSound = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cm">Content Manager</param>
        /// <param name="sp">Sprite Batch</param>
        /// <param name="gdm">Graphics Devices Manager</param>
        /// <param name="menuType">Menu Type</param>
        public Menu(ContentManager cm, SpriteBatch sp, GraphicsDeviceManager gdm, MenuType menuType)
        {
            //Set Content Manager
            this.cm = cm;

            //Set Sprite Batch
            this.sp = sp;

            //Set Graphics Device Manager
            this.gdm = gdm;

            //Set Menu Type
            mt = menuType;

            //If is Main menu
            if (menuType == MenuType.MainMenu)
            {
                //Main menu load method
                MainMenuLoad();
            }
        }

        /// <summary>
        /// Main menu load resource
        /// </summary>
        public void MainMenuLoad()
        {
            //Adding main menu and akternate song
            mainSong = Resource.AddSong(cm, "MainMenu1");
            altSong = Resource.AddSong(cm, "MainMenu2");
            menuSelect = Resource.AddSounds(cm, "MenuSelect");
            gameStart = Resource.AddSounds(cm, "Start");
            MediaPlayer.Play(mainSong);
        }

        /// <summary>
        /// Draw Menu
        /// </summary>
        public void DrawMenu()
        {
            //Draw Main menu
            DrawMainMenu();
        }

        /// <summary>
        /// Get keyboard State
        /// </summary>
        /// <param name="keyboardState">Current Keyboard State</param>
        public void GetKeyboerdState(KeyboardState keyboardState)
        {
            //If keyboard time span does not equal to passed time, then jump to Judge key passed time span
            if (nowKeyTimer != keyTimer) goto IsKeyTimer;

            //If pressed "Down"
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                menuSelect.Play();
                //If Menu Type is Main Menu or Pause Menu
                if (mt == MenuType.MainMenu || mt == MenuType.PauseMenu)
                {
                    //If Current Menu Index is 4
                    if (menuItemIndex == 4)
                    {
                        //Set Menu Item Index 1
                        menuItemIndex = 1;
                    }
                    else
                    {
                        //Current Menu Item Index++
                        menuItemIndex++;
                    }
                }
            }
            //If Judgement are not correct, then judge if Pressed "UP"
            else if (keyboardState.IsKeyDown(Keys.Up))
            {
                menuSelect.Play();
                //If Current Menu is Main Menu or Pause Menu
                if (mt == MenuType.MainMenu || mt == MenuType.PauseMenu)
                {

                    //If current Menu Index is 1
                    if (menuItemIndex == 1)
                    {
                        //Set Menu Index 4
                        menuItemIndex = 4;
                    }
                    else
                    {
                        //1Menu Index --
                        menuItemIndex--;
                    }
                }
            }

            //If pressed "Enter" without enter "Alt" ,then Active
            if (keyboardState.IsKeyDown(Keys.Enter) && !keyboardState.IsKeyDown(Keys.LeftAlt) && !keyboardState.IsKeyDown(Keys.RightAlt))
            {
                //If Menu type is Main Menu
                if (mt == MenuType.MainMenu)
                {
                    //Changed the different thing that will happen if you chose the second two options
                    //Judge Menu index
                    switch (menuItemIndex)
                    {
                        case 1:
                            gameStart.Play();
                            MediaPlayer.Stop();
                            //If is false in Menu display
                            Menu.isMenuDisplay = false;
                            
                            break;
                        case 2:
                            MediaPlayer.Stop();
                            MediaPlayer.Play(altSong);
                            if (MediaPlayer.State == MediaState.Stopped)
                                MediaPlayer.Play(mainSong);
                            break;
                        case 3:
                            System.Windows.Forms.MessageBox.Show("Tung Cui, advised by Professor Yechiam Yemini", "Produced By", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                            break;
                        case 4:
                            //Exit
                            System.Windows.Forms.Application.Exit();
                            break;
                    }
                }
            }

        //Judge If passed key judge time span
        IsKeyTimer:
            //If passed time span
            if (keyTimer == nowKeyTimer)
                //Clear current passed time
                nowKeyTimer = 0;
            else
                //Current Passed time ++
                nowKeyTimer++;
        }

        //Draw Main menu
        private void DrawMainMenu()
        {
            //If Menu type is not Main menu then return
            if (mt != MenuType.MainMenu) return;

            //Define screen width
            int screenWidth = gdm.GraphicsDevice.Viewport.Width;

            //Define screen Height
            int screenHeight = gdm.GraphicsDevice.Viewport.Height;

            //Begin draw
            sp.Begin();

            //Main Menu picture was changed to twin swords
            //Draw Main menu background
            sp.Draw(Resource.AddTexture2D(gdm.GraphicsDevice, "Ui\\MainMenu"), new Rectangle(0, 0, screenWidth, screenHeight), Color.White);

            //Define recrangle and instantiate
            Rectangle rect = new Rectangle();

            //i adjusted the Labels and such to change the main menu
            //Judge current menu index
            switch (menuItemIndex)
            {
                case 1:
                    //Assign new value to the rectangle
                    rect = new Rectangle((int)(screenWidth * 0.58), (int)(screenHeight * 0.4), Resource.AddTexture2D(gdm.GraphicsDevice, "Ui\\CrossDart").Width, Resource.AddTexture2D(gdm.GraphicsDevice, "Ui\\CrossDart").Height);
                    break;
                case 2:
                    //Assign new value to the rectangle
                    rect = new Rectangle((int)(screenWidth * 0.44), (int)(screenHeight * 0.55), Resource.AddTexture2D(gdm.GraphicsDevice, "Ui\\CrossDart").Width, Resource.AddTexture2D(gdm.GraphicsDevice, "Ui\\CrossDart").Height);
                    break;
                case 3:
                    //Assign new value to the rectangle
                    rect = new Rectangle((int)(screenWidth * 0.48), (int)(screenHeight * 0.7), Resource.AddTexture2D(gdm.GraphicsDevice, "Ui\\CrossDart").Width, Resource.AddTexture2D(gdm.GraphicsDevice, "Ui\\CrossDart").Height);
                    break;
                case 4:
                    //Assign new value to the rectangle
                    rect = new Rectangle((int)(screenWidth * 0.58), (int)(screenHeight * 0.85), Resource.AddTexture2D(gdm.GraphicsDevice, "Ui\\CrossDart").Width, Resource.AddTexture2D(gdm.GraphicsDevice, "Ui\\CrossDart").Height);
                    break;
            }

            //Changed the options and changed the layout
            //Draw title
            sp.DrawString(gdm.IsFullScreen == true ? Resource.AddFont(cm, "TitleChicsaBig") : Resource.AddFont(cm, "TitleChicsa"), "D.R.E.A.M Project", new Vector2((int)(screenWidth * 0.0), (int)(screenHeight * 0.1)), Color.White);

            //Draw Start game
            sp.DrawString(Resource.AddFont(cm, "MenuItemChicsa"), "Start", new Vector2((int)(screenWidth * 0.7), (int)(screenHeight * 0.42)), Color.SkyBlue);

            //Draw Alternate Song
            sp.DrawString(Resource.AddFont(cm, "MenuItemChicsa"), "Alternate Song", new Vector2((int)(screenWidth * 0.53), (int)(screenHeight * 0.57)), Color.SkyBlue);

            //Draw Producer Name
            sp.DrawString(Resource.AddFont(cm, "MenuItemChicsa"), "Producer", new Vector2((int)(screenWidth * 0.65), (int)(screenHeight * 0.72)), Color.SkyBlue);

            //Draw Exit
            sp.DrawString(Resource.AddFont(cm, "MenuItemChicsa"), "Quit", new Vector2((int)(screenWidth * 0.7), (int)(screenHeight * 0.87)), Color.SkyBlue);

            //Draw logo
            sp.Draw(Resource.AddTexture2D(gdm.GraphicsDevice, "Ui\\CrossDart"), rect, Color.White);

            //End Draw
            sp.End();
        }
    }
}
