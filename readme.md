## MJIoT C# Devices
This project contains a base library for all C#-based devices. It also contains reference implementation of MJIoT devices:
* Universal console device - makes use of library that makes it easier to read console arguments
* Chatter (message communicator built in WPF with the usage of MVVM pattern)

# Technologies
.NET Framework 4.6.2
.NET Core 2

# Dependencies
Azure IoTHub Client lib
DashArgsReader - simple framework used to read console arguments, developed for the purpose of MJIoT simulators
Windows Presentation Framework
MJMVVM - simple library containing helper classes for implementing Model-View-ViewModel pattern (developed for the purpose of this project)
