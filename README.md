KinectV2OSC
===========
Broadcasts [KinectV2](http://www.microsoft.com/en-us/kinectforwindows/purchase/) skeletal data over OSC. That's it.

![A Kinect V2 sensor](kinect.jpg)

Handy if you want to quickly get skeleton data off of Windows and onto Mac, or into some other Windows app.

Instructions
------------
- Note that you need Windows 8.1, USB3, and a new V2 Kinect sensor
- Download and install the [Kinect for Windows SDK 2.0 Public Preview](http://www.microsoft.com/en-us/download/details.aspx?id=43661)
- Install Visual Studio (I am using [Visual Studio Express 2013 for Windows Desktop](http://www.visualstudio.com/en-us/products/visual-studio-express-vs.aspx) - scroll down to find the download link)
- Clone this repo, and open KinectV2OSC.sln in Visual Studio. Hit the green 'Start' button. You should see a screen like this:

![Screenshot of KinectV2OSC in action](screenshot.png)

- To send to another destination, change the IP and port number here, and then re-launch:

![How to configure IP and port number](config.png)

You can enter a single IP Address or multiple, separated by commas.

OSC
---
OSC messages are sent every frame. For each detected body, you will get a set of joints:

```sh
Address: /bodies/{bodyId}/joints/{jointId}
Values: - float:  positionX
        - float:  positionY
        - float:  positionZ
        - string: trackingState (Tracked, NotTracked or Inferred)
```

...and a pair of hands:

```sh
Address: /bodies/{bodyId}/hands/{handId} (Left or Right)
Values: - string: handState (Open, Closed, NotTracked, Unknown)
        - string: handConfidence (High, Low)
```

Project dependencies
--------------------
- [Rug OSC](https://www.nuget.org/packages/Rug.Osc/) to format and send messages