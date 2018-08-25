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
>![Console logger output](data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAZMAAAB4BAMAAADFxgKnAAAAIVBMVEUAAAD////AwMD//wD/AP//AAAYg9ePkpfKzM3T2eBoeZGwtQXQAAAHMklEQVR42uyYX3KbQAzGJecC+p76muEEmfGF0iP0qVfoFZrTFv2z1osDtJnsENciRuJjl9GPZVkF+o47MXmgHNAeKEe0B8oR7e5Qvv10+4EvbIHye7a3t7d7QPnl9sMUQAC2HYt6IgDEoQkJFuYNhK9PMUZZoZCbojDYdoUSR5Ka/qVJk7fY33FQPDcDsNwCwfhCAzqUio+AAiK5oMgqiliKpE6YCASROSoUYheEVLQehE+2QmESaVHkggLuUezQiTxi0Q3CeQoucHUaMD6FQoQLiu15NmiGheJaYIDFkSgCGE1GCBQiazMSRYQbFPAKCtgVBcrM05nUogT6AGvmCrP4G6weIgsJHIeh5TEvUPgaRdgk5qEobrGu9Ci5rkTGpDorCunZfMCIrSFfBGENKM59tvUopW90e7+J4O/sCDWY4Fj275WxMI5ld1fk34U9UI5oD5Qj2gPliCb0SndjrwCRMpGodxGQir2SbHTp2mT/jOs6DDlNk4UndxfdHfQHjv/WLroQVI9rqWB9THvfvvtFoM01yktV7FHphL5N9ve4epg2nZ4tblGyMbwnQSOL4YgM1fNajkSmbaIIGC2KLFBK93revPGDsr9bj0LT+yi6ufdeiQLTWWoL0DV7JUfxrBoUJMA+FOIVlGmi+fd8mjeanqcORRoUalDsmKWuqNoqSqUS88LSzZjI/BVKxsSpF4ppwBWKIrj3rUXJ+deNfuhC+isUrKKweFfzkEq50vHdEqX0QnENdIUyTR3Ke6PCce3Q4+ljR8XWqCBTuT1XcpKvPGCbKJp8h7I+V4RW5soH32Cle1tUG49tp7FrUigBkCg6Z7beYBFxB7L5Bstn1fxyXUl9a11hwLBMg1yvK/Pv9Dz5lJ+jW+sKASbcWlfilpm2Ou3Hmo3MB+xAKDpCYV8epbWD12DEFXPThtNf+tecYNDe2mtgDRZrWlFljBLDWT/fWPbWXuNqsLiLt1CIFiiR5v7aa2wNBl6MCgOOAnQokrUW9tZeA2swlgWKpwSYb1FiXhjK3tprYA3GAG6hEGiJIpdR2F17Da7B9qMwVN9fe42swf4OJdMEy97aa2QNRoDFta4YgoYaL9cVvwVbtdd/ttrT3dg9oXxyDfb0QmdqrT1kXBU+nGsMENr2d7GBNdgqClX/rLcqF8Ke72LDarAepbfu1ikKHEX9dm02qgYrlPOZns5PL0+z19j9y/k2SuYi2PNdbEANViiasgIpzOxnMvUed0+Br0+BAuz5LjamBiuU8/lPu2aXpToIw3Hk2He7A05WcJfgQxfgSzdw97+Imw/TtKmIc/AyTEdqJhSo5mdBmD9FlD/PUSgsBpLryb+ii7VcgzGAjJgciqlgnNNYxvLarMEarIxCd8qj7H/ByrpYwzUYokjnwsjvCAxHYFRvKNJl3LxS1MU6mO0F5xgLF/xBPgoKpZ+1BhvmeQ4+mWY8nmyc4aurvcidDjaHIbdgERVfj/72Ir0ONqPPo4QtSF97kV4Hm8NtmAe2gH2NvaFcPEpHe5Gni0eZEWGY0QiI84YyXtQIhXxHe5GE7+9KmG8zGqOQ394VWxeT720vcouymKA8Gytd7UU+QBkcSuYXrL+9SLcGo3llwOFCJsP+5uYV/c+RXz9pL3KTPguXX4AC8GRjOlglwIu7vJAi5BpA+I/p73ofPWZQIp88Dyluror7+rYo4D7dnuUqo8AGpXxDIrwfJSzPbEWApF4+LVl5TOxjojoI2/aWD+xDYqDVQyFar+0VRc5jIqseK2n9zFZAj0cCQYGlnFC4HJhjaSde2mhecGKyc7nKrrOwpT4CWS0KR8YHeg7fUFIeRduhNxTLY2MKW8ryKFqfgK1+rGh4AWRIGErMomg79oqyyisK2SMU6WBaL/YOlKjfpn5QgnsooOWFDuY7VNx3MPR7FEOH98wrFKcORRAERdmVy/BFb/VLaEDGnQ607m5ab+010TkksmqUr05d1vTt19SjvP6N+Lb113zWYN/zPNh5usrJFMxrduI2ptTIe6oPIgOxK+ljLXSwCQ9FOItfsfiH2iWO04XfQ0pG8qGkjzXQwc5Xi91yDoX/GoraSsAIBX2sgQ5GKNjHCGhCP7FXcyjBobBTlJI+1kAHI5QwnTFmftEJllEpmUcZLzmUkj7WQAcTlKtDITc9QDntUTj0cCrpYw10MAybB75D4Tvz6lgp62NtdLBpjcLhM4ZHCeMe5SvP6jfQwWheOXNf4nHPHo07HeU384r++IpJGR1jeU/yYLN9OEw6EsozTSsmp31BdvVeoX010MGcdOdQnK5VoX010cG0ziqzpxXaVxMdLNGx1r7MvK7VQPuq0sGkTKWJRTExBRIaaV/1OhjAAxRy4FAaaF/VOtgDFC53KA20ryodLIMih+pajbSveh2My9WATLUur2s10L6qdTDoWvt6WQeTup61r6OuwcJh0gelx/RB6TD9AzOqLmaZwDf5AAAAAElFTkSuQmCC)

## Loggers and features
##### Out the box loggers:
1. Console logger
2. File logger
3. Rolling file logger
4. DB(mssql) logger
5. Debug(visual studio output) logger
6. Lambda logger
7. Empty logger

And logger wrappers:
1. Composite logger
2. Thread safe logger
3. Pipe logger

##### Customizable output template and formats:
Log message model look like:
* Timestamp: DateTime
* Message : String
* Level: LogLevel   
* Exception: Exception
* Extra: StringSet

For most loggers used string log serializer, and any part of log message can have own format.
Default output template for string log serializer: *`"{Timestamp} [{Level}] {Message}{ExNewLine}{Exception}"`*, 
and default timestamp format: *"yyyy-MM-dd HH:mm:ss.ff"*
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
>*"01:09:21 | MyPC | DEBUG MESSAGE"*

Or we can replace default serializer by class that implemented IStringLogSerializer.
```csharp
var logger = LoggingWizard.Spell.ConsoleLogger(
    new JsonLogSerializer()
);
logger.Debug("DEBUG MESSAGE");
```
>Output: 
>*`"{"Timestamp":"2018-08-25T01:15:09.8698551+03:00","Message":"DEBUG MESSAGE","Exception":null,"Level":0,"Extra":{}}"`*

**Log level verification**
Log level verifier determine can we write log message or not.
The are five log levels: Debug, Info, Warning, Error, Critical and three type of verifiers: minimum level verifier, range level verifier, multiple level verifier.
By default used minimum log level verifier. Code below not write debug message in output.
```csharp
var logger = LoggingWizard.Spell.ConsoleLogger(LogLevel.Info)
logger.Debug("DEBUG MESSAGE");
logger.Info("INFO MESSAGE");
logger.Warning("WARNING MESSAGE");
```
>Output: 
>*`"2018-08-25 19:33:57.28 [Info] INFO MESSAGE"`*
>*`"2018-08-25 19:33:57.31 [Warning] WARNING MESSAGE"`*

Also log level verifier can be a custom class that implement ILogLevelVerifier.

**Loggers composition**
We can combine loggers by using logger wrappers or lambda logger.
In basic scenario expected that loggers work in single thread and does not use lock.
But if we need thread safe logger, just use wrapper:
```csharp
//use existed logger
var logger = LoggingWizard.Spell.ThreadSafeLogger(new ConsoleLogger());
//use factory
var logger = LoggingWizard.Spell.ThreadSafeLogger(f => f.ConsoleLogger())
```
We can write to multiple loggers at once by using composite logger:
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
We can write log by lambda, combine loggers and use specific predicates etc.:
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
