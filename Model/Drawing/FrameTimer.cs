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
    public class FrameTimer
    {
        private BodyFrameReference currentFrame;
        private uint framesSinceUpdate = 0;
        private double framesPerSecond;
        private bool isFirstFrame;

        private Stopwatch stopwatch;
        private TimeSpan startTime;
        private DateTime nextUpdateTime = DateTime.MinValue;

        public FrameTimer()
        {
            this.stopwatch = new Stopwatch();
            this.isFirstFrame = true;
        }

        public void AddFrame(BodyFrameReference frame)
        {
            this.currentFrame = frame;
            this.framesSinceUpdate++;
            this.InitStartTimeOnFirstFrame();

            if (this.IsReadyForUpdate())
            {
                this.PerformUpdate();
                this.SetNextUpdateTime();
            }

            this.InitStopwatchOnFirstFrame();
            this.SetFirstFramePassed();
        }

        public double GetFramesPerSecond()
        {
            return this.framesPerSecond;
        }

        public TimeSpan GetRunningTime()
        {
            return this.currentFrame.RelativeTime - this.startTime;
        }

        private void InitStartTimeOnFirstFrame()
        {
            if (this.isFirstFrame)
            {
                this.startTime = this.currentFrame.RelativeTime;
            }
        }

        private void InitStopwatchOnFirstFrame()
        {
            if(this.isFirstFrame)
            {
                this.stopwatch.Start();
            }
        }

        private void SetFirstFramePassed()
        {
            this.isFirstFrame = false;
        }

        private bool IsReadyForUpdate()
        {
            return DateTime.Now >= this.nextUpdateTime;
        }

        private void PerformUpdate()
        {
            framesPerSecond = 0.0;

            if (this.stopwatch.IsRunning)
            {
                this.stopwatch.Stop();
                this.LogFramesPerSecond();
                this.stopwatch.Reset();
                this.stopwatch.Start();
            }
        }

        private void SetNextUpdateTime()
        {
            this.nextUpdateTime = DateTime.Now + TimeSpan.FromSeconds(1);
        }

        private void LogFramesPerSecond()
        {
            this.framesPerSecond = this.framesSinceUpdate / this.stopwatch.Elapsed.TotalSeconds;
            this.framesSinceUpdate = 0;
        }
    }
}
