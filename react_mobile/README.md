
### Creating a React Native app executable (.APK) for Android OS in VS Code under Win10 using only Android Command Line Tools (no installation of Android Studio required)

### 1. Downloads and environment setup

Create folder for Android SDK files:<br>
`YourPath\android_sdk\`

Download command line tools:<br>
https://developer.android.com/studio#command-tools

Extract to:<br>
`YourPath\android_sdk\`

In the unzipped cmdline-tools directory, create a sub-directory called `latest`.

Move the original `cmdline-tools` directory contents, including the `lib` directory, `bin` directory, `NOTICE.txt` file, and `source.properties` file, into `latest`.

Add command tools location to environmental variable `Path`:<br/>
`YourPath\android_sdk\cmdline-tools`

Add new environmental variable:<br/>
name = `ANDROID_HOME`<br/>
value = `YourPath\android_sdk`<br/>

Download openJDK v17.0.2 to run sdkmanager from command tools:<br/>
https://jdk.java.net/archive/

Extract to:<br>
`YourPath\android_sdk\`

Add Java location to environmental variable Path:<br>
`YourPath\android_sdk\jdk-17.0.2\bin`

Accept licenses, run:<br>
>YourPath\android_sdk\cmdline-tools\latest\bin\sdkmanager --licenses

(this should create folder `YourPath\android_sdk\licenses`)

### 2. Creating an app

Create a folder for react native projects, e.g., `reactNative` and open it in VS Code

Run:<br/>
>npx react-native init MyFirstProject

Run:<br/>
>cd .\MyFirstProject\

Create `assets` folder in `reactNative/MyFirstProject/android/app/src/main/`

Run:<br/>
>npx react-native bundle --platform android --dev false --entry-file index.js --bundle-output android/app/src/main/assets/index.android.bundle --assets-dest android/app/src/main/res/

In folder `reactNative/MyFirstProject/android/app/src/main/res/`<br/>
rename `drawable-mdpi` to `_drawable-mdpi` to avoid resource duplication conflict

Run:<br/>
>cd ./android

Run:<br/>
>./gradlew.bat assembleRelease

Android executable file (.apk) will be located in<br>
`reactNative/MyFirstProject\android\app\build\outputs\apk\release`

This file can be uploaded online, opened in a browser on an android mobile, downloaded and installed.

### 3. Setting up emulator

To see the list of available system images, run:<br/>
>YourPath\android_sdk\cmdline-tools\latest\bin\sdkmanager.bat --list | findstr "system-images"

Choose an image, for example:<br/>
`system-images;android-30;google_apis;x86_64`

Download the chosen image (installs into `YourPath\android_sdk\system-images`), run:<br/>
>YourPath\android_sdk\cmdline-tools\latest\bin\sdkmanager.bat --install "system-images;android-30;google_apis;x86_64"

Find the version of Android OS for the chosen API level, for example here:<br/>
https://developer.android.com/tools/releases/platforms<br/>
API level 30 -> Android 11<br/>

Look up Android 11 devices or refer to this (possibly incomplete) chart:<br/>
[Android 11 devices](./android_11_devices.png)

Cross-reference the devices with the ones supported by avdmanager, run:<br/>
>YourPath\android_sdk\cmdline-tools\latest\bin\avdmanager.bat list device

Create an avd for the chosen OS image and the device model (installs into `YourPath\android_sdk\avd`), run:<br/>
>YourPath\android_sdk\cmdline-tools\latest\bin\avdmanager create avd -n PixelAVD -k "system-images;android-30;google_apis;x86_64" -d "pixel_4" -p "../../../avd/"

Start the emulator, run:<br/>
>YourPath\android_sdk\emulator\emulator.exe -avd PixelAVD

### 4. Virtualisation setup (in case of errors running emulator)

General guide:<br/>
https://developer.android.com/studio/run/emulator-acceleration#accel-vm

Check if CPU Virtualization Technology is enabled in BIOS (UEFI)

Check if Hyper-V Windows feature is disabled, run:<br/>
>YourPath\android_sdk\emulator\emulator-check.exe hyper-v

Find a suitable hypervisor package, run:<br/>
>YourPath\android_sdk\cmdline-tools\latest\bin\sdkmanager.bat --list | findstr "extras"

Install the package files, run:<br/>
>YourPath\android_sdk\cmdline-tools\latest\bin\sdkmanager --install "extras;google;Android_Emulator_Hypervisor_Driver"

(installes into `YourPath\android_sdk\extras\google\Android_Emulator_Hypervisor_Driver`)

Install the hypervisor service, run:<br/>
>YourPath\android_sdk\extras\google\Android_Emulator_Hypervisor_Driver\silent_install.bat

In case that fails, install manually, run:<br/>
>YourPath\android_sdk\extras\google\Android_Emulator_Hypervisor_Driver/RUNDLL32.EXE SETUPAPI.DLL,InstallHinfSection DefaultInstall 132 .\gvm.Inf

Start the service, run:<br/>
>sc start gvm

Start the emulator, run:<br/>
>YourPath\android_sdk\emulator\emulator.exe -avd PixelAVD

-----
You can also:<br/>
query VM service status - `sc query gvm`<br/>
stop VM service - `sc stop gvm`<br/>
delete VM service - `sc delete gvm`<br/>

### 5. Graphics acceleration

General guide:<br/>
https://developer.android.com/studio/run/emulator-acceleration#command-gpu


### 6. Connecting the app to the emulator in real-time

Bundle
>npx react-native bundle ...

Rename folder...

Assemble...
>./gradlew.bat assembleRelease

Start gvm...

Start emulator, wait for the OS to fully load

In VS Code terminal, run:<br/>
> MyFirstProject> npx react-native run-android