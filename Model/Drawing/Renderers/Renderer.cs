using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;

namespace KinectV2OSC.Model.Drawing.Renderers
{
    public abstract class Renderer
    {
        protected DrawingContext drawingContext;

        protected Body body;
        protected Inspector inspector;
        protected IReadOnlyDictionary<JointType, Joint> joints;
        protected IReadOnlyDictionary<JointType, Point> drawingPoints;

        protected Size displaySize;

        public Renderer(Size displaySize)
        {
            this.inspector = new Inspector();
            this.displaySize = displaySize;
        }

        public void Update(DrawingContext drawingContext, Body body, IReadOnlyDictionary<JointType, Point> drawingPoints)
        {
            this.drawingContext = drawingContext;
            this.body = body;
            this.joints = body.Joints;
            this.drawingPoints = drawingPoints;
        }

        public abstract void Draw();
    }
}
