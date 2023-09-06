using Graphics4Proj.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Graphics4Proj.Shapes
{
    public class SphereGenerator
    {
        private int index;
        private Dictionary<long, int> middlePointIndexCache;

        private List<Vector3> points;

        public Face[] Create(int recursionLevel)
        {
            middlePointIndexCache = new Dictionary<long, int>();
            points = new List<Vector3>();
            index = 0;
            var t = (float)((1.0 + Math.Sqrt(5.0)) / 2.0);
            var s = 1;

            AddVertex(new Vector3(-s, t, 0));
            AddVertex(new Vector3(s, t, 0));
            AddVertex(new Vector3(-s, -t, 0));
            AddVertex(new Vector3(s, -t, 0));

            AddVertex(new Vector3(0, -s, t));
            AddVertex(new Vector3(0, s, t));
            AddVertex(new Vector3(0, -s, -t));
            AddVertex(new Vector3(0, s, -t));

            AddVertex(new Vector3(t, 0, -s));
            AddVertex(new Vector3(t, 0, s));
            AddVertex(new Vector3(-t, 0, -s));
            AddVertex(new Vector3(-t, 0, s));

            var faces = new List<Face>();

            // 5 faces around point 0
            faces.Add(new Face(points[0], points[11], points[5]));
            faces.Add(new Face(points[0], points[5], points[1]));
            faces.Add(new Face(points[0], points[1], points[7]));
            faces.Add(new Face(points[0], points[7], points[10]));
            faces.Add(new Face(points[0], points[10], points[11]));

            // 5 adjacent faces 
            faces.Add(new Face(points[1], points[5], points[9]));
            faces.Add(new Face(points[5], points[11], points[4]));
            faces.Add(new Face(points[11], points[10], points[2]));
            faces.Add(new Face(points[10], points[7], points[6]));
            faces.Add(new Face(points[7], points[1], points[8]));

            // 5 faces around point 3
            faces.Add(new Face(points[3], points[9], points[4]));
            faces.Add(new Face(points[3], points[4], points[2]));
            faces.Add(new Face(points[3], points[2], points[6]));
            faces.Add(new Face(points[3], points[6], points[8]));
            faces.Add(new Face(points[3], points[8], points[9]));

            // 5 adjacent faces 
            faces.Add(new Face(points[4], points[9], points[5]));
            faces.Add(new Face(points[2], points[4], points[11]));
            faces.Add(new Face(points[6], points[2], points[10]));
            faces.Add(new Face(points[8], points[6], points[7]));
            faces.Add(new Face(points[9], points[8], points[1]));


            // refine triangles
            for (var i = 0; i < recursionLevel; i++)
            {
                var faces2 = new List<Face>();
                foreach (var tri in faces)
                {
                    // replace triangle by 4 triangles
                    var a = GetMiddlePoint(tri.V1, tri.V2);
                    var b = GetMiddlePoint(tri.V2, tri.V3);
                    var c = GetMiddlePoint(tri.V3, tri.V1);

                    faces2.Add(new Face(tri.V1, points[a], points[c]));
                    faces2.Add(new Face(tri.V2, points[b], points[a]));
                    faces2.Add(new Face(tri.V3, points[c], points[b]));
                    faces2.Add(new Face(points[a], points[b], points[c]));
                }

                faces = faces2;
            }

            return faces.ToArray();
        }

        private int AddVertex(Vector3 p)
        {
            points.Add(Vector3.Normalize(p));
            return index++;
        }

        // return index of point in the middle of p1 and p2
        private int GetMiddlePoint(Vector3 point1, Vector3 point2)
        {
            long i1 = points.IndexOf(point1);
            long i2 = points.IndexOf(point2);
            // first check if we have it already
            var firstIsSmaller = i1 < i2;
            var smallerIndex = firstIsSmaller ? i1 : i2;
            var greaterIndex = firstIsSmaller ? i2 : i1;
            var key = (smallerIndex << 32) + greaterIndex;

            int ret;
            if (middlePointIndexCache.TryGetValue(key, out ret)) return ret;

            // not in cache, calculate it

            var middle = new Vector3(
                (point1.X + point2.X) / 2.0f,
                (point1.Y + point2.Y) / 2.0f,
                (point1.Z + point2.Z) / 2.0f);

            // add vertex makes sure point is on unit sphere
            var i = AddVertex(middle);

            // store it, return index
            middlePointIndexCache.Add(key, i);
            return i;
        }

        public struct Face
        {
            public Vector3 V1;
            public Vector3 V2;
            public Vector3 V3;

            public Face(Vector3 v1, Vector3 v2, Vector3 v3)
            {
                V1 = v1;
                V2 = v2;
                V3 = v3;
            }
        }
    }

    public static class ShapesGenerator
    {
        private static readonly Random Rnd = new();

        public static Mesh CreateFloor(Color color)
        {
            var floor = new Mesh();

            Vector3[] vertices =
            {
            new(-1, 0, 1),
            new(1, 0, 1),
            new(1, 0, -1),
            new(-1, 0, -1),
        };

            Vector3[] normals =
            {
            new(0, 1, 0),
            new(0, 1, 0),
            new(0, 1, 0),
            new(0, 1, 0)
        };

            floor.Faces.Add(new Face(vertices[0], vertices[1], vertices[3],
                normals[0], normals[1], normals[2])
            {
                Color = color
            });

            floor.Faces.Add(new Face(vertices[3], vertices[2], vertices[1],
                normals[0], normals[2], normals[3])
            {
                Color = color
            });

            return floor;
        }

        public static Mesh CreateCube()
        {
            var cube = new Mesh();

            Vector3[] vertices =
            {
            new(-1, -1, -1),
            new(1, -1, -1),
            new(1, 1, -1),
            new(-1, 1, -1),
            new(-1, 1, 1),
            new(1, 1, 1),
            new(1, -1, 1),
            new(-1, -1, 1)
        };

            Vector3[] normals =
            {
            new(0, 0, -1),
            new(0, 1, 0),
            new(1, 0, 0),
            new(-1, 0, 0),
            new(0, 0, 1),
            new(0, -1, 0)
        };

            int[] triangles =
            {
            0, 2, 1,
            0, 3, 2,
            2, 3, 4,
            2, 4, 5,
            1, 2, 5,
            1, 5, 6,
            0, 7, 4,
            0, 4, 3,
            5, 4, 7,
            5, 7, 6,
            0, 6, 7,
            0, 1, 6
        };

            var color = Color.FromArgb(Rnd.Next(256), Rnd.Next(256), Rnd.Next(256));

            for (var i = 0; i < triangles.Length; i += 3)
                cube.Faces.Add(new Face(vertices[triangles[i]], vertices[triangles[i + 1]], vertices[triangles[i + 2]], normals[i / 6], normals[i / 6], normals[i / 6])
                {
                    Color = color
                });

            return cube;
        }


        public static Mesh CreateSphere(int recursionLevel, Color color)
        {
            var generator = new SphereGenerator();
            var faces = generator.Create(recursionLevel);
            var sphere = new Mesh();

            foreach (var face in faces)
            {
                var side = new Face(face.V1, face.V2, face.V3, Vector3.Normalize(face.V1), Vector3.Normalize(face.V2), Vector3.Normalize(face.V3))
                {
                    Color = color
                };
                sphere.Faces.Add(side);
            }

            return sphere;
        }

        public static Mesh CreateCone(int subdivisions, float radius, float height)
        {
            var cone = new Mesh();

            var vertices = new Vector3[subdivisions + 2];
            var uv = new Vector2[vertices.Length];
            var triangles = new int[subdivisions * 2 * 3];

            vertices[0] = new Vector3(0, 0, 1);
            uv[0] = new Vector2(0.5f, 0f);
            for (int i = 0, n = subdivisions - 1; i < subdivisions; i++)
            {
                var ratio = (float)i / n;
                var r = (float)(ratio * (Math.PI * 2f));
                var x = (float)(Math.Cos(r) * radius);
                var z = (float)(Math.Sin(r) * radius);
                vertices[i + 1] = new Vector3(x, 0f, z + 1);

                uv[i + 1] = new Vector2(ratio, 0f);
            }

            vertices[subdivisions + 1] = new Vector3(0f, height, 1f);
            uv[subdivisions + 1] = new Vector2(0.5f, 1f);

            // construct bottom
            for (int i = 0, n = subdivisions - 1; i < n; i++)
            {
                var offset = i * 3;
                triangles[offset] = 0;
                triangles[offset + 1] = i + 1;
                triangles[offset + 2] = i + 2;
            }

            // construct sides
            var bottomOffset = subdivisions * 3;
            for (int i = 0, n = subdivisions - 1; i < n; i++)
            {
                var offset = i * 3 + bottomOffset;
                triangles[offset] = i + 1;
                triangles[offset + 1] = subdivisions + 1;
                triangles[offset + 2] = i + 2;
            }


            var color = Color.FromArgb(Rnd.Next(256), Rnd.Next(256), Rnd.Next(256));

            for (var i = 0; i < triangles.Length; i += 3)
            {
                var v1 = triangles[i];
                var v2 = triangles[i + 1];
                var v3 = triangles[i + 2];

                var nv1 = Vector3.Normalize(vertices[v1]);
                var nv2 = Vector3.Normalize(vertices[v2]);
                var nv3 = Vector3.Normalize(vertices[v3]);

                var side = new Face(vertices[v1], vertices[v2], vertices[v3], nv1, nv2, nv3)
                {
                    Color = color
                };
                cone.Faces.Add(side);
            }


            return cone;
        }

        public static Mesh CreateCylinder(int division, double cylinderLength, double cylinderRadius, Color color)
        {
            var cylinder = new Mesh { Color = color };
            var angleDifference = 2 * Math.PI / division;
            double end = -cylinderLength / 2;
            var beginning = cylinderLength / 2;

            for (var i = 0; i < division; i++)
            {
                var firstAngle = angleDifference * i - Math.PI;
                var secondAngle = angleDifference * (i + 1) - Math.PI;

                var firstX = cylinderRadius * Math.Sin(firstAngle);
                var firstY = cylinderRadius * Math.Cos(firstAngle);

                var secondX = cylinderRadius * Math.Sin(secondAngle);
                var secondY = cylinderRadius * Math.Cos(secondAngle);

                var ending = new Face(new Vector3(0, 0, (float)end),
                        new Vector3((float)firstX, (float)firstY, (float)end),
                        new Vector3((float)secondX, (float)secondY, (float)end),
                        new Vector3(0, 0, -1), new Vector3(0, 0, -1), new Vector3(0, 0, -1))
                { Color = color };


                var longSide1 = new Face(new Vector3((float)firstX, (float)firstY, (float)end),
                        new Vector3((float)secondX, (float)secondY, (float)end),
                        new Vector3((float)firstX, (float)firstY, (float)beginning),
                        Vector3.Normalize(new Vector3((float)firstX, (float)firstY, 0)),
                        Vector3.Normalize(new Vector3((float)secondX, (float)secondY, 0)),
                        Vector3.Normalize(new Vector3((float)firstX, (float)firstY, 0)))
                { Color = color };

                var longSide2 = new Face(new Vector3((float)secondX, (float)secondY, (float)end),
                        new Vector3((float)secondX, (float)secondY, (float)beginning),
                        new Vector3((float)firstX, (float)firstY, (float)beginning),
                        Vector3.Normalize(new Vector3((float)secondX, (float)secondY, 0)),
                        Vector3.Normalize(new Vector3((float)secondX, (float)secondY, 0)),
                        Vector3.Normalize(new Vector3((float)firstX, (float)firstY, 0)))
                { Color = color };


                var opening = new Face(new Vector3(0, 0, (float)beginning),
                        new Vector3((float)firstX, (float)firstY, (float)beginning),
                        new Vector3((float)secondX, (float)secondY, (float)beginning),
                        new Vector3(0, 0, 1), new Vector3(0, 0, 1), new Vector3(0, 0, 1))
                { Color = color, ChangeColor = true };

                cylinder.Faces.Add(opening);
                cylinder.Faces.Add(ending);
                cylinder.Faces.Add(longSide1);
                cylinder.Faces.Add(longSide2);
            }

            return cylinder;
        }
    }
}