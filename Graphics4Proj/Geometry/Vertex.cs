using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Graphics4Proj.Geometry
{
    public class Vertex
    {
        private readonly float initX;
        private readonly float initY;
        private readonly float initZ;

        public float X;
        public float Y;
        public float Z;

        public Vertex(float x, float y, float z)
        {
            initX = x;
            initY = y;
            initZ = z;
        }

        public Vector4 Vector => new(X, Y, Z, 1);
        public Vector4 Normal { get; set; }
        public Vector4 TransformedNormal { get; set; }

        public void ResetVertex()
        {
            X = initX;
            Y = initY;
            Z = initZ;
        }

        public void Transform(Matrix4x4 matrix, bool useNormals)
        {
            var transformed = matrix.Multiply(new Vector4(X, Y, Z, 1));

            transformed *= 1 / transformed.W;
            X = transformed.X;
            Y = transformed.Y;
            Z = transformed.Z;

            if (useNormals)
            {
                Matrix4x4.Invert(matrix, out matrix);
                var transposed = Matrix4x4.Transpose(matrix);
                var pNormal = transposed.Multiply(Normal);
                TransformedNormal = Vector4.Normalize(pNormal);
            }
        }
    }

    public static class ExtensionMethods
    {
        public static Vector4 Multiply(this Matrix4x4 matrix, Vector4 self)
        {
            return new Vector4(
                matrix.M11 * self.X + matrix.M12 * self.Y + matrix.M13 * self.Z + matrix.M14 * self.W,
                matrix.M21 * self.X + matrix.M22 * self.Y + matrix.M23 * self.Z + matrix.M24 * self.W,
                matrix.M31 * self.X + matrix.M32 * self.Y + matrix.M33 * self.Z + matrix.M34 * self.W,
                matrix.M41 * self.X + matrix.M42 * self.Y + matrix.M43 * self.Z + matrix.M44 * self.W
            );
        }
    }

    public enum Axis
    {
        X,
        Y,
        Z
    }
}
