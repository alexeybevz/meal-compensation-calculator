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
        /// Стоимость заказа (руб.)
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// Не оплачено (руб.)
        /// </summary>
        public decimal CostUnpaid { get; set; }
        /// <summary>
        /// Оплачено (руб.)
        /// </summary>
        public decimal CostPaid { get; set; }

        public Payment(DateTime transactionDateTime, decimal cost, decimal costUnpaid, decimal costPaid)
        {
            TransactionDateTime = transactionDateTime;
            Cost = cost;
            CostUnpaid = costUnpaid;
            CostPaid = costPaid;
        }
    }
}