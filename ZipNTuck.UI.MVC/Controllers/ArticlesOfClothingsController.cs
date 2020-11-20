using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZipNTuck.Data.EF;

namespace ZipNTuck.UI.MVC.Controllers
{
    public class ArticlesOfClothingsController : Controller
    {
        private ZipNTuckEntities db = new ZipNTuckEntities();

        // GET: ArticlesOfClothings
        public ActionResult Index()
        {
            var articlesOfClothings = db.ArticlesOfClothings.Include(a => a.UserDetail);
            return View(articlesOfClothings.ToList());
        }

        // GET: ArticlesOfClothings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticlesOfClothing articlesOfClothing = db.ArticlesOfClothings.Find(id);
            if (articlesOfClothing == null)
            {
                return HttpNotFound();
            }
            return View(articlesOfClothing);
        }

        // GET: ArticlesOfClothings/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.UserDetails, "UserID", "FirstName");
            return View();
        }

        // POST: ArticlesOfClothings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArticleID,ArticleName,UserID,ArticlePhoto,SpecialNotes,IsActive,DateAdded")] ArticlesOfClothing articlesOfClothing)
        {
            if (ModelState.IsValid)
            {
                db.ArticlesOfClothings.Add(articlesOfClothing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.UserDetails, "UserID", "FirstName", articlesOfClothing.UserID);
            return View(articlesOfClothing);
        }

        // GET: ArticlesOfClothings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticlesOfClothing articlesOfClothing = db.ArticlesOfClothings.Find(id);
            if (articlesOfClothing == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.UserDetails, "UserID", "FirstName", articlesOfClothing.UserID);
            return View(articlesOfClothing);
        }

        // POST: ArticlesOfClothings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArticleID,ArticleName,UserID,ArticlePhoto,SpecialNotes,IsActive,DateAdded")] ArticlesOfClothing articlesOfClothing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(articlesOfClothing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.UserDetails, "UserID", "FirstName", articlesOfClothing.UserID);
            return View(articlesOfClothing);
        }

        // GET: ArticlesOfClothings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticlesOfClothing articlesOfClothing = db.ArticlesOfClothings.Find(id);
            if (articlesOfClothing == null)
            {
                return HttpNotFound();
            }
            return View(articlesOfClothing);
        }

        // POST: ArticlesOfClothings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArticlesOfClothing articlesOfClothing = db.ArticlesOfClothings.Find(id);
            db.ArticlesOfClothings.Remove(articlesOfClothing);
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
