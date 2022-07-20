#! /bin/bash

echo "installing / updating dotnet new operation-contract template"
dotnet new --install ./App.ModuleName.Domain.Contracts/Operations/Commands/UpdateExample | grep "operation-contract"

echo "installing / updating dotnet new operation-body template"
dotnet new --install ./App.ModuleName.Domain/Operations/Commands/UpdateExample | grep "operation-body"
