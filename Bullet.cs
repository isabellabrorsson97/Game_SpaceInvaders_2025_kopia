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
    internal class Bullet
    {

        public Rectangle Position;                      //Postion bullet
        public Color Color = Color.Red;
        public int speed = 3;                           //Hastighet Bullet

        public Bullet(Rectangle position)
        {
            this.Position = position;
        }

        public void Update(GameTime gameTime)
        {
            Position = new Rectangle(Position.X, Position.Y - speed, Position.Width, Position.Height);
        }


        public void Draw(SpriteBatch spriteBatch, Texture2D pixel2)
        {
            spriteBatch.Draw(pixel2, Position, Color);
        }

        
    }
}
