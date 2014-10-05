using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dreamer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using dreamer.Level;

namespace dreamer.Level
{
    public class MonsterManager : FSMManager
    {
        private FSMManager fsmManager;
        private List<Monster> monsters;
        /// <summary>
        /// Monster List
        /// </summary>
        public List<Monster> Monsters
        {
            get { return this.monsters; }
        }
        //Ninja object
        private Person ninja;
        Stopwatch sw = new Stopwatch();
        
        ContentManager cm;

        //New monster generator
        private MonsterGenerator monsterGenerator;
        //The Number of monster has been killed
        private int monsterBeKilledCount = 0;
        private int maxNumberOfMonster = 5;
        public int MonsterKilledNumber
        {
            get { return monsterBeKilledCount * 10; }
        }
        
        /// <summary>
        /// All monsters have been killed, active this event
        /// </summary>
        public event EventHandler OnAllMonstersDead;



        /// <summary>
        /// Construct
        /// </summary>
        public MonsterManager(Person ninja)
        {
           
            this.monsters = new List<Monster>(MonsterManagerConfig.monstersCount);
            this.monsterGenerator = new MonsterGenerator(MonsterManagerConfig.minTimeSpan_GenerateMonster,
                                                         MonsterManagerConfig.maxTimeSpan_GenerateMonster);
            
            
                    this.monsterGenerator.OnGenerateNewMonster += new EventHandler(monsterGenerator_OnGenerateNewNinja);
                
                    this.monsterGenerator.OnGenerateNewMonster += new EventHandler(monsterGenerator_OnGenerateNewSamurai);
            
                
            
            this.ninja = ninja;
            
            
        }



        //Generate Monster
        private void monsterGenerator_OnGenerateNewSamurai(object sender, EventArgs e)
        {
            if (this.monsters.Count < 10 && this.maxNumberOfMonster <= 6)
            {
                Monster Samurai = new Monster(BaseSupport.Instance.gd.GraphicsDevice,
                                            BaseSupport.Instance.Content,
                                            BaseSupport.Instance.sbc,
                                            Resource.AddTexture2D(BaseSupport.Instance.gd.GraphicsDevice, "Graphics\\eSamurai"),
                                            "Content\\Data\\Samurai.txt",
                                            Resource.AddTexture2D(BaseSupport.Instance.gd.GraphicsDevice, "Graphics\\eFace1"))
                {
                    //Initialize Monster Status
                    actionName = "Stand",
                    attack = new Attack(Level.MonsterManagerConfig.attack_TimeSpan_Monster)
                };
                Samurai.currentBehavior = new AI.ChaseBehavior(MonsterManagerConfig.minTimeSpan_ChoseBehavior,
                                                                    MonsterManagerConfig.maxTimeSpan_ChoseBehavior,
                                                                    ninja,
                                                                    Samurai, cm);
                this.maxNumberOfMonster++;
                Samurai.OnMonsterDead += new EventHandler(newMonster_OnMonsterDead);
                this.monsters.Add(Samurai);
            }
        }
        private void monsterGenerator_OnGenerateNewNinja(object sender, EventArgs e)
        {
            if (this.monsters.Count < 10 && this.maxNumberOfMonster <= 6)
            {
                Monster Ninjay = new Monster(BaseSupport.Instance.gd.GraphicsDevice,
                                            BaseSupport.Instance.Content,
                                            BaseSupport.Instance.sbc,
                                            Resource.AddTexture2D(BaseSupport.Instance.gd.GraphicsDevice, "Graphics\\eNinja"),
                                            "Content\\Data\\Ninja.txt",
                                            Resource.AddTexture2D(BaseSupport.Instance.gd.GraphicsDevice, "Graphics\\eFace2"))
                {
                    //Initialize Monster status
                    actionName = "Stand",
                    attack = new Attack(Level.MonsterManagerConfig.attack_TimeSpan_Monster)
                };
                Ninjay.currentBehavior = new AI.ChaseBehavior(MonsterManagerConfig.minTimeSpan_ChoseBehavior,
                                                                    MonsterManagerConfig.maxTimeSpan_ChoseBehavior,
                                                                    ninja,
                                                                    Ninjay,
                                                                    cm);
                this.maxNumberOfMonster++;
                Ninjay.OnMonsterDead += new EventHandler(newMonster_OnMonsterDead);
                this.monsters.Add(Ninjay);
            }
        }
        //private void monsterGenerator_OnGenerateNewBoss(object sender, EventArgs e)
        //{
        //    if (MonsterKilledNumber >= 10)
        //    {
        //        Monster boss = new Monster(BaseSupport.Instance.gd.GraphicsDevice,
        //                                    BaseSupport.Instance.Content,
        //                                    BaseSupport.Instance.sbc,
        //                                    Resource.AddTexture2D(BaseSupport.Instance.gd.GraphicsDevice, "Graphics\\boss"),
        //                                    "Content\\Data\\boss.txt",
        //                                    Resource.AddTexture2D(BaseSupport.Instance.gd.GraphicsDevice, "Graphics\\BossFace"))
        //        {
        //            //Initialize Monster status
        //            actionName = "Stand",
        //            attack = new Attack(Level.MonsterManagerConfig.attack_TimeSpan_Monster)
        //        };
        //        boss.currentBehavior = new AI.ChaseBehavior(MonsterManagerConfig.minTimeSpan_ChoseBehavior,
        //                                                            MonsterManagerConfig.maxTimeSpan_ChoseBehavior,
        //                                                            ninja,
        //                                                            boss,
        //                                                            cm);
        //        maxNumberOfMonster++;
        //        boss.OnMonsterDead += new EventHandler(newMonster_OnMonsterDead);
        //        this.monsters.Add(boss);
        //    }
        
        //}

        public void Update(TimeSpan gameTime)
        {
            this.monsterGenerator.Update(gameTime);
            //Monster update
            for (int i = 0; i < this.monsters.Count; i++)
            {
                monsters[i].Move(Keyboard.GetState(), gameTime);
            }
        }
        /// <summary>
        /// Monster has to be move by background move
        /// </summary>
        /// <param name="distance">The distance has to move</param>
        public void MoveMonsterByBackground(int distance)
        {
            for (int i = 0; i < monsters.Count; i++)
            {
                monsters[i].monsterX += distance;
            }
        }

        //Monster died event
        private void newMonster_OnMonsterDead(object sender, EventArgs e)
        {
            Monster deadMonster = sender as Monster;
            //Remove this monster
            this.monsters.Remove(deadMonster);
            maxNumberOfMonster--;
            //The number of monster has been killed ++
            monsterBeKilledCount++;
            if (monsterBeKilledCount >= MonsterManagerConfig.monstersCount &&
                this.OnAllMonstersDead != null)
            {
                //Active this event
                this.OnAllMonstersDead(this, null);
            }
        }


        public void Draw()
        {
            for (int i = 0; i < monsters.Count; i++)
            
            {
                //Draw Every Monster
                monsters[i].Draw();
                monsters[i].DrawArticleBlood();
                monsters[i].DrawFace();
            }
        }
    }
}
