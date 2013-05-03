using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HardShadows.Engine;


namespace HardShadows
{
    class ConvexHull
    {
        static BasicEffect drawingEffect;

        public static void InitializeStaticMembers(GraphicsDevice device)
        {
            //by making these variables static between objects,
            //we save time and memory
            drawingEffect = new BasicEffect(device);
            drawingEffect.TextureEnabled = false;
            drawingEffect.VertexColorEnabled = true;
            drawingEffect.LightingEnabled = false;

            PresentationParameters pp = device.PresentationParameters;
            Matrix proj = Matrix.CreateOrthographicOffCenter(0, pp.BackBufferWidth, pp.BackBufferHeight, 0, 1, 50);
            Matrix viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 5), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            drawingEffect.World = Matrix.Identity;
            drawingEffect.Projection = proj;
            drawingEffect.View = viewMatrix;
        }


        private Game game;
        private VertexPositionColor[] vertices;
        public VertexPositionColor[] Vertices
        {
            get
            {
                return vertices;
            }
        }

        private short[] indices;
        int primitiveCount;

        bool[] backFacing;
        VertexPositionColor[] shadowVertices;

        private BoundingRect boundingBox;
        private BoundingCircle boundingSphere;

        private bool sphereBox;

        private Vector2 position = Vector2.Zero;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public ConvexHull(Game game, Vector2[] points, Color color, Vector2 position)
        {
            this.game = game;
            this.position = position;

            int vertexCount = points.Length;
            vertices = new VertexPositionColor[vertexCount + 1];
            Vector2 center = Vector2.Zero;

            bool zeroPoint = false;

            for (int i = 0; i < vertexCount; i++)
            {
                vertices[i] = new VertexPositionColor(new Vector3(points[i], 0), color);
                center += points[i];

                if (!zeroPoint && points[i] == Vector2.Zero)
                {
                    zeroPoint = true;
                }
            }
            center /= points.Length;
            vertices[vertexCount] = new VertexPositionColor(new Vector3(center, 0), color);

            primitiveCount = points.Length;
            indices = new short[primitiveCount * 3];

            for (int i = 0; i < primitiveCount; i++)
            {
                indices[3 * i] = (short)i;
                indices[3 * i + 1] = (short)((i + 1) % vertexCount);
                indices[3 * i + 2] = (short)vertexCount;
            }
            backFacing = new bool[vertexCount];

            sphereBox = points.Length != 4 || !zeroPoint;

            if (sphereBox)
            {
                boundingSphere = new BoundingCircle();
                float boundingRadius = 0;

                foreach (Vector2 point in points)
                {
                    if ((center - point).Length() > boundingRadius)
                    {
                        boundingRadius = (center - point).Length();
                    }
                }

                boundingSphere.Center = center + position;
                boundingSphere.Radius = boundingRadius;
            }
            else
            {
                boundingBox = new BoundingRect();
                List<Vector2> realPoints = new List<Vector2>();

                foreach (Vector2 point in points)
                {
                    realPoints.Add(point + position);
                }

                boundingBox.CreateFromPoints(realPoints);
            }
        }

        public void Draw()
        {
            GraphicsDevice device = game.GraphicsDevice;

            device.RasterizerState = RasterizerState.CullNone;
            device.BlendState = BlendState.Opaque;

            drawingEffect.World = Matrix.CreateTranslation(position.X - 0.01f, position.Y - 0.01f, 0);

            foreach (EffectPass pass in drawingEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, primitiveCount);
            }
        }

        public void DrawShadows(LightSource lightSource)
        {
            if (!Intersects(new BoundingCircle(lightSource.Position, lightSource.Range)))
                return;
            //compute facing of each edge, using N*L
            for (int i = 0; i < primitiveCount; i++)
            {
                Vector2 firstVertex = new Vector2(vertices[i].Position.X, vertices[i].Position.Y) + position;
                int secondIndex = (i + 1) % primitiveCount;
                Vector2 secondVertex = new Vector2(vertices[secondIndex].Position.X, vertices[secondIndex].Position.Y) + position;
                Vector2 middle = (firstVertex + secondVertex) / 2;

                Vector2 L = lightSource.Position - middle;

                Vector2 N = new Vector2();
                N.X = -(secondVertex.Y - firstVertex.Y);
                N.Y = secondVertex.X - firstVertex.X;

                if (Vector2.Dot(N, L) > 0)
                    backFacing[i] = false;
                else
                    backFacing[i] = true;
            }

            //find beginning and ending vertices which
            //belong to the shadow
            int startingIndex = 0;
            int endingIndex = 0;
            for (int i = 0; i < primitiveCount; i++)
            {
                int currentEdge = i;
                int nextEdge = (i + 1) % primitiveCount;

                if (backFacing[currentEdge] && !backFacing[nextEdge])
                    endingIndex = nextEdge;

                if (!backFacing[currentEdge] && backFacing[nextEdge])
                    startingIndex = nextEdge;
            }

            int shadowVertexCount;

            //nr of vertices that are in the shadow

            if (endingIndex > startingIndex)
                shadowVertexCount = endingIndex - startingIndex + 1;
            else
                shadowVertexCount = primitiveCount + 1 - startingIndex + endingIndex;

            shadowVertices = new VertexPositionColor[shadowVertexCount * 2];

            //create a triangle strip that has the shape of the shadow
            int currentIndex = startingIndex;
            int svCount = 0;
            while (svCount != shadowVertexCount * 2)
            {
                Vector3 vertexPos = vertices[currentIndex].Position + new Vector3(position, 0);

                //one vertex on the hull
                shadowVertices[svCount] = new VertexPositionColor();
                shadowVertices[svCount].Color = Color.Transparent;
                shadowVertices[svCount].Position = vertexPos;

                //shadowVertices[svCount].Position.X *= 1.00002f;
                //shadowVertices[svCount].Position.Y *= 1.00002f;

                //one extruded by the light direction
                shadowVertices[svCount + 1] = new VertexPositionColor();
                shadowVertices[svCount + 1].Color = Color.Transparent;
                Vector3 L2P = vertexPos - new Vector3(lightSource.Position, 0);
                L2P.Normalize();
                shadowVertices[svCount + 1].Position = new Vector3(lightSource.Position, 0) + L2P * 9000;

                svCount += 2;
                currentIndex = (currentIndex + 1) % primitiveCount;
            }


            drawingEffect.World = Matrix.Identity;
            drawingEffect.CurrentTechnique.Passes[0].Apply();

            game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, shadowVertices, 0, shadowVertexCount * 2 - 2);
        }

        private bool Intersects(BoundingCircle intersector)
        {
            //(lightSource.Position - (center + position)).LengthSquared() > (boundingRadius + lightSource.Range) * (boundingRadius + lightSource.Range)
            if (sphereBox)
            {
                return boundingSphere.Intersects(intersector);
            }
            else
            {
                return boundingBox.Intersects(intersector);
            }
        }
    }
}
