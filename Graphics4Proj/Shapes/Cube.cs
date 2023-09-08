using Graphics4Proj.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Graphics4Proj.Shapes
{
    public class Cube : Mesh
    {
        public Cube(Color color)
        {
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

            

            for (var i = 0; i < triangles.Length; i += 3)
                Faces.Add(new Face(vertices[triangles[i]], vertices[triangles[i + 1]], vertices[triangles[i + 2]], normals[i / 6], normals[i / 6], normals[i / 6])
                {
                    Color = color
                });
        }
    }
}
