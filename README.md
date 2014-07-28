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
Dependencies
------------
- Kinect for Windows June SDK Update (You must be enrolled in the [Developer Preview Program](http://www.microsoft.com/en-us/kinectforwindowsdev/newdevkit.aspx) to download this SDK)
- [Rug OSC](https://www.nuget.org/packages/Rug.Osc/) to format and send messages

Disclaimer
----------
This is preliminary software and/or hardware and APIs are preliminary and subject to change.

(If you want to share code written against the Microsoft SDK you are asked to include this disclaimer along with your code)