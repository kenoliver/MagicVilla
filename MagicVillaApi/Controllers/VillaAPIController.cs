using System;
using MagicVillaApi.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using MagicVillaApi.Data;
namespace MagicVillaApi.Controllers

{
	[Route("api/[controller]")]
	[ApiController]
	public class VillaAPIController : ControllerBase

	{
		[HttpGet]
		public ActionResult<IEnumerable<VillaDTO>> GetVillas()
		{
			return Ok(VillaStore.villaList);
		}
		[HttpGet("id", Name = "GetVilla")]		//[ProducesResponseType(200, Type = typeof(VillaDTO))]
		//[ProducesResponseType(400)]
  //      [ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
			if (id == 0)
			{
				return BadRequest();
			}
			var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
			if (villa == null)
			{
				return NotFound();
			}
            return Ok(villa);
        }
		[HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
		{
			if(VillaStore.villaList.FirstOrDefault(u=>u.Name.ToLower() == villaDTO.Name.ToLower())!=null)
			{
				ModelState.AddModelError("CustomError", "Villa AlreadyExists");
				return BadRequest(ModelState);
			}

			if (villaDTO == null)
			{
				return BadRequest(villaDTO);
			}
			if (villaDTO.Id > 0)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
			villaDTO.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
			VillaStore.villaList.Add(villaDTO);
			return CreatedAtRoute("GetVilla", new { id = villaDTO.Id },villaDTO);
		}
    }
}

