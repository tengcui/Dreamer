using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace dreamer.AI
{
    public abstract class BaseBehavior
    {
        protected readonly TimeSpan minTime_ChoseBehavior;
        protected readonly TimeSpan maxTime_ChoseBehavior;
        protected TimeSpan currentTime_ChoseBehavior;

        public BaseBehavior(TimeSpan minTime_ChoseBehavior, TimeSpan maxTime_ChoseBehavior)
        {
            this.minTime_ChoseBehavior = minTime_ChoseBehavior;
            this.maxTime_ChoseBehavior = maxTime_ChoseBehavior;
            GetTimeSpan_ChoseBehavior();
        }
        private void GetTimeSpan_ChoseBehavior()
        {
            this.currentTime_ChoseBehavior = TimeSpan.FromMilliseconds(HelperDLL.RandomHelper.GetRandomDouble(minTime_ChoseBehavior.TotalMilliseconds,
                                                                                                              maxTime_ChoseBehavior.TotalMilliseconds));
        }

        public virtual void Update(TimeSpan gameTime)
        {
            this.currentTime_ChoseBehavior -= gameTime;
            if (this.currentTime_ChoseBehavior.TotalMilliseconds <= 0)
            {
                //Get timespan again
                GetTimeSpan_ChoseBehavior();
                //chose behavior agian
                ChoseBehavior();
            }
        }

        protected abstract void ChoseBehavior();

    }
}
