using System;

namespace CDR.DataHolder.Domain.ValueObjects
{
    public class AccountTransactionFilter
    {
        public string AccountId { get; set; }

        public string TransactionId { get; set; }

        public Guid CustomerId { get; set; }


        public override string ToString()
        {
            return $"AccountTransactionFilter(Accountid:{AccountId}, TransactionId:{TransactionId}, CustomerId:{CustomerId})";
        }
    }
}
