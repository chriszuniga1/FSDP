using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZipNTuck.Data.EF;
using System.Data;
using System.Data.Entity;
using PagedList;


namespace ZipNTuck.UI.MVC.Controllers
{
    public class FiltersController : Controller
    {
        private ZipNTuckEntities db = new ZipNTuckEntities();

        // GET: Filters
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReservationClientSide()
        {
            var reservations = db.Reservations.Include(r => r.ArticlesOfClothing).Include(r => r.Location);
            return View(reservations.ToList().OrderBy(l => l.Location.LocationName));
        }



        public ActionResult ReservationsQS(string searchFilter)
        {
            #region Search Filter
            if (String.IsNullOrEmpty(searchFilter))
            {
                var reservations = db.Reservations.Include(r => r.ArticlesOfClothing).Include(r => r.Location);
                return View(reservations.ToList().OrderBy(l => l.Location.LocationName));
            }
            else
            {
                string searchUpCase = searchFilter.ToUpper();

                List<Reservation> searchResults = db.Reservations.Where(r => r.Location.LocationName.ToUpper().Contains(searchUpCase)).ToList();

                List<Reservation> searchResults2 = (
                    from r in db.Reservations
                    where r.Location.LocationName.ToUpper().Contains(searchUpCase)
                    select r
                    ).ToList();

                return View(searchResults);
            }

            #endregion
        }
        
            

    }
}