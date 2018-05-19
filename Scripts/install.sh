#! /bin/sh

BASE_URL=https://download.unity3d.com/download_unity
HASH=b8cbb5de9840
VERSION=2018.1.1f1

ANDROID_BUILD_TOOLS_VERSION="25.0.2"
ANDROID_PLATFORM_VERSION="android-23"

echo "executing: brew cask install caskroom/versions/java8"
brew cask install caskroom/versions/java8
echo "executing: export JAVA_HOME=$(/usr/libexec/java_home)"
export JAVA_HOME=$(/usr/libexec/java_home)

echo "executing: brew cask install android-sdk"
brew cask install android-sdk
echo "executing: export ANDROID_SDK_ROOT=\"/usr/local/share/android-sdk\""
export ANDROID_SDK_ROOT="/usr/local/share/android-sdk"

# echo "executing: brew cask install android-ndk"
# brew cask install android-ndk
# echo "executing: export ANDROID_NDK_HOME=\"/usr/local/share/android-ndk\""
# export ANDROID_NDK_HOME="/usr/local/share/android-ndk"

echo "executing: sdkmanager --update"
yes y | ${ANDROID_SDK_ROOT}/tools/bin/sdkmanager --update
echo "executing: sdkmanager \"build-tools;${ANDROID_BUILD_TOOLS_VERSION}\" \"platform-tools\" \"platforms;${ANDROID_PLATFORM_VERSION}\""
${ANDROID_SDK_ROOT}/tools/bin/sdkmanager "build-tools;${ANDROID_BUILD_TOOLS_VERSION}" "platform-tools" "platforms;${ANDROID_PLATFORM_VERSION}"

download()
{
  file=$1
  url="$BASE_URL/$HASH/$package"

  echo "Downloading from $url: "
  curl -o `basename "$package"` "$url"
}

install()
{
  package=$1
  download "$package"

  echo "Installing "`basename "$package"`
  sudo installer -dumplog -package `basename "$package"` -target /
}

# See $BASE_URL/$HASH/unity-$VERSION-$PLATFORM.ini for complete list
# of available packages, where PLATFORM is `osx` or `win`

install "MacEditorInstaller/Unity-$VERSION.pkg"
# install "MacEditorTargetInstaller/UnitySetup-Windows-Support-for-Editor-$VERSION.pkg"
install "MacEditorTargetInstaller/UnitySetup-Android-Support-for-Editor-$VERSION.pkg"
ls -la /Applications/Unity/PlaybackEngines/AndroidPlayer/

# install "MacEditorTargetInstaller/UnitySetup-Mac-Support-for-Editor-$VERSION.pkg"
