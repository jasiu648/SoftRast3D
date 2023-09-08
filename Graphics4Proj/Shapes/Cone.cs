using Graphics4Proj.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Graphics4Proj.Shapes
{
    public class Cone : Mesh
    {
        public Cone(Color color, int subdivisions, float radius, float height)
        {
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

            for (int i = 0, n = subdivisions - 1; i < n; i++)
            {
                var offset = i * 3;
                triangles[offset] = 0;
                triangles[offset + 1] = i + 1;
                triangles[offset + 2] = i + 2;
            }

            var bottomOffset = subdivisions * 3;
            for (int i = 0, n = subdivisions - 1; i < n; i++)
            {
                var offset = i * 3 + bottomOffset;
                triangles[offset] = i + 1;
                triangles[offset + 1] = subdivisions + 1;
                triangles[offset + 2] = i + 2;
            }  

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
                Faces.Add(side);
            }
        }
    }
}
