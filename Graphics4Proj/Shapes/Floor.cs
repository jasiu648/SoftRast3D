using Graphics4Proj.Geometry;
using System.Drawing;
using System.Numerics;

namespace Graphics4Proj.Shapes
{
    class Floor : Mesh
    {
        public Floor(Color color)
        {
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
            
            Faces.Add(new Face(vertices[0], vertices[1], vertices[3],
                normals[0], normals[1], normals[2])
            {
                Color = color
            });

            Faces.Add(new Face(vertices[3], vertices[2], vertices[1],
                normals[0], normals[2], normals[3])
            {
                Color = color
            });

        }
    }
}
