#! /bin/bash

#check number of args
if [ "$#" -ne 4 ]; then
    echo "Error: please supply module_name, operation_name, operation_type_singular, and operation_type_plural"
    exit
fi

module_name=${1}
operation_name=${2}
operation_type_singular=${3}
operation_type_plural=${4}

echo "installing / updating dotnet new operation-contract template"
dotnet new --install ./App.ModuleName.Domain.Contracts/Operations/Commands/UpdateExample | grep "operation-contract"
echo
echo "installing / updating dotnet new operation-body template"
dotnet new --install ./App.ModuleName.Domain/Operations/Commands/UpdateExample | grep "operation-body"
echo
echo "installing / updating dotnet new operation-unit-tests template"
dotnet new --install ./App.ModuleName.Domain.Tests.Unit/Operations/Commands/ | grep "operation-unit-tests"
echo
echo "Generating ${operation_type_singular} Contracts in ./App.${module_name}.Domain.Contracts/Operations/${operation_type_plural}/${operation_name}"
dotnet new operation-contract --name "${module_name}" --operationName "${operation_name}" --operationTypeSingular "${operation_type_singular}" --operationTypePlural ".${operation_type_plural}" -o "./App.${module_name}.Domain.Contracts/Operations/${operation_type_plural}/${operation_name}"
echo
echo "Generating ${operation_type_singular} Body in ./App.${module_name}.Domain/Operations/${operation_type_plural}/${operation_name}"
dotnet new operation-body --name "${module_name}" --operationName "${operation_name}" --operationTypeSingular "${operation_type_singular}" --operationTypePlural ".${operation_type_plural}" -o "./App.${module_name}.Domain/Operations/${operation_type_plural}/${operation_name}"
echo
echo "Generating ${operation_type_singular} Unit Tests in ./App.${module_name}.Domain.Tests.Unit/Operations/${operation_type_plural}/${operation_name}UnitTests.cs"
dotnet new operation-unit-tests --name "${module_name}" --operationName "${operation_name}" --operationTypeSingular "${operation_type_singular}" --operationTypePlural ".${operation_type_plural}" -o "./App.${module_name}.Domain.Tests.Unit/Operations/${operation_type_plural}"
