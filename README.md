# DOCUMENTATION IS UNDER PROGRESS.
# ChromeDroid-TabMan
A personal project for trying to retrieve all the more than 3000 tabs I have open on my Chrome (Android version). It will probably be useful to others as well, but I'm mostly doing it for myself.


## Build (Release):
```powershell
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=True -p:PublishTrimmed=True -p:TrimMode=CopyUsed -p:PublishReadyToRun=True
```