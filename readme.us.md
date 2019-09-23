![Logo Navegar](http://www.kopigi.fr/navegar/navegar.png)

Navegar help you to navigate inside your's Windows Presentation Foundation and Universal Windows Platform applications with use of ViewModel First approach.

Navegar help you too, on same technique, use this navigation (inspire by WinRT platform) on WPF's and UWP's applications

Binaries class are available on  [Nuget](https://www.nuget.org/packages/Navegar/) and source code here.

##Installation
For install Navegar inside your application :

    PM> Install-Package Navegar 

##Changelog :

V5.0 :

- Suppress support for Xamarin.Forms, due to issue on MvvmLight on Android Support
- Support align on .Net Full  Framework v4.7.2 and UWP minimal version 1809 (build 17763)

V4.5.9 :

- UAP/UWP/Xamarin.Forms platforms : add PreNavigationArgs parmaeter on PreviewNavigate event for changed load function on Navigate function.

V4.5.8.8 :

- Bug fix on ShowVirtualBackButton function

V4.5.8.6 :

- Bug fix on GetViewModelCurrent function

V4.0.3 Release Candidate :

- Windows 10 version : Add implementation for back button virtuel in title bar of UWP app desktop. ShowVirtualBackButton function (showing button) and BackVirtualButtonPressed event (to customize back in your application)
- Windows 10 version : add HasVirtualBackButtonShow property indicating that current page has a back button virtual visible

V4.0.2 Release Candidate :

- Windows 10 version : add HasBackButton property indicating that device has a back button

V4 Release Candidate :

- Add support for Universal Window Platform on Windows 10
- Check back button on device with new capabilities for Windows 10 SDK

<code class="language-csharp">
Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons")
</code>

- Visual Stdio 2015 RC Solution with Navegar's Class and sample application for Windows 10 (VS 2013 solution is always available for Windows 8.1 support)
