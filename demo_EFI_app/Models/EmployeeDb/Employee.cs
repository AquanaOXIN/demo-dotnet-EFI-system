// Models/EmployeeDb/Employee.cs
namespace demo_EFI_app.Models.EmployeeDb
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? HomeAddress { get; set; }
        public DateTime HireDate { get; set; }
        public byte EmploymentStatus { get; set; }
    }
}