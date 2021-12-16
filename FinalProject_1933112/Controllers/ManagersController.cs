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
    public class ManagersController : Controller
    {
        private PropertyManagementDBEntities3 db = new PropertyManagementDBEntities3();

        // GET: Managers
        //public ActionResult Index()
        //{
        //   var managers = db.Managers.Include(m => m.Tenant);
        //    //return View(managers.ToList());
        //    return View();
        //}

        
        public ActionResult Index(Manager model)
        {
            using (PropertyManagementDBEntities3 db = new PropertyManagementDBEntities3())
            {
                bool isValidUser = db.Managers.Any(user => user.FirstName.ToLower() ==
                model.FirstName.ToLower() && user.Password == model.Password);
                if (isValidUser)
                {
                    FormsAuthentication.SetAuthCookie(model.FirstName, false);
                    
                    var fname = model.FirstName;

                    var test = db.Managers.FirstOrDefault(p => p.FirstName == fname);
                    var test2 = test.ManagerID;

                    string x = "Details/" + test2.ToString();
                    return RedirectToAction(x, "Managers");
                }
                ModelState.AddModelError("", "Invalid username or password !");
                return View();
            }
        }

        public ActionResult ListManagers()
        {
            var managers = db.Managers.Include(m => m.Owners);
            return View(managers.ToList());
            
        }

        // GET: Managers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        // GET: Managers/Create
        public ActionResult Create()
        {
            ViewBag.TenantID = new SelectList(db.Tenants, "tenantID", "firstName");
            return View();
        }

        // POST: Managers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ManagerID,FirstName,LastName,Password,TenantID")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                db.Managers.Add(manager);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TenantID = new SelectList(db.Tenants, "tenantID", "firstName", manager.TenantID);
            return View(manager);
        }

        // GET: Managers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            ViewBag.TenantID = new SelectList(db.Tenants, "tenantID", "firstName", manager.TenantID);
            return View(manager);
        }

        // POST: Managers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ManagerID,FirstName,LastName,Password,TenantID")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                db.Entry(manager).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TenantID = new SelectList(db.Tenants, "tenantID", "firstName", manager.TenantID);
            return View(manager);
        }

        // GET: Managers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        // POST: Managers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Manager manager = db.Managers.Find(id);
            db.Managers.Remove(manager);
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
