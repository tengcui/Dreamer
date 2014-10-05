using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dreamer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using dreamer.Level;

namespace dreamer
{
    class Weapon
    {
        Person Ninja;
        float velory = 2.0f;
        
        SpriteBatch sp;
        public Texture2D crossTexture;
        GraphicsDevice gd;
        public Rectangle cross = new Rectangle();
        public bool throwed = false;
        public Vector2 Position;


        public Weapon()
        {
            LoadWeapon();
        }
        public void LoadWeapon()
        {
            crossTexture = Resource.AddTexture2D(gd, "Ui\\CrossDart");
            cross = new Rectangle(0, 0, crossTexture.Width, crossTexture.Height);
        }

        public void Update(GameTime gameTime)
        {
            if(Keyboard.GetState().IsKeyDown(Keys.G))
            {
                throwed = true;
                Position.X = Ninja.personX;
                Position.Y = Ninja.personY;
                
                Position.X += velory;
            }
            if(Keyboard.GetState().IsKeyUp(Keys.G))
            {
                throwed = false;
            }

        }

        public void Draw()
        {
            sp.Begin();
            
            sp.Draw(crossTexture,Position, null, Color.White,1.5f, Vector2.Zero,0.0f, SpriteEffects.None, 0.0f);

            sp.End();
        }

    }
}
