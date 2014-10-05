using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dreamer.Level
{
    public class Monsterstate : FSMState 
    {
        override
        public void Reason(Person player, Person npc) { 
        
        }

        override
        public void Act(Person player, Person npc) { 
        }
    }
    public enum MonsterState 
    {
        Stand,
        Attack,

    }

}
