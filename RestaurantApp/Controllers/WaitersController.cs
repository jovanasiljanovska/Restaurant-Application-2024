using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RestaurantApp.Models;

namespace RestaurantApp.Controllers
{
    public class WaitersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Waiters
        public ActionResult Index()
        {
            foreach (var i in db.waiters.ToList())
            {

                i.countTables = i.tables.Count();

            }
            return View(db.waiters.ToList());
        }

        // GET: Waiters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Waiters waiters = db.waiters.Find(id);
            if (waiters == null)
            {
                return HttpNotFound();
            }
            return View(waiters);
        }

        // GET: Waiters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Waiters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,countTables")] Waiters waiters)
        {
            if (ModelState.IsValid)
            {
                db.waiters.Add(waiters);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(waiters);
        }

        // GET: Waiters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Waiters waiters = db.waiters.Find(id);
            if (waiters == null)
            {
                return HttpNotFound();
            }
            return View(waiters);
        }

        // POST: Waiters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,countTables")] Waiters waiters)
        {
            if (ModelState.IsValid)
            {
                db.Entry(waiters).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(waiters);
        }

        // GET: Waiters/Delete/5
        [Authorize(Roles ="Owner")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Waiters waiters = db.waiters.Find(id);
            if (waiters == null)
            {
                return HttpNotFound();
            }
            return View(waiters);
        }

        // POST: Waiters/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles ="Owner")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Waiters waiters = db.waiters.Find(id);
            foreach (var table in waiters.tables.ToList())
            {
                db.tables.Remove(table); // Remove the table from the database
            }
            db.SaveChanges();
            db.waiters.Remove(waiters);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult AddToWaiter(int id)
        {
            var model = new AddTableToWaiter();
            model.waiterId = id;
            model.tables = db.tables.ToList();
            var waiter = db.waiters.Find(id);
            ViewBag.StoreName = waiter.Name;
            return View(model);

        }

        [HttpPost]
        public ActionResult AddToWaiter(AddTableToWaiter model)
        {
            var waiter = db.waiters.Find(model.waiterId);
            var table = db.tables.Find(model.tableId);
            waiter.tables.Add(table);
            //waiter.countTables++;
            db.waiters.Find(model.waiterId).countTables=waiter.tables.Count();
            db.SaveChanges();
            return RedirectToAction("Index", db.waiters.ToList());
        }
    }
}
