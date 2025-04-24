using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MealCompensationCalculator.Domain.Models;
using MealCompensationCalculator.Domain.Queries;
using Moq;
using Xunit;

namespace MealCompensationCalculatorTest
{
    public class GetEmployeeTotalPaymentsTest
    {
        [Fact]
        public void GetEmployeeTotalPaymentsQueryMockTest()
        {
            var mock = new GetEmployeeTotalPaymentsQueryMock().Execute().Object;
            var data = mock.Execute().Result;

            Assert.True(data.SelectMany(x => x.Payments).Sum(x => x.Cost) == 1263);
        }
    }

    internal class GetEmployeeTotalPaymentsQueryMock : Mock<IGetEmployeeTotalPaymentsQuery>
    {
        public GetEmployeeTotalPaymentsQueryMock Execute()
        {
            var employee = new Employee()
            {
                EmployeeNumber = 1344,
                FullName = "Бевз Алексей Сергеевич",
                Department = "Отдел №14"
            };

            var payments = new List<Payment>()
            {
                new Payment()
                {
                    TransactionDateTime = new DateTime(2017, 10, 2, 12, 39, 0),
                    Cost = 78,
                    CostCash = 0,
                    CostCashless = 78
                },
                new Payment()
                {
                    TransactionDateTime = new DateTime(2017, 10, 3, 12, 39, 0),
                    Cost = 88,
                    CostCash = 0,
                    CostCashless = 88
                },
                new Payment()
                {
                    TransactionDateTime = new DateTime(2017, 10, 4, 12, 42, 0),
                    Cost = 103,
                    CostCash = 0,
                    CostCashless = 103
                },
                new Payment()
                {
                    TransactionDateTime = new DateTime(2017, 10, 5, 12, 45, 0),
                    Cost = 70,
                    CostCash = 0,
                    CostCashless = 70
                },
                new Payment()
                {
                    TransactionDateTime = new DateTime(2017, 10, 6, 12, 41, 0),
                    Cost = 100,
                    CostCash = 0,
                    CostCashless = 100
                },
                new Payment()
                {
                    TransactionDateTime = new DateTime(2017, 10, 18, 12, 48, 0),
                    Cost = 79,
                    CostCash = 0,
                    CostCashless = 79
                },
                new Payment()
                {
                    TransactionDateTime = new DateTime(2017, 10, 19, 12, 42, 0),
                    Cost = 90,
                    CostCash = 0,
                    CostCashless = 90
                },
                new Payment()
                {
                    TransactionDateTime = new DateTime(2017, 10, 20, 12, 45, 0),
                    Cost = 83,
                    CostCash = 0,
                    CostCashless = 83
                },
                new Payment()
                {
                    TransactionDateTime = new DateTime(2017, 10, 23, 12, 44, 0),
                    Cost = 76,
                    CostCash = 0,
                    CostCashless = 76
                },
                new Payment()
                {
                    TransactionDateTime = new DateTime(2017, 10, 24, 12, 44, 0),
                    Cost = 63,
                    CostCash = 0,
                    CostCashless = 63
                },
                new Payment()
                {
                    TransactionDateTime = new DateTime(2017, 10, 25, 12, 40, 0),
                    Cost = 97,
                    CostCash = 0,
                    CostCashless = 97
                },
                new Payment()
                {
                    TransactionDateTime = new DateTime(2017, 10, 26, 12, 39, 0),
                    Cost = 99,
                    CostCash = 0,
                    CostCashless = 99
                },
                new Payment()
                {
                    TransactionDateTime = new DateTime(2017, 10, 27, 12, 42, 0),
                    Cost = 92,
                    CostCash = 0,
                    CostCashless = 92
                },
                new Payment()
                {
                    TransactionDateTime = new DateTime(2017, 10, 30, 12, 38, 0),
                    Cost = 70,
                    CostCash = 0,
                    CostCashless = 70
                },
                new Payment()
                {
                    TransactionDateTime = new DateTime(2017, 10, 31, 12, 40, 0),
                    Cost = 75,
                    CostCash = 0,
                    CostCashless = 75
                },
            };

            Setup(x => x.Execute())
                .Returns(Task.FromResult<IEnumerable<EmployeeTotalPayment>>(
                    new List<EmployeeTotalPayment>() { new EmployeeTotalPayment(employee, payments) }));

            return this;
        }
    }
}