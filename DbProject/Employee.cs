using System;

namespace DbProject
{
    public class Employee
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public virtual EmployeeDetails Details { get; set; }
    }
}