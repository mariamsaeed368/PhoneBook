using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    [Authorize]
    public class PeopleController : Controller
    {
        private Entities db = new Entities();

        // GET: People
        public ActionResult Index()
        {
            List<Person> listOfPersons = db.People.ToList();
            List<Person> birthdays = new List<Person>();
            List<Person> updated = new List<Person>();

            foreach (Person item in listOfPersons)
            {
                DateTime dt = Convert.ToDateTime(item.DateOfBirth);
                DateTime today = DateTime.Now;
                Double diffe = (today - dt).TotalDays;
                //int diff2 = Convert.ToInt32(diffe);
                if(diffe<=10&& diffe>=0)
                {
                    birthdays.Add(item);
                }

                DateTime dt2 = Convert.ToDateTime(item.UpdateOn);
                DateTime today2 = DateTime.Now;
                double diff3 = (dt2 - today2).TotalDays;
                //int diff4 = Convert.ToInt32(diff3);
                if (diff3 <= 7&&diff3>=0)
                {
                    updated.Add(item);
                }
            }

            ViewBag.updated = updated;
            ViewBag.birthdayList = birthdays;

            return View(listOfPersons);
        }

        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: People/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonId,FirstName,MiddleName,LastName,DateOfBirth,AddedOn,AddedBy,HomeAddress,HomeCity,FaceBookAccountId,LinkedInId,UpdateOn,ImagePath,TwitterId,EmailId")] Person person)
        {
            if (ModelState.IsValid)
            {
                int count = db.People.Count();
                person.PersonId = count++;
                DateTime today = DateTime.Today;
                person.AddedBy = User.Identity.GetUserName();
                person.AddedOn = DateTime.Today;
                person.UpdateOn = DateTime.Today;
                db.People.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(person);
        }

        // GET: People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FirstName,MiddleName,LastName,DateOfBirth,HomeAddress,HomeCity,FaceBookAccountId,LinkedInId,UpdateOn,TwitterId,EmailId")] Person person)
        {
            if (ModelState.IsValid)
            {
                person.AddedOn = Convert.ToDateTime(person.AddedOn);
                person.AddedBy = User.Identity.GetUserName();
                person.UpdateOn = DateTime.Now;
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.People.Find(id);
            db.People.Remove(person);
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
