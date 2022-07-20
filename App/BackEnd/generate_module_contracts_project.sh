#! /bin/bash

if [ -z "$1" ]
  then
    echo "Error: Please supply new module name"
    exit
fi

echo "installing / updating dotnet new module-contracts template"
dotnet new --install ./App.ModuleName.Domain.Contracts | grep "module-contracts"
echo
module_name=$1
echo "generating App.${module_name}.Domain.Contracts"
dotnet new module-contracts --name ${module_name} --output App.${module_name}.Domain.Contracts
echo
