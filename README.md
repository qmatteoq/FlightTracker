# FlightTracker
A sample app that demonstrates all the steps of the Desktop Bridge to move from a traditional Windows Forms app to a Universal Windows Platform app.
The application simulates a simple flight tracking tool, by allowing to:

- Add the data about a flight you want to track
- Save the data so that it's persisted across usages
- Export a boarding pass on the user's desktop

1. <b>Convert</b>: The Windows Forms app is converted, as it is, to leverage the Universal Windows Platform container and the new deployment model based on AppX
2. <b>Enhance</b>: The Windows Forms app uses some APIs of the Universal Windows Platform to integrate better with Windows 10. In this specific case, the app will show a toast notification when the operation to export a boarding pass is completed.
3. <b>Extend</b>: The Windows Forms app uses some components of the Universal Windows Platform to offer feature that wouldn't be as easy to implement in the .NET Framework. In this specific case, the app will show a toast notification every time the time zone of the computer will change, thanks to a background task.
4. <b>Migrate</b>: The Windows Forms app has now been migrated to a real Universal Windows Platform app, that can run on multiple devices (phone, Xbox, HoloLens, etc.) and not only on a traditional computer. However, since the feature to export a boarding pass uses some features which aren't available in the Universal Windows Platforms, it will be available only when the app is running on the desktop: in this case, the UWP app is able to launch the Win32 process that takes care of exporting the pass.
5. <b>Reach all</b>: All the features of the app have been migrated to use Universal Windows Platfomr APIs: now the boarding pass export feature is implemented using the FileSavePicker API, which allows the user to choose where he wants to save the file. Now the app can run, without any limitation, on any device and it doesn't require any special capability or approval to be published on the Store.

Some resources to know more about the Desktop Bridge:
- The official website: http://aka.ms/desktopbridge
- The official documentation: https://msdn.microsoft.com/windows/uwp/porting/desktop-to-uwp-root
- The form where to candidate your converted desktop app to publish it on the store: https://developer.microsoft.com/en-us/windows/projects/campaigns/desktop-bridge
- The official AppConsult blog: http://blogs.msdn.microsoft.com/appconsult
