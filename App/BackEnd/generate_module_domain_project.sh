#! /bin/bash

if [ -z "$1" ]
  then
    echo "Error: Please supply new module name"
    exit
fi

echo "installing / updating dotnet new module-domain template..."
dotnet new --install ./App.ModuleName.Domain | grep "module-domain"
echo
module_name=$1
echo "generating App.${module_name}.Domain"
dotnet new module-domain --name ${module_name} --output App.${module_name}.Domain
echo
