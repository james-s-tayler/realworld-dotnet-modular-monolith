#! /bin/bash

if [ -z "$1" ]
  then
    echo "Error: Please supply new module name"
    exit
fi

./generate_module_contracts_project.sh $1
./generate_module_domain_project.sh $1
./generate_module_unit_test_project.sh $1
