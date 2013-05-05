using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO.IsolatedStorage;

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

        Color wallColor;
        Texture2D lightTexture;

        string levelFileName;

        public Level(Game game, string fileName)
        {
            this.game = game;
            levelFileName = fileName;
        }

        public void Build()
        {
            ObjectManager.Instance.Clear();

            wallColor = Color.Black;
            tileTexture = game.Content.Load<Texture2D>("tile");
            lightTexture = game.Content.Load<Texture2D>("light");
            Texture2D playerTexture = game.Content.Load<Texture2D>("lightSphere");

            using (StreamReader sr = new StreamReader(TitleContainer.OpenStream("Content/" + levelFileName + ".txt")))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("player"))
                    {
                        string[] values = line.Split(' ');
                        float x = (float)Int32.Parse(values[1]);
                        float y = (float)Int32.Parse(values[2]);
                        int size = Int32.Parse(values[3]);
                        int lightRange = Int32.Parse(values[4]);
                        int colorIndex = Int32.Parse(values[5]);

                        ObjectManager.Instance.Player = new Player(playerTexture, new Vector2(x, y), ObjectManager.Instance.Colors[colorIndex], size);
                        ObjectManager.Instance.Lights.Add(new LightSource(lightTexture, ObjectManager.Instance.Colors[colorIndex], lightRange, new Vector2(x, y)));
                        ObjectManager.Instance.Lights[0].Active = true;
                    }
                    else if (line.StartsWith("rect"))
                    {
                        string[] values = line.Split(' ');
                        float x = (float)Int32.Parse(values[1]);
                        float y = (float)Int32.Parse(values[2]);
                        int width = Int32.Parse(values[3]);
                        int height = Int32.Parse(values[4]);

                        Vector2[] points = new Vector2[4];

                        points[0] = new Vector2(0, 0);
                        points[1] = new Vector2(0, height);
                        points[2] = new Vector2(width, height);
                        points[3] = new Vector2(width, 0);

                        ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(x, y)));
                    }
                    else if (line.StartsWith("drect"))
                    {
                        string[] values = line.Split(' ');
                        float x = (float)Int32.Parse(values[1]);
                        float y = (float)Int32.Parse(values[2]);

                        Vector2[] points = new Vector2[4];
                        points[0] = new Vector2(Int32.Parse(values[3]), Int32.Parse(values[4]));
                        points[1] = new Vector2(Int32.Parse(values[4]), Int32.Parse(values[5]));
                        points[2] = new Vector2(Int32.Parse(values[5]), Int32.Parse(values[6]));
                        points[3] = new Vector2(Int32.Parse(values[6]), Int32.Parse(values[3]));

                        ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(x, y)));
                    }
                    else if (line.StartsWith("ngon"))
                    {
                        string[] values = line.Split(' ');
                        float x = (float)Int32.Parse(values[1]);
                        float y = (float)Int32.Parse(values[2]);
                        int numSides = Int32.Parse(values[3]);
                        int width = Int32.Parse(values[4]);

                        Vector2[] points = new Vector2[numSides];
                        float angleSlice = MathHelper.TwoPi / ((float)numSides);

                        for (int i = 0; i < numSides; i++)
                        {
                            points[i] = new Vector2((float)Math.Sin(angleSlice * i), (float)Math.Cos(angleSlice * i)) * width;
                        }

                        ObjectManager.Instance.Objects.Add(new ConvexHull(game, points, wallColor, new Vector2(x, y)));
                    }
                    else if (line.StartsWith("light"))
                    {
                        string[] values = line.Split(' ');
                        float x = (float)Int32.Parse(values[1]);
                        float y = (float)Int32.Parse(values[2]);
                        float range = Int32.Parse(values[3]);
                        int colorIndex = Int32.Parse(values[4]);

                        ObjectManager.Instance.StaticLights.Add(new LightSource(lightTexture, ObjectManager.Instance.Colors[colorIndex], range, new Vector2(x, y)));
                    }
                }
            }

            ObjectManager.Instance.CacheIsDirty = true;

            ambientColor = new Color(15, 15, 15, 255);
        }
    }
}
