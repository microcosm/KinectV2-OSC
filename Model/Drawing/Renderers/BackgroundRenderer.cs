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
    /// Draw 2D shapes representing a background into a drawing context
    /// </summary>
    public class BackgroundRenderer : Renderer
    {
        public BackgroundRenderer(Size displaySize) : base(displaySize) { }

        public void Update(DrawingContext drawingContext)
        {
            this.drawingContext = drawingContext;
        }

        public override void Draw()
        {
            this.drawingContext.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, this.displaySize.Width, this.displaySize.Height));
        }
    }
}
