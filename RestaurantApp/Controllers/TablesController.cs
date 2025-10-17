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
    public class TablesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tables
        public ActionResult Index()
        {
            return View(db.tables.ToList());
        }

        // GET: Tables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tables tables = db.tables.Find(id);
            if (tables == null)
            {
                return HttpNotFound();
            }
            return View(tables);
        }

        // GET: Tables/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Capacity,Bill")] Tables tables)
        {
            if (ModelState.IsValid)
            {
                db.tables.Add(tables);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tables);
        }

        // GET: Tables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tables tables = db.tables.Find(id);
            if (tables == null)
            {
                return HttpNotFound();
            }
            return View(tables);
        }

        // POST: Tables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Capacity,Bill")] Tables tables)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tables).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tables);
        }

        // GET: Tables/Delete/5
        /*public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tables tables = db.tables.Find(id);
            if (tables == null)
            {
                return HttpNotFound();
            }
            return View(tables);
        }*/

        // POST: Tables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {

            //Tables tables = db.tables.Find(id);
            Tables tables = db.tables.Include(t => t.items).SingleOrDefault(t => t.Id == id);
            foreach (var item in tables.items.ToList())
            {
                tables.items.Remove(item);
               // db.items.Remove(item);
              // db.SaveChanges();
            }
            
            if (tables.waiter != null)
            {
                db.waiters.Find(db.tables.Find(id).waiter.Id).countTables--;
                db.waiters.Find(db.tables.Find(id).waiter.Id).tables.Remove(tables);
               // db.waiters.Find(tables.waiter.Id).countTables-=1;
                tables.waiter.countTables--;
                //db.Entry(tables.waiter).State = EntityState.Modified;
               // db.SaveChanges();
            }
           // tables.waiter = null;
            db.tables.Remove(tables);
            //db.SaveChanges();
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

        public ActionResult AddToTable(int id)
        {
            var model = new AddItemToTable();
            model.tableId = id;
            model.items = db.items.ToList();
            var store = db.tables.Find(id);
            ViewBag.StoreName = store.Name;
            return View(model);

        }

        [HttpPost]
        public ActionResult AddToTable(AddItemToTable model)
        {
            var table = db.tables.Find(model.tableId);
            var item = db.items.Find(model.itemId);
            table.items.Add(item);
            table.Bill += item.Price;
            db.SaveChanges();
            return View("Index", db.tables.ToList());
        }

    }
}
