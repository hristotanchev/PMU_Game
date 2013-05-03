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
       
        Texture2D alphaClearTexture;

        Player player;

        FPSCounter fpsCounter;

        Level level;

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

            level = new Level(this);

            level.Build();

            Texture2D playerTexture = Content.Load<Texture2D>("lightSphere");
            Vector2 playerPosition = new Vector2(100, 100);

            player = new Player(playerTexture, playerPosition, Color.CornflowerBlue, 32);
            
            Texture2D lightTexture = Content.Load<Texture2D>("light");
            ObjectManager.Instance.Lights.Add(new LightSource(lightTexture, player.Color, 120, playerPosition));

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

            alphaClearTexture = Content.Load<Texture2D>("AlphaOne");

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

            TouchCollection touches = TouchPanel.GetState();
            if (touches.Count > 0)
            {
                if (touches[0].State == TouchLocationState.Moved || touches[0].State == TouchLocationState.Pressed)
                {
                    player.Position = touches[0].Position;
                }
            }

            ObjectManager.Instance.Lights[0].Position = player.Position;

            //double time = gameTime.TotalGameTime.TotalSeconds / 4.0f;
            //lights[3].Position = new Vector2(700, 300 + (float)Math.Sin(time) * 200);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            // Cache static lights
            if (ObjectManager.Instance.CacheIsDirty)
            {
                CacheStaticLightMap();
                CacheObjectMap();
            }

            //build lightmap
            DrawLightmap();

            graphics.GraphicsDevice.Clear(Color.White);

            //draw objects
            DrawObjects();

            //multiply scene with lightmap
            spriteBatch.Begin(SpriteSortMode.Immediate, CustomBlendStates.Multiplicative);
            spriteBatch.Draw(ObjectManager.Instance.LightMap, Vector2.Zero, Color.White);
            spriteBatch.End();

            //draw player, fully lit
            Vector2 center = new Vector2(player.Texture.Width / 2, player.Texture.Height / 2);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            //Vector2 origin = new Vector2(playerTexture.Width, playerTexture.Height) / 2.0f;
            spriteBatch.Draw(player.Texture, player.Position, null, player.Color, 0, center, player.Scale, SpriteEffects.None, 0);
            spriteBatch.End();

            ++fpsCounter.TotalFrames;
            spriteBatch.Begin();
            spriteBatch.DrawString(fpsCounter.Font, string.Format("{0}", fpsCounter.FPS),
                new Vector2(10.0f, 20.0f), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawObjects()
        {

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            spriteBatch.Draw(ObjectManager.Instance.ObjectCacheMap, Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        private void DrawGround()
        {
            //draw the tile texture tiles across the screen
            Rectangle source = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            spriteBatch.Draw(level.TileTexture, Vector2.Zero, source, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.End();
        }

        private void DrawLightmap()
        {
            GraphicsDevice.SetRenderTarget(ObjectManager.Instance.LightMap);

            //clear to some small ambient light
            GraphicsDevice.Clear(level.AmbientColor);

            // Draw the cached lightmap to the full lightmap
            spriteBatch.Begin(SpriteSortMode.Immediate, CustomBlendStates.MultiplyWithAlpha);
            spriteBatch.Draw(ObjectManager.Instance.LightCache, Vector2.Zero, Color.White);
            spriteBatch.End();

            DrawShadowsToRenderTarget(ObjectManager.Instance.Lights);
        }

        private void CacheStaticLightMap()
        {
            GraphicsDevice.SetRenderTarget(ObjectManager.Instance.LightCache);

            //clear to some small ambient light
            GraphicsDevice.Clear(level.AmbientColor);


            DrawShadowsToRenderTarget(ObjectManager.Instance.StaticLights);
        }

        private void CacheObjectMap()
        {
            GraphicsDevice.SetRenderTarget(ObjectManager.Instance.ObjectCacheMap);

            // Draw the ground to the cache
            DrawGround();

            foreach (ConvexHull hull in ObjectManager.Instance.Objects)
            {
                hull.Draw();
            }
        }

        private void DrawShadowsToRenderTarget(List<LightSource> lightsToShade)
        {
            foreach (LightSource light in lightsToShade)
            {
                if (light.Active)
                {
                    //clear alpha to 1
                    ClearAlphaToOne();

                    //draw all shadows
                    //write only to the alpha channel, which sets alpha to 0
                    GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                    GraphicsDevice.BlendState = CustomBlendStates.WriteToAlpha;

                    foreach (ConvexHull ch in ObjectManager.Instance.Objects)
                    {
                        //draw shadow
                        ch.DrawShadows(light);
                    }

                    //draw the light shape
                    //where Alpha is 0, nothing will be written
                    spriteBatch.Begin(SpriteSortMode.Immediate, CustomBlendStates.MultiplyWithAlpha);
                    light.Draw(spriteBatch);
                    spriteBatch.End();
                }
            }
            //clear alpha, to avoid messing stuff up later
            ClearAlphaToOne();
            GraphicsDevice.SetRenderTarget(null);

            ObjectManager.Instance.CacheIsDirty = false;
        }

        private void ClearAlphaToOne()
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, CustomBlendStates.WriteToAlpha);
            spriteBatch.Draw(alphaClearTexture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.End();
        }
    }
}
