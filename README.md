To use, [download the latest Release](https://github.com/metatablecat/DisableBetaApp/raw/master/DisableBetaApp.exe) and run the program. This will install the web protocol directly.
(Don't move the executable after you've done this, you'll break something)

The executable is slightly large due to internal packages (NET 6 Standalone building isn't perfect, but I want anyone to be able to run it)

## If Roblox updates, it undoes the patch because a new installer process gets started that cant be hooked onto, just run the BetaApp Disabler progran again to reapply registry patches

## Technical Explanation

When you start a game on `roblox.com`, your web browser is being silently redirected to `roblox-player://...`, this triggers modern web browsers to start what is known as an `App Protocol`, that then starts the launcher.

For example, if you type `roblox-player://1` into your URL bar, you can start Roblox (although it won't be able to do anything since no data is sent to the launcher.)

This protocol also has some useful data that's important for the launcher to utilise, when you start game, the launcher is given something along the lines of this wall of text
```
roblox-player:1+launchmode:play+gameinfo:0+launchtime:1663127725140+placelauncherurl:https%3A%2F%2Fassetgame.roblox.com%2Fgame%2FPlaceLauncher.ashx%3Frequest%3DRequestGame%26browserTrackerId%3D130033680605%26placeId%3D6708206173%26isPlayTogetherGame%3Dfalse+browsertrackerid:130033680605+robloxLocale:en_us+gameLocale:en_us+channel:+LaunchExp:InApp

gameinfo was shrunk because its an auth ticket lol.
```

Most of this data is important, but the one we're interested in is `LaunchExp` at the end, when this is set to `InApp`, it enables the Beta App. If we set it instead to `InBrowser`, the beta app is disabled.

Now, if you start the same process with this tool, it takes that massive block of text and changes `LaunchExp:InApp` to `LaunchExp:InBrowser`, then starts the launcher with the modified data.
