# dotnetsay .NET Tool Sample

This is a small API created using .NET used for endpoints that currently do not exist in the EthosAPI.

## Development and Testing

Take a look an an example [ASP.NET Core Tutorial](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-6.0&tabs=visual-studio-code) to make sure you have the needed tools and libraries on development computer. Open to the repository using [vscode](https://code.visualstudio.com/). Once the folder is open you can start a development version of the API by pressing Ctrl-F5.

You will need to add the needed settings for the API to work. It is recommended to use the [Secret Manager](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows) since setting it directly in the settings json will save sensivite data in the repository.

First enable secret storage if you have not used it before:

```console
dotnet user-secrets init
```

You can now locally store values for the needed settings. Here is an example of savings the connection string for Colleague:

```console
dotnet user-secrets set "ConnectionStrings:Colleague" "Server=SERVER;Database=DATABASE;User Id=USER;Password=PASSWORD;"
```

## Publish and Deploy API

You can publish the API using the command:

```console
dotnet publish --configuration Release
```

Move the contents of the bin/Release/{TARGET FRAMEWORK}/publish folder to the IIS site folder on the server, which is the site's Physical path in IIS Manager.