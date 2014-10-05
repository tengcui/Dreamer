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
    public class blood : Microsoft.Xna.Framework.GameComponent
    {
       public  int startingTick;
        Vector2 myPosition;
        Texture2D myTexture;
        SpriteBatch myPen;
        public blood(Game game,Vector2 pos, ref Texture2D te,ref SpriteBatch p1)
            : base(game)
        {
            startingTick = System.Environment.TickCount;
            myTexture = te;
            myPosition = pos;
            myPen = p1;
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

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
           
            base.Update(gameTime);
        }
        public void Draw()
        {
            // TODO: Add your update code here
            myPen.Begin();
            myPen.Draw(myTexture, myPosition, Color.White);
            myPen.End();
        }
    }
}
