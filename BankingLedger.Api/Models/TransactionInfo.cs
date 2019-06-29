using System.ComponentModel.DataAnnotations;

namespace BankingLedger.Api.Models
{
	public class TransactionInfo
	{
		[Range(0, double.MaxValue)]
		public decimal Amount { get; set; }
	}
}
