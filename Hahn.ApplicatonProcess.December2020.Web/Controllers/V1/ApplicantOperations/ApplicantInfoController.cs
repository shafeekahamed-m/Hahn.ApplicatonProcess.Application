using Hahn.ApplicatonProcess.December2020.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hahn.ApplicatonProcess.December2020.Domain.ApplicantOperations.Interface;
using Hahn.ApplicatonProcess.December2020.Domain.ApplicantOperations.Models;
using System.Net.Http;
using System.Net;
using Serilog;
using System.Web;

namespace Hahn.ApplicatonProcess.December2020.Web.Controllers.V1.ApplicantOperations
{
    [Produces("application/json")]
    //[ProducesResponseType]
    [ApiController]
    //[ApiVersion("1")]
    //[Route("v{api-version:apiVersion}/applicant}")]
    [Route("[controller]")]
    public class ApplicantInfoController : Controller
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        //private readonly ApiContext _context;
        private readonly ILogger<ApplicantInfoController> _logger;
        private readonly IApplicantService _service;

        public ApplicantInfoController(ILogger<ApplicantInfoController> logger, IApplicantService service)
        {
            _service = service;
            //_context = context;
            _logger = logger;
        }
        /// <summary>
        /// Adds an Applicant item
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Adds
        ///     {
        ///         "Name":"shafeekahamed",
        ///         "FamilyName":"shafeek",
        ///         "Address":"shafeek,11111",
        ///         "CountryOfOrigin":"india",
        ///         "EMailAddress":"shafeek@g.com",
        ///         "Age":26,
        ///         "Hired":false
        ///      }
        ///
        /// </remarks>
        /// <param name="objApplicant"></param>
        /// <returns>Returns the newly Added item</returns>
        /// <response code="201">Returns the newly Added item</response>
        /// <response code="400">If the item is null</response> 
        [ProducesResponseType(typeof(int), 201)]
        [ProducesResponseType(typeof(Message), 400)]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Route("save")]
        public ActionResult Applicant([FromBody] Applicant objApplicant)
        {
            //var baseUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~")
            //var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            //var chck= HttpUtility.ParseQueryString(Request.Url.Query);
            try
            {
                var check = _service.ApplicantValidate(objApplicant, false);
                if (check.Code > 0)
                {
                    int idReturn = _service.SaveApplicant(objApplicant);
                    return Created("https://localhost:44348/ApplicantInfo/" + idReturn, idReturn);
                }
                return BadRequest(check.Messages);
            }
            catch (Exception ex)
            {
                Log.Information("Exception: " + ex.Message);
                return BadRequest();
            }


            //return BadRequest("https://localhost:44348/ApplicantInfo/1", _service.SaveApplicant(objApplicant));
            ////var check = GetUser();
            //_context.Applicants.Add(new Applicant {
            //    ID = _context.Applicants.Max(x=>x.ID)+1,
            //    Name = objApplicant.Name,
            //    FamilyName = objApplicant.FamilyName,
            //    Address = objApplicant.Address,
            //    CountryOfOrigin = objApplicant.CountryOfOrigin,
            //    EMailAddress = objApplicant.EMailAddress,
            //    Age = objApplicant.Age,
            //    Hired = objApplicant.Hired,
            //});
            //_context.SaveChanges();
            ////var rng = new Random();
            ////return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            ////{
            ////    Date = DateTime.Now.AddDays(index),
            ////    TemperatureC = rng.Next(-20, 55),
            ////    Summary = Summaries[rng.Next(Summaries.Length)]
            ////})
            ////.ToArray();
            //return _context.Users.ToList();service.SaveApplicant(objApplicant)

        }
        /// <summary>
        /// Updates an Applicant item
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Update
        ///     {
        ///         "ID":1,
        ///         "Name":"Luke",
        ///         "FamilyName":"Skywalker",
        ///         "Address":"144,Blecker st.",
        ///         "CountryOfOrigin":"Germany",
        ///         "EMailAddress":"Luke_skywalker@gmail.com",
        ///         "Age":26,
        ///         "Hired":true
        ///      }
        ///
        /// </remarks>
        /// <param name="objApplicant"></param>
        /// <returns>Returns 1 for Update success and -1 for Id not found (or) not successful</returns>
        /// <response code="201">Returns 1 for Update success</response>
        /// <response code="400">Returns -1 for Id not found (or) not successful</response> 
        [ProducesResponseType(typeof(int), 201)]
        [ProducesResponseType(typeof(Message), 400)]
        [HttpPut]
        [Route("save")]
        public ActionResult UpdateApplicant([FromBody] Applicant objApplicant)
        {
            ////var check = GetUser();
            //_context.Applicants.Add(new Applicant {
            //    ID = _context.Applicants.Max(x=>x.ID)+1,
            //    Name = objApplicant.Name,
            //    FamilyName = objApplicant.FamilyName,
            //    Address = objApplicant.Address,
            //    CountryOfOrigin = objApplicant.CountryOfOrigin,
            //    EMailAddress = objApplicant.EMailAddress,
            //    Age = objApplicant.Age,
            //    Hired = objApplicant.Hired,
            //});
            //_context.SaveChanges();
            ////var rng = new Random();
            ////return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            ////{
            ////    Date = DateTime.Now.AddDays(index),
            ////    TemperatureC = rng.Next(-20, 55),
            ////    Summary = Summaries[rng.Next(Summaries.Length)]
            ////})
            ////.ToArray();
            //return _context.Users.ToList();service.SaveApplicant(objApplicant)
            try
            {
                var check = _service.ApplicantValidate(objApplicant, true);
                if (check.Code > 0)
                {
                    int idReturn = _service.UpdateApplicant(objApplicant);
                    return Created("https://localhost:44348/ApplicantInfo/" + idReturn, idReturn);
                }
                return BadRequest(check.Messages);
            }
            catch(Exception ex)
            {
                Log.Information("Exception: " + ex.Message);
                return BadRequest();
            }
            
        }
        
        /// <summary>
        /// Fetches an Applicant item based on Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Returns 1 on successful deletion </returns>
        /// <response code="201">Returns the applicant object on match found </response>
        /// <response code="400">Returns 400 If the item is null or does not match any applicant item</response>
        [ProducesResponseType(typeof(Applicant), 201)]
        [ProducesResponseType(typeof(Message), 400)]
        [HttpGet("{Id}")]   
        //[Route("weather")]
        public ActionResult Applicant(int Id)
        {
            //var check = GetUser();
            //_context.Users.Add(new Domain.ApplicantOperations.Models.User
            //{
            //    Id = "shafeek",
            //    FirstName = "ahamed",
            //    LastName = "one"
            //});
            //_context.SaveChanges();
            try
            {
                var returnObj = _service.GetApplicant(Id);
                if (returnObj.Count() > 0)
                {
                    return Ok(returnObj);
                }
                Message objMessage = new Message();
                objMessage.Code = 400;
                objMessage.Messages.Add("Id :" + Id + " Not found");
                return BadRequest(objMessage);
            }
            catch (Exception ex)
            {
                Log.Information("Exception: " + ex.Message);
                return BadRequest();
            }
            
            //_context.Users.Where(u => u.Id == name).ToList();

            //return response;
        }
        /// <summary>
        /// Deletes an Applicant item based on Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Returns 1 on successful deletion </returns>
        /// <response code="201">Returns code 201 on successful deletion </response>
        /// <response code="400">Returns code 400 If the Id is null or does not match any applicant item</response>
        [ProducesResponseType(typeof(int), 201)]
        [ProducesResponseType(typeof(Message), 400)]
        [HttpDelete("{Id}")]
        //[Route("weather")]
        public ActionResult DeleteApplicant(int Id)
        {
            //var check = GetUser();
            //_context.Users.Add(new Domain.ApplicantOperations.Models.User
            //{
            //    Id = "shafeek",
            //    FirstName = "ahamed",
            //    LastName = "one"
            //});
            //_context.SaveChanges();
            try
            {
                int returnVal = _service.DeleteApplicant(Id);
                Message objMessage = new Message();
                if (returnVal == 1)
                {
                    objMessage.Code = 201;
                    objMessage.Messages.Add("Success");
                    return Ok(returnVal);
                }
                objMessage.Code = 400;
                objMessage.Messages.Add("Id : " + Id + " Not found");
                return BadRequest(returnVal);
            }
            catch (Exception ex)
            {
                Log.Information("Exception: " + ex.Message);
                return BadRequest();
            }
            
            //_context.Users.Where(u => u.Id == name).ToList();

            //return response;
        }
        /// <summary>
        /// Fetches all applicants.
        /// </summary>
        /// <returns>Returns 1 on successful deletion </returns>
        /// <response code="201">Returns the applicant objects </response>
        /// <response code="400">Returns 400 If no applicant found</response>
        [ProducesResponseType(typeof(Applicant), 201)]
        [ProducesResponseType(typeof(Message), 400)]
        [HttpGet()]
        //[Route("weather")]
        public ActionResult Applicant()
        {
            //var check = GetUser();
            //_context.Users.Add(new Domain.ApplicantOperations.Models.User
            //{
            //    Id = "shafeek",
            //    FirstName = "ahamed",
            //    LastName = "one"
            //});
            //_context.SaveChanges();
            try
            {
                var returnObj = _service.GetAllApplicant();
                if (returnObj.Count() > 0)
                {
                    return Ok(returnObj);
                }
                Message objMessage = new Message();
                objMessage.Code = 400;
                objMessage.Messages.Add(" Not found");
                return BadRequest(objMessage);
            }
            catch (Exception ex)
            {
                Log.Information("Exception: " + ex.Message);
                return BadRequest();
            }

            //_context.Users.Where(u => u.Id == name).ToList();

            //return response;
        }
        //public async Task<IActionResult> GetUser()
        //{
        //    var users = await _context.Users
        //        .Include(u => u.Posts)
        //        .ToArrayAsync();

        //    var response = users.Select(u => new
        //    {
        //        firstName = u.FirstName,
        //        lastName = u.LastName,
        //        posts = u.Posts.Select(p => p.Content)
        //    });

        //    return Ok(response);
        //}
    }
}
