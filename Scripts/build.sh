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
/Applications/Unity/Unity.app/Contents/MacOS/Unity
  -batchmode
  -nographics
  -silent-crashes
  -logFile $(pwd)/unity.log
  -projectPath $(pwd)/$(projectPath)
  -quit
  -executeMethod BuildScript.BuildAndroid $(pwd)/Build/android/${project}-${versionName}.apk

# echo "Attempting to build $project for Android"
# /Applications/Unity/Unity.app/Contents/MacOS/Unity \
#   -batchmode \
#   -nographics \
#   -silent-crashes \
#   -logFile \
#   -projectPath $(pwd)/ \
#   -quit \
#   -executeMethod BuildScript.BuildAndroid $(pwd)/Build/android/${project}-${versionName}.apk

echo 'Logs from build'
cat $(pwd)/unity.log

echo 'Attempting to zip builds'
zip -r $(pwd)/Build/android.zip $(pwd)/Build/android/