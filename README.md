# NWrath.Logging
Simple .NET logging infrastructure.

### Getting started
Install NuGet:
```
Install-Package NWrath.Logging
```
Set up logger by Wizard:
```csharp
var logger = LoggingWizard.Spell.ConsoleLogger();
```
Or just create class instance:
```csharp
var logger = new ConsoleLogger();
```
Then use it:
```csharp
logger.Debug("DEBUG MESSAGE");
logger.Info("INFO MESSAGE");
logger.Warning("WARNING MESSAGE");
logger.Error("ERROR MESSAGE", new NotImplementedException("err"));
logger.Critical("CRITICAL MESSAGE", new ApplicationException("err"));
```
>Output:
>
>![Console logger output](https://res.cloudinary.com/di745chv1/image/upload/v1535222860/ConsoleLoggerOutput1.png)

## Loggers and features
##### Out the box loggers:
1. Console logger
2. File logger
3. Rolling file logger
4. DB(mssql) logger
5. Debug(visual studio output) logger
6. Lambda logger
7. Empty logger
8. Composite logger
9. Background logger

##### Customizable output template and formats:
Log message model look like:
* Timestamp: DateTime
* Message : String
* Level: LogLevel   
* Exception: Exception
* Extra: StringSet

For most loggers used string log serializer, and any part of log message can have own format.
Default output template for string log serializer: *`"{Timestamp} [{Level}] {Message}{ExNewLine}{Exception}"`*, 
and default timestamp format: *"yyyy-MM-dd HH:mm:ss.fff"*
But we can easily change it and even extend.
```csharp
var logger = LoggingWizard.Spell.ConsoleLogger(s =>
{
    s.OutputTemplate = "{Timestamp} | {machine} | {Message}";
    s.Formats.Timestamp = m => $"{m.Timestamp:HH:mm:ss}";
    s.Formats["machine"] = m => Environment.MachineName;
});
logger.Debug("DEBUG MESSAGE");
```
>Output: 
>
>*"01:09:21 | MyPC | DEBUG MESSAGE"*

Or we can replace default serializer by class that implement IStringLogSerializer.
```csharp
var logger = LoggingWizard.Spell.ConsoleLogger(
    new JsonLogSerializer()
);
logger.Debug("DEBUG MESSAGE");
```
>Output: 
>
>*`"{"Timestamp":"2018-08-25T01:15:09.8698551+03:00","Message":"DEBUG MESSAGE","Exception":null,"Level":0,"Extra":{}}"`*

**Log level verification** 

Log level verifier determine can we write log message or not.
The are five log levels: Debug, Info, Warning, Error, Critical and three type of implemented verifiers: minimum level verifier, range level verifier, multiple level verifier.
By default used minimum log level verifier. Code below not write debug message in output.
```csharp
var logger = LoggingWizard.Spell.ConsoleLogger(LogLevel.Info)
logger.Debug("DEBUG MESSAGE");
logger.Info("INFO MESSAGE");
logger.Warning("WARNING MESSAGE");
```
>Output: 
>
>*`"2018-08-25 19:33:57.280 [Info] INFO MESSAGE"`*
>
>*`"2018-08-25 19:33:57.310 [Warning] WARNING MESSAGE"`*

Also log level verifier can be a custom class that implement ILogLevelVerifier.

**Loggers composition**

We can combine loggers by using composite or lambda logger.
Use composite logger for write to multiple loggers at once:
```csharp
//use existed loggers
var logger = LoggingWizard.Spell.CompositeLogger(
                new ConsoleLogger(),
                new DebugLogger()
                );
//use factory
var logger = LoggingWizard.Spell.CompositeLogger(
                f => f.ConsoleLogger(),
                f => f.DebugLogger()
                );
```
Lambda logger is used for combine loggers or using specific predicates etc.:
```csharp
//use debug logger for ApplicationException and console logger for other messages
var debugLogger = LoggingWizard.Spell.DebugLogger();
var consoleLogger = LoggingWizard.Spell.ConsoleLogger();

var logger = LoggingWizard.Spell.LambdaLogger(m => {
    if (m.Exception is ApplicationException)
    {
        debugLogger.Log(m);
    }
    else
    {
        consoleLogger.Log(m);
    }
});
```
