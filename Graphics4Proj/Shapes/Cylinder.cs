using Graphics4Proj.Geometry;
using System.Drawing;
using System.Numerics;

namespace Graphics4Proj.Shapes
{
    public class Cylinder : Mesh
    {
        public Cylinder(int division, double cylinderLength, double cylinderRadius, Color color)
        {
            Color = color;
            var angleDifference = 2 * Math.PI / division;
            double end = -cylinderLength / 2;
            var beginning = cylinderLength / 2;

                for (var i = 0; i<division; i++)
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

                    Faces.Add(opening);
                    Faces.Add(ending);
                    Faces.Add(longSide1);
                    Faces.Add(longSide2);
                }

            
        }
    }
}
