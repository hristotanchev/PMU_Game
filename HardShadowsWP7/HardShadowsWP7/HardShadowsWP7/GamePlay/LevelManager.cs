using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace HardShadows.GamePlay
{
    class LevelManager
    {
        private static LevelManager instance;

        public static LevelManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LevelManager();
                }

                return instance;
            }
        }

        private List<Level> levels;
        public List<Level> Levels
        {
            get
            {
                return levels;
            }
        }

        private int levelIndex;

        private Game game;
        private SpriteBatch spriteBatch;
        private Texture2D alphaClearTexture;
        private Texture2D presentTexture;
        public Rectangle presentRectangle = new Rectangle(700, 30, 50, 50);
        public Song launch;

        private LevelManager()
        {
            levels = new List<Level>();
        }

        public void Init(Game game, SpriteBatch spriteBatch)
        {
            levels.Add(new Level(game, "Level1"));
            levels.Add(new Level(game, "Level2"));
            levels.Add(new Level(game, "Level3"));
            levels.Add(new Level(game, "Level4"));
            this.game = game;
            this.spriteBatch = spriteBatch;
            levelIndex = 0;
            alphaClearTexture = game.Content.Load<Texture2D>("AlphaOne");
            presentTexture = game.Content.Load<Texture2D>("redPresent");
            launch = game.Content.Load<Song>("melody");
        }

        public void SwitchLevel(int index)
        {
            levelIndex = index;
            levels[index].Build();
        }

        public void Draw()
        {
            // Cache static lights
            if (ObjectManager.Instance.LightCacheIsDirty)
            {
                CacheStaticLightMap();
            }

            if (ObjectManager.Instance.ObjectCacheIsDirty)
            {
                CacheObjectMap();
            }

            //build lightmap
            DrawLightmap();

            game.GraphicsDevice.Clear(Color.White);

            //draw objects
            DrawObjects();

            //multiply scene with lightmap
            spriteBatch.Begin(SpriteSortMode.Immediate, CustomBlendStates.Multiplicative);
            spriteBatch.Draw(ObjectManager.Instance.LightMap, Vector2.Zero, Color.White);
            spriteBatch.End();

            //draw present 
            spriteBatch.Begin();
            spriteBatch.Draw(presentTexture, presentRectangle, Color.Red);
            spriteBatch.End();
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
            Rectangle source = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            game.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            spriteBatch.Draw(LevelManager.Instance.Levels[levelIndex].TileTexture, Vector2.Zero, source, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.End();
        }

        private void DrawLightmap()
        {
            game.GraphicsDevice.SetRenderTarget(ObjectManager.Instance.LightMap);

            //clear to some small ambient light
            game.GraphicsDevice.Clear(LevelManager.Instance.Levels[levelIndex].AmbientColor);

            // Draw the cached lightmap to the full lightmap
            spriteBatch.Begin(SpriteSortMode.Immediate, CustomBlendStates.MultiplyWithAlpha);
            spriteBatch.Draw(ObjectManager.Instance.LightCache, Vector2.Zero, Color.White);
            spriteBatch.End();

            DrawShadowsToRenderTarget(ObjectManager.Instance.Lights);
        }

        private void CacheStaticLightMap()
        {
            game.GraphicsDevice.SetRenderTarget(ObjectManager.Instance.LightCache);

            //clear to some small ambient light
            game.GraphicsDevice.Clear(LevelManager.Instance.Levels[levelIndex].AmbientColor);


            DrawShadowsToRenderTarget(ObjectManager.Instance.StaticLights);
        }

        private void CacheObjectMap()
        {
            game.GraphicsDevice.SetRenderTarget(ObjectManager.Instance.ObjectCacheMap);

            // Draw the ground to the cache
            DrawGround();

            foreach (ConvexHull hull in ObjectManager.Instance.Objects)
            {
                hull.Draw();
            }

            ObjectManager.Instance.ObjectCacheIsDirty = false;
        }

        private void DrawShadowsToRenderTarget(List<LightSource> lightsToShade)
        {
            foreach (LightSource light in lightsToShade)
            {
                //clear alpha to 1
                ClearAlphaToOne();

                //draw all shadows
                //write only to the alpha channel, which sets alpha to 0
                game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                game.GraphicsDevice.BlendState = CustomBlendStates.WriteToAlpha;

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
            //clear alpha, to avoid messing stuff up later
            ClearAlphaToOne();
            game.GraphicsDevice.SetRenderTarget(null);

            ObjectManager.Instance.LightCacheIsDirty = false;
        }

        private void ClearAlphaToOne()
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, CustomBlendStates.WriteToAlpha);
            spriteBatch.Draw(alphaClearTexture, new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.End();
        }
    }
}
