using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GameApplication.Models;

namespace GameApplication.Controllers
{
    public class RaceDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all races
        /// </summary>
        /// <returns>
        /// Content: all races in the db, including associated race
        /// </returns>
        /// <example>
        /// GET: api/RaceData/ListRace
        /// </example>
        [HttpGet]
        [ResponseType(typeof(RaceDto))]
        public IHttpActionResult ListRaces()
        {
            List<Race> Races = db.Races.ToList();
            List<RaceDto> RaceDtos = new List<RaceDto>();

            Races.ForEach(r => RaceDtos.Add(new RaceDto()
            {
                RaceID = r.RaceID,
                RaceName = r.RaceName,
                RaceOffensive = r.RaceOffensive
            }));

            return Ok(RaceDtos);
        }

        /// <summary>
        /// Returns all Races in the system.
        /// </summary>
        /// <returns>
        /// Content: A race in the system matching up to the Race ID primary key
        /// </returns>
        /// <param name="id">Primary key of the race</param>
        /// <example>
        /// GET: api/RaceData/FindRace/5
        /// </example>
        [ResponseType(typeof(RaceDto))]
        [HttpGet]
        public IHttpActionResult FindRace(int id)
        {
            Race Race = db.Races.Find(id);
            RaceDto RaceDto = new RaceDto()
            {
                RaceID = Race.RaceID,
                RaceName = Race.RaceName,
                RaceOffensive = Race.RaceOffensive
            };
            if (Race == null)
            {
                return NotFound();
            }

            return Ok(RaceDto);
        }

        /// <summary>
        /// Updates a particular Race in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Race ID primary key</param>
        /// <param name="Race">JSON form data of a race</param>
        /// <example>
        /// POST: api/RaceData/UpdateRace/5
        /// FORM DATA: Race JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateRace(int id, Race Race)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Race.RaceID)
            {

                return BadRequest();
            }

            db.Entry(Race).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RaceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds an Race to the system
        /// </summary>
        /// <param name="Race">JSON form data of a Race</param>
        /// <example>
        /// POST: api/RaceData/AddRace
        /// FORM DATA: Race JSON Object
        /// </example>
        [ResponseType(typeof(Race))]
        [HttpPost]
        public IHttpActionResult AddRace(Race Race)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Races.Add(Race);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Race.RaceID }, Race);
        }

        /// <summary>
        /// Deletes a race from the system by its ID
        /// </summary>
        /// <param name="id">The primary key of the Race</param>
        /// <example>
        /// POST: api/RaceData/DeleteRace/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Race))]
        [HttpPost]
        public IHttpActionResult DeleteRace(int id)
        {
            Race Race = db.Races.Find(id);
            if (Race == null)
            {
                return NotFound();
            }

            db.Races.Remove(Race);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RaceExists(int id)
        {
            return db.Races.Count(e => e.RaceID == id) > 0;
        }
    }
}