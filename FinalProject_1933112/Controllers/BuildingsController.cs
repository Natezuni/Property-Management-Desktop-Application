using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProject_1933112.Models;

namespace FinalProject_1933112.Controllers
{
    public class BuildingsController : Controller
    {
        private PropertyManagementDBEntities3 db = new PropertyManagementDBEntities3();

        // GET: Buildings
        public ActionResult Index()
        {
            var buildings = db.Buildings.Include(b => b.Appartment).Include(b => b.Manager);
            return View(buildings.ToList());
        }

        public ActionResult ListBuildings()
        {
            var buildings = db.Buildings.Include(m => m.Manager);
            return View(buildings.ToList());

        }

        // GET: Buildings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        // GET: Buildings/Create
        public ActionResult Create()
        {
            ViewBag.AppartmentID = new SelectList(db.Appartments, "AppartmentID", "AppartmentID");
            ViewBag.ManagerID = new SelectList(db.Managers, "ManagerID", "FirstName");
            return View();
        }

        // POST: Buildings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BuildingID,nbAppartments,AppartmentID,ManagerID")] Building building)
        {
            if (ModelState.IsValid)
            {
                db.Buildings.Add(building);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AppartmentID = new SelectList(db.Appartments, "AppartmentID", "AppartmentID", building.AppartmentID);
            ViewBag.ManagerID = new SelectList(db.Managers, "ManagerID", "FirstName", building.ManagerID);
            return View(building);
        }

        // GET: Buildings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            ViewBag.AppartmentID = new SelectList(db.Appartments, "AppartmentID", "AppartmentID", building.AppartmentID);
            ViewBag.ManagerID = new SelectList(db.Managers, "ManagerID", "FirstName", building.ManagerID);
            return View(building);
        }

        // POST: Buildings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BuildingID,nbAppartments,AppartmentID,ManagerID")] Building building)
        {
            if (ModelState.IsValid)
            {
                db.Entry(building).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AppartmentID = new SelectList(db.Appartments, "AppartmentID", "AppartmentID", building.AppartmentID);
            ViewBag.ManagerID = new SelectList(db.Managers, "ManagerID", "FirstName", building.ManagerID);
            return View(building);
        }

        // GET: Buildings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        // POST: Buildings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Building building = db.Buildings.Find(id);
            db.Buildings.Remove(building);
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
    }
}
