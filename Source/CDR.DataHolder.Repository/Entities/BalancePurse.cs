using System.ComponentModel.DataAnnotations;

namespace CDR.DataHolder.Repository.Entities
{
	public class BalancePurse
	{
		[Key, Required]
		public string BankingBalancePurseId { get; set; }

		[Required]
		public decimal Amount { get; set; }

		[MaxLength(3)]
		public string Currency { get; set; }

		public string BalanceId { get; set; }

		public virtual Balance Balance { get; set; }
	}
}
