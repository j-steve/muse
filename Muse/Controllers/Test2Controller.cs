using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Muse.Models;

namespace Muse.Controllers
{
    public class Test2Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Test2/
        public ActionResult Index()
        {
            return View(db.UserTvShows.ToList());
        }

        // GET: /Test2/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTvShow usertvshow = db.UserTvShows.Find(id);
            if (usertvshow == null)
            {
                return HttpNotFound();
            }
            return View(usertvshow);
        }

        // GET: /Test2/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Test2/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID")] UserTvShow usertvshow)
        {
            if (ModelState.IsValid)
            {
                db.UserTvShows.Add(usertvshow);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usertvshow);
        }

        // GET: /Test2/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTvShow usertvshow = db.UserTvShows.Find(id);
            if (usertvshow == null)
            {
                return HttpNotFound();
            }
            return View(usertvshow);
        }

        // POST: /Test2/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID")] UserTvShow usertvshow)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usertvshow).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usertvshow);
        }

        // GET: /Test2/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTvShow usertvshow = db.UserTvShows.Find(id);
            if (usertvshow == null)
            {
                return HttpNotFound();
            }
            return View(usertvshow);
        }

        // POST: /Test2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserTvShow usertvshow = db.UserTvShows.Find(id);
            db.UserTvShows.Remove(usertvshow);
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
