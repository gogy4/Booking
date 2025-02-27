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

        // Обновление описания комнаты
        [HttpPatch("{id}/update-description")]
        public async Task<IActionResult> UpdateDescription(Guid id, [FromForm] string description)
        {
            var room = await roomServices.GetById(id);
            if (room == null)
            {
                return NotFound("Room not found");
            }

            room.ChangeDescription(description);
            await roomServices.UpdateRoom(room);

            return Ok(new { Message = "Room description updated successfully" });
        }

        [HttpPatch("{id}/update-image")]
        public async Task<IActionResult> UpdateImage(Guid id, [FromForm] IFormFile image)
        {
            var room = await roomServices.GetById(id);
            if (room == null)
            {
                return NotFound("Room not found");
            }

            if (image != null && image.Length > 0)
            {
                try
                {
                    var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                    // Ensure directory exists
                    if (!Directory.Exists(imagesPath))
                    {
                        Directory.CreateDirectory(imagesPath);
                    }

                    var filePath = Path.Combine(imagesPath, image.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    var imageUrl = "/images/" + image.FileName;
                    room.ChangeImageUrl(imageUrl);

                    await roomServices.UpdateRoom(room);
                    return Ok(new { Message = "Room image updated successfully" });
                }
                catch (IOException ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Error saving the image", Error = ex.Message });
                }
            }

            return BadRequest("No image provided");
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
