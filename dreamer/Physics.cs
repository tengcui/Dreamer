﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace dreamer
{
    public class Physics
    {
        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels 
        /// between two sprites.
        /// </summary>
        /// <param name="rectangleA">Bounding rectangle of the first sprite</param> 
        /// <param name="dataA">Pixel data of the first sprite</param> 
        /// <param name="rectangleB">Bouding rectangle of the second sprite</param>  
        /// <param name="dataB">Pixel data of the second sprite</param>  
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns> 
        public static bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
                                    Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection  
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds  
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point  
                    try
                    {
                        Color colorA = dataA[(x - rectangleA.Left) +
                                             (y - rectangleA.Top) * rectangleA.Width];
                        Color colorB = dataB[(x - rectangleB.Left) +
                                             (y - rectangleB.Top) * rectangleB.Width];
                        // If both pixels are not completely transparent, 
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            // then an intersection has been found  
                            return true;
                        }
                    }
                    catch { }
                }
            }

            // No intersection found  
            return false;
        }
    }
}
