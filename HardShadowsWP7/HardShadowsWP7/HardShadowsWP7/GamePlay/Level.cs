using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HardShadows.GamePlay
{
    class Level
    {
        private Game game;

        private Color ambientColor;
        public Color AmbientColor
        {
            get { return ambientColor; }
            set { ambientColor = value; }
        }

        private Texture2D tileTexture;
        public Texture2D TileTexture
        {
            get { return tileTexture; }
            set { tileTexture = value; }
        }

        public Level(Game game)
        {
            this.game = game;
        }

        public void Build()
        {
            ObjectManager.Instance.Clear();

            Color wallColor = Color.Black;

            Vector2[] points = new Vector2[4];

            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(0, 20);
            points[2] = new Vector2(580, 20);
            points[3] = new Vector2(580, 0);


            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(0, 140)));
            //ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(0, 440)));

            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(0, 140);
            points[2] = new Vector2(20, 140);
            points[3] = new Vector2(20, 0);


            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(580, 140)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(580, 320)));


            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(0, 20);
            points[2] = new Vector2(20, 20);
            points[3] = new Vector2(20, 0);


            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(640, 140)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(640, 240)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(640, 340)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(640, 440)));

            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(720, 140)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(720, 240)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(720, 340)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(720, 440)));


            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(0, 80);
            points[2] = new Vector2(20, 80);
            points[3] = new Vector2(20, 0);


            /*ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(100, 500)));

            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(200, 460)));

            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(300, 500)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(400, 460)));

            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(500, 500)));*/
            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(580, 460)));


            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(0, 40);
            points[2] = new Vector2(60, 40);
            points[3] = new Vector2(60, 0);


            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(160, 280)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(360, 280)));

            points[0] = new Vector2(0, -20);
            points[1] = new Vector2(-20, 0);
            points[2] = new Vector2(0, 20);
            points[3] = new Vector2(20, 0);

            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(460, 80)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(560, 80)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(160, 80)));

            points = new Vector2[8];
            float angleSlice = MathHelper.TwoPi / 8.0f;

            for (int i = 0; i < 8; i++)
            {
                points[i] = new Vector2((float)Math.Sin(angleSlice * i), (float)Math.Cos(angleSlice * i)) * 20;
            }

            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(140, 220)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(240, 220)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(340, 220)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(440, 220)));

            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(140, 380)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(240, 380)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(340, 380)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(440, 380)));

            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(80, 300)));
            ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(500, 300)));

            tileTexture = game.Content.Load<Texture2D>("tile");

            Texture2D lightTexture = game.Content.Load<Texture2D>("light");

            //ObjectManager.Instance.StaticLights.Add(new LightSource(lightTexture, Color.Crimson, 250, new Vector2(40, 400)));
            ObjectManager.Instance.StaticLights.Add(new LightSource(lightTexture, Color.Orange, 250, new Vector2(40, 200)));
            ObjectManager.Instance.StaticLights.Add(new LightSource(lightTexture, Color.Gold, 200, new Vector2(700, 450)));
            ObjectManager.Instance.StaticLights.Add(new LightSource(lightTexture, Color.Red, 150, new Vector2(510, 30)));
            ObjectManager.Instance.StaticLights.Add(new LightSource(lightTexture, Color.ForestGreen, 300, new Vector2(100, 440)));

            ObjectManager.Instance.CacheIsDirty = true;

            ambientColor = new Color(15, 15, 15, 255);
        }
    }
}
