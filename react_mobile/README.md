
### Creating a React Native app executable (.APK) for Android OS in VS Code under Win10 using only Android Command Line Tools (no installation of Android Studio required)

#### 1. Downloads and the environment

Create folder for Android SDK files:<br>
`YourPath\android_sdk\`

Download command line tools:<br>
https://developer.android.com/studio#command-tools

Extract to:<br>
`YourPath\android_sdk\`

In the unzipped cmdline-tools directory, create a sub-directory called `latest`.

Move the original `cmdline-tools` directory contents, including the `lib` directory, `bin` directory, `NOTICE.txt` file, and `source.properties` file, into `latest`.

Add command tools location to environmental variable `Path`:<br>
`YourPath\android_sdk\cmdline-tools`

Add new environmental variable<br>
name = `ANDROID_HOME`<br>
value = `YourPath\android_sdk`<br>

Download openJDK v17.0.2 to run sdkmanager from command tools:<br>
https://jdk.java.net/archive/

Extract to:<br>
`YourPath\android_sdk\`

Add Java location to environmental variable Path:<br>
`YourPath\android_sdk\jdk-17.0.2\bin`

Accept licenses, run:<br>
`YourPath\android_sdk\cmdline-tools\latest\bin\sdkmanager --licenses`

(this should create folder YourPath\android_sdk\licenses)

#### 2. Creating an app

Create folder for react native projects, e.g., `reactNative` and open it in VS Code

Run:<br>
npx react-native init MyFirstProject

Run:<br>
cd .\MyFirstProject\

Create `assets` folder in `reactNative/MyFirstProject/android/app/src/main/`

Run:<br>
npx react-native bundle --platform android --dev false --entry-file index.js --bundle-output android/app/src/main/assets/index.android.bundle --assets-dest android/app/src/main/res/

In folder `reactNative/MyFirstProject/android/app/src/main/res/`<br>
rename `drawable-mdpi` to `_drawable-mdpi` to avoid resource duplication conflict

Run:<br>
cd ./android

Run:<br>
./gradlew.bat assembleRelease

Android executable file (.apk) will be located in<br>
`reactNative/MyFirstProject\android\app\build\outputs\apk\release`

This file can be uploaded online, opened in a browser on an android mobile, downloaded and installed.