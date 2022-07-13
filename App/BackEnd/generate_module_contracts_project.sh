#! /bin/bash

if [ -z "$1" ]
  then
    echo "Error: Please supply new module name"
    exit
fi

echo "installing / updating dotnet new module-contracts template"
dotnet new --install ./Application.ModuleName.Domain.Contracts | grep "module-contracts"
echo
module_name=$1
echo "generating Application.${module_name}.Domain.Contracts"
dotnet new module-contracts --name ${module_name} --output Application.${module_name}.Domain.Contracts
echo
