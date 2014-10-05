using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using dreamer.Level;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
namespace dreamer.AI
{
    public class ChaseBehavior : BaseBehavior
    {
        //monster object
        
        Monster mySelf;
        // ninja object
        Person ninja;
        SoundEffect defense;
        /// <summary>
        /// content manager
        /// </summary>
        ContentManager cm = null;


        /// <summary>
        /// Construct
        /// </summary>
        /// <param name="minTime_ChoseBehavior">Min time to choose behavior</param>
        /// <param name="maxTime_ChoseBehavior">Max time to choose behavior</param>
        /// <param name="ninja">Target position</param>
        /// <param name="mySelf">Current monster position</param>
        public ChaseBehavior(TimeSpan minTime_ChoseBehavior, TimeSpan maxTime_ChoseBehavior, Person ninja, Monster mySelf, ContentManager cm)
            : base(minTime_ChoseBehavior, maxTime_ChoseBehavior)
        {
            this.ninja = ninja;
            this.mySelf = mySelf;
            LoadEffect();
        }

        public override void Update(TimeSpan gameTime)
        {
            //computer the time of choose behavior
            base.Update(gameTime);

            Run();

            DoAttack();

            IsBeHit();
        }

        protected override void ChoseBehavior()
        {
            
                mySelf.currentDirection = (ninja.personX > mySelf.monsterX) ? Direction.Left : Direction.Right;
        }

        protected void Run()
        {
            
                switch (mySelf.actionName)
                {
                    case "Stand":
                        {
                            switch (mySelf.currentDirection)
                            {
                                case Direction.Up:
                                    break;
                                case Direction.Down:
                                    break;
                                case Direction.Left:
                                    mySelf.monsterX += MonsterManagerConfig.monsterSpeed;
                                    break;
                                case Direction.Right:
                                    mySelf.monsterX -= MonsterManagerConfig.monsterSpeed;
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case "Attack":
                        break;
                    default:
                        break;
                }

        }

        public void LoadEffect()
        {
            defense = Resource.AddSounds(cm, "swordecho");

        }

        protected void DoAttack()
        {
            if (Math.Abs(mySelf.monsterX - ninja.personX) <= MonsterManagerConfig.monsterMinimumDistanceToAttack_X &&
                Math.Abs(mySelf.monsterY - ninja.personY) <= MonsterManagerConfig.monsterMinimumDistanceToAttack_Y)
            {
                
                    mySelf.actionName = "Attack";
                    IsHitNinja();
                
            }
            else
            {
                
                    mySelf.actionName = "Stand";
            }
        }
        /// <summary>
        /// If attacked Ninja
        /// </summary>
        protected void IsHitNinja()
        {
            if (mySelf.attack.IsEnableAttack() &&
                Math.Abs(mySelf.monsterX - ninja.personX) <= MonsterManagerConfig.hitDistance_Monster_X &&
                Math.Abs(mySelf.monsterY - ninja.personY) <= MonsterManagerConfig.hitDistance_Monster_Y && ninja.canGetHit == true)
            {
                ninja.BloodNum -= HelperDLL.RandomHelper.GetRandomInt(MonsterManagerConfig.monsterMinAttack,
                                                                        MonsterManagerConfig.monsterMaxAttack);

            }
            else if (ninja.canGetHit == false)
            {
                defense.Play();
            }

        }
        /// <summary>
        /// If be attacked by Ninja
        /// </summary>
        protected void IsBeHit()
        {
            if (ninja.actionName == "Attack" || ninja.actionName == "StandAtt" || ninja.actionName == "CrouchAtt" || ninja.actionName == "RunAtt")
            {
                if (ninja.attack.IsEnableAttack() &&
                    IsFaceToNinjaDirection() &&
                    Math.Abs(mySelf.monsterX - ninja.personX) <= MonsterManagerConfig.hitDistance_Ninja_X &&
                    Math.Abs(mySelf.monsterY - ninja.personY) <= MonsterManagerConfig.hitDistance_Ninja_Y)
                {
                    mySelf.MonsterBloodNum -= HelperDLL.RandomHelper.GetRandomInt(MonsterManagerConfig.ninjaMinAttack,
                                                                                        MonsterManagerConfig.ninjaMaxAttack);
                }
            }

        }

        /// <summary>
        /// Judge if ninja is attacking forward monster direction
        /// </summary>
        /// <returns>If ninja attack forward monster</returns>
        private bool IsFaceToNinjaDirection()
        {

            return ((ninja.personX >= mySelf.monsterX && Game_Main.PersonDirection == Direction.Left) ||
                   (ninja.personX <= mySelf.monsterX && Game_Main.PersonDirection == Direction.Right));
        }

    }
}
