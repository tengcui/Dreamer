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
    public class Map
    {
        /// <summary>
        /// The number of map extend
        /// </summary>
        public static int mapNum = 0;

        /// <summary>
        /// Map texture
        /// </summary>
        public static Texture2D mapTexture;

        /// <summary>
        /// Background texture
        /// </summary>
        public static Texture2D mapBgImageTexture;

        /// <summary>
        /// Set Map texture
        /// </summary>
        /// <param name="texture">texture</param>
        public static void SetMapTexture(Texture2D texture)
        {
            //set texture
            mapTexture = texture;

            //Instantiate and set array size
            colorMap = new Color[mapTexture.Width * mapTexture.Height];

            //Get color array and assign the value
            mapTexture.GetData(colorMap);
        }

        /// <summary>
        /// Increase the number of maps
        /// </summary>
        public static void AddMap()
        {
            mapNum++;
            startX[mapNum] = mapTexture.Width * mapNum;
        }

        /// <summary>
        /// Map color array
        /// </summary>
        public static Color[] colorMap;

        /// <summary>
        /// Map X coordinate array
        /// </summary>
        public static int[] startX = new int[10000];

        /// <summary>
        /// Map Y coordinate array
        /// </summary>
        public static int[] startY = new int[10000];

        /// <summary>
        /// Draw Map
        /// </summary>
        /// <param name="sb">Sprite Batch</param>
        /// <param name="gdm">Graphics Device</param>
        public static void Draw(SpriteBatch sb, GraphicsDeviceManager gdm)
        {
            //Begin Draw
            sb.Begin();

            //Draw background full screen
           //sb.Draw(mapBgImageTexture, new Rectangle(0, 0, gdm.PreferredBackBufferWidth, gdm.PreferredBackBufferHeight), Color.White);

            //Read map content in a loop
            for (int i = 0; i <= mapNum; i++)
            {
                //Judge if the map in the screen vision, if so then draw the map
                if (startX[i] >= 0 - mapTexture.Width && startX[i] <= gdm.PreferredBackBufferWidth && startY[i] >= 0 - mapTexture.Height && startY[i] <= gdm.PreferredBackBufferHeight)
                    sb.Draw(mapTexture, new Rectangle(startX[i], startY[i], mapTexture.Width, mapTexture.Height), Color.White);
            }

            //End draw
            sb.End();
        }
    }
}
