using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly RoomServices roomServices;

        public RoomController(RoomServices roomServices)
        {
            this.roomServices = roomServices;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var room = await roomServices.GetById(id);
            if (room == null) return NotFound("Room not found");
            return Ok(room);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Room roomDto)
        {
            var room = await roomServices.CreateRoom(roomDto.Number, roomDto.RoomType, roomDto.PricePerNight, roomDto.Description, roomDto.ImageUrl);
            return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
        }

        [HttpPatch("{id}/update-short-description")]
        public async Task<IActionResult> UpdateShortDescription(Guid id, [FromForm] string description)
        {
            var room = await roomServices.GetById(id);
            if (room == null)
            {
                return NotFound("Room not found");
            }

            room.ChangeShortDescription(description);
            await roomServices.UpdateRoom(room);

            return Ok(new { Message = "Room description updated successfully" });
        }
        
        [HttpPatch("{id}/update-full-description")]
        public async Task<IActionResult> UpdateFullDescription(Guid id, [FromForm] string description)
        {
            var room = await roomServices.GetById(id);
            if (room == null)
            {
                return NotFound("Room not found");
            }

            room.ChangeFullDescription(description);
            await roomServices.UpdateRoom(room);

            return Ok(new { Message = "Room description updated successfully" });
        }

        [HttpPatch("{id}/update-image")]
        [Consumes("multipart/form-data")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateImage(Guid id, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Файл не загружен");
            }

            var room = await roomServices.GetById(id);
            if (room == null)
            {
                return NotFound("Room not found");
            }

            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            var filePath = Path.Combine(uploads, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            room.ChangeImageUrl("/images/" + file.FileName);
            await roomServices.UpdateRoom(room);

            return Ok(new { Message = "Room image updated successfully", ImageUrl = room.ImageUrl });
        }




        // Обновление цены за ночь
        [HttpPatch("{id}/change-price")]
        public async Task<IActionResult> ChangePricePerNight(Guid id, [FromBody] int newPrice)
        {
            var room = await roomServices.GetById(id);
            if (room == null)
            {
                return NotFound("Room not found");
            }

            await roomServices.ChangePricePerNight(room, newPrice);
            return NoContent();
        }
    }
}
