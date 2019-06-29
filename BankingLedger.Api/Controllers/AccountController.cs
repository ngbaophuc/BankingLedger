using BankingLedger.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BankingLedger.Api.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
		private readonly IConfiguration _config;

		public AccountController(IConfiguration config)
		{
			_config = config;
		}

		[HttpPost("signup")]
		public IActionResult Signup([FromBody] SignupInfo signupInfo)
		{
			var account = Ledger.Accounts.SingleOrDefault(a => a.Username == signupInfo.Username.ToLower());

			if (account != null)
				return BadRequest("Username is already registered.");

			CreatePasswordHash(signupInfo.Password, out byte[] passwordHash, out byte[] passwordSalt);

			account = new Account
			{
				Username = signupInfo.Username.ToLower(),
				FirstName = signupInfo.FirstName,
				LastName = signupInfo.LastName,
				PasswordHash = passwordHash,
				PasswordSalt = passwordSalt
			};

			Ledger.Accounts.Add(account);

			return StatusCode(StatusCodes.Status201Created);
		}

		[HttpPost("signin")]
		public IActionResult Signin([FromBody] SigninInfo signinInfo)
		{
			var account = Ledger.Accounts.SingleOrDefault(a => a.Username == signinInfo.Username.ToLower());

			if (account == null)
				return Unauthorized();

			if (!VerifyPasswordHash(signinInfo.Password, account.PasswordHash, account.PasswordSalt))
				return Unauthorized();

			var claims = new[]
			{
				new Claim(ClaimTypes.Name, account.Username)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:SecretKey").Value));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(1),
				SigningCredentials = credentials
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);

			return Ok(new { token = tokenHandler.WriteToken(token) });
		}

		[HttpGet("balance")]
		[Authorize]
		public IActionResult GetBalance()
		{
			var account = Ledger.Accounts.Single(a => a.Username == HttpContext.User.Identity.Name);

			return Ok(new { balance = account.Balance });
		}

		[HttpGet("user_profile")]
		[Authorize]
		public IActionResult GetUserProfile()
		{
			var account = Ledger.Accounts.Single(a => a.Username == HttpContext.User.Identity.Name);

			return Ok(new { username = account.Username, firstName = account.FirstName, lastName = account.LastName });
		}

		private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			using (var hmac = new HMACSHA512(passwordSalt))
			{
				var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

				for (var i = 0; i < computedHash.Length; i++)
				{
					if (computedHash[i] != passwordHash[i]) return false;
				}
			}

			return true;
		}

		private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using (var hmac = new HMACSHA512())
			{
				passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
				passwordSalt = hmac.Key;
			}
		}
    }
}