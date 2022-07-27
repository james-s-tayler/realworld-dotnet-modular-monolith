#! /bin/sh
echo "PWD: ${PWD}"
git log -1 | grep "\[[A-Z]*-[0-9]*\]"
