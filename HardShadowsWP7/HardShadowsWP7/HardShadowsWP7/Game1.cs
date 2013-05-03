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
        Texture2D playerTexture;
        Vector2 playerPosition;

        Texture2D tileTexture;

        Color ambientColor;

        SpriteFont FPSFont;
        int _total_frames = 0;
        float _elapsed_time = 0.0f;
        int _fps = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
        }

        private void BuildObjectList()
        {

            ConvexHull.InitializeStaticMembers(GraphicsDevice);
            Color wallColor = Color.Black;

            Vector2[] points = new Vector2[4];

            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(0, 20);
            points[2] = new Vector2(580, 20);
            points[3] = new Vector2(580, 0);


            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(0, 140)));
            //ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(0, 440)));

            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(0, 140);
            points[2] = new Vector2(20, 140);
            points[3] = new Vector2(20, 0);


            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(580, 140)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(580, 320)));


            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(0, 20);
            points[2] = new Vector2(20, 20);
            points[3] = new Vector2(20, 0);


            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(640, 140)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(640, 240)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(640, 340)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(640, 440)));

            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(720, 140)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(720, 240)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(720, 340)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(720, 440)));


            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(0, 80);
            points[2] = new Vector2(20, 80);
            points[3] = new Vector2(20, 0);


            /*ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(100, 500)));

            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(200, 460)));

            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(300, 500)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(400, 460)));

            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(500, 500)));*/
            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(580, 460)));


            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(0, 40);
            points[2] = new Vector2(60, 40);
            points[3] = new Vector2(60, 0);


            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(160, 280)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(360, 280)));

            points[0] = new Vector2(0, -20);
            points[1] = new Vector2(-20, 0);
            points[2] = new Vector2(0, 20);
            points[3] = new Vector2(20, 0);

            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(460, 80)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(560, 80)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(160, 80)));

            points = new Vector2[8];
            float angleSlice = MathHelper.TwoPi / 8.0f;

            for (int i = 0; i < 8; i++)
            {
                points[i] = new Vector2((float)Math.Sin(angleSlice * i), (float)Math.Cos(angleSlice * i)) * 20;
            }

            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(140, 220)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(240, 220)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(340, 220)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(440, 220)));

            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(140, 380)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(240, 380)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(340, 380)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(440, 380)));

            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(80, 300)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(this, points, wallColor, new Vector2(500, 300)));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            BuildObjectList();

            playerTexture = Content.Load<Texture2D>("lightSphere");
            playerPosition = new Vector2(100, 100);

            tileTexture = Content.Load<Texture2D>("tile");
            
            Texture2D lightTexture = Content.Load<Texture2D>("light");
            ObjectManager.Instance.Lights.Add(new LightSource(lightTexture, Color.CornflowerBlue, 120, playerPosition));

            //ObjectManager.Instance.StaticLights.Add(new LightSource(lightTexture, Color.Crimson, 250, new Vector2(40, 400)));
            ObjectManager.Instance.StaticLights.Add(new LightSource(lightTexture, Color.Orange, 250, new Vector2(40, 200)));
            ObjectManager.Instance.StaticLights.Add(new LightSource(lightTexture, Color.Gold, 200, new Vector2(700, 450)));
            ObjectManager.Instance.StaticLights.Add(new LightSource(lightTexture, Color.Red, 150, new Vector2(510, 30)));
            ObjectManager.Instance.StaticLights.Add(new LightSource(lightTexture, Color.ForestGreen, 300, new Vector2(50, 540)));


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

            ambientColor = new Color(15, 15, 15, 255);

            FPSFont = Content.Load<SpriteFont>("FPSFont");
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

            _elapsed_time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
 
            // 1 Second has passed
            if (_elapsed_time >= 1000.0f)
            {
                _fps = _total_frames;
                _total_frames = 0;
                _elapsed_time = 0;
            }

            TouchCollection touches = TouchPanel.GetState();
            if (touches.Count > 0)
            {
                if (touches[0].State == TouchLocationState.Moved || touches[0].State == TouchLocationState.Pressed)
                {
                    playerPosition = touches[0].Position;
                }
            }

            ObjectManager.Instance.Lights[0].Position = playerPosition;

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
            Vector2 center = new Vector2(playerTexture.Width / 2, playerTexture.Height / 2);
            float scale = 16 / ((float)playerTexture.Width / 2.0f);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            //Vector2 origin = new Vector2(playerTexture.Width, playerTexture.Height) / 2.0f;
            spriteBatch.Draw(playerTexture, playerPosition, null, ObjectManager.Instance.Lights[0].Color, 0, center, scale, SpriteEffects.None, 0);
            spriteBatch.End();

            ++_total_frames;
            spriteBatch.Begin();
            spriteBatch.DrawString(FPSFont, string.Format("{0}", _fps),
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
            spriteBatch.Draw(tileTexture, Vector2.Zero, source, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.End();
        }

        private void DrawLightmap()
        {
            GraphicsDevice.SetRenderTarget(ObjectManager.Instance.LightMap);

            //clear to some small ambient light
            GraphicsDevice.Clear(ambientColor);

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
            GraphicsDevice.Clear(ambientColor);


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
