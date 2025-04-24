using System;

namespace MealCompensationCalculator.Domain.Models
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
    }
}