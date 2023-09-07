using Graphics4Proj.Devices;
using Graphics4Proj.Geometry;
using Graphics4Proj.Lights;
using Graphics4Proj.Shapes;
using System.Numerics;
using Timer = System.Windows.Forms.Timer;

namespace Graphics4Proj
{
    public partial class Form1 : Form
    {
        //Constants
        private const float CubeZDist = 5;
        private const float CubeMovementRange = 3;
        

        private double cubeAngle;
        private float cubeChange = 0.05f;
        private float cubeShift;
        private double cylinderAngle;

        //Shapes
        private Mesh cube;
        private Mesh lighter;
        private Mesh lighter2;
        private Mesh sphere;
        private Floor floor;
        private Mesh cone;

        private Device device;
        private DirectBitmap bitmap;

        private Light globalLight;
        private Light spotLight;
        private Light dynamicLight;

        private Camera staticCamera;
        private Camera thirdPersonCamera;
        private Camera followingCamera;

        private Fog fog;

        private Timer timer;

        public Form1()
        {
            InitializeComponent();
            bitmap = new DirectBitmap(pictureBox1.Width, pictureBox1.Height);
            device = new Device(bitmap);
            InitializeShapes();
            InitializeLigths();
            InitializeCameras();

            timer = new Timer();
            timer.Enabled = true;
            timer.Interval = 10;
            timer.Tick += new EventHandler(this.timer1_Tick);
        }

        private void InitializeShapes()
        {
            cube = ShapesGenerator.CreateCube();

            sphere = ShapesGenerator.CreateSphere(2, Color.Blue);
            //sphere.Translate(-2, 0.2f, 0);

            lighter = ShapesGenerator.CreateCylinder(15, 1.5, 0.2, Color.Purple);
            lighter.Translate(-5, -0.5f, 0);

            lighter2 = ShapesGenerator.CreateCylinder(15, 1.5, 0.2, Color.Brown);
            lighter2.Translate(0, -0.5f, -4);

            floor = new Floor(Color.FromArgb(86, 125, 70));
            floor.Scale(3.5f, 1f, 3);
            floor.Translate(0, -0.5f, 0);

            cone = ShapesGenerator.CreateCone(10, 1, 2);
            cone.Translate(2, -0.5f, 2);

            device.Meshes.Add(cone);
            device.Meshes.Add(lighter);
            device.Meshes.Add(cube);
            device.Meshes.Add(sphere);
            device.Meshes.Add(floor);
            device.Meshes.Add(lighter2);
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

            dynamicLight = new Light
            {
                IsSpotLight = true,
                Position = new Vector3(0, 0, -3),
                IsTurnedOn = false,
                Direction = new Vector3(0, 0, 1),
                P = 16
            };

            device.Lights.Add(globalLight);
            device.Lights.Add(spotLight);
            device.Lights.Add(dynamicLight);

            fog = new Fog(Color.LightGray, 40);
        }

        private void InitializeCameras()
        {
            staticCamera = new Camera(new Vector3(0f, 20f, -10f), new Vector3(0, 0, 0), new Vector3(0, 0, -1), 50);
            thirdPersonCamera = new Camera(new Vector3(0f, 10f, -10f), new Vector3(0, 0, 0), new Vector3(0, -1, 0), 65);
            followingCamera = new Camera(new Vector3(0f, 10f, -10f), new Vector3(0, 0, 0), new Vector3(0, -1, 0), 65);

            device.SelectedCamera = staticCamera;
        }

        private void MoveCube()
        {
            cube.ResetModelMatrix();
            lighter2.ResetModelMatrix();
            cube.Rotate(Axis.Z, cubeAngle);
            //cone.Rotate(Axis.Z, cubeAngle);
            cubeAngle += Math.PI / 60;
            cubeAngle %= 2 * Math.PI;
            cube.Translate(cubeShift, 0, CubeZDist);
            lighter2.Translate(0, 0, -3f + cubeShift / 4);

            cubeShift += cubeChange;

            if (Math.Abs(cubeShift) >= CubeMovementRange)
                cubeChange *= -1;
        }

        private void MoveLighter()
        {
            sphere.ResetModelMatrix();
            sphere.Rotate(Axis.Z, cubeAngle);

            //lighter2.Translate(0, 0, cubeShift);
            //cubeAngle += Math.PI / 60;
            //cubeAngle %= 2 * Math.PI;
            //sphere.Translate(0, cubeShift, CubeZDist);

            
        }

        private void MoveCone()
        {
            cone.ResetModelMatrix();
            cone.Rotate(Axis.Y, cubeAngle);
            cubeAngle += Math.PI / 60;
            cubeAngle %= 2 * Math.PI;
            cone.Translate(cubeShift, 0, CubeZDist);

            cubeShift += cubeChange;

            if (Math.Abs(cubeShift) >= CubeMovementRange)
                cubeChange *= -1;
        }

        private void SetThirdPersonCamera()
        {
            thirdPersonCamera.Position = new Vector3(0, 0, -3 + cubeShift / 4);
            dynamicLight.Position =  new Vector3(0, 0, -3 + cubeShift / 4);
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
            MoveLighter();
            SetThirdPersonCamera();
            SetFollowingCamera();
            RotateSpotLight();

            bitmap = new DirectBitmap(pictureBox1.Width, pictureBox1.Height);
            device.Bitmap = bitmap;
            device.Render();
            pictureBox1.Image = device.Bitmap.Bitmap;
        }

        #region Events
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                device.SelectedCamera = staticCamera;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                device.SelectedCamera = followingCamera;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                device.SelectedCamera = thirdPersonCamera;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                device.ShadingType = ShadingType.Flat;
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
            {
                device.ShadingType = ShadingType.Phong;
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
            {
                device.ShadingType = ShadingType.Gouraud;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                device.Fog = fog;
            }
            else
            {
                device.Fog = null;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                timer.Stop();
            }
            else
            {
                timer.Start();
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                globalLight.IsTurnedOn = true;
            }
            else
            {
                globalLight.IsTurnedOn = false;
            }
        }
        #endregion Events

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                spotLight.IsTurnedOn = true;
            }
            else
            {
                spotLight.IsTurnedOn = false;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                dynamicLight.IsTurnedOn = true;
            }
            else
            {
                dynamicLight.IsTurnedOn = false;
            }
        }
    }
}