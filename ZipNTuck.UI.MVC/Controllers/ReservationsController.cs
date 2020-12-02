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

namespace ZipNTuck.UI.MVC.Controllers
{
    public class ReservationsController : Controller
    {
        private ZipNTuckEntities db = new ZipNTuckEntities();

        // GET: Reservations
        public ActionResult Index()
        {
            if (User.IsInRole("Customer"))
            {
                string currentUser = User.Identity.GetUserId();
                var customerReservations = db.Reservations.Where(r => r.ArticlesOfClothing.UserID == currentUser);
                return View(customerReservations.ToList().OrderBy(l => l.Location.LocationName));
            }
            var locationID = Request.QueryString["location"];

            if (locationID == null)
            {
                var reservations = db.Reservations.Include(r => r.ArticlesOfClothing).Include(r => r.Location);
                return View(reservations.ToList().OrderBy(l => l.Location.LocationName));
            }
            else
            {
                var lid = Convert.ToInt32(locationID);
                var reservations = db.Reservations.Include(r => r.ArticlesOfClothing).Include(r => r.Location).Where(r => r.Location.LocationID == lid);
                return View(reservations.ToList().OrderBy(l => l.Location.LocationName));
            }
        }

        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        public ActionResult Create()
        {
            if (User.IsInRole("Customer"))
            {
            string currentUser = User.Identity.GetUserId();
            ViewBag.ArticleID = new SelectList(db.ArticlesOfClothings.Where(x=>x.UserID==currentUser), "ArticleID", "ArticleName");
            }
            else
            {
                ViewBag.ArticleID = new SelectList(db.ArticlesOfClothings, "ArticleID", "ArticleName");
            }
            

            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReservationID,ArticleID,LocationID,ReservationDate")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                int nbrReservation = db.Reservations.Where(r => r.ReservationDate == reservation.ReservationDate && r.LocationID == reservation.LocationID).Count();
                Location locationReservation = db.Locations.Find(reservation.LocationID);

                if (locationReservation.ReservationLimit > nbrReservation)
                {
                    db.Reservations.Add(reservation);
                    db.SaveChanges();
                }
                else
                {
                    if (locationReservation.ReservationLimit <= nbrReservation && User.IsInRole("Admin"))
                    {
                        db.Reservations.Add(reservation);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View("OverBooked");
                }
                return RedirectToAction("Index");
            }

            ViewBag.ArticleID = new SelectList(db.ArticlesOfClothings, "ArticleID", "ArticleName", reservation.ArticleID);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", reservation.LocationID);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArticleID = new SelectList(db.ArticlesOfClothings, "ArticleID", "ArticleName", reservation.ArticleID);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", reservation.LocationID);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReservationID,ArticleID,LocationID,ReservationDate")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArticleID = new SelectList(db.ArticlesOfClothings, "ArticleID", "ArticleName", reservation.ArticleID);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", reservation.LocationID);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
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
