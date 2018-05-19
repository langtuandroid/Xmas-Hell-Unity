#! /bin/sh

export ANDROID_SDK_ROOT="/usr/local/share/android-sdk"
export ANDROID_NDK_HOME="/usr/local/share/android-ndk"
export JAVA_HOME=$(/usr/libexec/java_home)

project="Xmas-Hell"
projectPath="Xmas-Hell"
versionName=${TRAVIS_BUILD_NUMBER}

# echo "Attempting to build $project for Windows"
# /Applications/Unity/Unity.app/Contents/MacOS/Unity
#   -batchmode
#   -nographics
#   -silent-crashes
#   -logFile $(pwd)/unity.log
#   -projectPath $(pwd)/Xmas-Hell-Unity
#   -buildWindowsPlayer "$(pwd)/Build/windows/$project.exe"
#   -quit

echo "Attempting to build $project for Android"
./Scripts/unity-logger.sh \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile \
  -projectPath $(pwd)/$(projectPath) \
  -buildAndroidPlayer "$(pwd)/$(projectPath)/Build/Android/$(project)-$(versionName).apk" \
  -quit

# echo "Attempting to build $project for Android"
# /Applications/Unity/Unity.app/Contents/MacOS/Unity \
#   -batchmode \
#   -nographics \
#   -silent-crashes \
#   -logFile \
#   -projectPath $(pwd)/ \
#   -quit \
#   -executeMethod BuildScript.BuildAndroid $(pwd)/Build/android/${project}-${versionName}.apk

echo 'Attempting to zip builds'
zip -r $(pwd)/$(projectPath)/Build/android.zip $(pwd)/$(projectPath)/Build/android/