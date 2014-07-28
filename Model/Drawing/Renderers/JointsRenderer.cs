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
    /// Draw 2D shapes representing human joints into a drawing context
    /// </summary>
    public class JointsRenderer : Renderer
    {
        private const double JointThickness = 6;
        private readonly Brush trackedJointBrush = Brushes.Green;
        private readonly Brush inferredJointBrush = Brushes.LightGreen;

        public JointsRenderer(Size displaySize) : base(displaySize) { }

        public override void Draw()
        {
            foreach (JointType jointType in joints.Keys)
            {
                var brush = this.ChooseBrush(jointType);

                if (brush != null)
                {
                    drawingContext.DrawEllipse(brush, null, drawingPoints[jointType], JointThickness, JointThickness);
                }
            }
        }

        private Brush ChooseBrush(JointType jointType)
        {
            Brush brush = null;
            var trackingState = joints[jointType].TrackingState;

            if (trackingState == TrackingState.Tracked)
            {
                brush = this.trackedJointBrush;
            }
            else if (trackingState == TrackingState.Inferred)
            {
                brush = this.inferredJointBrush;
            }

            return brush;
        }
    }
}
