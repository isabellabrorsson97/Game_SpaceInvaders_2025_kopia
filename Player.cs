using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_SpaceInvaders_2025
{
    internal class Player
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D PlayerShip;                                                   //Player content
        public Vector2 position2;                                                       //Player position
        public float ringsSpeed;                                                       //Player movement

        public Player(Vector2 position, Texture2D player, float speed)
        {
            position2 = position;
            PlayerShip = player;
            ringsSpeed = speed;
        }

        public void Update(GameTime gameTime)
        {

            KeyboardState keyboardState = Keyboard.GetState();                         //Keyboard
            if (keyboardState.IsKeyDown(Keys.Left))
                position2.X -= ringsSpeed;
            if (keyboardState.IsKeyDown(Keys.Right))
                position2.X += ringsSpeed;

            // TODO: Add your update logic here

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PlayerShip, position2, Color.White);

        }




    }
}
