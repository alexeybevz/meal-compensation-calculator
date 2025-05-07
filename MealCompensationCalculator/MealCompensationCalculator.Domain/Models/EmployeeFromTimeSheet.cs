using System;

namespace MealCompensationCalculator.Domain.Models
{
    public class EmployeeFromTimeSheet
    {
        /// <summary>
        /// Табельный номер
        /// </summary>
        public int EmployeeNumber { get; }
        /// <summary>
        /// Фамилия И. О., Должность
        /// </summary>
        public string FullNameAndJobTitle { get; }
        /// <summary>
        /// Фамилия И. О.
        /// </summary>
        public string FullName { get; }
        /// <summary>
        /// Должность
        /// </summary>
        public string JobTitle { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="employeeNumber">Табельный номер</param>
        /// <param name="fullNameAndJobTitle">Фамилия, инициалы, должность (специальность, профессия).
        /// Формат: Фамилия И. О., Должность</param>
        public EmployeeFromTimeSheet(int employeeNumber, string fullNameAndJobTitle)
        {
            if (employeeNumber == 0)
                throw new ArgumentException("employeeNumber is equals zero");

            EmployeeNumber = employeeNumber;
            FullNameAndJobTitle = fullNameAndJobTitle;

            var split = fullNameAndJobTitle.Split(',');
            FullName = split[0].Trim();
            JobTitle = split[1].Trim();
        }
    }
}