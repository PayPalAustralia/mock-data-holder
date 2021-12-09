using System;
using CDR.DataHolder.Domain.ValueObjects;

namespace CDR.DataHolder.Domain.Entities
{
	public class BalancePurse
	{
		public decimal Amount { get; set; }

		public string Currency { get; set; }
	}
}