using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CDR.DataHolder.Repository.Entities
{
	public class Balance
	{
		[Key, Required]
		public string BalanceId { get; set; }

		[Required]
		public decimal CurrentBalance { get; set; }

		[Required]
		public decimal AvailableBalance { get; set; }

		public decimal CreditLimit { get; set; }
		
		public decimal AmortisedLimit { get; set; }

		[MaxLength(3)]
		public string Currency { get; set; }

		public virtual ICollection<BalancePurse> Purses { get; set; }

		public string AccountId { get; set; }
		
		public virtual Account Account { get; set; }
	}
}
