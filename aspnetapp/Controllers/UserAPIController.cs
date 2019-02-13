using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aspnetapp.Models;
using MySql.Data.MySqlClient;  
using aspnetapp.Utils;
using aspnetapp.Dao;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace aspnetapp.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller 
    {
      private readonly ILogger _logger;
      public UserController(ILogger<UserController> logger) {
          _logger = logger;
      }
      [HttpGet]  
      public async Task<IActionResult> List()
        {
            using (var db = AppDb.GetAppDb())
            {
                if (db == null || db.Connection == null) {
                    _logger.LogError("Mysql db can not create");
                    return ResultUtil.GetFailureResult(StatusCodes.Status500InternalServerError, "Do not found mysql instance");
                }
                var query = new UserInfoQuery(db);
                var result = await query.LatestPostsAsync();
                return ResultUtil.GetSuccessResult(result);
            }
        }  
       [HttpDelete("{id}")] 
      public async Task<IActionResult> Delete(int id)
        {
            using (var db = AppDb.GetAppDb())
            {
                if (db == null) {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Do not found mysql instance");
                }
                await db.Connection.OpenAsync();
                var query = new UserInfoQuery(db);
                var result = await query.FindOneAsync(id);
                if (result == null)
                    return new NotFoundResult();
                await result.DeleteAsync();
                return new OkResult();
            }
        } 
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]UserInfo body)
        {
             using (var db = AppDb.GetAppDb())
            {
                if (db == null) {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Do not found mysql instance");
                }
                body.Db = db;
                await body.InsertAsync();
                return new OkObjectResult(body);
            }
        } 
        [HttpPut("{id}")]
         public async Task<IActionResult> Update(int id, [FromBody]UserInfo body)
        {
            using (var db = AppDb.GetAppDb())
            {
                if (db == null) {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Do not found mysql instance");
                }
                var query = new UserInfoQuery(db);
                var result = await query.FindOneAsync(id);
                if (result == null)
                    return new NotFoundResult();
                result.Name = body.Name;
                result.Address = body.Address;
                result.Age = body.Age;
                result.Gender = body.Gender;
                await result.UpdateAsync();
                return new OkObjectResult(result);
            }
        } 
        [HttpGet("{id}")]
         public async Task<IActionResult> Get(int id)
        {
            using (var db = AppDb.GetAppDb())
            {
                if (db == null) {
                    return ResultUtil.GetFailureResult(StatusCodes.Status500InternalServerError, "Do not found mysql instance");
                }
                var query = new UserInfoQuery(db);
                var result = await query.FindOneAsync(id);
                if (result == null)
                    return ResultUtil.GetFailureResult(StatusCodes.Status404NotFound,"user not found");
                return ResultUtil.GetSuccessResult(result);
            }
        }   
    }
}
