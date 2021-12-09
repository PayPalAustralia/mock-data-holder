using System;
using CDR.DataHolder.Domain.ValueObjects;

namespace CDR.DataHolder.Domain.Entities
{
	public class Balance
	{
		public string AccountId { get; set; }
		public decimal CurrentBalance { get; set; }

		public decimal AvailableBalance { get; set; }

		public decimal CreditLimit { get; set; }
		
		public decimal AmortisedLimit { get; set; }

		public string Currency { get; set; }

		public BalancePurse[] Purses { get; set; }
	}
}