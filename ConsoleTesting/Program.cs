using POCO.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTesting
{
    class Program
    {
        static void Main(string[] args)
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
    }

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
}
