#!/bin/sh

UNITY_LOG_PATH=~/Library/Logs/Unity
UNITY_LOG_FILE="Editor.log"

echo "Unity log path: $UNITY_LOG_PATH"

if [[ ! -e ${UNITY_LOG_PATH}/${UNITY_LOG_FILE} ]]; then
  mkdir -p ${UNITY_LOG_PATH}
  touch ${UNITY_LOG_PATH}/${UNITY_LOG_FILE}
fi

tail -F ${UNITY_LOG_PATH}/${UNITY_LOG_FILE} &
/Applications/Unity/Unity.app/Contents/MacOS/Unity "$@"
EXITCODE="$?"
kill %1
exit "$EXITCODE"