using Graphics4Proj.Devices;
using Graphics4Proj.Geometry;
using Graphics4Proj.Lights;
using Graphics4Proj.Shapes;
using System.Numerics;

namespace Graphics4Proj
{
    public partial class Form1 : Form
    {
        //Constants
        private const float BehindDistance = 6;
        private const float BehindHeight = 2;
        private const float CubeZDist = 6;
        private const float CubeMovementRange = 4;
        private const float TargetDistance = 7;

        private double cubeAngle;
        private float cubeChange = 0.05f;
        private float cubeShift;
        private double cylinderAngle;

        //Options chosen
        private bool fogOn;
        private bool stopAnimation;
        private bool changedLight;

        //Shapes
        private Mesh cube;
        private Mesh lighter;
        private Mesh sphere;
        private Mesh floor;
        private Mesh cone;

        private readonly Device device;
        private DirectBitmap bitmap;

        private Light globalLight;
        private Light spotLight;

        private Camera staticCamera;
        private Camera dynamicCamera;
        private Camera followingCamera;

        public Form1()
        {
            InitializeComponent();
            bitmap = new DirectBitmap(pictureBox1.Width, pictureBox1.Height);
            device = new Device(bitmap);
            InitializeShapes();
            InitializeLigths();
            InitializeCameras();
        }

        private void InitializeShapes()
        {
            lighter = ShapesGenerator.CreateCube();

            sphere = ShapesGenerator.CreateSphere(2, Color.Blue);
            sphere.Translate(-3, 0.2f, 0);

            cube = ShapesGenerator.CreateCylinder(15, 1.5, 0.2, Color.Purple);

            floor = ShapesGenerator.CreateFloor(Color.FromArgb(86, 125, 70));
            floor.Scale(3.5f, 1f, 3);
            floor.Translate(0, -0.5f, 0);

            cone = ShapesGenerator.CreateCone(10, 1, 2);
            cone.Translate(3, -0.5f, -2);

            device.Meshes.Add(cone);
            device.Meshes.Add(lighter);
            device.Meshes.Add(cube);
            device.Meshes.Add(sphere);
            device.Meshes.Add(floor);
        }

        private void InitializeLigths()
        {
            globalLight = new Light
            {
                IsSpotLight = false,
                Position = new Vector3(5, 5, 0),
                IsTurnedOn = true
            };

            spotLight = new Light
            {
                IsSpotLight = true,
                Position = new Vector3(0, 0, 0),
                IsTurnedOn = false,
                Direction = new Vector3(0, 0, 1),
                P = 16
            };

            device.Lights.Add(globalLight);
            device.Lights.Add(spotLight);
        }

        private void InitializeCameras()
        {
            staticCamera = new Camera(new Vector3(0f, 20f, -10f), new Vector3(0, 0, 0), new Vector3(0, 0, -1), 50);
            dynamicCamera = new Camera(new Vector3(0, BehindHeight, -BehindDistance), new Vector3(0, 0, TargetDistance), new Vector3(0, 0, -1), 65);
            followingCamera = new Camera(new Vector3(0f, 10f, -10f), new Vector3(0, 0, 0), new Vector3(0, -1, 0), 65);

            device.SelectedCamera = staticCamera;
        }

        private void MoveCube()
        {
            cube.ResetModelMatrix();
            cube.Rotate(Axis.Y, cubeAngle);
            cubeAngle += Math.PI / 60;
            cubeAngle %= 2 * Math.PI;
            cube.Translate(cubeShift, 0, CubeZDist);

            cubeShift += cubeChange;

            if (Math.Abs(cubeShift) >= CubeMovementRange)
                cubeChange *= -1;
        }

        private void SetDynamicCamera()
        {
            var xV = Math.Sin(cylinderAngle * Math.PI);
            var zV = Math.Cos(cylinderAngle * Math.PI);

            dynamicCamera.Position = new Vector3((float)(-BehindDistance * xV), BehindHeight, (float)(-BehindDistance * zV));
            dynamicCamera.Target = new Vector3((float)(TargetDistance * xV), 0, (float)(TargetDistance * zV));
            dynamicCamera.UpVector = Vector3.Normalize(new Vector3((float)-xV, 0, (float)-zV));
        }

        private void RotateSpotLight()
        {
            var xV = Math.Sin(cylinderAngle * Math.PI);
            var zV = Math.Cos(cylinderAngle * Math.PI);

            lighter.ResetModelMatrix();
            lighter.Rotate(Axis.Y, Math.PI * cylinderAngle);
            spotLight.Direction = new Vector3((float)xV, 0, (float)zV);
        }

        private void SetFollowingCamera()
        {
            followingCamera.Target = new Vector3(cubeShift, 0, CubeZDist);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MoveCube();
            SetDynamicCamera();
            SetFollowingCamera();
            RotateSpotLight();
            bitmap = new DirectBitmap(pictureBox1.Width, pictureBox1.Height);
            device.Bitmap = bitmap;
            device.Render();
            pictureBox1.Image = device.Bitmap.Bitmap;
        }
    }
}