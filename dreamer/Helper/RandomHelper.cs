using System;

namespace HelperDLL
{
    /// <summary>
    /// Random helper
    /// </summary>
    public sealed class RandomHelper
    {
        //Private constructor use this class can use sealed keyword
        private RandomHelper()
        { }

        #region Variables
        /// <summary>
        /// Global random generator
        /// </summary>
        public static Random globalRandomGenerator =
            GenerateNewRandomGenerator();
        #endregion

        #region Generate a new random generator
        /// <summary>
        /// Generate a new random generator with help of
        /// WindowsHelper.GetPerformanceCounter.
        /// Also used for all GetRandom methods here.
        /// </summary>
        /// <returns>Random</returns>
        public static Random GenerateNewRandomGenerator()
        {
            globalRandomGenerator =
                new Random((int)DateTime.Now.Ticks);
            //needs Interop: (int)WindowsHelper.GetPerformanceCounter());
            return globalRandomGenerator;
        }
        #endregion

        #region Get random float and byte methods
        /// <summary>
        /// Get random int
        /// </summary>
        /// <param name="max">Maximum</param>
        /// <returns>Int</returns>
        public static int GetRandomInt(int max)
        {
            return globalRandomGenerator.Next(max);
        }
        //Overload
        public static int GetRandomInt(int min, int max)
        {
            return globalRandomGenerator.Next(min, max);
        }

        /// <summary>
        /// Get random float between min and max
        /// </summary>
        /// <param name="min">Min</param>
        /// <param name="max">Max</param>
        /// <returns>Float</returns>
        public static float GetRandomFloat(float min, float max)
        {
            return (float)globalRandomGenerator.NextDouble() * (max - min) + min;
        }

        public static double GetRandomDouble(double min, double max)
        {
            return globalRandomGenerator.NextDouble() * (max - min) + min;
        }

        /// <summary>
        /// Get random byte between min and max
        /// </summary>
        /// <param name="min">Min</param>
        /// <param name="max">Max</param>
        /// <returns>Byte</returns>
        public static byte GetRandomByte(byte min, byte max)
        {
            return (byte)(globalRandomGenerator.Next(min, max));
        }




        #endregion
    }
}
