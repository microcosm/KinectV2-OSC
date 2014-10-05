using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Microsoft.Kinect;
using Rug.Osc;

namespace KinectV2OSC.Model.Network
{
    public class BodySender
    {
        private MessageBuilder messageBuilder;
        private List<OscSender> oscSenders;
        private List<IPAddress> ipAddresses;
        private OscMessage message;
        private string port;
        private string status;

        public BodySender(string delimitedIpAddresses, string port)
        {
            this.status = "";
            this.ipAddresses = this.Parse(delimitedIpAddresses);
            this.oscSenders = new List<OscSender>();
            this.port = port;
            this.messageBuilder = new MessageBuilder();
            this.TryConnect();
        }

        private void TryConnect()
        {
            foreach(var ipAddress in this.ipAddresses)
            {
                try
                {
                    var oscSender = new OscSender(ipAddress, int.Parse(this.port));
                    oscSender.Connect();
                    this.oscSenders.Add(oscSender);
                    this.status += "OSC connection established on\nIP: " + ipAddress + "\nPort: " + port + "\n";
                }
                catch (Exception e)
                {
                    this.status += "Unable to make OSC connection on\nIP: " + ipAddress + "\nPort: " + port + "\n";
                    Console.WriteLine("Exception on OSC connection...");
                    Console.WriteLine(e.StackTrace);
                }
            }

        }

        public void Send(Body[] bodies)
        {
            foreach (Body body in bodies)
            {
                if (body.IsTracked)
                {
                    this.Send(body);
                }
            }
        }

        public string GetStatusText()
        {
            return this.status;
        }

        private void Send(Body body)
        {
            foreach (var joint in body.Joints)
            {
                message = messageBuilder.BuildJointMessage(body, joint);
                this.Broadcast(message);
            }

            message = messageBuilder.BuildHandMessage(body, "Left", body.HandLeftState, body.HandLeftConfidence);
            this.Broadcast(message);

            message = messageBuilder.BuildHandMessage(body, "Right", body.HandRightState, body.HandRightConfidence);
            this.Broadcast(message);
        }

        private void Broadcast(OscMessage message)
        {
            foreach (var oscSender in this.oscSenders)
            {
                oscSender.Send(message);
            }
        }

        private List<IPAddress> Parse(string delimitedIpAddresses)
        {
            try
            {
                var ipAddressStrings = delimitedIpAddresses.Split(',');
                var ipAddresses = new List<IPAddress>();
                foreach (var ipAddressString in ipAddressStrings)
                {
                    ipAddresses.Add(IPAddress.Parse(ipAddressString));
                }
                return ipAddresses;
            }
            catch (Exception e)
            {
                status += "Unable to parse IP address string: '" + delimitedIpAddresses + "'";
                Console.WriteLine("Exception parsing IP address string...");
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }
    }
}
