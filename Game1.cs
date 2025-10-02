using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using SharpDX;
//using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using static Game_SpaceInvaders_2025.Enemy;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

// new version
namespace Game_SpaceInvaders_2025
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Player player;
        public int windowHeight;                                            
        SpriteFont spriteFont1;
        SpriteFont spriteFont2;

        Texture2D pixel;                                                                                          //Enemy
        List<Enemy> enemyList = new List<Enemy>();                                                                //Lista med enemies
        List<Enemy> deadenemy = new List<Enemy>();                                                                //Lista med döda fiender
        List<Color> enemycolor = new List<Color>()                                                                //Lista enemy-färg ********ny      
        {
            Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue
        };
        
        int raderx = 10;                                                                                          //Enemy rektanglar - mått/rad
        int antalradery = 5;
        Enemy[,] enemyArray;                                                                                      //enemyArray - 2D lista  *****ny
        int rectHeight;
        int rectWidth;
       
        public enum Direction                                                                                     //enemy direction
        {
            Left, Down, Right
        }
        public Direction direction;                                                                               //Riktning X & Y
        public Direction cameFrom;                                                                                //Riktning X rektangel senast vart på
        public int lastY;                                                                                         //Position Y rektangel senast vart på
        public bool enemyHitside = false;

        public Texture2D PlayerShip;                                                                              //Player
        public Vector2 position2;                                                                                 //Player position
        
        Vector2 position = Vector2.Zero;                                 
        const float ringsSpeed = 6;
        KeyboardState previousKeyboardState;                             

        Texture2D pixel2;                                                                                         //Bullet
        List<Bullet> bulletList = new List<Bullet>();                                                     
        List<Bullet> removebullet = new List<Bullet>();

        int score = 0;                                                                                            //Score + Life - Enemy
        List<int> enemyscore = new List<int>()                                                            
        {
            20, 10, 10, 5, 5
        };
        int life = 3;                                                                                     
        
        private int windowWidth;
        enum GameState { Start, Playing, GameOver };                                                                  //Startskärm
        GameState currentGameState = GameState.Start;
        public Texture2D screenImage;
        public Texture2D animation;
        public Texture2D spacebackground;
        public Texture2D StartKnapp;
        Point frameSize = new Point(110, 100);
        Point currentFrame = new Point(0, 0);
        Point sheetSize = new Point(5, 1);
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame = 500;
        public Texture2D spaceinvader;
        Vector2 positionbackground = new Vector2(100, 10);
        int spriteWidth2 = 600;
        int spriteHeight2 = 300;

        public Texture2D explosion;
        List<Vector2> explosions = new List<Vector2>();
        int spriteWidth = 100;
        int spriteHeight = 100;
        Random random = new Random();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 800;                                                                        // Skärm
            graphics.PreferredBackBufferHeight = 950;                                                       
            graphics.ApplyChanges();

            Window.Title = "Space Invaders               Score : ";                                                         //Text på skärm
            spriteFont1 = Content.Load<SpriteFont>("spritefont1");                                          
            spriteFont2 = Content.Load<SpriteFont>("spritefont2");
            currentFrame = new Point(0, 0);
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            pixel = new Texture2D(GraphicsDevice, 1, 1);                                                                    // Enemy - Rektangel
            pixel.SetData(new[] { Color.White });
            windowWidth = Window.ClientBounds.Width;
            windowHeight = Window.ClientBounds.Height;

            int raderx = 10;                                                                                     
            int antalradery = 5;
            rectWidth = 50;
            rectHeight = 50;
            int radY = 10;
            int radX = 10;
            int startX = 110;
            int startY = 50;

            enemyArray = new Enemy[antalradery, raderx];                                                                    //enemy-Array lista ***ny

            for (int antalrader2y = 0; antalrader2y < antalradery; antalrader2y++)
            {
                for (int rader2x = 0; rader2x < raderx; rader2x++)
                {
                    int x = startX + rader2x * (rectWidth + radX);
                    int y = startY + antalrader2y * (rectHeight + radY);

                    Rectangle rect = new Rectangle(x, y, rectWidth, rectHeight);                                            //Rektangel skapad - plats
                    
                    enemyArray[antalrader2y, rader2x] = new Enemy(rect, enemycolor[antalrader2y], enemyscore[antalrader2y], windowWidth, x);            //Lägger enemies i lista + färg (rader2y = specifika platser ej antal ****ny
                    enemyArray[antalrader2y, rader2x].direction = Enemy.Direction.Left;
                    
                }

            }
            
            PlayerShip = Content.Load<Texture2D>("Ship_01-1 (1)");                                                                      //Player
            Vector2 startPosition = new Vector2(550, 850);                                                        
            player = new Player(startPosition, PlayerShip, ringsSpeed);                                           

            pixel2 = new Texture2D(GraphicsDevice, 1, 1);                                                                               //Bullet
            pixel2.SetData(new[] { Color.White });
            screenImage = Content.Load<Texture2D>("space_light");                               
            animation = Content.Load<Texture2D>("explotion01_sprites");
            StartKnapp = Content.Load<Texture2D>("Startknapp");
            explosion = Content.Load<Texture2D>("explosion");
            spacebackground = Content.Load<Texture2D>("spacebackround");
            spaceinvader = Content.Load<Texture2D>("spaceinvadertext2");
            
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keyboardState = Keyboard.GetState();                                                      
            
            switch(currentGameState)                                                                                                    //Startskärm
            {
                case GameState.Start:
                    timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;                                                        //animation
                    if (timeSinceLastFrame > millisecondsPerFrame)
                    {
                        timeSinceLastFrame -= millisecondsPerFrame;
                        currentFrame.X++;
                        if (currentFrame.X >= sheetSize.X)
                        {
                            currentFrame.X = 0;
                        }
                        MouseState mouse = Mouse.GetState();                                                                            //muspekare
                        Rectangle startKnappBounds = new Rectangle(410, 410, StartKnapp.Width, StartKnapp.Height);
                        if (startKnappBounds.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                        {
                            currentGameState = GameState.Playing;
                        }
                    }
                    break;

                case GameState.Playing:

                    Rectangle playerRect = new Rectangle((int)player.position2.X, (int)player.position2.Y, PlayerShip.Width, PlayerShip.Height);
                    bool enemyisalive = false;
                    for (int antalrader2y = 0; antalrader2y < antalradery; antalrader2y++)                                              //Lista enemyArray / update 
                    {
                        for (int rader2x = 0; rader2x < raderx; rader2x++)
                        {
                            Enemy enemy = enemyArray[antalrader2y, rader2x];
                            if (enemy != null && enemy.enemyisalive)
                            {
                                enemy.Update(gameTime);
                                enemyisalive = true;

                                if (enemy.Position.X <= 0 || enemy.Position.X + rectWidth >= graphics.PreferredBackBufferWidth)
                                {
                                    for (int antalRader2y = 0; antalRader2y < antalradery; antalRader2y++)                                //Lista enemyArray / update 
                                    {
                                        for (int Rader2x = 0; Rader2x < raderx; Rader2x++)
                                        {
                                            Enemy enemy2 = enemyArray[antalRader2y, Rader2x];
                                            enemy2.Position.Y += rectHeight;
                                            enemy2.speed *= -1;
                                            if (enemy.speed < 0)
                                            {
                                                enemy2.Position.X -= 1;
                                            }
                                            else
                                            {
                                                enemy2.Position.X += 1;
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }

                    if (!enemyisalive)
                    {
                        currentGameState = GameState.GameOver;
                    }


                    player.Update(gameTime);                                                                                  //Uppdatera player movement

                    foreach (Bullet bullet in bulletList)
                    {
                        bullet.Update(gameTime);                                                                              //Bullet uppdaterar sig själv - Lista
                    }

                    if (keyboardState.IsKeyDown(Keys.Space) && !previousKeyboardState.IsKeyDown(Keys.Space))
                    {

                        Rectangle bulletrect = new Rectangle((int)player.position2.X + 32, (int)player.position2.Y, 5, 10);    //Bullet skapad - plats på players plats

                        bulletList.Add(new Bullet(bulletrect));
                    }

                    foreach (Bullet bullet in bulletList)
                    {
                        for (int antalrader2y = 0; antalrader2y < antalradery; antalrader2y++)                              //Lista enemyArray /bullet List ****ny
                        {
                            for (int rader2x = 0; rader2x < raderx; rader2x++)
                            {
                                Enemy enemy = enemyArray[antalrader2y, rader2x];
                                if (enemy != null && enemy.enemyisalive && bullet.Position.Intersects(enemy.Position))
                                {
                                    enemy.enemyisalive = false;
                                    removebullet.Add(bullet);                                                               //Lägger bullet i remove lista - inga dubbla poäng
                                    deadenemy.Add(enemy);                                                                   //Lägger död enemy i remove lista - inga dubbla poäng
                                    score = score + enemy.Score;
                                }
                            }
                        }
                    }

                    foreach (var b in removebullet)                                                                      //tar bort bullet i remove listan
                        bulletList.Remove(b);
                    removebullet.Clear();
                    foreach (var E in deadenemy)                                                                         // tar bort enemy i remove listan
                        enemyList.Remove(E);
                    deadenemy.Clear();

                    for (int antalrader2y = 0; antalrader2y < antalradery; antalrader2y++)                                  //Lista enemyArray / score 
                    {
                        for (int rader2x = 0; rader2x < raderx; rader2x++)
                        {
                            Enemy enemy = enemyArray[antalrader2y, rader2x];
                            if (enemy != null && enemy.enemyisalive)
                            {
                                if (enemy.Position.Bottom >= windowHeight)
                                {
                                    life = life - 1;
                                    enemy.enemyisalive = false;
                                    deadenemy.Add(enemy);
                                }
                                if (enemy.Position.Intersects(playerRect))
                                {
                                    life = life - 1;
                                    enemy.enemyisalive = false;
                                    deadenemy.Add(enemy);
                                }

                            }
                        }
                    }

                    if (life <= 0)                                                                                        
                    {
                        currentGameState = GameState.GameOver;
                    }
                    break;
                
                case GameState.GameOver:
                    if (explosions.Count == 0)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            int randomX = random.Next(0, graphics.PreferredBackBufferWidth - spriteWidth);
                            int randomY = random.Next(0, graphics.PreferredBackBufferHeight - spriteHeight);
                            explosions.Add(new Vector2(randomX, randomY)); 
                        }
                    }
                    break;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            Window.Title = "Space Invaders      Score : " + score;                                              
            previousKeyboardState = keyboardState;                                                                

            // TODO: Add your update logic here

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
           
            switch (currentGameState)                                                                             //Startskärm 
            {
                case GameState.Start:
                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                    spriteBatch.Draw(screenImage,
                    new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), 
                    null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                    spriteBatch.Draw(spaceinvader,
                        new Rectangle((int)positionbackground.X, (int)positionbackground.Y, spriteWidth2, spriteHeight2),
                        null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
                    spriteBatch.Draw(animation, new Vector2(200, 400),
                        new Rectangle(currentFrame.X * frameSize.X, 0, frameSize.X, frameSize.Y), Color.White);
                    spriteBatch.Draw(StartKnapp, new Vector2(410, 410), Color.White);

                    spriteBatch.End();
                    break;

                case GameState.Playing:
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin();
                spriteBatch.Draw(spacebackground,
                new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), null,
                Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                    for (int antalrader2y = 0; antalrader2y < antalradery; antalrader2y++)                             //Lista Rita enemyArray 
                {
                    for (int rader2x = 0; rader2x < raderx; rader2x++)
                    {
                        Enemy enemy = enemyArray[antalrader2y, rader2x];
                        if (enemy != null && enemy.enemyisalive)
                        {
                            enemy.Draw(spriteBatch, pixel);
                        }
                    }
                }

                player.Draw(spriteBatch);

                foreach (Bullet bullet in bulletList)
                {
                    bullet.Draw(spriteBatch, pixel2);
                }

                spriteBatch.DrawString(spriteFont1, "Score: " + score, Vector2.Zero, Color.White);
                spriteBatch.DrawString(spriteFont2, "Lifes: " + life, new Vector2(0, 25), Color.White);

                spriteBatch.End();
                break;

                case GameState.GameOver:
                    spriteBatch.Begin();
                    spriteBatch.Draw(screenImage,
                        new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height),
                        null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                    foreach (var position in explosions)
                    {
                        spriteBatch.Draw(explosion,
                            new Rectangle((int)position.X, (int)position.Y, spriteWidth, spriteHeight), Color.White);
                    }
                    spriteBatch.End();
                    break;
            }
            
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
