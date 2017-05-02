# Bluetooth-Messenger


##    Why would I use this?


If you've ever seen a religious Apple user, you've probably been at least jealous
of their integration. This project brings a degree of the messaging capability
between iphones and osx computers to the pc and droid. By establishing a bluetooth
connection between your phone and your computer, these applications will allow you
to text from your computer. The only time you have to take the phone out of your pocket
is to open the app to connect to your PC. As of right now, the application supports
the following:

* Android 5.0 and Up (Only tested on 5.0 currently)
* Windows Vista and Up (Only tested on Windows 10 currently)
* Contact matching so computer application displays name of person you are messaging
* Previous Messages display so you can see all prior text messaging history on PC side
* Message deletion so upon disconnection, all messages saved on computer delete in order
	to preseve privacy.
	
	
##    Installation and Building

Note, the projects in this solution require bluetooth hardware to work properly. It is not recommended
to try running these within an emulator. Also the Android app currently requires the phone to already be paired
with the PC before running the app.

1. The Android part relies on C# Xamarin. To install this feature go to Add/remove features
for Visual Studio (under the control panel) and click the drop down next to "Cross Platform Mobile Development."
2. Next check the box next to "C#/.NET (Xamarin v4.2.1)" and continue the installation with next.
Once Xamarin is installed on the system load the solution. The NuGet package manager should
restore the libraries that are used (32feet.NET, for bluetooth; Newtonsoft.Json, for serializing to json).
3. Select the AndroidMessenger build option to set Visual Studio to run the AndroidMessenger project.
At this point the play button will show the Android device that you will be deploying to.
4. Select the target Android device from the dropdown in the play button.
5. Right click on the Solution in the Solution Explorer and click on properties.
6. Navigate to "Common Properties/Startup Project" and click on Multiple startup projects.
7. Set the action for "AndroidMessenger" and "WindowsMessenger" to start and click OK.
8. Pressing play (F5) will compile and deploy/run both the "AndroidMessenger" and "WindowsMessenger" projects.
