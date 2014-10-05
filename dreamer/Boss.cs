using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace dreamer
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    /// 
   
    public class Boss : Microsoft.Xna.Framework.DrawableGameComponent
    {
       public int deathStartTime;
       public int health = 50;
        const int k = 200;
        enum Direction { left, right };
        public Vector2 position;
        Vector2 velocity;
        AnimatedTexture myTexture;
        Direction myDirection;
        SpriteBatch myPen;
        Vector2 FixedVelocity = new Vector2(5, 0);
        int ranY, k1;
        const int VISION_RANGE =300;
        Person myTarget;
        public bool death;
        public Boss(Game game, Texture2D myTe, SpriteBatch p1, Person per1)
            : base(game)
        {
            death = false;
            k1 = 1;
            ranY = 0;
            myTarget = per1;
            position = new Vector2(100, 100);
            myTexture = new AnimatedTexture(ref myTe, 6, 1, 10);
            myPen = p1;
            myDirection = Direction.right;
            velocity = new Vector2(1, 0);
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }
        public void GoRandom()
        {
            if (myDirection == Direction.right)
                velocity = FixedVelocity;
            else
                velocity = -1 * FixedVelocity;

            if (position.X >= 800)
             myDirection = Direction.left; 
            if (position.X <= 0)
                myDirection = Direction.right;
            position += velocity;
        }

        public void Chase(Vector2 targetPos)
        {

            if (velocity.X > 0) myDirection = Direction.right;
            else myDirection = Direction.left;
            Vector2 force1 = new Vector2(0, 0);
          


            force1 = targetPos - position;
            force1 /= k;
            velocity = force1;
            ranY += k1;
            if (ranY >= 10 || ranY <= -10) k1 *= -1;


            velocity.Y += ranY;


            position += velocity;
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (health <= 0 && !death)
            {
                death = true;
                deathStartTime = System.Environment.TickCount;
            }
            myTexture.Update(gameTime);
            // TODO: Add your update code here
            //AI
   
              Vector2 targetPos = new Vector2(myTarget.personX, myTarget.personY);

              if (Vector2.Distance(targetPos, position) <= VISION_RANGE)
                  Chase(targetPos);
              else
                  GoRandom();
            base.Update(gameTime);
        }
        public void Draw()
        {
            if (death) return;
            // TODO: Add your update code here
            myPen.Begin();
            if (myDirection == Direction.left)
                myPen.Draw(myTexture.gridImage,position, myTexture.currentFrame, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            else myPen.Draw(myTexture.gridImage, position, myTexture.currentFrame, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);

            myPen.End();
        }

    }
}
