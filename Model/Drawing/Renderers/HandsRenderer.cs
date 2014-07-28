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
    /// <summary>
    /// Draw 2D shapes representing human hands into a drawing context
    /// </summary>
    public class HandsRenderer : Renderer
    {
        private const double HandSize = 20;

        private readonly Brush handClosedBrush = new SolidColorBrush(Color.FromArgb(128, 255, 255, 0));
        private readonly Brush handOpenBrush = new SolidColorBrush(Color.FromArgb(128, 0, 255, 255));
        private readonly Brush handLassoBrush = new SolidColorBrush(Color.FromArgb(128, 255, 0, 255));

        public HandsRenderer(Size displaySize) : base(displaySize) { }

        public override void Draw()
        {
            this.DrawHand(body.HandLeftState, drawingPoints[JointType.HandLeft]);
            this.DrawHand(body.HandRightState, drawingPoints[JointType.HandRight]);
        }

        private void DrawHand(HandState handState, Point handPosition)
        {
            switch (handState)
            {
                case HandState.Closed:
                    drawingContext.DrawEllipse(this.handClosedBrush, null, handPosition, HandSize, HandSize);
                    break;

                case HandState.Open:
                    drawingContext.DrawEllipse(this.handOpenBrush, null, handPosition, HandSize, HandSize);
                    break;

                case HandState.Lasso:
                    drawingContext.DrawEllipse(this.handLassoBrush, null, handPosition, HandSize, HandSize);
                    break;
            }
        }
    }
}
