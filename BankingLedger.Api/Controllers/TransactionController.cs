using BankingLedger.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BankingLedger.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class TransactionController : ControllerBase
	{
		[HttpPost("deposit")]
		public IActionResult Deposit([FromBody] TransactionInfo transactionInfo)
		{
			if (transactionInfo.Amount <= 0)
				return BadRequest();

			var transaction = new Transaction
			{
				DateTime = DateTime.UtcNow,
				Username = HttpContext.User.Identity.Name,
				Amount = transactionInfo.Amount
			};

			Ledger.Transactions.Add(transaction);

			var account = Ledger.Accounts.Single(a => a.Username == HttpContext.User.Identity.Name);
			account.Balance += transactionInfo.Amount;

			return StatusCode(StatusCodes.Status204NoContent);
		}

		[HttpPost("withdraw")]
		public IActionResult Withdraw([FromBody] TransactionInfo transactionInfo)
		{
			if (transactionInfo.Amount <= 0)
				return BadRequest();

			var account = Ledger.Accounts.Single(a => a.Username == HttpContext.User.Identity.Name);
			if (account.Balance < transactionInfo.Amount)
				return BadRequest();

			var transaction = new Transaction
			{
				DateTime = DateTime.UtcNow,
				Username = HttpContext.User.Identity.Name,
				Amount = -transactionInfo.Amount
			};

			Ledger.Transactions.Add(transaction);
			account.Balance -= transactionInfo.Amount;

			return StatusCode(StatusCodes.Status204NoContent);
		}

		[HttpGet("transactions")]
		public IActionResult GetTransactions(DateTime? from, DateTime? to)
		{
			var transactions = Ledger.Transactions
				.Where(t => t.Username == HttpContext.User.Identity.Name)
				.Where(t => from == default || DateTime.Compare(t.DateTime, ((DateTime)from).ToUniversalTime()) >= 0)
				.Where(t => to == default || DateTime.Compare(t.DateTime, ((DateTime)to).AddSeconds(1).ToUniversalTime()) < 0)
				.OrderBy(t => t.DateTime);

			return Ok(new { transactions });
		}
	}
}