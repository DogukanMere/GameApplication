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
using System.Diagnostics;

namespace GameApplication.Controllers
{
    public class DungeonDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Dungeons in the system.
        /// </summary>
        /// <returns>
        /// Content: all Dungeons in the database, including their associated race.
        /// </returns>
        /// <example>
        /// GET: api/DungeonData/ListDungeons
        /// </example>
        [HttpGet]
        [ResponseType(typeof(DungeonDto))]
        public IHttpActionResult ListDungeons()
        {
            List<Dungeon> Dungeons = db.Dungeons.ToList();
            List<DungeonDto> DungeonDtos = new List<DungeonDto>();

            Dungeons.ForEach(d => DungeonDtos.Add(new DungeonDto()
            {
                DungeonID = d.DungeonID,
                DungeonName = d.DungeonName,
                DungeonLocation = d.DungeonLocation
            }));

            return Ok(DungeonDtos);
        }

        /// <summary>
        /// Returns all Dungeons in the system associated with a particular creature
        /// </summary>
        /// <returns>
        /// Content: all Dungeons having a particular creature
        /// </returns>
        /// <param name="id">Creature Primary Key</param>
        /// <example>
        /// GET: api/DungeonData/ListDungeonsForCreature/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(DungeonDto))]
        public IHttpActionResult ListDungeonsForCreature(int id)
        {
            List<Dungeon> Dungeons = db.Dungeons.Where(
                d => d.Creatures.Any(
                    c => c.CreatureID == id)
                ).ToList();
            List<DungeonDto> DungeonDtos = new List<DungeonDto>();

            Dungeons.ForEach(d => DungeonDtos.Add(new DungeonDto()
            {
                DungeonID = d.DungeonID,
                DungeonName = d.DungeonName,
                DungeonLocation = d.DungeonLocation
            }));

            return Ok(DungeonDtos);
        }


        /// <summary>
        /// Returns Dungeons in the system not having a particular creature
        /// </summary>
        /// <returns>
        /// Content: all Dungeons in the database not having a particular creature
        /// </returns>
        /// <param name="id">Creature Primary Key</param>
        /// <example>
        /// GET: api/DungeonData/ListDungeonsNotHavingCreature/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(DungeonDto))]
        public IHttpActionResult ListDungeonsNotHavingCreature(int id)
        {
            List<Dungeon> Dungeons = db.Dungeons.Where(
                d => !d.Creatures.Any(
                    c => c.CreatureID == id)
                ).ToList();
            List<DungeonDto> DungeonDtos = new List<DungeonDto>();

            Dungeons.ForEach(d => DungeonDtos.Add(new DungeonDto()
            {
                DungeonID = d.DungeonID,
                DungeonName = d.DungeonName,
                DungeonLocation = d.DungeonLocation
            }));

            return Ok(DungeonDtos);
        }

        /// <summary>
        /// Returns all Dungeons in the system.
        /// </summary>
        /// <returns>
        /// Content: A Dungeon in the system matching up to the Dungeon ID primary key
        /// </returns>
        /// <param name="id">The primary key of the Dungeon</param>
        /// <example>
        /// GET: api/DungeonData/FindDungeon/5
        /// </example>
        [ResponseType(typeof(DungeonDto))]
        [HttpGet]
        public IHttpActionResult FindDungeon(int id)
        {
            Dungeon Dungeon = db.Dungeons.Find(id);
            DungeonDto DungeonDto = new DungeonDto()
            {
                DungeonID = Dungeon.DungeonID,
                DungeonName = Dungeon.DungeonName,
                DungeonLocation = Dungeon.DungeonLocation
            };
            if (Dungeon == null)
            {
                return NotFound();
            }

            return Ok(DungeonDto);
        }

        /// <summary>
        /// Updates a particular Dungeon in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Dungeon ID primary key</param>
        /// <param name="Dungeon">JSON form data of a Dungeon</param>
        /// <example>
        /// POST: api/DungeonData/UpdateDungeon/5
        /// Form Data: Dungeon JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDungeon(int id, Dungeon Dungeon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Dungeon.DungeonID)
            {

                return BadRequest();
            }

            db.Entry(Dungeon).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DungeonExists(id))
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
        /// Adds a Dungeon to the system
        /// </summary>
        /// <param name="Dungeon">JSON form data of a Dungeon</param>
        /// <returns>
        /// Content: Dungeon ID, Dungeon Data
        /// </returns>
        /// <example>
        /// POST: api/DungeonData/AddDungeon
        /// Form Data: Dungeon JSON Object
        /// </example>
        [ResponseType(typeof(Dungeon))]
        [HttpPost]
        public IHttpActionResult AddDungeon(Dungeon Dungeon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Dungeons.Add(Dungeon);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Dungeon.DungeonID }, Dungeon);
        }

        /// <summary>
        /// Deletes a Dungeon from the system by its ID.
        /// </summary>
        /// <param name="id">The primary key of the Dungeon</param>
        /// <example>
        /// POST: api/DungeonData/DeleteDungeon/5
        /// Form Data: (empty)
        /// </example>
        [ResponseType(typeof(Dungeon))]
        [HttpPost]
        public IHttpActionResult DeleteDungeon(int id)
        {
            Dungeon Dungeon = db.Dungeons.Find(id);
            if (Dungeon == null)
            {
                return NotFound();
            }

            db.Dungeons.Remove(Dungeon);
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

        private bool DungeonExists(int id)
        {
            return db.Dungeons.Count(e => e.DungeonID == id) > 0;
        }
    }
}