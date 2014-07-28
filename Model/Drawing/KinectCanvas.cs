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
using KinectV2OSC.Model.Drawing.Renderers;

namespace KinectV2OSC.Model.Drawing
{
    /// <summary>
    /// Draw 2D shapes from Kinect into a drawing group
    /// </summary>
    public class KinectCanvas
    {
        private KinectSensor kinectSensor;
        private KinectMapper mapper;
        private Size displaySize;

        private DrawingGroup drawingGroup;
        private BackgroundRenderer backgroundRenderer;
        private SkeletonRenderer skeletonRenderer;
        private JointsRenderer jointsRenderer;
        private HandsRenderer handsRenderer;
        private EdgeRenderer edgeRenderer;

        public KinectCanvas(KinectSensor kinectSensor, Size displaySize)
        {
            this.kinectSensor = kinectSensor;
            this.mapper = new KinectMapper(kinectSensor);
            this.displaySize = displaySize;

            this.drawingGroup = new DrawingGroup();
            this.backgroundRenderer = new BackgroundRenderer(displaySize);
            this.skeletonRenderer = new SkeletonRenderer(displaySize);
            this.jointsRenderer = new JointsRenderer(displaySize);
            this.handsRenderer = new HandsRenderer(displaySize);
            this.edgeRenderer = new EdgeRenderer(displaySize);
        }

        public void Draw(Body[] bodies)
        {
            using (var drawingContext = this.drawingGroup.Open())
            {
                this.DrawBackground(drawingContext);
                this.DrawAllTracked(bodies, drawingContext);
                this.PreventDrawingOutsideCanvas();
            }
        }

        public DrawingImage GetDrawingImage()
        {
            return new DrawingImage(this.drawingGroup);
        }

        private void DrawBackground(DrawingContext drawingContext)
        {
            this.backgroundRenderer.Update(drawingContext);
            this.backgroundRenderer.Draw();
        }

        private void DrawAllTracked(Body[] bodies, DrawingContext drawingContext)
        {
            foreach (Body body in bodies)
            {
                if (body.IsTracked)
                {
                    var points = mapper.JointsTo2DPoints(body);

                    this.edgeRenderer.Update(drawingContext, body, points);
                    this.skeletonRenderer.Update(drawingContext, body, points);
                    this.jointsRenderer.Update(drawingContext, body, points);
                    this.handsRenderer.Update(drawingContext, body, points);

                    this.edgeRenderer.Draw();
                    this.skeletonRenderer.Draw();
                    this.jointsRenderer.Draw();
                    this.handsRenderer.Draw();
                }
            }
        }

        private void PreventDrawingOutsideCanvas()
        {
            this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, this.displaySize.Width, this.displaySize.Height));
        }
    }
}
