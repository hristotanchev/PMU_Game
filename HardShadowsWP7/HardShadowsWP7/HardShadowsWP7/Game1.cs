using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using HardShadows.GamePlay;
using HardShadows.Engine;

namespace HardShadows
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Vector2 directionToMove;
        List<Vector2> movingPath = new List<Vector2>();
        float differenceX;
        float differenceY;
        bool isGameOver = false;
        bool isLevelCompleted = false;
        bool isInMenu = true;
        bool isInLevelMenu = false;
        bool intersectLevelButton = false;
        int changeLevel = 1;
    

        FPSCounter fpsCounter;

        Vector2 _touchLocation;
        bool showDPat = false;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


// 	        Rectangle _leftRect, _upRect, _downRect, _rightRect, _intersectRect; 

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>

        private MenuButton button;
        private MenuButton button1;
        private MenuButton button2;
        private MenuButton button3;
        private MenuButton button4;
        private MenuButton button5;
        private MenuButton button6;

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ConvexHull.InitializeStaticMembers(GraphicsDevice);

            LevelManager.Instance.Init(this, spriteBatch);
            LevelManager.Instance.SwitchLevel(0);
            //LevelManager.Instance.Levels[levelIndex].Build();
            
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            ObjectManager.Instance.LightMap = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false,
                                   pp.BackBufferFormat, pp.DepthStencilFormat, pp.MultiSampleCount,
                                   RenderTargetUsage.DiscardContents);
            ObjectManager.Instance.LightCache = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false,
                                   pp.BackBufferFormat, pp.DepthStencilFormat, pp.MultiSampleCount,
                                   RenderTargetUsage.DiscardContents);
            ObjectManager.Instance.ObjectCacheMap = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false,
                                  pp.BackBufferFormat, pp.DepthStencilFormat, pp.MultiSampleCount,
                                  RenderTargetUsage.DiscardContents);
           
            button = new MenuButton();
            button.boundingBody = new BoundingRect(20, 20, 40, 40);
            button.Click += CallbackManager.Instance.LevelClick;

            // start buton
            button1 = new MenuButton();
            button1.boundingBody = new BoundingRect(graphics.PreferredBackBufferWidth / 2 - 100, graphics.PreferredBackBufferHeight / 2 - 100, graphics.PreferredBackBufferWidth / 2 , graphics.PreferredBackBufferHeight / 2 - 80);
            //button1.Click += CallbackManager.Instance.LevelClick;
            
            // level buton
            button2 = new MenuButton();
            button2.boundingBody = new BoundingRect(graphics.PreferredBackBufferWidth / 2 - 100, graphics.PreferredBackBufferHeight / 2, graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2 + 23);
            //button2.Click += CallbackManager.Instance.LevelClick;

            // exit buton
            button3 = new MenuButton();
            button3.boundingBody = new BoundingRect(graphics.PreferredBackBufferWidth / 2 - 90, graphics.PreferredBackBufferHeight / 2 + 90, graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2 + 150);
            //button3.Click += CallbackManager.Instance.LevelClick;

            // level 1
            button4 = new MenuButton();
            button4.boundingBody = new BoundingRect(100, graphics.PreferredBackBufferHeight / 2 - 100, graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2 - 80);
            //button1.Click += CallbackManager.Instance.LevelClick;

            // level 2
            button5 = new MenuButton();
            button5.boundingBody = new BoundingRect(100, graphics.PreferredBackBufferHeight / 2, 190, graphics.PreferredBackBufferHeight / 2 + 23);
            //button5.Click += CallbackManager.Instance.LevelClick;

            // level 3
            button6 = new MenuButton();
            button6.boundingBody = new BoundingRect(90, graphics.PreferredBackBufferHeight / 2 + 90, graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2 + 150);
            //button6.Click += CallbackManager.Instance.LevelClick;

            ObjectManager.Instance.CacheIsDirty = true;

            fpsCounter = new FPSCounter(Content.Load<SpriteFont>("FPSFont"));
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(LevelManager.Instance.launch); MediaPlayer.Play(LevelManager.Instance.launch);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Player player = ObjectManager.Instance.Player;
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            fpsCounter.Update(gameTime);

            Vector2 currenet_position = ObjectManager.Instance.Player.Position;

            TouchCollection touches = TouchPanel.GetState();
            if (touches.Count > 0)
            {
                if (isInMenu)
                {
                    if (button1.boundingBody.Intersects(touches[0].Position))
                    {
                        button.Click(1);
                        isInMenu = false;
                       // isInLevelMenu = false;
                        
                        
                    }

                    if (button2.boundingBody.Intersects(touches[0].Position))
                    {

                        button.Click(1);
                        isInMenu = false;
                        isInLevelMenu = true;
                        intersectLevelButton = true;
                    }

                    if (button3.boundingBody.Intersects(touches[0].Position))
                    {
                        this.Exit();
                    }
                    /*
                    if (button.boundingBody.Intersects(touches[0].Position))
                    {
                        if (isInMenu == true)
                         {
                             for (int i = 1; i <= 2; i++)
                             {
                                 i = changeLevel;
                        button.Click(2);

                        fpsCounter.elapsedTime = 70000;

                    }*/
                }
                
                if (isInLevelMenu)
                {
                    if (button4.boundingBody.Intersects(touches[0].Position))
                    {
                        button.Click(1);
                      //  isInMenu = false;
                        isInLevelMenu = false;
                    }

                    if (button5.boundingBody.Intersects(touches[0].Position))
                    {
                        button.Click(2);
                      //  isInMenu = false;
                        //if (!intersectLevelButton)
                            isInLevelMenu = false;
                    }

                    if (button6.boundingBody.Intersects(touches[0].Position))
                    {
                        button.Click(3);
                       // isInMenu = false;
                        isInLevelMenu = false;
                    }
                }
                if (touches[0].State == TouchLocationState.Pressed)
                {
                    showDPat = true;
                }

                TouchPanel.EnabledGestures = GestureType.FreeDrag;
                //movingPath.Clear();
                while (TouchPanel.IsGestureAvailable)
                {
                    GestureSample gesture = TouchPanel.ReadGesture();
                    Vector2 pos1 = gesture.Position;
                    movingPath.Add(pos1);
                    TimeSpan s = gesture.Timestamp;
                }

                int counterLastElement = movingPath.Count - 1;

                    if ((movingPath.Count != 0)&&(!isGameOver))
                    {
                        Rectangle Dphat = new Rectangle((int)(_touchLocation.X), (int)(_touchLocation.Y), 110, 110);
                        Rectangle lastTouchRect = new Rectangle((int)(movingPath[counterLastElement].X), (int)(movingPath[counterLastElement].Y), 10, 10);

                        if (lastTouchRect.Intersects(Dphat))
                        {

                            differenceX = movingPath[movingPath.Count - 1].X - movingPath[0].X;
                            differenceY = movingPath[movingPath.Count - 1].Y - movingPath[0].Y;


                            if (Math.Abs(differenceX) > Math.Abs(differenceY))
                            {
                                movingPath.Clear();
                                //Moving Horizontal
                                if (differenceX > 0)
                                {
                                    if (directionToMove.X <= graphics.PreferredBackBufferWidth - 20)
                                    {
                                        //Moving right
                                        if (isInCollision())
                                            directionToMove = player.Origin;
                                        else
                                        {
                                            directionToMove.X += 1000 * (float)gameTime.ElapsedGameTime.TotalSeconds;

                                        }
                                    }
                                }
                                else if (differenceX < 0)
                                {
                                    //Moving left
                                    if (directionToMove.X >= 20)
                                        if (isInCollision())
                                            directionToMove = player.Origin;
                                        else
                                            directionToMove.X -= 1000 * (float)gameTime.ElapsedGameTime.TotalSeconds; 
                                }
                            }
                            else if (Math.Abs(differenceX) < Math.Abs(differenceY))
                            {
                                movingPath.Clear();
                                //Moving Vertical
                                if (differenceY > 0)
                                {
                                    //Moving down
                                    if (directionToMove.Y <= graphics.PreferredBackBufferHeight - 20)
                                        if (isInCollision())
                                            directionToMove = player.Origin;
                                        else
                                            directionToMove.Y += 1000 * (float)gameTime.ElapsedGameTime.TotalSeconds; 
                                }
                                else if (differenceY < 0)
                                {
                                    //Moving up
                                    if (directionToMove.Y >= 20)
                                        if (isInCollision())
                                            directionToMove = player.Origin;
                                        else
                                            directionToMove.Y -= 1000 * (float)gameTime.ElapsedGameTime.TotalSeconds; 
                                }
                            }
                        }
                    }



                    if (showDPat == true)
                    {
                        _touchLocation = new Vector2(touches[0].Position.X - 60, touches[0].Position.Y - 60);
                        showDPat = false;
                    }

                
                    if ((showDPat == false) && (touches[0].State == TouchLocationState.Released))
                    {
                        _touchLocation = new Vector2(touches[0].Position.X - 1600, touches[0].Position.Y - 1600);
                        showDPat = true;
                    }
                }
            int activatedLights = 0;
            foreach (LightSource ls in ObjectManager.Instance.StaticLights)
            {
                if (ObjectManager.Instance.Player.TriggerBody.Intersects(ls.Position))
                {
                    /*if (!ls.Active)
                    {
                        ls.Active = true;
                        ObjectManager.Instance.LightCacheIsDirty = true;
                    }*/
                    ls.SwitchState();

                }
                else
                {
                    if (ls.currentState == LightSource.LightSourceState.ON)
                        activatedLights++;
                }
            }

            if (activatedLights == ObjectManager.Instance.StaticLights.Count)
            {
                isLevelCompleted = true;
                fpsCounter.elapsedTime = 0;
            }

            if (fpsCounter.FPS == 0)
            {
                isGameOver = true;
            }
            
            TransitionManager.Instance.Update(gameTime);

            foreach (LightSource ls in ObjectManager.Instance.StaticLights)
            {
                ls.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            if (isInMenu == true)
            {
                LevelManager.Instance.Draw();
                //start button
                spriteBatch.Begin();
                spriteBatch.DrawString(fpsCounter.Font, "Start",
                    new Vector2(graphics.PreferredBackBufferWidth / 2 - 100, graphics.PreferredBackBufferHeight / 2 - 100), Color.White);
                spriteBatch.End();

                // level buton
                spriteBatch.Begin();
                spriteBatch.DrawString(fpsCounter.Font, "Level",
                    new Vector2(graphics.PreferredBackBufferWidth / 2 - 100, graphics.PreferredBackBufferHeight / 2), Color.White);
                spriteBatch.End();

                //exit button
                spriteBatch.Begin();
                spriteBatch.DrawString(fpsCounter.Font, "Exit",
                    new Vector2(graphics.PreferredBackBufferWidth / 2 - 90, graphics.PreferredBackBufferHeight / 2 + 100), Color.White);
                spriteBatch.End();

                TransitionManager.Instance.Draw(spriteBatch);
            }
            else if (isInLevelMenu == true)
            {
                //isInLevelMenu = true;
                LevelManager.Instance.Draw();

                //intersectLevelButton = true;

                //level 1
                spriteBatch.Begin();
                spriteBatch.DrawString(fpsCounter.Font, "Level 1",
                    new Vector2(100, graphics.PreferredBackBufferHeight / 2 - 100), Color.White);
                spriteBatch.End();

                // level 2
                spriteBatch.Begin();
                spriteBatch.DrawString(fpsCounter.Font, "Level 2",
                    new Vector2(100, graphics.PreferredBackBufferHeight / 2), Color.White);
                spriteBatch.End();

                //level 2
                spriteBatch.Begin();
                spriteBatch.DrawString(fpsCounter.Font, "Level 3",
                    new Vector2(100, graphics.PreferredBackBufferHeight / 2 + 100), Color.White);
                spriteBatch.End();

                TransitionManager.Instance.Draw(spriteBatch);
            } else {
                LevelManager.Instance.Draw();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                Player player = ObjectManager.Instance.Player;
                spriteBatch.Draw(player.Texture, directionToMove, null, player.Color, 0, player.Origin, player.Scale, SpriteEffects.None, 0);
                spriteBatch.End();

                TransitionManager.Instance.Draw(spriteBatch);

                // ++fpsCounter.TotalFrames;
                spriteBatch.Begin();
                spriteBatch.DrawString(fpsCounter.Font, string.Format("{0}", fpsCounter.FPS),
                    new Vector2(10.0f, 20.0f), Color.White);
                spriteBatch.End();

                if ((isGameOver == true) && (isInMenu == false))
                {
                    spriteBatch.Begin();
                    spriteBatch.DrawString(fpsCounter.Font, "Game over",
                        new Vector2(graphics.PreferredBackBufferWidth / 2 - 100, graphics.PreferredBackBufferHeight / 2), Color.White);
                    spriteBatch.End();
                    isInMenu = true;
                    isGameOver = false;
                    fpsCounter.elapsedTime = 70000;
                }

                if (isLevelCompleted == true)
                {
                    spriteBatch.Begin();
                    spriteBatch.DrawString(fpsCounter.Font, "Level is Completed",
                        new Vector2(graphics.PreferredBackBufferWidth / 2 - 120, graphics.PreferredBackBufferHeight / 2), Color.White);
                    spriteBatch.End();
                    isInLevelMenu = true;
                    isLevelCompleted = false;
                    fpsCounter.elapsedTime = 70000;
                }

                spriteBatch.Begin();
                spriteBatch.Draw(player.Texture, _touchLocation, Color.White);
                spriteBatch.End();
            }
            base.Draw(gameTime);
         
        }
        
        // Check for Collision
        private bool isInCollision()
        {
            bool inColision = false;
            ObjectManager.Instance.Player.Position = directionToMove; // new Vector2(directionToMove.X + 2, directionToMove.Y + 2);
            Vector2 delta_position = ObjectManager.Instance.Player.Position - directionToMove;
            delta_position.Normalize();

            foreach (ConvexHull hull in ObjectManager.Instance.Objects)
            {
                if (hull.Intersects(ObjectManager.Instance.Player.BoundingBody))
                {
                    Vector2 delta_x = new Vector2(delta_position.X, 0);
                    ObjectManager.Instance.Player.Position = directionToMove + delta_x;
                    if (hull.Intersects(ObjectManager.Instance.Player.BoundingBody))
                    {
                        Vector2 delta_y = new Vector2(0, delta_position.Y);
                        ObjectManager.Instance.Player.Position = directionToMove + delta_y;
                        if (hull.Intersects(ObjectManager.Instance.Player.BoundingBody))
                        {
                            ObjectManager.Instance.Player.Position = directionToMove;
                            inColision = true;
                            break;
                        }
                    }
                }
            }

            if (LevelManager.Instance.presentRectangle.Intersects(new Rectangle((int)directionToMove.X, (int)directionToMove.Y, 20, 20)))
            {
               fpsCounter.elapsedTime += 60000;
               LevelManager.Instance.presentRectangle = new Rectangle();
            }

            return inColision;  
        }
    }
}
