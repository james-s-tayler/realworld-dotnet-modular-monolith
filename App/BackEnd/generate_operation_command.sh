#! /bin/bash
#check number of args
if [ "$#" -ne 2 ]; then
    echo "Error: please supply module_name and operation_name"
    exit
fi

./generate_operation.sh "${1}" "${2}" "Command" "Commands"