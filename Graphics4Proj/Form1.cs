using Graphics4Proj.Devices;

namespace Graphics4Proj
{
    public partial class Form1 : Form
    {
        //Options chosen
        private bool fogOn;
        private bool stopAnimation;
        private bool changedLight;

        private readonly Device device;
        private DirectBitmap bitmap;

        public Form1()
        {
            InitializeComponent();
        }
    }
}