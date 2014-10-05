using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dreamer.Level
{
    class MonsterManagerConfig
    {
        //Minimum time span to generate monster
        public static readonly TimeSpan minTimeSpan_GenerateMonster = TimeSpan.FromMilliseconds(3000);
        //Maximum time span to generate monster
        public static readonly TimeSpan maxTimeSpan_GenerateMonster = TimeSpan.FromMilliseconds(6000);
        //Total number of monsters
        public const int monstersCount = 10;

        //Time span for monster choose new behavior
        public static readonly TimeSpan minTimeSpan_ChoseBehavior = TimeSpan.FromMilliseconds(800);
        public static readonly TimeSpan maxTimeSpan_ChoseBehavior = TimeSpan.FromMilliseconds(2000);
        //Monster move speed
        public const int monsterSpeed = 4;
        //The distance between monster and ninja to begin attack
        public const int monsterMinimumDistanceToAttack_X = 40;
        public const int monsterMinimumDistanceToAttack_Y = 60;
        public const int hitDistance_Monster_X = 80;
        public const int hitDistance_Monster_Y = 80;
        public const int hitDistance_Ninja_X = 150;
        public const int hitDistance_Ninja_Y = 80;
        //Monster attack power
        public const int monsterMinAttack = 2;
        public const int monsterMaxAttack = 8;
        //Ninja attack power
        public const int ninjaMinAttack = 20;
        public const int ninjaMaxAttack = 50;

        //Min time span for monster attack
        public static readonly TimeSpan attack_TimeSpan_Monster = TimeSpan.FromMilliseconds(1000);
        //Min time span for Ninja attack
        public static readonly TimeSpan attack_TimeSpan_Ninja = TimeSpan.FromMilliseconds(100);

    }
}
