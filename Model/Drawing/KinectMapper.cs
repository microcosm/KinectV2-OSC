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

namespace KinectV2OSC.Model.Drawing
{
    public class KinectMapper
    {
        private KinectSensor kinectSensor;

        public KinectMapper(KinectSensor kinectSensor)
        {
            this.kinectSensor = kinectSensor;
        }

        public IReadOnlyDictionary<JointType, Point> JointsTo2DPoints(Body body)
        {
            var joints = body.Joints;
            var points = new Dictionary<JointType, Point>();

            foreach (JointType jointType in joints.Keys)
            {
                var depthSpacePoint = this.kinectSensor.CoordinateMapper.MapCameraPointToDepthSpace(joints[jointType].Position);
                points[jointType] = new Point(depthSpacePoint.X, depthSpacePoint.Y);
            }

            return points;
        }
    }
}
