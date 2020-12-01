using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZipNTuck.Data.EF;
using Microsoft.AspNet.Identity;
using System.Drawing;
using ZipNTuck.UI.MVC.Utilities;

namespace ZipNTuck.UI.MVC.Controllers
{
    public class ArticlesOfClothingsController : Controller
    {
        private ZipNTuckEntities db = new ZipNTuckEntities();

        // GET: ArticlesOfClothings
        public ActionResult Index()
        {
            if (User.IsInRole("Customer"))
            {
                string currentUserID = User.Identity.GetUserId();
                var customerArticlesOfClothings = db.ArticlesOfClothings.Where(a => a.UserID == currentUserID).Include(a => a.UserDetail);
                return View(customerArticlesOfClothings.ToList());
            }
            var articlesOfClothings = db.ArticlesOfClothings.Include(a => a.UserDetail);
            //return View(articlesOfClothings.ToList());
            return View(db.ArticlesOfClothings.Where(p => p.IsActive).ToList());
        }

        public ActionResult Inactive()
        {
            return View(db.ArticlesOfClothings.Where(p => !p.IsActive).ToList());
        }

        // GET: ArticlesOfClothings/Details/5
        [Authorize]
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
        [Authorize]
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
        public ActionResult Create([Bind(Include = "ArticleID,ArticleName,UserID,ArticlePhoto,SpecialNotes,IsActive,DateAdded")] ArticlesOfClothing articlesOfClothing, HttpPostedFileBase clothingImage)
        {
            if (ModelState.IsValid)
            {
                //File upload will go here
                #region File Upload 
                string file = "NoImage.png";

                if (clothingImage != null)
                {
                    file = clothingImage.FileName;
                    string ext = file.Substring(file.LastIndexOf('.'));
                    string[] goodExts = { ".jpeg", ".jpg", ".png", ".gif" };
                    //check that the uploaded file ext is in our list of good file extenstions
                    if (goodExts.Contains(ext))
                    {
                        //if valid ext, check file size <= 4mb (max by default from ASP.NET)
                        if (clothingImage.ContentLength <= 4194304)
                        {
                            //create a new file name using a guid (yes that's spelled correctly)
                            file = Guid.NewGuid() + ext;

                            #region Resize Image
                            string savePath = Server.MapPath("~/Content/clothingimg/");

                            Image convertedImage = Image.FromStream(clothingImage.InputStream);

                            int maxImageSize = 500;

                            int maxThumbSize = 100;

                            UploadUtility.ResizeImage(savePath, file, convertedImage, maxImageSize, maxThumbSize);
                            #endregion
                        }
                    }
                    articlesOfClothing.ArticlePhoto = file;
                }
                #endregion


                if (User.IsInRole("Customer"))
                {
                    string currentUserID = User.Identity.GetUserId();
                    articlesOfClothing.UserID = currentUserID;
                    articlesOfClothing.DateAdded = DateTime.Today;
                }
                db.ArticlesOfClothings.Add(articlesOfClothing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.UserDetails, "UserID", "FirstName", articlesOfClothing.UserID);
            return View(articlesOfClothing);
        }

        // GET: ArticlesOfClothings/Edit/5
        [Authorize]
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
        public ActionResult Edit([Bind(Include = "ArticleID,ArticleName,UserID,ArticlePhoto,SpecialNotes,IsActive,DateAdded")] ArticlesOfClothing articlesOfClothing, HttpPostedFileBase clothingImage)
        {
            if (ModelState.IsValid)
            {
                //file upload
                #region File Upload 
                string file = "NoImage.png";

                if (clothingImage != null)
                {
                    file = clothingImage.FileName;
                    string ext = file.Substring(file.LastIndexOf('.'));
                    string[] goodExts = { ".jpeg", ".jpg", ".png", ".gif" };
                    //check that the uploaded file ext is in our list of good file extenstions
                    if (goodExts.Contains(ext))
                    {
                        //if valid ext, check file size <= 4mb (max by default from ASP.NET)
                        if (clothingImage.ContentLength <= 4194304)
                        {
                            //create a new file name using a guid (yes that's spelled correctly)
                            file = Guid.NewGuid() + ext;

                            #region Resize Image
                            string savePath = Server.MapPath("~/Content/imgstore/books/");

                            Image convertedImage = Image.FromStream(clothingImage.InputStream);

                            int maxImageSize = 500;

                            int maxThumbSize = 100;

                            UploadUtility.ResizeImage(savePath, file, convertedImage, maxImageSize, maxThumbSize);
                            #endregion

                            if (articlesOfClothing.ArticlePhoto != null && articlesOfClothing.ArticlePhoto != "NoImage.png")
                            {
                                string path = Server.MapPath("~/Content/clothingimg/");
                                UploadUtility.Delete(path, articlesOfClothing.ArticlePhoto);
                            }
                        }
                    }
                    articlesOfClothing.ArticlePhoto = file;
                }
                #endregion


                db.Entry(articlesOfClothing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.UserDetails, "UserID", "FirstName", articlesOfClothing.UserID);
            return View(articlesOfClothing);
        }

        // GET: ArticlesOfClothings/Delete/5
        [Authorize]
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

            //Delete the image file of the record that is being removed 
            string path = Server.MapPath("~/Content/clothingimg/");
            UploadUtility.Delete(path, articlesOfClothing.ArticlePhoto);

            //db.ArticlesOfClothings.Remove(articlesOfClothing);
            //
            articlesOfClothing.IsActive = !articlesOfClothing.IsActive;

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
