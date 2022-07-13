#! /bin/bash

if [ -z "$1" ]
  then
    echo "Error: Please supply new module name"
    exit
fi

echo "installing / updating dotnet new module-domain template..."
dotnet new --install ./Application.ModuleName.Domain | grep "module-domain"
echo
module_name=$1
echo "generating Application.${module_name}.Domain"
dotnet new module-domain --name ${module_name} --output Application.${module_name}.Domain
echo
