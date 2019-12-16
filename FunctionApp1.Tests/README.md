Add an `appsettings.json` file.

Copy the code snippet below into your `appsettings.json` file:

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

Add an `appsettings.Development.json` file.

Copy the code snippet below into your `appsettings.Development.json` file:

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  }
}
```

