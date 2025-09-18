using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Game_SpaceInvaders_2025
{
    internal class Enemy
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Rectangle Position;
        public Color Color = Color.Green;
        public int speed = 1;                           //Hastighet i y-led
        public bool enemyisalive = true;


        public Enemy(Rectangle position)
        {
            this.Position = position;
        }

        public void Update(GameTime gameTime)
        {
            Position = new Rectangle(Position.X, Position.Y + speed, Position.Width, Position.Height);

            // TODO: Add your update logic here
        }

        public void Stop()
        {
            speed = 0;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
        {
            spriteBatch.Draw(pixel, Position, Color);

        }
      


    }








}
