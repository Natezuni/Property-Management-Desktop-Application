using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FinalProject_1933112.Models;

namespace FinalProject_1933112.Controllers
{
    public class OwnersController : Controller
    {
        private PropertyManagementDBEntities3 db = new PropertyManagementDBEntities3();

        // GET: Owners
        //public ActionResult Index()
        //{
        //    var owners = db.Owners.Include(o => o.Manager).Include(o => o.Tenant);
        //    //return View(owners.ToList());
        //    return View();
        //}

        // GET: Owners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Owner owner = db.Owners.Find(id);
            if (owner == null)
            {
                return HttpNotFound();
            }
            return View(owner);
        }

        public ActionResult Index(Owner model)
        {
            using (PropertyManagementDBEntities3 db = new PropertyManagementDBEntities3())
            {
                bool isValidUser = db.Owners.Any(user => user.FirstName.ToLower() ==
                model.FirstName.ToLower() && user.Password == model.Password);
                if (isValidUser)
                {
                    FormsAuthentication.SetAuthCookie(model.FirstName, false);

                    var fname = model.FirstName;

                    var test = db.Owners.FirstOrDefault(p => p.FirstName == fname);
                    var test2 = test.OwnerID;

                    string x = "Details/" + test2.ToString();
                    return RedirectToAction(x, "Owners");
                }
                ModelState.AddModelError("", "Invalid username or password !");
                return View();
            }
        }

        // GET: Owners/Create
        public ActionResult Create()
        {
            ViewBag.ManagerID = new SelectList(db.Managers, "ManagerID", "FirstName");
            ViewBag.TenantID = new SelectList(db.Tenants, "tenantID", "firstName");
            return View();
        }

        // POST: Owners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OwnerID,FirstName,LastName,Password,ManagerID, TenantID")] Owner owner)
        {
            if (ModelState.IsValid)
            {
                db.Owners.Add(owner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ManagerID = new SelectList(db.Managers, "ManagerID", "FirstName", owner.ManagerID);
            ViewBag.TenantID = new SelectList(db.Tenants, "tenantID", "firstName", owner.TenantID);
            return View(owner);
        }

        // GET: Owners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Owner owner = db.Owners.Find(id);
            if (owner == null)
            {
                return HttpNotFound();
            }
            ViewBag.ManagerID = new SelectList(db.Managers, "ManagerID", "FirstName", owner.ManagerID);
            ViewBag.TenantID = new SelectList(db.Tenants, "tenantID", "firstName", owner.TenantID);
            return View(owner);
        }

        // POST: Owners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OwnerID,FirstName,LastName,Password,ManagerID, TenantID")] Owner owner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(owner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ManagerID = new SelectList(db.Managers, "ManagerID", "FirstName", owner.ManagerID);
            ViewBag.TenantID = new SelectList(db.Tenants, "tenantID", "firstName", owner.TenantID);
            return View(owner);
        }

        // GET: Owners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Owner owner = db.Owners.Find(id);
            if (owner == null)
            {
                return HttpNotFound();
            }
            return View(owner);
        }

        // POST: Owners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Owner owner = db.Owners.Find(id);
            db.Owners.Remove(owner);
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
