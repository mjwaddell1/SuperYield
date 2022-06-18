using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SYMVC.Models;

namespace SYMVC.Controllers
{
    public class FAQController : ControllerBase
    {
        private DbCtxt db = DbCtxt.Create();

		// GET: FAQ
		public ActionResult Index()
        {
            try
            {
                Log.Info("FAQ Index", Session);
                return View(db.FAQSet.Where(f => !f.Deleted).ToList());
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
        }

        // GET: FAQ/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
                if (!Util.UserIsAdmin(Session)) return Redirect(Url.Content("~/Account/Login"));

                if (id == null)
                {
                    Log.Error("FAQ Details - No ID provided", Session, null);
                    return ErrorView("No ID provided");
                }
                FAQ faq = db.FAQSet.Find(id);
                if (faq == null)
                {
                    return HttpNotFound();
                }
                return View(faq);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
        }

        // GET: FAQ/Create
        public ActionResult Create()
        {
            if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
            if (!Util.UserIsAdmin(Session)) return Redirect(Url.Content("~/Account/Login"));

            return View();
        }

        // POST: FAQ/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Question,Answer,CreateUser,CreateDate,UpdateUser,UpdateDate")] FAQ faq)
        {
            try
            {
                if (ModelState.IsValid)
                {
					faq.CreateUser = int.Parse(Session["UserAccountId"].ToString());
					faq.CreateDate = DateTime.Now;

					db.FAQSet.Add(faq);
                    db.SaveChanges();

					Session["UserMsg"] = "FAQ created successfully";
					return RedirectToAction("Index");
                }

                return View(faq);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
        }

        // GET: FAQ/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
                if (!Util.UserIsAdmin(Session)) return Redirect(Url.Content("~/Account/Login"));

                if (id == null)
                {
                    Log.Error("FAQ Edit - No ID provided", Session, null);
                    return ErrorView("No ID provided");
                }
                FAQ faq = db.FAQSet.Find(id);
                if (faq == null)
                {
                    return HttpNotFound();
                }
                return View(faq);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
        }

        // POST: FAQ/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Question,Answer,CreateUser,CreateDate,UpdateUser,UpdateDate")] FAQ faq)
        {
            try
            {
                if (ModelState.IsValid)
                {
					faq.UpdateUser = int.Parse(Session["UserAccountId"].ToString());
					faq.UpdateDate = DateTime.Now;

					db.Entry(faq).State = EntityState.Modified;
                    db.SaveChanges();
					Session["UserMsg"] = "Update successful";
					return RedirectToAction("Index");
                }
                return View(faq);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
        }

        // GET: FAQ/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
                if (!Util.UserIsAdmin(Session)) return Redirect(Url.Content("~/Account/Login"));

                if (id == null)
                {
                    Log.Error("FAQ Delete - No ID provided", Session, null);
					Session["ErrMsg"] = "No ID Provided";
					return Redirect(Url.Content("~/Account/Login"));
					//return ErrorView("No ID provided");
                }
                FAQ faq = db.FAQSet.Find(id);
                if (faq == null)
                {
					Session["ErrMsg"] = "FAQ ID (" + id + ") not found.";
					return RedirectToAction("Index");
					//return HttpNotFound();
                }
                return View(faq);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
        }

        // POST: FAQ/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                FAQ faq = db.FAQSet.Find(id);
				faq.Deleted = true;
				faq.UpdateUser = int.Parse(Session["UserAccountId"].ToString());
				faq.UpdateDate = DateTime.Now;
                //db.FAQ.Remove(faq);
                db.SaveChanges();
				Session["UserMsg"] = "Deletion successful";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
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
