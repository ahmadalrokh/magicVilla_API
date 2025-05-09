using magicVilla_VillaAPI.Data;
using magicVilla_VillaAPI.Models;
using magicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace magicVilla_VillaAPI.Controllers
{
    //To giv the api a rout 
    //[Route("Api/[controller]")]
    [Route("api/villaApi")]
    //That requred To work with api
    [ApiController]
    public class VillaApiControllers : ControllerBase
    {
        [HttpGet]//that requred you till the api that this endPoit is get
        [ProducesResponseType(200)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas() => Ok(VillaStore.villaList);// Status Code// 1 - ok// 2 - Badrequset// 3 - NotFound
        [HttpGet("id:int", Name = "GetVilla")]
        //her we can define what are the multiple response type that can be produce 
        //[ProducesResponseType(200, Type:typeof(VillaDto))]  //OK and the return type is VillaDto
        [ProducesResponseType(200)]//OK
        [ProducesResponseType(400)]//Bad Request
        [ProducesResponseType(404)]//Not Found
        public ActionResult<VillaDto> GetVilla(int id)
        {

            if (id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            return Ok(villa);

        }
        [HttpPost]
        [ProducesResponseType(201)]//Crated
        [ProducesResponseType(400)]//Bad Request
        [ProducesResponseType(500)]//Servar erorr
        public ActionResult<VillaDto> CreatVilla([FromBody] VillaDto villa)
        {
            // if you want to add error state
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //Custom Validation
            if (VillaStore.villaList.FirstOrDefault(v => v.Name.ToLower() == villa.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "This Name Alerdy Exisit!");
                return BadRequest(ModelState);
            }
            if (villa == null)
            {
                return BadRequest();
            }
            if (villa.Id > 0)
            {
                return StatusCode(500);
            }
            villa.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villa);
            //return Ok(villa);
            //Like Redirect To Action
            return CreatedAtRoute("GetVilla", new { id = villa.Id }, villa);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        //the diffrence between the IActionResalt And ActionResalt is ReturnValue 
        [ProducesResponseType(204)]//No Content
        [ProducesResponseType(400)]//Bad Request
        [ProducesResponseType(404)]//Not found
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            VillaStore.villaList.Remove(villa);
            //return Ok(villa);
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(204)]//No Content
        [ProducesResponseType(400)]//Bad Request
        [ProducesResponseType(404)]//Not found
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villa)
        {
            if (id == 0 || villa.Id != id)
            {
                return BadRequest();
            }
            var villaFromStor = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            if (villaFromStor == null)
                return NotFound();

            villaFromStor.Name = villa.Name;
            villaFromStor.Occupancy = villa.Occupancy;
            villaFromStor.Sqft = villa.Sqft;

            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(204)]//No Content
        [ProducesResponseType(400)]//Bad Request
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDto> patchDTO)
        {
            if (id == 0 || patchDTO == null)
            {
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(villa => villa.Id == id);

            if (villa == null)
                return NotFound();

            patchDTO.ApplyTo(villa, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
            // Path = "What you whant to update name or sqflt or ...."
            // op ="replace"
            // value ="your now value"
        }
        // on video in 1h.26m he explane how to use postman 
    }
}
    
