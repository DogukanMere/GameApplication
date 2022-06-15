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
using System.Web;
using System.IO;

namespace GameApplication.Controllers
{
    public class CreatureDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all creatures in the system.
        /// </summary>
        /// <returns>
        /// Content: all creatures in the database, including their associated race.
        /// </returns>
        /// <example>
        /// GET: api/CreatureData/ListCreatures
        /// </example>
        [HttpGet]
        [ResponseType(typeof(CreatureDto))]
        public IHttpActionResult ListCreatures()
        {
            List<Creature> Creatures = db.Creatures.ToList();
            List<CreatureDto> CreatureDtos = new List<CreatureDto>();

            Creatures.ForEach(c => CreatureDtos.Add(new CreatureDto()
            {
                CreatureID = c.CreatureID,
                CreatureName = c.CreatureName,
                CreaturePower = c.CreaturePower,
                CreatureHasPic = c.CreatureHasPic,
                PicExtension = c.PicExtension,
                RaceID = c.Race.RaceID,
                RaceName = c.Race.RaceName
            }));

            return Ok(CreatureDtos);
        }

        /// <summary>
        /// Gathers information about all creatures related to a particular race ID
        /// </summary>
        /// <returns>
        /// Content: all creatures in the database, including their associated race matched with a particular race ID
        /// </returns>
        /// <param name="id">Race ID.</param>
        /// <example>
        /// GET: api/CreatureData/ListCreaturesForRace/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(CreatureDto))]
        public IHttpActionResult ListCreaturesForRace(int id)
        {
            List<Creature> Creatures = db.Creatures.Where(a => a.RaceID == id).ToList();
            List<CreatureDto> CreatureDtos = new List<CreatureDto>();

            Creatures.ForEach(c => CreatureDtos.Add(new CreatureDto()
            {
                CreatureID = c.CreatureID,
                CreatureName = c.CreatureName,
                CreaturePower = c.CreaturePower,
                RaceID = c.Race.RaceID,
                RaceName = c.Race.RaceName
            }));

            return Ok(CreatureDtos);
        }

        /// <summary>
        /// Gathers information about creatures related to a particular dungeon
        /// </summary>
        /// <returns>
        /// Content: all creatures in the database, including their associated race that match to a particular dungeon id
        /// </returns>
        /// <param name="id">Dungeon ID.</param>
        /// <example>
        /// GET: api/CreatureData/ListCreaturesForDungeon/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(CreatureDto))]
        public IHttpActionResult ListCreaturesForDungeon(int id)
        {
            //all creatures that have dungeons which match with ID
            List<Creature> Creatures = db.Creatures.Where(
                c => c.Dungeons.Any(
                    d => d.DungeonID == id
                )).ToList();
            List<CreatureDto> CreatureDtos = new List<CreatureDto>();

            Creatures.ForEach(c => CreatureDtos.Add(new CreatureDto()
            {
                CreatureID = c.CreatureID,
                CreatureName = c.CreatureName,
                CreaturePower = c.CreaturePower,
                RaceID = c.Race.RaceID,
                RaceName = c.Race.RaceName
            }));

            return Ok(CreatureDtos);
        }


        /// <summary>
        /// Associates a particular dungeon with a particular creature
        /// </summary>
        /// <param name="creatureid">The creature ID primary key</param>
        /// <param name="dungeonid">The dungeon ID primary key</param>
        /// <example>
        /// POST api/CreatureData/AssociateCreatureWithDungeon/9/1
        /// </example>
        [HttpPost]
        [Route("api/CreatureData/AssociateCreatureWithDungeon/{creatureid}/{dungeonid}")]
        public IHttpActionResult AssociateCreatureWithDungeon(int creatureid, int dungeonid)
        {

            Creature SelectedCreature = db.Creatures.Include(c => c.Dungeons).Where(c => c.CreatureID == creatureid).FirstOrDefault();
            Dungeon SelectedDungeon = db.Dungeons.Find(dungeonid);

            if (SelectedCreature == null || SelectedDungeon == null)
            {
                return NotFound();
            }

            SelectedCreature.Dungeons.Add(SelectedDungeon);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes an association between a particular dungeon and a particular creature
        /// </summary>
        /// <param name="creatureid">The creature ID primary key</param>
        /// <param name="dungeonid">The dungeon ID primary key</param>
        /// <example>
        /// POST api/CreatureData/AssociateCreatureWithDungeon/9/1
        /// </example>
        [HttpPost]
        [Route("api/CreatureData/UnAssociateCreatureWithDungeon/{creatureid}/{dungeonid}")]
        public IHttpActionResult UnAssociateCreatureWithDungeon(int creatureid, int dungeonid)
        {

            Creature SelectedCreature = db.Creatures.Include(c => c.Dungeons).Where(c => c.CreatureID == creatureid).FirstOrDefault();
            Dungeon SelectedDungeon = db.Dungeons.Find(dungeonid);

            if (SelectedCreature == null || SelectedDungeon == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input creature id is: " + creatureid);
            Debug.WriteLine("selected creature name is: " + SelectedCreature.CreatureName);
            Debug.WriteLine("input dungeon id is: " + dungeonid);
            Debug.WriteLine("selected dungeon name is: " + SelectedDungeon.DungeonName);


            SelectedCreature.Dungeons.Remove(SelectedDungeon);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Returns a specific creature by its id
        /// </summary>
        /// <returns>
        /// Content: A creature in the system matching up to the creature ID primary key
        /// </returns>
        /// <param name="id">The primary key of the creature</param>
        /// <example>
        /// GET: api/CreatureData/FindCreature/5
        /// </example>
        [ResponseType(typeof(CreatureDto))]
        [HttpGet]
        public IHttpActionResult FindCreature(int id)
        {
            Creature Creature = db.Creatures.Find(id);
            CreatureDto CreatureDto = new CreatureDto()
            {
                CreatureID = Creature.CreatureID,
                CreatureName = Creature.CreatureName,
                CreaturePower = Creature.CreaturePower,
                CreatureHasPic = Creature.CreatureHasPic,
                PicExtension = Creature.PicExtension,
                RaceID = Creature.Race.RaceID,
                RaceName = Creature.Race.RaceName
            };
            if (Creature == null)
            {
                return NotFound();
            }

            return Ok(CreatureDto);
        }

        /// <summary>
        /// Updates a particular creature in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Creature ID primary key</param>
        /// <param name="creature">JSON form data of a creature</param>
        /// <example>
        /// POST: api/CreatureData/UpdateCreature/5
        /// Form Data: Creature JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCreature(int id, Creature creature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != creature.CreatureID)
            {

                return BadRequest();
            }

            db.Entry(creature).State = EntityState.Modified;
            db.Entry(creature).Property(a => a.CreatureHasPic).IsModified = false;
            db.Entry(creature).Property(a => a.PicExtension).IsModified = false;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CreatureExists(id))
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
        /// Receives a creature pic. data, uploads the picture to the web server
        /// </summary>
        /// <param name="id">the creature id</param>
        /// <example>
        /// curl -F creaturelpic=@file.jpg "https://localhost:xx/api/creaturedata/uploadcreaturepic/2"
        /// POST: api/creatureData/UpdatecreaturePic/3
        /// HEADER: enctype=multipart/form-data
        /// FORM-DATA: image
        /// </example>
        /// https://stackoverflow.com/questions/28369529/how-to-set-up-a-web-api-controller-for-multipart-form-data

        [HttpPost]
        public IHttpActionResult UploadCreaturePic(int id)
        {

            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                int numfiles = HttpContext.Current.Request.Files.Count;
                //Debug.WriteLine("Files Received: " + numfiles);

                //Check whether a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var creaturePic = HttpContext.Current.Request.Files[0];
                    //Check if it is empty
                    if (creaturePic.ContentLength > 0)
                    {
                        //accepted file types
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(creaturePic.FileName).Substring(1);
                        //Check the file extension
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //get a direct file path to ~/Content/creatures/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Creatures/"), fn);

                                //save the file
                                creaturePic.SaveAs(path);

                                //if these are all successful then we can set these fields
                                haspic = true;
                                picextension = extension;

                                //Update the creature haspic and picextension fields in the database
                                Creature Selectedcreature = db.Creatures.Find(id);
                                Selectedcreature.CreatureHasPic = haspic;
                                Selectedcreature.PicExtension = extension;
                                db.Entry(Selectedcreature).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Creature Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                                return BadRequest();
                            }
                        }
                    }

                }

                return Ok();
            }
            else
            {
                return BadRequest();

            }

        }

        /// <summary>
        /// Adds a creature to the system
        /// </summary>
        /// <param name="creature">JSON form data of a creature</param>
        /// <returns>
        /// Content: Creature ID, Creature Data
        /// </returns>
        /// <example>
        /// POST: api/CreatureData/AddCreature
        /// Form Data: Creature JSON Object
        /// </example>
        [ResponseType(typeof(Creature))]
        [HttpPost]
        public IHttpActionResult AddCreature(Creature creature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Creatures.Add(creature);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = creature.CreatureID }, creature);
        }

        /// <summary>
        /// Deletes a creature from the system by its ID.
        /// </summary>
        /// <param name="id">The primary key of the creature</param>
        /// <example>
        /// POST: api/CreatureData/DeleteCreature/5
        /// Form Data: (empty)
        /// </example>
        [ResponseType(typeof(Creature))]
        [HttpPost]
        public IHttpActionResult DeleteCreature(int id)
        {
            Creature creature= db.Creatures.Find(id);
            if (creature == null)
            {
                return NotFound();
            }

            if (creature.CreatureHasPic && creature.PicExtension != "")
            {
                //delete image from path
                string path = HttpContext.Current.Server.MapPath("~/Content/Images/Creatures/" + id + "." + creature.PicExtension);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

            db.Creatures.Remove(creature);
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

        private bool CreatureExists(int id)
        {
            return db.Creatures.Count(e => e.CreatureID == id) > 0;
        }
    }
}