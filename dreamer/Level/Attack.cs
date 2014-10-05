using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dreamer.Level
{
    public class Attack
    {
        /// <summary>
        /// Judge if can attack
        /// </summary>
        /// <returns>If can attack</returns>
        public bool IsEnableAttack()
        {
            if (this.currentTime == TimeSpan.Zero)
            {
                DoAttack();
                return true;
            }
            else
            {
                return false;
            }
        }
        private readonly TimeSpan minimumTimeToAttack;
        private TimeSpan currentTime;
        /// <summary>
        /// Construct
        /// </summary>
        /// <param name="minimumTimeToAttack">Min attack timspan</param>
        public Attack(TimeSpan minimumTimeToAttack)
        {
            this.currentTime = TimeSpan.Zero;
            this.minimumTimeToAttack = minimumTimeToAttack;
        }
        private void DoAttack()
        {
            this.currentTime = this.minimumTimeToAttack;
        }
        /// <summary>
        /// Update gametime
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(TimeSpan gameTime)
        {
            if (this.currentTime != TimeSpan.Zero)
            {
                this.currentTime -= gameTime;
                if (this.currentTime < TimeSpan.Zero)
                {
                    this.currentTime = TimeSpan.Zero;
                }
            }
        }
    }
}
