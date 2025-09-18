using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using SharpDX;
//using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Game_SpaceInvaders_2025
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Player player;
        public int windowHeight;                                            //Bredd på fönster
        

        Texture2D pixel;                                                   //Enemy
        List<Enemy> enemyList = new List<Enemy>();                         //Lista med enemies
        List<Enemy> deadenemy = new List<Enemy>();                        //Lista med döda fiender

        public Texture2D PlayerShip;                                      //Player
        public Vector2 position2;                                         //Player position

        Vector2 position = Vector2.Zero;                                 //Keyboard
        const float ringsSpeed = 6;
        KeyboardState previousKeyboardState;                             //Keyboard//Hastighetrörelse

        Texture2D pixel2;                                               //Bullet
        List<Bullet> bulletList = new List<Bullet>();                   //Lista med bullets
        List<Bullet> removebullet = new List<Bullet>();                 //Lista med bullets efter den skjutit fiende

        int score = 0;                                                  //poäng vid dödade fiender
        int life = 3;                                                   //antal liv player har

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 800;                        // Bredd skärm
            graphics.PreferredBackBufferHeight = 950;                       // Höjd skärm
            graphics.ApplyChanges();

            Window.Title = "Space Invaders               Score : ";         //Titel fönster

            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            pixel = new Texture2D(GraphicsDevice, 1, 1);                                    // Rektangel
            pixel.SetData(new[] { Color.White });

            int raderx = 11;                                                                //Enemy rektanglar - mått/rad
            int antalradery = 3;
            int rectWidth = 64;
            int rectHeight = 60;
            int radY = 10;
            int radX = 10;
            int startX = 0;
            int startY = 0;

            for (int antalrader2y = 0; antalrader2y < antalradery; antalrader2y++)
            {
                for (int rader2x = 0; rader2x < raderx; rader2x++)
                {
                    int x = startX + rader2x * (rectWidth + radX);
                    int y = startY + antalrader2y * (rectHeight + radY);

                    Rectangle rect = new Rectangle(x, y, rectWidth, rectHeight);          //Rektangel skapad - plats
                    //spriteBatch.Draw(pixel, rect, Color.Green);
                    enemyList.Add(new Enemy(rect));                                       //Lägger enemies i lista
                }

            }

            PlayerShip = Content.Load<Texture2D>("Ship_01-1 (1)");                          //Player bild content
            Vector2 startPosition = new Vector2(550, 850);                                  //Player position skärm
            player = new Player(startPosition, PlayerShip, ringsSpeed);                     //Player Movement

            pixel2 = new Texture2D(GraphicsDevice, 1, 1);                                   //Bullet
            pixel2.SetData(new[] { Color.White });
                                                                                            //Lägger till bullet i lista

            windowHeight = Window.ClientBounds.Height;   
            

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keyboardState = Keyboard.GetState();                                                      //Keyboard


            bool stoprad = false;                                                                                   //Beslut stoppa rad vis skärm
            foreach (Enemy enemy in enemyList)                                                                     //Loop (Klass, namn på fiende, lista)
            {
                if (enemy.Position.Bottom >= windowHeight)                                                      //Om enemy är vid skärm - -1 liv
                {                                                                                   
                    life = life - 1;
                    deadenemy.Add(enemy);                                                                      //Lägg död enemy i dödenemy Lista
                }
            }
            
            foreach (Enemy enemy in deadenemy)
            {
                enemyList.Remove(enemy);                                                                      //Ta bort död enemy i dödenemy Lista
            }

            if (life <= 0)                                                                                  //Om livet är <= 0 = Exit skärm
            {
                Exit();
            }

            foreach (Enemy enemy in enemyList)
            {
                enemy.Update(gameTime);
            }
                                                                                             

            player.Update(gameTime);                                                                               //Uppdatera player movement

            foreach (Bullet bullet in bulletList)
            {
                bullet.Update(gameTime);                                                                           //Bullet uppdaterar sig själv - Lista
            }

            if (keyboardState.IsKeyDown(Keys.Space) && !previousKeyboardState.IsKeyDown(Keys.Space))
            {
                Rectangle bulletrect = new Rectangle((int)player.position2.X, (int)player.position2.Y, 5, 10);    //Bullet skapad - plats på players plats

                bulletList.Add(new Bullet(bulletrect));
            }

           foreach (Bullet bullet in bulletList)                                                                 //Om bullet träffar enemy - raderar 
            {
                foreach (Enemy enemy in enemyList)
                { 
                    if (bullet.Position.Intersects(enemy.Position))
                    {
                        enemy.enemyisalive = false;
                        removebullet.Add(bullet);                                                               //Lägger bullet i remove lista - inga dubbla poäng
                        deadenemy.Add(enemy);                                                                   //Lägger död enemy i remove lista - inga dubbla poäng
                        score = score + 10;
                    }


                }
            }

            foreach (var b in removebullet)                                                                      //tar bort bullet i remove listan
                bulletList.Remove(b);
            foreach (var E in deadenemy)                                                                         // tar bort enemy i remove listan
                enemyList.Remove(E);

            Window.Title = "Space Invaders      Score : " + score + "   Lifes : " + life;                      //Titel med nam och score
             

            previousKeyboardState = keyboardState;                                                                //Uppdatera Keyboard

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();                                        //Starta alltid med Begin();

            foreach (Enemy enemy in enemyList)                          //Rita enemies
            {
                if(enemy.enemyisalive)
                {
                    enemy.Draw(spriteBatch, pixel);
                }
                
            }

            player.Draw(spriteBatch);

            foreach (Bullet bullet in bulletList)
            {
                bullet.Draw(spriteBatch, pixel2);
            }

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
