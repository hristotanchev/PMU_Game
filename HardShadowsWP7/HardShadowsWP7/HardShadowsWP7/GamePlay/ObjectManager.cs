using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HardShadows.GamePlay
{
    class ObjectManager
    {
        private static ObjectManager instance;

        public static ObjectManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ObjectManager();
                }

                return instance;
            }
        }

        List<ConvexHull> objects;
        public List<ConvexHull> Objects
        {
            get
            {
                return objects;
            }
        }

        List<LightSource> lights;
        public List<LightSource> Lights
        {
            get
            {
                return lights;
            }
        }

        List<LightSource> staticLights;
        public List<LightSource> StaticLights
        {
            get
            {
                return staticLights;
            }
        }

        RenderTarget2D lightMap;
        public RenderTarget2D LightMap
        {
            get
            {
                return lightMap;
            }

            set
            {
                lightMap = value;
            }
        }

        RenderTarget2D lightCache;
        public RenderTarget2D LightCache
        {
            get
            {
                return lightCache;
            }

            set
            {
                lightCache = value;
            }
        }

        RenderTarget2D objectCacheMap;
        public RenderTarget2D ObjectCacheMap
        {
            get
            {
                return objectCacheMap;
            }

            set
            {
                objectCacheMap = value;
            }
        }

        bool lightCacheIsDirty;
        public bool LightCacheIsDirty
        {
            set { lightCacheIsDirty = value; }
            get { return lightCacheIsDirty; }
        }

        public bool CacheIsDirty
        {
            set { lightCacheIsDirty = value; objectCacheIsDirty = value; }
            get { return lightCacheIsDirty || objectCacheIsDirty; }
        }

        bool objectCacheIsDirty;
        public bool ObjectCacheIsDirty
        {
            set { objectCacheIsDirty = value; }
            get { return objectCacheIsDirty; }
        }

        Player player;
        public Player Player
        {
            get { return player; }
            set { player = value; }
        }

        List<Color> colors;
        public List<Color> Colors
        {
            get { return colors; }
        }

        private ObjectManager()
        {
            objects = new List<ConvexHull>();
            lights = new List<LightSource>();
            staticLights = new List<LightSource>();
            
            colors = new List<Color>();
            colors.Add(Color.CornflowerBlue);
            colors.Add(Color.Orange);
            colors.Add(Color.Gold);
            colors.Add(Color.Red);
            colors.Add(Color.ForestGreen);

            lightCacheIsDirty = true;
            objectCacheIsDirty = true;
        }

        public void Clear()
        {
            lights.Clear();
            staticLights.Clear();
            objects.Clear();
        }
    }
}
