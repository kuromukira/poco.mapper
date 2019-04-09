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
        private IList<Work> WorkHistory()
        {
            return new List<Work>
            {
                new Work
                {
                    WorkId = Guid.NewGuid(),
                    Title = ".NET Developer",
                    Company = "Arcnays",
                    Address = "Cebu",
                    Status  = Global.Status.Active
                },
                new Work
                {
                    WorkId = Guid.NewGuid(),
                    Title = "Software Development and Production Manager",
                    Company = "Jinisys Software Inc",
                    Address = "Cebu",
                    Status  = Global.Status.InActive
                },
                new Work
                {
                    WorkId = Guid.NewGuid(),
                    Title = "Senior Software Engineer",
                    Company = "Jinisys Software Inc",
                    Address = "Cebu",
                    Status  = Global.Status.InActive
                },
                new Work
                {
                    WorkId = Guid.NewGuid(),
                    Title = "Junior Software Engineer",
                    Company = "root+ Technology Service",
                    Address = "Cebu",
                    Status  = Global.Status.InActive
                }
            };
        }

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
                    Company = "Arcanys",
                    Address = "Cebu",
                    Status = Global.Status.Active
                },
                HistoryArray = WorkHistory().ToArray(),
                HistoryList = WorkHistory(),

                StringArray = new List<string> { "A", "B", "C", "D" }.ToArray(),
                StringList = new List<string> { "A", "B", "C", "D" },

                DecimalArray = new List<decimal> { 0.1m, 0.2m, 0.3m, 0.4m }.ToArray(),
                DecimalList = new List<decimal> { 0.1m, 0.2m, 0.3m, 0.4m },

                IntArray = new List<int> { 1, 2, 3, 4 }.ToArray(),
                IntList = new List<int> { 1, 2, 3, 4 },

                LongArray = new List<long> { 1, 2, 3, 4 }.ToArray(),
                LongList = new List<long> { 1, 2, 3, 4 }
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
                        Company = "Arcanys",
                        Address = "Cebu",
                    Status = Global.Status.Active
                    },
                    HistoryArray=WorkHistory().ToArray(),
                    HistoryList=WorkHistory()
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
                        Company = "Google Inc",
                        Address = "Cebu",
                    Status = Global.Status.Active
                    },
                    HistoryArray=WorkHistory().ToArray(),
                    HistoryList=WorkHistory()
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
                        Company = "Oracle",
                        Address = "Cebu",
                    Status = Global.Status.Active
                    },
                    HistoryArray=WorkHistory().ToArray(),
                    HistoryList=WorkHistory()
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

            foreach (EmployeeViewModel _employeeVM in _employeesVM)
                AssertResults(_employees[_employeesVM.IndexOf(_employeeVM)], _employeeVM);
        }

        private void AssertResults(Employee employee, EmployeeViewModel employeeVM)
        {
            Assert.Equal(employee.EmployeeId, employeeVM.Id);
            Assert.Equal(employee.FullName, employeeVM.EmployeeName);
            Assert.Equal(employee.Work.Title, employeeVM.Work.JobTitle);
            Assert.Equal(employee.Work.Company, employeeVM.Work.CompanyName);
            Assert.Equal(employee.Work.Address, employeeVM.Work.WorkAddress);

            Assert.Equal(employee.IntArray, employeeVM.IntArrayVM);
            Assert.Equal(employee.IntList, employeeVM.IntListVM);

            Assert.Equal(employee.DecimalArray, employeeVM.DecimalArrayVM);
            Assert.Equal(employee.DecimalList, employeeVM.DecimalListVM);

            Assert.Equal(employee.LongArray, employeeVM.LongArrayVM);
            Assert.Equal(employee.LongList, employeeVM.LongListVM);

            // Check Array
            for (int i = 0; i < employee.HistoryArray.Length; i++)
                AssertResults(employee.HistoryArray[i], employeeVM.WorkHistoryArray[i]);

            // Check List
            for (int i = 0; i < employee.HistoryList.Count; i++)
                AssertResults(employee.HistoryList[i], employeeVM.WorkHistoryList[i]);
        }

        private void AssertResults(Work work, WorkViewModel workVM)
        {
            Assert.Equal(work.Address, workVM.WorkAddress);
            Assert.Equal(work.Company, workVM.CompanyName);
            Assert.Equal(work.Title, workVM.JobTitle);
        }
    }

    public class Global
    {
        public enum Status { Active, InActive }
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
        [MappedTo("WorkHistoryArray")]
        public Work[] HistoryArray { get; set; }
        [MappedTo("WorkHistoryList")]
        public IList<Work> HistoryList { get; set; }

        [MappedTo("StringListVM")]
        public IList<string> StringList { get; set; }
        [MappedTo("StringArrayVM")]
        public string[] StringArray { get; set; }
        [MappedTo("DecimalListVM")]
        public IList<decimal> DecimalList { get; set; }
        [MappedTo("DecimalArrayVM")]
        public decimal[] DecimalArray { get; set; }
        [MappedTo("IntListVM")]
        public IList<int> IntList { get; set; }
        [MappedTo("IntArrayVM")]
        public int[] IntArray { get; set; }
        [MappedTo("LongListVM")]
        public IList<long> LongList { get; set; }
        [MappedTo("LongArrayVM")]
        public long[] LongArray { get; set; }
    }

    public class Work
    {
        public Guid WorkId { get; set; }
        [MappedTo("JobTitle")]
        public string Title { get; set; }
        [MappedTo("CompanyName")]
        public string Company { get; set; }
        [MappedTo("WorkAddress")]
        public string Address { get; set; }
        [MappedTo("WorkStatus")]
        public Global.Status Status { get; set; } = Global.Status.Active;
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
        public WorkViewModel[] WorkHistoryArray { get; set; }
        public IList<WorkViewModel> WorkHistoryList { get; set; }
        public IList<string> StringListVM { get; set; }
        public string[] StringArrayVM { get; set; }
        public IList<decimal> DecimalListVM { get; set; }
        public decimal[] DecimalArrayVM { get; set; }
        public IList<int> IntListVM { get; set; }
        public int[] IntArrayVM { get; set; }
        public IList<long> LongListVM { get; set; }
        public long[] LongArrayVM { get; set; }
    }

    public class WorkViewModel
    {
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string WorkAddress { get; set; }
        public Global.Status WorkStatus { get; set; } = Global.Status.Active;
    }
}
