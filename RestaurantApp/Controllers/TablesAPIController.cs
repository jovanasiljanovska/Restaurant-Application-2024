using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Numerics;
using System.Web.Http;
using System.Web.Http.Description;
using RestaurantApp.Models;

namespace RestaurantApp.Controllers
{
    public class TablesAPIController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Tables1
        public IQueryable<Tables> Gettables()
        {
            return db.tables;
        }

        // GET: api/Tables1/5
        [ResponseType(typeof(Tables))]
        public IHttpActionResult GetTables(int id)
        {
            Tables tables = db.tables.Find(id);
            if (tables == null)
            {
                return NotFound();
            }

            return Ok(tables);
        }

        // PUT: api/Tables1/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTables(int id, Tables tables)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tables.Id)
            {
                return BadRequest();
            }

            db.Entry(tables).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TablesExists(id))
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

        // POST: api/Tables1
        [ResponseType(typeof(Tables))]
        public IHttpActionResult PostTables(Tables tables)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tables.Add(tables);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tables.Id }, tables);
        }

        // DELETE: api/TablesAPI/5
        [ResponseType(typeof(Tables))]
        public IHttpActionResult DeleteTables(int id)
        {
            Tables tables = db.tables.Include(t => t.items).SingleOrDefault(t => t.Id == id);
            if (tables == null)
            {
                return NotFound();
            }
            foreach(var item in tables.items.ToList())
            {
                tables.items = null;
            }
            var tmp = db.tables.Find(id).waiter;
            

           
            if (tables.waiter != null)
            {
                //db.waiters.Find(tmp.Id).countTables = db.waiters.Find(tmp.Id).tables.Count();
                db.waiters.Find(db.tables.Find(id).waiter.Id).countTables--;
                db.waiters.Find(db.tables.Find(id).waiter.Id).tables.Remove(tables);
                db.Entry(tmp).State = EntityState.Modified;
                db.SaveChanges();
            }
            db.tables.Remove(tables);
            db.SaveChanges();

            return Ok(tables);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TablesExists(int id)
        {
            return db.tables.Count(e => e.Id == id) > 0;
        }
    }
}