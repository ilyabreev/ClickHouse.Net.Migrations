[![Build Status](https://travis-ci.org/ilyabreev/ClickHouse.Net.Migrations.svg?branch=master)](https://travis-ci.org/ilyabreev/ClickHouse.Net.Migrations)
[![NuGet version](https://badge.fury.io/nu/ClickHouse.Net.Migrations.svg)](https://badge.fury.io/nu/ClickHouse.Net.Migrations)

# ClickHouse.Net.Migrations
Basic migrations functionality for ClickHouse.Net

## Install

### via Package Manager
```powershell
PM> Install-Package ClickHouse.Net.Migrations
```

### via dotnet CLI
```
> dotnet add package ClickHouse.Net.Migrations
```

## Use

In your `Startup.cs` add to `ConfigureServices`:

```c#
services.AddClickHouseMigrations();
```

and define how to resolve `ClickHouseConnectionSettings`:

```c#
services.AddTransient(p => new ClickHouseConnectionSettings(connectionString));
```

Then add `IClickHouseMigrations` as dependency in any of your classes.