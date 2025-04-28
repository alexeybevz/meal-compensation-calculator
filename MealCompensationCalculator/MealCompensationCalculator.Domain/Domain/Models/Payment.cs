using System;

namespace MealCompensationCalculator.Domain.Domain.Models
{
    public class Payment
    {
        /// <summary>
        /// Дата оплаты
        /// </summary>
        public DateTime TransactionDateTime { get; set; }
        /// <summary>
        /// Стоимость заказа
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// Безналичная оплата
        /// </summary>
        public decimal CostCashless { get; set; }
        /// <summary>
        /// Наличная оплата
        /// </summary>
        public decimal CostCash { get; set; }

        public Payment(DateTime transactionDateTime, decimal cost, decimal costCashless, decimal costCash)
        {
            TransactionDateTime = transactionDateTime;
            Cost = cost;
            CostCashless = costCashless;
            CostCash = costCash;
        }
    }
}