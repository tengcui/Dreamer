using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dreamer.Level
{
    internal class MonsterGenerator
    {
        //Generator new monster event
        public event EventHandler OnGenerateNewMonster;

        private readonly TimeSpan minTimeSpan;
        private readonly TimeSpan maxTimeSpan;
        private TimeSpan timeSpan_GenerateMonster;
        private TimeSpan currentTime;
        /// <summary>
        /// Construct
        /// </summary>
        /// <param name="minTimeSpan">Min time span</param>
        /// <param name="maxTimeSpan">Max time span</param>
        public MonsterGenerator(TimeSpan minTimeSpan, TimeSpan maxTimeSpan)
        {
            this.minTimeSpan = minTimeSpan;
            this.maxTimeSpan = maxTimeSpan;
            this.currentTime = TimeSpan.Zero;
            //Get the time for generate the monster
            GetNextTimeToGenerate();
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="gameTime">Game Time</param>
        public void Update(TimeSpan gameTime)
        {
            this.timeSpan_GenerateMonster -= gameTime;
            if (timeSpan_GenerateMonster.TotalMilliseconds <= 0)
            {
                //Generate new monster, active event
                if (this.OnGenerateNewMonster != null)
                {
                    this.OnGenerateNewMonster(this, null);
                }
                //Get next monster generate time
                GetNextTimeToGenerate();
            }
        }
        /// <summary>
        /// Get the time span for new monster generate
        /// </summary>
        private void GetNextTimeToGenerate()
        {
            this.timeSpan_GenerateMonster = TimeSpan.FromMilliseconds(HelperDLL.RandomHelper.GetRandomDouble(minTimeSpan.TotalMilliseconds, maxTimeSpan.TotalMilliseconds));
        }
    }
}
