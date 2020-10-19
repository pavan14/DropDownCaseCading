using DropDownCaseCading.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DropDownCaseCading.Controllers
{
    public class HomeController : Controller
    {
        PeopleDataEntities db = new PeopleDataEntities();

        // GET: Home
        public ActionResult Index()
        {
            List<Country> CountryList = db.Countries.ToList();
            ViewBag.CountryList = new SelectList(CountryList, "CountryID", "CountryName");
            return View();
        }

        public JsonResult GetStateList(int CountryID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<State> StateList = db.States.Where(x => x.CountryID == CountryID).ToList();
            return Json(StateList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCityList(int StateID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<City> CityList = db.Cities.Where(x => x.StateID == StateID).ToList();
            return Json(CityList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult StoreOrEdit(int id=0)
        {
            if (id == 0)
                return View(new PersonalInfo());
            else
            {
                using (PeopleDataEntities db = new PeopleDataEntities())
                {
                    return View(db.PersonalInfoes.Where(x => x.PersonID == id).FirstOrDefault<PersonalInfo>());
                }
            }              
        }

        [HttpPost]
        public ActionResult StoreOrEdit(PersonalInfo people)
        {
            using (PeopleDataEntities db = new PeopleDataEntities())
            {
                if (people.PersonID == 0)
                {
                    db.PersonalInfoes.Add(people);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully", JsonRequestBehavior.AllowGet });
                }

                else
                {
                    db.Entry(people).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully", JsonRequestBehavior.AllowGet });
                }
                }
            }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (PeopleDataEntities db = new PeopleDataEntities())
            {
                PersonalInfo personal = db.PersonalInfoes.Where(x => x.PersonID == id).FirstOrDefault<PersonalInfo>();
                db.PersonalInfoes.Remove(personal);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully", JsonRequestBehavior.AllowGet });
            } 

        }
    }
}
