using DotNet8WebApi.SqliteSample.Common;
using Microsoft.AspNetCore.Mvc;

namespace DotNet8WebApi.SqliteSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogV1Controller : ControllerBase
    {
        private readonly SQLiteService _service;

        public BlogV1Controller(SQLiteService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok();
        }
    }
}
