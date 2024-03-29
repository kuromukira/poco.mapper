# POCO.Mapper
An alternative "Plain Old C# Objects" mapper with minimal configuration required.

POCO stands for "Plain Old C# Object" or "Plain Old CLR Object", depending on who you ask. This library is a custom mapper for POCOs (map values of identical properties from one POCO to another POCO). Minimal configuration is needed.

![Nuget](https://img.shields.io/nuget/dt/POCOMapper)
![Nuget](https://img.shields.io/nuget/v/POCOMapper)
![GitHub](https://img.shields.io/github/license/kuromukira/poco.mapper)

# Why?

Click [me](https://medium.com/@norgelera/why-create-an-automapper-alternative-7181e6201d61) to learn more

# How-To

## [NuGet](https://www.nuget.org/packages/POCOMapper/)

Add as reference in your class
```c#
using POCO.Mapper;
using POCO.Mapper.Extension;
```
Let's say you have a different POCO for database models and for view-models. ```MappedTo("")``` attribute is required to map the property to a target POCO.

***Note***: *As of v2.0.0, you'll have an option to use extension methods or the IMapper interface. For extension methods, it is required that a POCO inherits from ```POCO.Mapper.Extension.ModelMap```.
```c#
/// <summary>
/// POCO entity based on actual database table
/// </summary>
public class Employee : ModelMap
{
    [MappedTo("Id")]
    public long EmployeeId { get; set; }
    
    public string FirstName { get; set; } // Will not be mapped
    
    public string Lastname { get; set; } // Will not be mapped
    
    [MappedTo("BDay")]
    [UseFormat("yyyy-MMM-dd")]
    public DateTime BirthDate { get; set; }
    
    [MappedTo("EmployeeName")]
    public string FullName
    {
        get { return Lastname + ", " + FirstName;  }
    }
    
    [MappedTo("WorkView")]
    public Work Work { get; set; } = new Work();
}

public class Work : ModelMap
{
    public Guid WorkId { get; set; } // Will not be mapped
    
    [MappedTo("JobTitle")]
    public string Title { get; set; }
    
    [MappedTo("WorkAddress")]
    [IgnoreIf(typeof(AnotherObject))] // This property will not get mapped if the target type is AnotherObject
    public string Address { get; set; }
}

/// <summary>
/// POCO view-model entity which will be consumed outside of your data layer
/// </summary>
public class EmployeeViewModel
{
    public long Id { get; set; }
    public string EmployeeName { get; set; }
    public string FirstName { get; set; } // Will be ignored
    public string Lastname { get; set; } // Will be ignored
    public string BDay { get; set; }
    public WorkViewModel WorkView { get; set; } = new WorkViewModel();
}

public class WorkViewModel
{
    public string JobTitle { get; set; }
    public string WorkAddress { get; set; }
}
```

### Using ```IMapper``` Interface

```c#
void Map()
{
    // Example Data Only
    using Employee _employee = new Employee
    {
        EmployeeId = 1,
        FirstName = "Nor",
        Lastname = "Gelera",
        BirthDate = DateTime.Now,
        Work = new Work
        {
            WorkId = Guid.NewGuid(),
            Title = ".NET Developer",
            Address = "Cebu"
        }
    };

    // Initialize Mapper
    IMapper<EmployeeViewModel, Employee> _mapper = new ModelMapper<EmployeeViewModel, Employee>();

    // Map to view-model
    EmployeeViewModel _employeeViewModel = _mapper.from(_employee);
}
```

### Using ```Extension Methods```

```c#
void Map()
{
    // Example Data Only
    using Employee _employee = new Employee
    {
        EmployeeId = 1,
        FirstName = "Nor",
        Lastname = "Gelera",
        Work = new Work
        {
            WorkId = Guid.NewGuid(),
            Title = ".NET Developer",
            Address = "Cebu"
        }
    };

    // Map to view-model
    EmployeeViewModel _employeeViewModel = _employee.MapTo<EmployeeViewModel>();
}
```

The result would be an instance of ```EmployeeViewModel``` with values for ```Id```, ```EmployeeName```, ```BDay``` from ```Employee``` entity. ```FirstName``` and ```LastName``` properties of ```Employee``` entity will be ignored by **POCOMapper** and will not be mapped to ```EmployeeViewModel```. Values for ```Work``` property of ```Employee``` will also be mapped to ```WorkViewModel```.

***Note***: *As of the current version, ```POCO.Mapper``` also supports mapping of values for ```IList<>```, ```List<>``` and ```Array[]``` properties (_interchangeably_).*

# Contributors
- [kuromukira](https://www.twitter.com/norgelera)

Install the following to get started

**IDE**
1. [Visual Studio Code](https://code.visualstudio.com/) 
2. [Visual Studio Community](https://visualstudio.microsoft.com/downloads/)

**Extensions**
1. [C# Language Extension for VSCode](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)

**Frameworks**
1. [.NET Core](https://www.microsoft.com/net/download)


Do you want to contribute? Send me an email or DM me in [twitter](https://www.twitter.com/norgelera).
