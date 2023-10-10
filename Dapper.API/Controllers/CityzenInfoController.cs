using Dapper.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Dapper.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityzenInfoController : ControllerBase
    {
        private readonly IConfiguration _congfig;

        public CityzenInfoController(IConfiguration congfig)
        {
            _congfig = congfig;
        }
        [HttpGet]
        public async Task<ActionResult<List<CityzenInfo>>> GetAllCityZen()
        {
            using var con = new SqlConnection(_congfig.GetConnectionString("conn"));
            IEnumerable<CityzenInfo> cityzen = await SelectAllCityZen(con);
            return Ok(cityzen);
        }
        [HttpGet("{cityzenId}")]
        public async Task<ActionResult<List<CityzenInfo>>> GetAllCityZen(int cityzenId)
        {
            using var con = new SqlConnection(_congfig.GetConnectionString("conn"));
            var cityzenbyId = await con.QueryFirstAsync<CityzenInfo>("Select * from CityzenInfo where id =@Id",new { Id = cityzenId });
            return Ok(cityzenbyId);
        }   
        [HttpPost]
        public async Task<ActionResult<List<CityzenInfo>>> CreateCityZen(CityzenInfo cityzeninfo)
        {
            using var con = new SqlConnection(_congfig.GetConnectionString("conn"));
            await con.ExecuteAsync("Insert into CityzenInfo (name,firstName,lastname,place) Values (@Name,@FirstName,@LastName,@Place)",cityzeninfo);
            return Ok(await SelectAllCityZen(con));
        }
        
        [HttpPut]
        public async Task<ActionResult<List<CityzenInfo>>> UpdateCityZen(CityzenInfo cityzeninfo)
        {
            using var con = new SqlConnection(_congfig.GetConnectionString("conn"));
            await con.ExecuteAsync("Update CityzenInfo Set name =@Name,firstName =@FirstName,lastname = @LastName,place=@Place where id =@Id",cityzeninfo);
            return Ok(await SelectAllCityZen(con));
        }
         
        [HttpDelete("{cityzenId}")]
        public async Task<ActionResult<List<CityzenInfo>>> DeleteCityZen(int cityzenId)
        {
            using var con = new SqlConnection(_congfig.GetConnectionString("conn"));
            await con.ExecuteAsync("Delete from CityzenInfo where id =@Id", new {Id = cityzenId });
            return Ok(await SelectAllCityZen(con));
        }

        private static async Task<IEnumerable<CityzenInfo>> SelectAllCityZen(SqlConnection com)
        {
            return await com.QueryAsync<CityzenInfo>("Select * from CityzenInfo");
        }
    }
}
