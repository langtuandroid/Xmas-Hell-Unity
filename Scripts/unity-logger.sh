 #!/bin/sh

 tail -F ~/Library/Logs/Unity/Editor.log &
 /Applications/Unity/Unity.app/Contents/MacOS/Unity "$@"
 EXITCODE="$?"
 kill %1
 exit "$EXITCODE"