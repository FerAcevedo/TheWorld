using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using TheWorldEF.Models;
using TheWorldEF.Services;

namespace TheWorldEF.Controllers.Api
{
    [Authorize]
    [Route("api/trips/{tripName}/stops")]
    public class StopController : Controller
    {
        private IWorldRepository _repository;
        private ILogger<StopController> _logger;
        private CoordService _coordService;

        public StopController(IWorldRepository repository, 
            ILogger<StopController> logger,
            CoordService coordService)
        {
            _repository = repository;
            _logger = logger;
            _coordService = coordService;
        }
        // GET: api/values
        [HttpGet("")]
        public JsonResult Get(string tripName)
        {
            try
            {
                var results = _repository.GetTripByName(tripName, User.Identity.Name);
                if (results == null)
                {
                    return Json(null);
                }
                else if (!results.Stops.Any())
                    return Json("No stops found");
                return Json(Mapper.Map<IEnumerable<StopViewModel>>(results.Stops.OrderBy(s=> s.Order)));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get stops for thip {tripName}", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Error ocurred finding trip name");
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<JsonResult> Post(string tripName, [FromBody]StopViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Map
                    var newStop = Mapper.Map<Stop>(vm);
                    _repository.AddStop(tripName, User.Identity.Name, newStop);

                    //Lookp for  coodenates
                    var coordResult = await _coordService.Lookup(newStop.Name);
                    if (!coordResult.Success)
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json(coordResult.Message);
                    }

                    newStop.Latitude = coordResult.Latitude;
                    newStop.Longitude = coordResult.Longitude;
                    //Save to DB
                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<StopViewModel>(newStop));
                    }
                }
                return Json(null);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save new stop", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Failed to save new stop");
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
