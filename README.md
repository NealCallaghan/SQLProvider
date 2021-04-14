# SQLProvider

This project is a work in progress.

## Using SQLProvider
SQLProvider generates classes that depend upon `netstandard-System.Data.Linq` which is a package for dot net core that is an unofficial port of Linq-To-Sql for core.

The project is a C#9 source generator, to use it you must reference it as an analyser from the project using it like in the csproj example below:

```
<PropertyGroup>
  <OutputType>Exe</OutputType>
  <TargetFramework>net5.0</TargetFramework>
</PropertyGroup>

<ItemGroup>
  <PackageReference Include="netstandard-System.Data.Linq" Version="1.0.1" />
</ItemGroup>

<ItemGroup>
  <ProjectReference Include="{project refernce...}" ReferenceOutputAssembly="false" OutputItemType="Analyzer" />
</ItemGroup>
```

Upon building you will have in your project a new namespace `CSharp.Data.Sql`, in adding a using you will be able to reference some classes and attributes used in creating a new SQLProvider.

To create a new SQLProvider you must create a new partial class which inherits from `SqlDataProvider`, this class must also have an attribute added to it called `ConnectionSet` which has two properties `ConnectionString` and `DatabaseType`. These settings are used at build time to connect to a database and extend your partial class.

The class must also not have a constructor with a single `string` as a parameter as this is created in the generated code at build time.

See the code below as an example in creating a SQLProvider:

```
[ConnectionSet(ConnectionString = "MyConnectionString", DatabaseType = DatabaseType.MsSqlServer)]
public partial class MyNewProvider : SqlDataProvider
{}
```

The connection string for the moment must be hard coded, however when using the generated code a new constructor allows you pass in a run time connection string which can come from anywhere.

## Limitations
At the moment only windows and SQLServer is supported. more to come
