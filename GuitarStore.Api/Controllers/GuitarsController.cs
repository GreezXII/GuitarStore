using GuitarStore.Api.Context.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GuitarStore.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GuitarsController : ControllerBase
    {
        [HttpGet]
        [Route("[action]")]
        public ActionResult<Guitar> GetAllGuitars()
        {
            var g = new Guitar() { Name = "Ibanez", Uid = Guid.NewGuid() };
            return Ok(g);
        }
    }
}
