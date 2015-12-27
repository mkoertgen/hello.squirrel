# hello.squirrel

[Squirrel.Windows](https://github.com/Squirrel/Squirrel.Windows) starter sample. It's a minimal WPF application showing how to update an existing application.

![](HelloSquirrel.png)

As extra candy i automated "releasifying" updates with msbuild, i.e.

    build /t:Deploy /v:m

does build & package the application for Squirrel.

## Versioning & Updates

You can build updates using either [GitVersion](https://github.com/GitTools/GitVersion) workflows] or manually adjusting the version number

    build /t:Deploy /v:m /p:PackageVersion=[YourSemVer]

## Adjusting drop location

After building the `Deploy` target you will find the Squirrel release files in `.\HelloSquirrel\Releases`:

    HelloSquirrel-0.1.7.0-full.nupkg
    HelloSquirrel-0.1.8.0-delta.nupkg
    HelloSquirrel-0.1.8.0-full.nupkg
    RELEASES
    Setup.exe

Note that `Setup.exe` always uses the latest full version. 	

The `Deploy` target includes an optional step to copy the final release files to a custom drop location (e.g. folder).
If not set, you will see a warning 

    warning : Property "DropLocation" not set. Skipping
	
Set the drop location like

    build /t:Deploy /v:m /p:DropLocation=c:\temp\Releases

Note that the sample app looks for updates at `http://localhost:8080/Releases`. 
For testing purposes i just started [node http-server](https://www.npmjs.com/package/http-server) from `c:\temp`.