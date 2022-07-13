#! /bin/bash

if [ -z "$1" ]
  then
    echo "Error: Please supply new module name"
    exit
fi

echo "installing / updating dotnet new module-unit-tests template..."
dotnet new --install ./Application.ModuleName.Domain.Tests.Unit | grep "module-unit-tests"
echo
module_name=$1
echo "generating Application.${module_name}.Domain.Tests.Unit"
dotnet new module-unit-tests --name ${module_name} --output Application.${module_name}.Domain.Tests.Unit
echo
