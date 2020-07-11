

## SHAutomation - C# library for interacting with Windows applications.

### Badges
| What | Badge |
| ---- | ----- |
| *Build* | [![Build status](https://dev.azure.com/Streets-Heaver/Automation/_apis/build/status/SHAutomation%20GitHub)](https://dev.azure.com/Streets-Heaver/Automation/_build/latest?definitionId=219) |
| *Nuget* | [![NuGet FlaUI.Core](http://flauschig.ch/nubadge.php?id=SHAutomation)](https://www.nuget.org/packages/SHAutomation) |


### Introduction
SHAutomation is a .NET library to help with writing automated UI tests for Windows application. The project is a fork of [FLAUI](https://github.com/FLAUI) but with undeeded code removed and various fixes/enhancements implemented.
#### Why fork FLAUI?
There was a lot of what FLAUI did that was really good but there were a few bits it didn't do too well or we thought we could improve. We removed everything that was obsolete or we thought wasn't really needed anymore such as UIA2. We tidied up a few bits and made everything a bit more user friendly.


### Getting Started
#### Installation
Simply install the SHAutomation nuget package, either use the Nuget package manager in Visual Studio or run the following command in the package manager console.
```
Install-Package SHAutomation 
```

#### Basic Usage
To interact with an application start by launching it and then getting the main window. From there you can find elements and interact with the application UI.
```csharp
using SHAutomation.UIA3;

using var application = Application.Launch("Application.exe");
using var automation = new UIA3Automation();
{
    var window = application.GetMainWindow(automation);
    var element = window.Find("elementAutomationId");
}
```

To interact with an element you would call ```Click``` after finding the element
```csharp
using SHAutomation.UIA3;

using var application = Application.Launch("Application.exe");
using var automation = new UIA3Automation();
{
    var window = application.GetMainWindow(automation);
    var element = window.Find("elementAutomationId").Click();
}
```

Once your code has finished running the application will automatically be closed. 

For advanced features and more examples please see the [Wiki](https://github.com/Streets-Heaver/SHAutomation/wiki).
