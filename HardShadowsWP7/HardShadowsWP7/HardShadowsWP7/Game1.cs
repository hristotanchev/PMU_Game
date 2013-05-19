using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
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
        private DPadControllerButtons dRight, dLeft, dUp, dDown, dNone;

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

            ObjectManager.Instance.CacheIsDirty = true;

            fpsCounter = new FPSCounter(Content.Load<SpriteFont>("FPSFont"));
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
                if (button.boundingBody.Intersects(touches[0].Position))
                {
                    button.Click(1);
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
                    //DateTime now = DateTime.Now;
                    //TimeSpan k = now - s;
                    //if (gesture.GestureType == GestureType.Hold)
                    //directionToMove.X += 60;//1000 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                int counterLastElement = movingPath.Count - 1;

                    if (movingPath.Count != 0)
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
                                            directionToMove.X -= 13;
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
                                            directionToMove.Y += 11;
                                }
                                else if (differenceY < 0)
                                {
                                    //Moving up
                                    if (directionToMove.Y >= 20)
                                        if (isInCollision())
                                            directionToMove = player.Origin;
                                        else
                                            directionToMove.Y -= 13;
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
            LevelManager.Instance.Draw();
            //draw player, fully lit
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            //Vector2 origin = new Vector2(playerTexture.Width, playerTexture.Height) / 2.0f;
            Player player = ObjectManager.Instance.Player;
            //directionToMove = player.Origin;
            spriteBatch.Draw(player.Texture, directionToMove, null, player.Color, 0, player.Origin, player.Scale, SpriteEffects.None, 0);
            spriteBatch.End();

            TransitionManager.Instance.Draw(spriteBatch);

            ++fpsCounter.TotalFrames;
            spriteBatch.Begin();
            spriteBatch.DrawString(fpsCounter.Font, string.Format("{0}", fpsCounter.FPS),
                new Vector2(10.0f, 20.0f), Color.White);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.Draw(player.Texture, _touchLocation, Color.White);
            spriteBatch.End();

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
            return inColision;  
        }
    }
}
