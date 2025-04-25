namespace MealCompensationCalculator.Domain.Models
{
    public class Employee
    {
        /// <summary>
        /// Табельный номер
        /// </summary>
        public int EmployeeNumber { get; set; }
        /// <summary>
        /// ФИО - Фамилия Имя Отчество
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Подразделение
        /// </summary>
        public string Department { get; set; }

        public Employee(int employeeNumber, string fullName, string department)
        {
            EmployeeNumber = employeeNumber;
            FullName = fullName;
            Department = department;
        }
    }
}