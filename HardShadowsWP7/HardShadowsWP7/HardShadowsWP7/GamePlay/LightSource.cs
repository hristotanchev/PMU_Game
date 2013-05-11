using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HardShadows.GamePlay;

namespace HardShadows
{
    class LightSource
    {
        private Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        private float range;

        public float Range
        {
            get { return range; }
            set { range = value; }
        }

        private Color color;

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        private Texture2D lightTexture;

        public Texture2D LightTexture
        {
            get { return lightTexture; }
            set { lightTexture = value; }
        }

        public void SwitchState()
        {
            if (currentState != LightSourceState.CHANGING)
            {
                targetState = currentState == LightSourceState.ON ? LightSourceState.OFF : LightSourceState.ON;
                currentState = LightSourceState.CHANGING;
            }
        }

        private float offRange;
        private float onRange;
        private int timeStep;
        private double timeElapsed;
        private float rangeStep;

        public void ForceOn()
        {
            range = onRange;
        }

        enum LightSourceState
        {
            ON = 0,
            OFF,
            CHANGING
        }

        LightSourceState currentState;
        LightSourceState targetState;

        public LightSource(Texture2D texture, Color color, float range, Vector2 position)
        {
            lightTexture = texture;
            this.color = color;
            offRange = 30;
            onRange = range;
            this.range = offRange;
            this.position = position;
            currentState = LightSourceState.OFF;
            timeElapsed = 0;
            timeStep = 40;
            rangeStep = 30;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 center = new Vector2(lightTexture.Width / 2, lightTexture.Height / 2);
            float scale = range / ((float)lightTexture.Width / 2.0f);
            spriteBatch.Draw(lightTexture, position, null, color, 0, center, scale, SpriteEffects.None, 1);
        }

        public void Update(GameTime gameTime)
        {
            if (currentState == LightSourceState.CHANGING)
            {
                timeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

                if (timeElapsed > timeStep)
                {
                    timeElapsed = 0;
                    if (targetState == LightSourceState.OFF)
                    {
                        range -= rangeStep;
                    }
                    else
                    {
                        range += rangeStep;
                    }

                    if (range < offRange)
                    {
                        range = offRange;
                        currentState = LightSourceState.OFF;
                    }
                    else if (range > onRange)
                    {
                        range = onRange;
                        currentState = LightSourceState.ON;
                    }
                }

                ObjectManager.Instance.LightCacheIsDirty = true;
            }
        }
    }
}
