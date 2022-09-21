# WPR

WPR is a WP7-8 XNA app runner.

## Features

- Installing WP7-8 **decrypted** XNA XAPs locally on your machine.
- Earning achievements locally for Xbox Live games, with a pop-up appear everytime achievement is unlocked.

## Dependencies

- Beside submodules included in this repostiory, this application depends on these native DLLs (please check the forks of those repository on this same account):
    * FNA3D
    * FAudio
    * libtheorafile
    
### Desktop

- Build these libraries and place them under the same folder as the executable
- Place FFMPEG executable (you can download from their website or make a custom version with WMA->OGG conversion supported)

### Android

- Clone [WPRNativeBuild](https://github.com/8212369/WPRNativeBuild)
- Go to the cloned folder, edit the build.bat **ANDROID_TOOLCHAIN** to point to your Android SDK CMake toolchain file.
- Run the build.bat. The built libraries will be emitted in the Source/lib folder, categorized by architectures
- Copy all the architecture folders to WPR root/UI/WPR.UI.Android/Libraries.
- Download ffmpeg-kit JARs **(use nightly)**, extract the JARs and copy native .so to *Libraries* folder of WPR.UI.Android
- Optional: you can also add Vulkan validation to the Libraries folder to enable validation layers.

## Xamarin build requirements

- Please note that these must not be enabled for Xamarin build
    * Trimming: It's unpredictable what assemblies the game will use. Trimming will make it so that only the DLLs needed by the runner (WPR.UI.Android) are included with the install package, which is not desired.
    * AOT: Some apps and games are obsfucated will not work well with AOT (they may throw InvalidProgramException).

- One of the reason that iOS build won't work can be these requirements. This is all required by iOS when building with Xamarin.

## Support for Sliverlight and Native applications

- Sliverlight is very massive, there needs to be a huge funding work. The core of this runner is dependent on FNA, which is already a very large codebase developed through out years.
- Morden native applications:
    * If we are talking about W10M and Universal applications, you can just play it on your computer, buy a game through Microsoft Store
    * But to talk specifically about native WP8 apps and games, a large number of them shipped with custom APIs provided by partners (Microsoft, Nokia, etc..) and is built to have binary code running on ARM processors. At that point, you should just make a emulator instead (I think spending effort for this is absurb in 2022, no one will use it)
    
    => This repository with the original author will not go over that great length.
    
## This runner existence

- It's for fun. If you are nostaglia mostly about achievements earning like me, you can try it out. There are some old games that is not released on Android or iOS, or some games that seems superior than Android or iOS version (I prefer Skulls of the Shogun on WP actually).
- However, resolution scaling is not yet implemented (game renders either in 480x800 or so...), but it's fun!

