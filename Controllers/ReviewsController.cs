using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ReviewArena.Models;

namespace ReviewArena.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reviews
        [AllowAnonymous]
        public ActionResult Index()
        {
            var reviews = db.Reviews.Include(r => r.Product);
            return View(reviews.ToList());
        }

        // GET: Reviews/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // GET: Reviews/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(db.Products, "ProductID", "name");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ReviewTitle,Pros,Cons,ReviewImage,ReviewDescription,AddedAt,LikesNumber,ProductId")] Review review)
        {
            if (ModelState.IsValid)
            {
                review.LikesNumber = 0;
                review.AddedAt = DateTime.Now;
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "ProductID", "name", review.ProductId);
            return View(review);
        }

        public ActionResult Like(int id)
        {
            
            Review update=db.Reviews.ToList().Find(u => u.Id == id);
            update.LikesNumber += 1;
            if(update.LikesNumber == 2)
            {
                update.LikesNumber = 1;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "ProductID", "name", review.ProductId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ReviewTitle,Pros,Cons,ReviewImage,ReviewDescription,AddedAt,LikesNumber,ProductId")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.Products, "ProductID", "name", review.ProductId);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
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

        public ActionResult AddToLike(int id)
        {
            var ReviewsInLike = new Like();
            var review = db.Likes.SingleOrDefault(u => u.ReviewId == id);
            if (review != null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ReviewsInLike.UserId = User.Identity.GetUserId();
                ReviewsInLike.ReviewId = id;
                db.Likes.Add(ReviewsInLike);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
        }

    }
}
