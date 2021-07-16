# mars-rover

### Angular v8 SPA with .NET Core 3.1 Web API

## To Run the Project
- Add your NASA API Key to the appsettings.json file

- Execute Script:
````

cd mars-rover/src/mars-rover/ClientApp

npm i

cd ..

dotnet restore
dotnet build

cd ../../tests/mars-rover-tests

cd ../../mars-rover

dotnet run --launch-profile mars_rover

````

- Navigate to: https://localhost:5001/gallery
