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
        public Color Color;
        public int speed = 2;                                                                   //Hastighet i y-led
        public bool enemyisalive = true;
        public int Score;
        public int startX;
        public enum Direction
        {
            Left, Down, Right
        }
        public Direction direction;                                                              //Riktning X & Y
        public Direction cameFrom;                                                              //Riktning X rektangel senast vart på
        public int lastY;                                                                       //Position Y rektangel senast vart på
        private int windowWidth;
        public bool enemyHitside = false;

        public Enemy(Rectangle position, Color color, int score, int windowWidth, int startX)
        {
            this.Position = position;
            this.Color = color;
            this.Score = score;
            this.windowWidth = windowWidth;
            this.startX = startX;

        }

        public void Update(GameTime gameTime)
        {
            if (enemyisalive)
            {
                Position.X += speed;

            }
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
