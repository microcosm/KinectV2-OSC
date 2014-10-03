using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace KinectV2OSC.Model
{
    public class Inspector
    {
        public bool IsLowConfidence(Joint joint1, Joint joint2)
        {
            if (joint1.TrackingState == TrackingState.NotTracked || joint2.TrackingState == TrackingState.NotTracked)
            {
                return true;
            }

            if (joint1.TrackingState == TrackingState.Inferred && joint2.TrackingState == TrackingState.Inferred)
            {
                return true;
            }

            return false;
        }

        public TrackingState GetCombinedTrackingState(Joint joint1, Joint joint2)
        {
            if (joint1.TrackingState == TrackingState.Tracked && joint2.TrackingState == TrackingState.Tracked)
            {
                return TrackingState.Tracked;
            }

            if (joint1.TrackingState == TrackingState.Inferred && joint2.TrackingState == TrackingState.Tracked)
            {
                return TrackingState.Inferred;
            }

            if (joint1.TrackingState == TrackingState.Tracked && joint2.TrackingState == TrackingState.Inferred)
            {
                return TrackingState.Inferred;
            }

            return TrackingState.NotTracked;
        }
    }
}
