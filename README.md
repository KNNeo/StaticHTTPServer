# StaticHTTPServer
 A simple HTTP Server for static web hosting.
 
 Built using .NET 8.0, can be built on platforms starting from .NET Standard 2.0 (project config will need to remove Nullable property).
 
 This application can run on console (click on exe after build) or registered as a Windows service:
 ```
 sc create StaticHTTPServer <location of exe>

 sc delete StaticHTTPServer
 ```
 
 Settings XML file (App.config) determines folder to host (FileDirectory), and what localhost-based address to use (ServerAddress).
