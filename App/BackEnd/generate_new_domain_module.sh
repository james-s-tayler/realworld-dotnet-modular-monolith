#! /bin/bash

if [ -z "$1" ]
  then
    echo "Error: Please supply new module name"
    exit
fi

echo "installing / updating dotnet new templates..."
dotnet new --install ./Application.ModuleName.Domain.Contracts | grep "module-contracts"
dotnet new --install ./Application.ModuleName.Domain | grep "module-domain"
dotnet new --install ./Application.ModuleName.Domain.Tests.Unit | grep "module-unit-tests"

echo

module_name=$1
echo "generating projects for module: ${module_name}..."
echo "generating Application.${module_name}.Domain.Contracts"
dotnet new module-contracts --name ${module_name} --output Application.${module_name}.Domain.Contracts
echo
echo "generating Application.${module_name}.Domain"
dotnet new module-domain --name ${module_name} --output Application.${module_name}.Domain
echo
echo "generating Application.${module_name}.Domain.Tests.Unit"
dotnet new module-unit-tests --name ${module_name} --output Application.${module_name}.Domain.Tests.Unit
echo
