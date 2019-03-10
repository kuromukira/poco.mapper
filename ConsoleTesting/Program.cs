using POCO.Mapper;
using System;

namespace ConsoleTesting
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Example Data Only
            Employee _employee = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                FirstName = "Nor",
                Lastname = "Gelera",
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
    }

    /// <summary>
    /// POCO entity based on actual database table
    /// </summary>
    public class Employee
    {
        [MappedTo("Id")]
        public Guid EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        [MappedTo("EmployeeName")]
        public string FullName { get { return Lastname + ", " + FirstName; } }
        [MappedTo("Work")]
        public Work Work { get; set; } = new Work();
    }

    public class Work
    {
        public Guid WorkId { get; set; }
        [MappedTo("JobTitle")]
        public string Title { get; set; }
        [MappedTo("WorkAddress")]
        public string Address { get; set; }
    }

    /// <summary>
    /// POCO view-model entity which will be consumed outside of your data layer
    /// </summary>
    public class EmployeeViewModel
    {
        public Guid Id { get; set; }
        public string EmployeeName { get; set; }
        public string FirstName { get; set; } // Will be ignored
        public string Lastname { get; set; } // Will be ignored
        public WorkViewModel Work { get; set; } = new WorkViewModel();
    }

    public class WorkViewModel
    {
        public string JobTitle { get; set; }
        public string WorkAddress { get; set; }
    }
}
