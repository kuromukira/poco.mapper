using POCO.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace POCOMapper.Test
{
    public class POCOMapperTest
    {
        #region SAMPLE DATA
        private Employee CreateDummyEmployee()
        {
            // Example Data Only
            return new Employee
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
        }

        private IList<Employee> CreateDummyEmployees()
        {
            // Example Data Only
            return new List<Employee>
            {
                new Employee
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
                },
                new Employee
                {
                    EmployeeId = Guid.NewGuid(),
                    FirstName = "John",
                    Lastname = "Doe",
                    Work = new Work
                    {
                        WorkId = Guid.NewGuid(),
                        Title = "Angular Developer",
                        Address = "Cebu"
                    }
                },
                new Employee
                {
                    EmployeeId = Guid.NewGuid(),
                    FirstName = "Jane",
                    Lastname = "Doe",
                    Work = new Work
                    {
                        WorkId = Guid.NewGuid(),
                        Title = "Java Developer",
                        Address = "Cebu"
                    }
                }
            };
        }
        #endregion

        private IMapper<EmployeeViewModel, Employee> lMapper { get; } = new ModelMapper<EmployeeViewModel, Employee>();

        [Fact]
        public void MapValue()
        {
            Employee _employee = CreateDummyEmployee();
            EmployeeViewModel _employeeVM = lMapper.from(_employee);

            AssertResults(_employee, _employeeVM);
        }

        [Fact]
        public void MapValues()
        {
            IList<Employee> _employees = CreateDummyEmployees();
            IList<EmployeeViewModel> _employeesVM = lMapper.from(_employees);

            foreach(EmployeeViewModel _employeeVM in _employeesVM)
                AssertResults(_employees[_employeesVM.IndexOf(_employeeVM)], _employeeVM);
        }

        private void AssertResults(Employee employee, EmployeeViewModel employeeVM)
        {
            Assert.Equal(employee.EmployeeId, employeeVM.Id);
            Assert.Equal(employee.FullName, employeeVM.EmployeeName);
            Assert.Equal(employee.Work.Title, employeeVM.Work.JobTitle);
            Assert.Equal(employee.Work.Address, employeeVM.Work.WorkAddress);
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
