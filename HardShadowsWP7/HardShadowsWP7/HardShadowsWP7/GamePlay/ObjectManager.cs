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

        List<ConvexHull> staticObjects;
        public List<ConvexHull> StaticObjects
        {
            get
            {
                return staticObjects;
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

        bool cacheIsDirty;
        public bool CacheIsDirty
        {
            set
            {
                cacheIsDirty = value;
            }

            get
            {
                return cacheIsDirty;
            }
        }

        private ObjectManager()
        {
            objects = new List<ConvexHull>();
            staticObjects = new List<ConvexHull>();
            lights = new List<LightSource>();
            staticLights = new List<LightSource>();

            cacheIsDirty = true;
        }
    }
}
