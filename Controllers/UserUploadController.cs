using Microsoft.AspNetCore.Mvc;
using MyFileManager.Controllers.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyFileManager.Controllers
{
    [Route("api/[controller]")] // This will be the URL path
    [ApiController]
    public class UserUploadController(MyDbContext _dbContext) : ControllerBase
    {
        // [HttpPost] // This will be the HTTP method
        [HttpPost("{userId}/")] // you can also add a parameter to the URL
        public async Task<IActionResult> Post([FromForm] FileUploadRequest value, string userId)
        {
            // storing it to wwwroot
            // checking if directory exist
            if (!Directory.Exists("wwwroot"))
            {
                Directory.CreateDirectory("wwwroot");
            }

            var filePath = Path.Combine("wwwroot", value.File.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                value.File.CopyTo(stream);
            }

            // storing it to database
            var user = _dbContext.MyDatas.FirstOrDefault(x => x.Id == Guid.Parse(userId));

            if (user == null)
            {
                return NotFound(); // automatic response 404
            }

            // get this Backend host and port
            var host = $"{Request.Scheme}://{Request.Host}";
            var FilePathURL = $"{host}/{value.File.FileName}";
            _dbContext.SaveChanges();

            // return the URL
            var response = new MyDataResponse
            {
                Id = user.Id,
                Name = user.Name,
                Phone = user.Phone,
                ProfilePicturePath = filePath
            };

            return Ok(response);
        }
    }
}
