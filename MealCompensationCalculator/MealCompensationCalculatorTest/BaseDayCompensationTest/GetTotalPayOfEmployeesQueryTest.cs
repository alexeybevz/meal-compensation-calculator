using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MealCompensationCalculator.Domain.Models;
using MealCompensationCalculator.Domain.Queries;
using Moq;
using Xunit;

namespace MealCompensationCalculatorTest.BaseDayCompensationTest
{
    public class GetTotalPayOfEmployeesQueryTest
    {
        [Fact]
        public void GetEmployeeTotalPaymentsQueryMockTest()
        {
            var mock = new GetTotalPayOfEmployeesQueryMock().Execute().Object;
            var totalPayOfEmployees = mock.Execute().Result;

            Assert.True(totalPayOfEmployees.EmployeesTotalPayments.SelectMany(x => x.Payments).Sum(x => x.Cost) == 1263);
        }
    }

    internal class GetTotalPayOfEmployeesQueryMock : Mock<IGetTotalPayOfEmployeesQuery>
    {
        public GetTotalPayOfEmployeesQueryMock Execute()
        {
            var employee = new Employee(777, "Фамилия Имя Отчество", "Отдел №1");

            var payments = new List<Payment>()
            {
                new Payment(new DateTime(2017, 10, 2, 12, 39, 0), 78, 0, 78),
                new Payment(new DateTime(2017, 10, 3, 12, 39, 0), 88, 0, 88),
                new Payment(new DateTime(2017, 10, 4, 12, 42, 0),103, 0, 103),
                new Payment(new DateTime(2017, 10, 5, 12, 45, 0), 70, 0, 70),
                new Payment(new DateTime(2017, 10, 6, 12, 41, 0), 100, 0, 100),
                new Payment(new DateTime(2017, 10, 18, 12, 48, 0), 79, 0, 79),
                new Payment(new DateTime(2017, 10, 19, 12, 42, 0), 90, 0, 90),
                new Payment(new DateTime(2017, 10, 20, 12, 45, 0), 83, 0, 83),
                new Payment(new DateTime(2017, 10, 23, 12, 44, 0), 76, 0, 76),
                new Payment(new DateTime(2017, 10, 24, 12, 44, 0), 63, 0, 63),
                new Payment(new DateTime(2017, 10, 25, 12, 40, 0), 97, 0, 97),
                new Payment(new DateTime(2017, 10, 26, 12, 39, 0), 99, 0, 99),
                new Payment(new DateTime(2017, 10, 27, 12, 42, 0), 92, 0, 92),
                new Payment(new DateTime(2017, 10, 30, 12, 38, 0), 70, 0, 70),
                new Payment(new DateTime(2017, 10, 31, 12, 40, 0), 75, 0, 75),
            };

            var employeePayments = new EmployeePayments(employee, payments);
            var totalPayOfEmployees = new TotalPayOfEmployees(new List<EmployeePayments>() { employeePayments });

            Setup(x => x.Execute())
                .Returns(Task.FromResult(totalPayOfEmployees));

            return this;
        }
    }
}