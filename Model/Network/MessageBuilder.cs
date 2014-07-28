using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Rug.Osc;

namespace KinectV2OSC.Model.Network
{
    public class MessageBuilder
    {
        public OscMessage Build(Body body, KeyValuePair<JointType, Joint> joint)
        {
            var address = String.Format("/body/{0}/joint/{1}", body.TrackingId, joint.Key);
            var position = joint.Value.Position;
            return new OscMessage(address, position.X, position.Y, position.Z);
        }
    }
}
