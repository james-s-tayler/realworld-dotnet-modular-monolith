#! /bin/sh
echo "Workspace: ${1}"
echo "PWD: ${PWD}"
ls -ltr
git log -1
git log -1 | grep "\[[A-Z]*-[0-9]*\]"
