echo off

:: Собрать проект с API
dotnet build

:: Создать файл OpenApi
set swaggerOutputFile=ClientGeneration\swagger.json
set apiDllLocation=.\\bin\\Debug\\net8.0\\GuitarStore.Api.dll
set apiVersion=v1

dotnet swagger tofile --output %swaggerOutputFile% %apiDllLocation% %apiVersion%

:: Сгенерировать клиент к API
set nswagDllLocation=%UserProfile%\\.nuget\\packages\\nswag.msbuild\\14.1.0\\tools\\Net80\\dotnet-nswag.dll
set generatedClassName=GuitarStoreClient
set generatedNamespace=GuitarStore
set generatedClassLocation=..\\GuitarStore.Infrastructure\\GuitarStoreClient.cs

dotnet --roll-forward-on-no-candidate-fx 2 %nswagDllLocation% openapi2csclient^
 /className:%generatedClassName% /namespace:%generatedNamespace%^
 /input:%swaggerOutputFile% /output:%generatedClassLocation%^
 /OperationGenerationMode:SingleClientFromOperationId