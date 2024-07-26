using Application.API.Dto;
using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Application.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class BankOperationController : ControllerBase
    {
        private readonly ILogger<BankOperationController> _logger;
        private readonly IBank _Bank;

        public BankOperationController(ILogger<BankOperationController> logger, IBank bank)
        {
            _logger = logger;
            _Bank = bank;
        }

        [HttpGet("AccountExists")]
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

        [HttpGet("GetBalance")]
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
        public async Task<IActionResult> GetTransactions([FromQuery] GetTransactionsRequest request)
        {
            try
            {
                var historyDemand = new HistoryDemand()
                {
                    IdAccount = request.IdAccount,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate ?? DateTime.Now
                };

                var result = await _Bank.GetTransactions(historyDemand);
                return result.Result switch
                {
                    HistoryResult.HistoryStatus.Ok => Ok(result),
                    HistoryResult.HistoryStatus.InvalidDateRange => BadRequest(new { Message = result.Message }),
                    HistoryResult.HistoryStatus.UnknownAccount => BadRequest(new { Message = result.Message }),
                    _ => Problem(detail: result.Message)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem(ex.Message);
            }
        }

        [HttpPost("AddDeposit")]
        public async Task<IActionResult> AddDeposit([FromBody] AddDepositRequest request)
        {
            try
            {
                var depositAmount = new DepositAmount(request.Amount);
                var depositDemand = new DepositDemand()
                {
                    IdAccount = request.IdAccount,
                    Amount = depositAmount,
                    TransactionDate = DateTime.Now
                };

                var result = await _Bank.AddDeposit(depositDemand);
                return result.Result switch
                {
                    TransactionResult.TransactionStatus.Ok => Ok(new { Result = result.Result.ToString(), Message = result.Message }),
                    TransactionResult.TransactionStatus.UnknownAccount => BadRequest(new { Message = result.Message }),
                    _ => Problem(detail: result.Message)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem(ex.Message);
            }
        }

        [HttpPost("AddWithdrawal")]
        public async Task<IActionResult> AddWithdrawal([FromBody] AddWithdrawalRequest request)
        {
            try
            {
                var withdrawalAmount = new WithdrawalAmount(request.Amount);
                var withdrawalDemand = new WithdrawalDemand()
                {
                    IdAccount = request.IdAccount,
                    Amount = withdrawalAmount,
                    TransactionDate = DateTime.Now
                };

                var result = await _Bank.AddWithdrawal(withdrawalDemand);
                return result.Result switch
                {
                    TransactionResult.TransactionStatus.Ok => Ok(new { Result = result.Result.ToString(), Message = result.Message }),
                    TransactionResult.TransactionStatus.UnknownAccount => BadRequest(new { Message = result.Message }),
                    TransactionResult.TransactionStatus.InsufficientFunds => BadRequest(new { Message = result.Message }),
                    _ => Problem(detail: result.Message)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem(ex.Message);
            }
        }
    }
}
