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
    /// Draw 2D shapes representing a skeleton into a drawing context
    /// </summary>
    public class SkeletonRenderer : Renderer
    {
        private readonly Pen trackedBonePen = new Pen(Brushes.Aqua, 8);
        private readonly Pen inferredBonePen = new Pen(Brushes.MediumAquamarine, 2);

        public SkeletonRenderer(Size displaySize) : base(displaySize) { }

        public override void Draw()
        {
            this.DrawTorso();
            this.DrawRightArm();
            this.DrawLeftArm();
            this.DrawRightLeg();
            this.DrawLeftLeg();
        }

        private void DrawTorso()
        {
            this.DrawBone(JointType.Head, JointType.Neck);
            this.DrawBone(JointType.Neck, JointType.SpineShoulder);
            this.DrawBone(JointType.SpineShoulder, JointType.SpineMid);
            this.DrawBone(JointType.SpineMid, JointType.SpineBase);
            this.DrawBone(JointType.SpineShoulder, JointType.ShoulderRight);
            this.DrawBone(JointType.SpineShoulder, JointType.ShoulderLeft);
            this.DrawBone(JointType.SpineBase, JointType.HipRight);
            this.DrawBone(JointType.SpineBase, JointType.HipLeft);
        }

        private void DrawRightArm()
        {
            this.DrawBone(JointType.ShoulderRight, JointType.ElbowRight);
            this.DrawBone(JointType.ElbowRight, JointType.WristRight);
            this.DrawBone(JointType.WristRight, JointType.HandRight);
            this.DrawBone(JointType.HandRight, JointType.HandTipRight);
            this.DrawBone(JointType.WristRight, JointType.ThumbRight);
        }

        private void DrawLeftArm()
        {
            this.DrawBone(JointType.ShoulderLeft, JointType.ElbowLeft);
            this.DrawBone(JointType.ElbowLeft, JointType.WristLeft);
            this.DrawBone(JointType.WristLeft, JointType.HandLeft);
            this.DrawBone(JointType.HandLeft, JointType.HandTipLeft);
            this.DrawBone(JointType.WristLeft, JointType.ThumbLeft);
        }

        private void DrawRightLeg()
        {
            this.DrawBone(JointType.HipRight, JointType.KneeRight);
            this.DrawBone(JointType.KneeRight, JointType.AnkleRight);
            this.DrawBone(JointType.AnkleRight, JointType.FootRight);
        }

        private void DrawLeftLeg()
        {
            this.DrawBone(JointType.HipLeft, JointType.KneeLeft);
            this.DrawBone(JointType.KneeLeft, JointType.AnkleLeft);
            this.DrawBone(JointType.AnkleLeft, JointType.FootLeft);
        }

        private void DrawBone(JointType jointType1, JointType jointType2)
        {
            Joint joint1 = joints[jointType1];
            Joint joint2 = joints[jointType2];

            if (this.inspector.IsLowConfidence(joint1, joint2)) return;
            Pen pen = this.ChoosePen(joint1, joint2);

            drawingContext.DrawLine(pen, drawingPoints[jointType1], drawingPoints[jointType2]);
        }

        private Pen ChoosePen(Joint joint1, Joint joint2)
        {
            Pen pen = this.inferredBonePen;

            if (this.inspector.GetCombinedTrackingState(joint1, joint2) == TrackingState.Tracked)
            {
                pen = this.trackedBonePen;
            }

            return pen;
        }
    }
}
