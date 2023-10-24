using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Transaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListTranssaction([FromQuery]GetTransactionPagingRequest request)
        {
            var transactions = await _transactionService.GetListTransaction(request);

            return Ok(new APIResponse<PaginatedResult<TransactionDto>>(transactions, StatusCodes.Status200OK));
        }

        [HttpGet("{transactionId}")]
        public async Task<IActionResult> GetTranssaction([FromRoute]long transactionId)
        {
            var transaction = await _transactionService.GetTransaction(transactionId);

            return Ok(new APIResponse<TransactionDto>(transaction, StatusCodes.Status200OK));
        }
    }
}