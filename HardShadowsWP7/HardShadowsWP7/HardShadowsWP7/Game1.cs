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

        FPSCounter fpsCounter;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ConvexHull.InitializeStaticMembers(GraphicsDevice);

            LevelManager.Instance.Init(this, spriteBatch);
            LevelManager.Instance.SwitchLevel(1);
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
                if (touches[0].State == TouchLocationState.Moved || touches[0].State == TouchLocationState.Pressed)
                {
                    ObjectManager.Instance.Player.Position = touches[0].Position;
                }
            }
            //double time = gameTime.TotalGameTime.TotalSeconds / 4.0f;
            //lights[3].Position = new Vector2(700, 300 + (float)Math.Sin(time) * 200);

            foreach (LightSource ls in ObjectManager.Instance.StaticLights)
            {
                if (ObjectManager.Instance.Player.TriggerBody.Intersects(ls.Position))
                {
                    if (!ls.Active)
                    {
                        ls.Active = true;
                        ObjectManager.Instance.LightCacheIsDirty = true;
                    }
                }
            }

            Vector2 delta_position = ObjectManager.Instance.Player.Position - currenet_position;
            delta_position.Normalize();

            foreach (ConvexHull hull in ObjectManager.Instance.Objects)
            {
                if (hull.Intersects(ObjectManager.Instance.Player.BoundingBody))
                {
                    Vector2 delta_x = new Vector2(delta_position.X, 0);
                    ObjectManager.Instance.Player.Position = currenet_position + delta_x;
                    if (hull.Intersects(ObjectManager.Instance.Player.BoundingBody))
                    {
                        Vector2 delta_y = new Vector2(0, delta_position.Y);
                        ObjectManager.Instance.Player.Position = currenet_position + delta_y;
                        if (hull.Intersects(ObjectManager.Instance.Player.BoundingBody))
                        {
                            ObjectManager.Instance.Player.Position = currenet_position;
                            break;
                        }
                    }
                }
            }

            ObjectManager.Instance.Lights[0].Position = ObjectManager.Instance.Player.Position;
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
            spriteBatch.Draw(player.Texture, player.Position, null, player.Color, 0, player.Origin, player.Scale, SpriteEffects.None, 0);
            spriteBatch.End();

            ++fpsCounter.TotalFrames;
            spriteBatch.Begin();
            spriteBatch.DrawString(fpsCounter.Font, string.Format("{0}", fpsCounter.FPS),
                new Vector2(10.0f, 20.0f), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        
    }
}
