# WifiSwitchService
Windows service to switch to a better wifi at home

Development steps:
1. dotnet new worker --name HWork.WifiSwitchService
2. note, the project uses TargetFramework = net8.0-windows10 in order to use Windows.Devices.WiFi namespace

Usage:
```
HWork.WifiSwitchService.exe allowedwifi=ssid_name1;ssid_name2;ssid_name3
```

or create windows service: 
```
sc.exe create "Better WIFI" binpath="c:\fullpath\HWork.WifiSwitchService.exe allowedwifi=ssid_name1;ssid_name2;ssid_name3"
```


delete service: 
```
sc.exe delete "Better WIFI"
```
