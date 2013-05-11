using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HardShadows.GamePlay
{
    class TransitionManager
    {
        public enum TransitionState
        {
            PASSIVE = 0,
            ACTIVE
        }

        private static TransitionManager instance;

        public static TransitionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TransitionManager();
                }

                return instance;
            }
        }

        private TransitionState state;
        public TransitionState CurrentState
        {
            get { return state; }
        }

        private int timeStep;
        private double timeElapsed;
        private float alphaStep;
        private float currentAlpha;
        private short fadeDirection;
        private Texture2D dummyTexture;

        public BasicCallback EndCallback;
        public int EndState;

        private TransitionManager()
        {
            state = TransitionState.PASSIVE;
            currentAlpha = 0;
            alphaStep = 0.1f;
            fadeDirection = 1;
            timeElapsed = 0;
            timeStep = 40;
        }

        public void InitiateTransition()
        {
            state = TransitionState.ACTIVE;
            fadeDirection = 1;
        }

        public void Update(GameTime gameTime)
        {
            if (state == TransitionState.PASSIVE)
                return;

            timeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timeElapsed > timeStep)
            {
                timeElapsed = 0;

                currentAlpha += alphaStep * fadeDirection;

                if (currentAlpha >= 1)
                {
                    currentAlpha = 1.0f;
                    EndCallback(EndState);
                    fadeDirection = -1;
                }

                if (currentAlpha <= 0)
                {
                    currentAlpha = 0;
                    state = TransitionState.PASSIVE;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (state == TransitionState.PASSIVE)
                return;

            if(dummyTexture == null)
            {
                dummyTexture = new Texture2D(spriteBatch.GraphicsDevice, spriteBatch.GraphicsDevice.Viewport.Width, spriteBatch.GraphicsDevice.Viewport.Height);
                Color[] data = new Color[dummyTexture.Width * dummyTexture.Height];
                for (int i = 0; i < data.Length; ++i) data[i] = Color.Black;
                dummyTexture.SetData(data);
            }

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(dummyTexture, Vector2.Zero, new Color((int)(currentAlpha * 255), (int)(currentAlpha * 255), (int)(currentAlpha * 255), (int)(currentAlpha * 255)));
            spriteBatch.End();
        }
    }
}
