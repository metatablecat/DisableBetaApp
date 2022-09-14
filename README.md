i hate net 6

**IMPORTANT:** This program requires that NET Core 3.1 is installed.

To use, download the latest Release and override the `roblox-player` protocol in your browser.

*Pull requests are welcome on trying to do this automatically since it gets overriden by the Roblox Launcher*

## Technical Explanation

When you start a game on `roblox.com`, your web browser is being silently redirected to `roblox-player://...`, this triggers modern web browsers to start what is known as an `App Protocol`, that then starts the launcher.

For exmaple, if you type `roblox-player://1` into your URL bar, you can start Roblox (although it wont be able to do anything since no data is sent to the launcher.)

This protocol also has some useful data that's important for the launcher to utilise, when you start game, the protocol is given this wall of text
```
roblox-player:1+launchmode:play+gameinfo:0+launchtime:1663127725140+placelauncherurl:https%3A%2F%2Fassetgame.roblox.com%2Fgame%2FPlaceLauncher.ashx%3Frequest%3DRequestGame%26browserTrackerId%3D130033680605%26placeId%3D6708206173%26isPlayTogetherGame%3Dfalse+browsertrackerid:130033680605+robloxLocale:en_us+gameLocale:en_us+channel:+LaunchExp:InApp

gameinfo was shrunk because its an auth ticket lol.
```

Most of this data is important, but the one we're interested in is `LaunchExp` at the end, when this is set to `InApp`, it enables the Beta App. If we set it instead to `InBrowser`, the beta app disables.

Now, if you start the same process with this tool, it takes that massive block of text and changes `LaunchExp:InApp` to `LaunchExp:InBrowser`, then starts the launcher with the modified data.

Its possible to have the `roblox-player` protocol automatically start this tool without needing it to be set on the browser level, however Roblox changes it back when the client starts, and there's no easy way to detect when that happens (Pull Request Welcome)
