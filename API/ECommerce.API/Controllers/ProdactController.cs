using ECommerce.BLL.DTO;
using ECommerce.DAL;
using ECommerce.BLL.IRepository;
using ECommerce.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using AutoMapper;
using System.Collections.Generic;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ProdactController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProdactController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> FindProdact(int ID)
        {
            var Prodact = await _unitOfWork.Prodact.GetItemAsync(x => x.ID == ID, new[] { "User" });
            if (Prodact == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(Prodact);
        }
        [HttpGet]
        //[Route(nameof(FindAllProdact))]
        public async Task<IActionResult> FindAllProdact()
        {
            var Prodacts = await _unitOfWork.Prodact.GetAllAsync(new[] { "User" , "ProdactImgs" }, o => o.Title, Constants.OrderBY.Ascending);
            if (Prodacts == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(Prodacts);
        }
        [HttpPost]
        //[Route(nameof(CreateProdact))]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        public async Task<IActionResult> CreateProdact([FromQuery] ProdactDto dto)
        {
            var Entity = _mapper.Map<Prodact>(dto);
            Entity.CreateDate = DateTime.Now;
            Entity.CreateBy = _unitOfWork.User.GetUserID(User); 

            var Prodact = await _unitOfWork.Prodact.AddaAync(Entity);
            if (Prodact == null)
                return BadRequest(Constants.Errors.CreateFailed);
            else
            {
                await _unitOfWork.SaveAsync();
                await _unitOfWork.ProdactImg.AddImgs(Prodact.ID, dto.Files);
                await _unitOfWork.SaveAsync();
              return Ok(Prodact);
            }
        }

        // PUT api/<ProdactController>/5
        [HttpPut]
        //[Route(nameof(UpdateProdact))]
        public async Task<IActionResult> UpdateProdact(int ID, [FromQuery] ProdactDto dto)
        {
            var Prodact = await _unitOfWork.Prodact.FindAsync(ID);

            if (Prodact == null)
                return BadRequest(Constants.Errors.NotFound);
            else
            {
                _mapper.Map(dto, Prodact);
                Prodact.ModifyAt = DateTime.Now;
                Prodact.ModifyBy = _unitOfWork.User.GetUserID(User);

                await _unitOfWork.ProdactImg.AddImgs(Prodact.ID, dto.Files);
                await _unitOfWork.SaveAsync();
                return Ok(Prodact);
            }
        }

        [HttpDelete]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        //[Route(nameof(DeleteProdact))]
        public async Task<IActionResult> DeleteProdact(int ID)
        {
            var Prodact = await _unitOfWork.Prodact.FindAsync(ID);
            if (Prodact == null)
                return NotFound(Constants.Errors.NotFound);
            else
                Prodact.IsDeleted = true;
                await _unitOfWork.SaveAsync();
            return Ok();
        }

        public async Task<IActionResult> AddToChart(int ID)
        {
          var Prodact =  await _unitOfWork.Prodact.FindAsync(ID);
          
            return Ok();
        }
    }
}