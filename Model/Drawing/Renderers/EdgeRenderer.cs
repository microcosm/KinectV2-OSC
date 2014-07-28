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
    /// Draw 2D shapes representing edge boundaries into a drawing context
    /// </summary>
    public class EdgeRenderer : Renderer
    {
        private const double HandSize = 30;
        private const double JointThickness = 3;
        private const double ClipBoundsThickness = 10;

        public EdgeRenderer(Size displaySize) : base(displaySize) { }

        public override void Draw()
        {
            this.DrawTop(body.ClippedEdges);
            this.DrawRight(body.ClippedEdges);
            this.DrawBottom(body.ClippedEdges);
            this.DrawLeft(body.ClippedEdges);
        }

        private void DrawTop(FrameEdges clippedEdges)
        {
            if (clippedEdges.HasFlag(FrameEdges.Top))
            {
                var rect = new Rect(0, 0, this.displaySize.Width, ClipBoundsThickness);
                drawingContext.DrawRectangle(Brushes.Red, null, rect);
            }
        }

        private void DrawRight(FrameEdges clippedEdges)
        {
            if (clippedEdges.HasFlag(FrameEdges.Right))
            {
                var rect = new Rect(this.displaySize.Width - ClipBoundsThickness, 0, ClipBoundsThickness, this.displaySize.Height);
                drawingContext.DrawRectangle(Brushes.Red, null, rect);
            }
        }

        private void DrawBottom(FrameEdges clippedEdges)
        {
            if (clippedEdges.HasFlag(FrameEdges.Bottom))
            {
                var rect = new Rect(0, this.displaySize.Height - ClipBoundsThickness, this.displaySize.Width, ClipBoundsThickness);
                drawingContext.DrawRectangle(Brushes.Red, null, rect);
            }
        }

        private void DrawLeft(FrameEdges clippedEdges)
        {
            if (clippedEdges.HasFlag(FrameEdges.Left))
            {
                var rect = new Rect(0, 0, ClipBoundsThickness, this.displaySize.Height);
                drawingContext.DrawRectangle(Brushes.Red, null, rect);
            }
        }
    }
}
