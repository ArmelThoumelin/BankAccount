using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Application.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankOperationController : ControllerBase
    {
        private readonly ILogger<BankOperationController> _logger;
        private readonly IBank _Bank;

        public BankOperationController(ILogger<BankOperationController> logger, IBank bank)
        {
            _logger = logger;
            _Bank = bank;
        }

        [HttpGet("AccountExists/{idAccount}")]
        public async Task<IActionResult> AccountExists(long idAccount)
        {
            try
            {
                var accountExists = await _Bank.AccountExists(idAccount);

                var result = accountExists?.Result == Domain.Models.AccountExistsResult.AccountStatus.Ok;

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem(ex.Message);
            }
        }

        [HttpGet("GetBalance/{idAccount}")]
        public async Task<IActionResult> GetBalance(long idAccount)
        {
            try
            {
                var result = await _Bank.GetBalance(idAccount);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem(ex.Message);
            }
        }

        [HttpGet("GetTransactions")]
        public async Task<IActionResult> GetTransactions(long idAccount, DateTime startDate, DateTime? endDate)
        {
            try
            {
                var historyDemand = new HistoryDemand()
                {
                    IdAccount = idAccount,
                    StartDate = startDate,
                    EndDate = endDate ?? DateTime.Now
                };

                var result = await _Bank.GetTransactions(historyDemand);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem(ex.Message);
            }
        }

        [HttpPost("AddDeposit/{idAccount}/{amount}")]
        public async Task<IActionResult> AddDeposit(long idAccount, decimal amount)
        {
            try
            {
                var depositAmount = new DepositAmount(amount);
                var depositDemand = new DepositDemand()
                {
                    IdAccount = idAccount,
                    Amount = depositAmount,
                    TransactionDate = DateTime.Now
                };

                var result = await _Bank.AddDeposit(depositDemand);

                return Ok(new { Result = result.Result.ToString(), Message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem(ex.Message);
            }
        }

        [HttpPost("AddWithdrawal/{idAccount}/{amount}")]
        public async Task<IActionResult> AddWithdrawal(long idAccount, decimal amount)
        {
            try
            {
                var withdrawalAmount = new WithdrawalAmount(amount);
                var withdrawalDemand = new WithdrawalDemand()
                {
                    IdAccount = idAccount,
                    Amount = withdrawalAmount,
                    TransactionDate = DateTime.Now
                };

                var result = await _Bank.AddWithdrawal(withdrawalDemand);

                return Ok(new { Result = result.Result.ToString(), Message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem(ex.Message);
            }
        }
    }
}
