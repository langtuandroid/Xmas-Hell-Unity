#! /bin/sh

project="Xmas-Hell"

echo "Attempting to build $project for Windows"
/Applications/Unity/Unity.app/Contents/MacOS/Unity
  -batchmode
  -nographics
  -silent-crashes
  -logFile $(pwd)/unity.log
  -projectPath $(pwd)
  -buildWindowsPlayer "$(pwd)/Build/windows/$project.exe"
  -quit

echo "Attempting to build $project for Android"
/Applications/Unity/Unity.app/Contents/MacOS/Unity
  -batchmode
  -nographics
  -silent-crashes
  -logFile $(pwd)/unity.log
  -projectPath $(pwd)
  -buildAndroidUniversalPlayer "$(pwd)/Build/android/$project.apk"
  -quit

echo 'Logs from build'
cat $(pwd)/unity.log

echo 'Attempting to zip builds'
zip -r $(pwd)/Build/linux.zip $(pwd)/Build/linux/
zip -r $(pwd)/Build/android.zip $(pwd)/Build/android/