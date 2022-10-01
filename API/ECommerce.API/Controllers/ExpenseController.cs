using ECommerce.BLL.DTO;
using ECommerce.BLL.IRepository;
using ECommerce.DAL;
using ECommerce.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExpenseController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        //[Route(nameof(FindExpense))]
        public async Task<IActionResult> FindExpense(int ID)
        {
            var Expense = await _unitOfWork.Expense.FindAsync(ID);
            if(Expense == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(Expense);
        }
        [HttpGet]
       // [Route(nameof(FindAllExpense))]
        public async Task<IActionResult> FindAllExpense()
        {
            var Expenses = await _unitOfWork.Expense.GetAllAsync();
            if (Expenses == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(Expenses);
        }
        [HttpPost]
        //[Route(nameof(CreateExpense))]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        public async Task<IActionResult>CreateExpense([FromForm]ExpenseDto dto)
        {
            var Mapping = _mapper.Map<Expense>(dto);
            Mapping.CreateDate = DateTime.Now;
            Mapping.CreateBy = _unitOfWork.User.GetUserID(User);

            var Expense = await _unitOfWork.Expense.AddaAync(Mapping);
            if (Expense == null)
                return BadRequest(Constants.Errors.CreateFailed);
            else
                await _unitOfWork.SaveAsync();
               return Ok(Expense);
        }

        // PUT api/<ExpenseController>/5
        [HttpPut]
        //[Route(nameof(UpdateExpense))]
        public async Task<IActionResult> UpdateExpense(int ID, [FromForm] ExpenseDto dto)
        {
            var Expense = await _unitOfWork.Expense.FindAsync(ID);
            if (Expense ==null)
                return BadRequest(Constants.Errors.NotFound);
            else
            {
                _mapper.Map(dto, Expense);
                Expense.ModifyAt = DateTime.Now;
                Expense.ModifyBy = _unitOfWork.User.GetUserID(User);
                await _unitOfWork.SaveAsync();
                return Ok(Expense);
            }
        }

        [HttpDelete]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        //[Route(nameof(DeleteExpense))]
        public async Task<IActionResult> SetAvtiveExpense(int ID)
        {
            var Expense = await _unitOfWork.Expense.FindAsync(ID);
            if (Expense == null)
                return NotFound(Constants.Errors.NotFound);
            else
                _unitOfWork.Expense.Delete(Expense);
            await _unitOfWork.SaveAsync();
            return Ok();
        }
    }
}
