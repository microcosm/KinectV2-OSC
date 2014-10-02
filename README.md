KinectV2OSC
===========
Broadcasts skeletal data from the KinectV2 sensor over OSC.

Messages are sent in the format:

```sh
Address: /body/{bodyId}/joint/{jointId}
Values: - float: positionX
        - float: positionY
        - float: positionZ
```

Instructions
------------
- Note that you need Windows 8.1, USB3, and a new V2 Kinect sensor
- Download and install the [Kinect for Windows SDK 2.0 Public Preview](http://www.microsoft.com/en-us/download/details.aspx?id=43661)
- Install Visual Studio (I am using [Visual Studio Express 2013 for Windows Desktop](http://www.visualstudio.com/en-us/products/visual-studio-express-vs.aspx) - scroll down to find the download link)

Dependencies
------------
- [Rug OSC](https://www.nuget.org/packages/Rug.Osc/) to format and send messages