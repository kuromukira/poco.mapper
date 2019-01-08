# POCO-Mapper
POCO stands for "Plain Old C# Object" or "Plain Old CLR Object", depending on who you ask. This library is a custom mapper for POCOs (map values of identical properties from one POCO to another POCO). Minimal configuration is needed.

# How-To

## [NuGet](https://www.nuget.org/packages/POCO.Mapper/)

Add as reference in your class
```c#
using POCO.Mapper;
```
Let's say you have a different POCO for database models and for view-models.
```c#
/// <summary>
/// POCO entity based on actual database table
/// </summary>
public class Employee
{
    [MappedTo("Id")]
    public long EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string Lastname { get; set; }
    [MappedTo("EmployeeName")]
    public string FullName
    {
        get { return Lastname + ", " + FirstName;  }
    }
}

/// <summary>
/// POCO view-model entity which will be consumed outside of your data layer
/// </summary>
public class EmployeeViewModel
{
    public long Id { get; set; }
    public string EmployeeName { get; set; }
}
```
The ```MappedTo("")``` attribute is required to map the property to a target POCO.

Initialize the ```IMapper``` interface.
```c#
void Map()
{
    // Example Data Only
    Employee _employee = new Employee
    {
        EmployeeId = 1,
        FirstName = "Nor",
        Lastname = "Gelera"
    };

    // Initialize Mapper
    IMapper<EmployeeViewModel, Employee> _mapper = new ModelMapper<EmployeeViewModel, Employee>();

    // Map to view-model
    EmployeeViewModel _employeeViewModel = _mapper.from(_employee);
}
```
The result would be an instance of ```EmployeeViewModel``` with values for ```Id``` and ```EmployeeName``` from ```Employee``` entity.

***Note***: *As of the current version, ```POCO.Mapper``` supports mapping of values for ```IList``` properties only.*

# Contributors
- [kuromukira](https://www.twitter.com/norgelera)

Install the following to get started

**IDE**
1. [Visual Studio Code](https://code.visualstudio.com/) 
2. [Visual Studio Community](https://visualstudio.microsoft.com/downloads/)

**Exntesions**
1. [C# Language Extension for VSCode](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)

**Frameworks**
1. [.NET](https://www.microsoft.com/net/download)


Do you want to contribute? Send me an email or DM me in [twitter](https://www.twitter.com/norgelera).
