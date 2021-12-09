namespace CDR.DataHolder.Resource.API.Business.Models
{
    public class BankingBalance
    {
		public string AccountId { get; set; }
		
		public string CurrentBalance { get; set; }

		public string AvailableBalance { get; set; }

		public string CreditLimit { get; set; }
		
		public string AmortisedLimit { get; set; }

		public string Currency { get; set; }

		public BankingBalancePurse[] Purses { get; set; }
	}
}
