using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MealCompensationCalculator.Domain.Domain.Models;
using MealCompensationCalculator.Domain.Domain.Queries;
using Moq;
using Xunit;

namespace MealCompensationCalculator.Test
{
    public class GetTotalPayOfEmployeesQueryTest
    {
        [Fact]
        public void GetEmployeeTotalPaymentsQueryMockTest()
        {
            var mock = new GetTotalPayOfEmployeesQueryMock().Execute().Object;
            var totalPayOfEmployees = mock.Execute().Result;

            Assert.True(totalPayOfEmployees.EmployeesTotalPayments.SelectMany(x => x.Payments).Sum(x => x.Cost) == 78);
        }

        class GetTotalPayOfEmployeesQueryMock : Mock<IGetTotalPayOfEmployeesQuery>
        {
            public GetTotalPayOfEmployeesQueryMock Execute()
            {
                var employee = new Employee(777, "Фамилия Имя Отчество", "Отдел №1");

                var payments = new List<Payment>()
                {
                    new Payment(new DateTime(2017, 10, 2, 12, 39, 0), 78, 0, 78),
                };

                var employeePayments = new EmployeePayments(employee, payments);
                var totalPayOfEmployees = new TotalPayOfEmployees(new List<EmployeePayments>() { employeePayments });

                Setup(x => x.Execute())
                    .Returns(Task.FromResult(totalPayOfEmployees));

                return this;
            }
        }
    }
}