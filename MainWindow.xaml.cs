namespace KinectV2OSC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Kinect;
    using Model.Drawing;
    using Model.Network;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private DrawingImage imageSource;
        private KinectSensor kinectSensor;
        private BodyFrameReader bodyFrameReader;
        private Body[] bodies;
        private FrameTimer timer;
        private KinectCanvas kinectCanvas;
        private BodySender bodySender;

        public event PropertyChangedEventHandler PropertyChanged;

        public ImageSource ImageSource
        {
            get { return this.imageSource; }
        }

        private string framesText;
        public string FramesText
        {
            get { return this.framesText; }
            set
            {
                this.framesText = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("FramesText"));
                }
            }
        }

        private string uptimeText;
        public string UptimeText
        {
            get { return this.uptimeText; }
            set
            {
                this.uptimeText = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("UptimeText"));
                }
            }
        }

        private string oscText;
        public string OscText
        {
            get { return this.oscText; }
            set
            {
                this.oscText = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("OscText"));
                }
            }
        }

        public MainWindow()
        {
            this.timer = new FrameTimer();
            this.InitKinect();
            this.InitNetwork();
            this.InitWindowObjectAsViewModel();
        }

        private void InitKinect()
        {
            Size displaySize = new Size(0, 0);
            this.kinectSensor = KinectSensor.GetDefault();

            if (this.kinectSensor != null)
            {
                this.kinectSensor.Open();

                var frameDescription = this.kinectSensor.DepthFrameSource.FrameDescription;
                displaySize.Width= frameDescription.Width;
                displaySize.Height = frameDescription.Height;

                this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();

                this.UptimeText = Properties.Resources.InitializingStatusTextFormat;
            }
            else
            {
                this.UptimeText = Properties.Resources.NoSensorFoundText;
            }

            this.kinectCanvas = new KinectCanvas(this.kinectSensor, displaySize);
        }

        private void InitNetwork()
        {
            var ipAddress = ReadIpAddressCsv();
            var port = Properties.Resources.PortNumber;
            this.bodySender = new BodySender(ipAddress, port);
        }

        private void InitWindowObjectAsViewModel()
        {
            this.imageSource = this.kinectCanvas.GetDrawingImage();
            this.DataContext = this;
            this.InitializeComponent();
        }

        private string ReadIpAddressCsv()
        {
            string ipAddressCsv;
            try
            {
                System.IO.TextReader file = new StreamReader(Properties.Resources.IpAddressFileName);
                ipAddressCsv = file.ReadLine();
                file.Close();
                file = null;
            }
            catch(Exception)
            {
                ipAddressCsv = Properties.Resources.DefaultIpAddressCsv;
            }
            return ipAddressCsv;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.bodyFrameReader != null)
            {
                this.bodyFrameReader.FrameArrived += this.Reader_FrameArrived;
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (this.bodyFrameReader != null)
            {
                this.bodyFrameReader.Dispose();
                this.bodyFrameReader = null;
            }

            if (this.kinectSensor != null)
            {
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }
        }

        private void Reader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            var frameReference = e.FrameReference;

            try
            {
                var frame = frameReference.AcquireFrame();

                if (frame != null)
                {
                    using (frame)
                    {
                        this.timer.AddFrame(frameReference);
                        this.setStatusText();
                        this.updateBodies(frame);
                        this.kinectCanvas.Draw(this.bodies);
                        this.bodySender.Send(this.bodies);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Frame exception encountered...");
            }
        }

        private void setStatusText()
        {
            var framesPerSecond = timer.GetFramesPerSecond();
            var runningTime = timer.GetRunningTime();
            this.FramesText = string.Format(Properties.Resources.StandardFramesTextFormat, framesPerSecond);
            this.UptimeText = string.Format(Properties.Resources.StandardUptimeTextFormat, runningTime);
            this.OscText = bodySender.GetStatusText();
        }

        private void updateBodies(BodyFrame frame)
        {
            if (this.bodies == null)
            {
                this.bodies = new Body[frame.BodyCount];
            }

            // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
            // As long as those body objects are not disposed and not set to null in the array,
            // those body objects will be re-used.
            frame.GetAndRefreshBodyData(this.bodies);
        }
    }
}
