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
    public class InvestmentController : ControllerBase
    {
        private DbCtxt db = DbCtxt.Create();

		// GET: Investment
		public ActionResult Index()
        {
            try
            {
                Log.Info("Investments Index", Session);
                return View(db.InvestmentSet.Where(i => !i.Deleted).ToList());
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
        }

        // GET: Investment/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
				if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
				if (!Util.UserIsAdmin(Session)) return Redirect(Url.Content("~/Account/Login"));

				if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Investment investment = db.InvestmentSet.Find(id);
                if (investment == null)
                {
                    return HttpNotFound();
                }
                return View(investment);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
        }

        // GET: Investment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Investment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Type,Title,Description,IRR,InceptionDate,LockPeriodDays,Closed,CreateUser,CreateDate,UpdateUser,UpdateDate,Status")] Investment investment)
        {
            try
            {
                Log.Info("Create Investment: " + investment.Id, Session);
                if (ModelState.IsValid)
                {
					investment.CreateUser = int.Parse(Session["UserAccountId"].ToString());
					investment.CreateDate = DateTime.Now;
					//investment.Status = (int)Util.InvStatus.Setup;
					db.InvestmentSet.Add(investment);
                    db.SaveChanges();
					Session["UsrMsg"] = "Investment created successfully";
					return RedirectToAction("Index");
                }

                return View(investment);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
        }

        // GET: Investment/Edit/5
        public ActionResult Edit(int? id)
        {
            Log.Info("Edit Investment (get): " + id, Session);

			if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
			if (!Util.UserIsAdmin(Session)) return Redirect(Url.Content("~/Account/Login"));

			if (id == null)
            {
                Log.Error("Inv Edit - No ID provided", Session, null);
                return ErrorView("No ID provided");
            }
            Investment investment = db.InvestmentSet.Find(id);
            if (investment == null)
            {
				return ErrorView("Investment ID (" + id + ") not found");
                //return HttpNotFound();
            }
            return View(investment);
        }

        // POST: Investment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Type,Title,Description,IRR,InceptionDate,LockPeriodDays,Closed,CreateUser,CreateDate,UpdateUser,UpdateDate,Status")] Investment investment)
        {
            try
            {
                Log.Info("Edit Investment: " + investment.Id, Session);
                if (ModelState.IsValid)
                {
					investment.UpdateUser = int.Parse(Session["UserAccountId"].ToString());
					investment.UpdateDate = DateTime.Now;

					db.Entry(investment).State = EntityState.Modified;
                    db.SaveChanges();

					Session["UsrMsg"] = "Update successful";
                    return RedirectToAction("Index");
                }
                return View(investment);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
        }

        // GET: Investment/Delete/5
        public ActionResult Delete(int? id)
        {
            Log.Info("Delete Investment (get): " + id, Session);
            if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
            if (!Util.UserIsAdmin(Session)) return Redirect(Url.Content("~/Account/Login"));

            if (id == null)
            {
                Log.Error("Inv Delete - No ID provided", Session, null);
                return ErrorView("No ID provided");
            }
            Investment investment = db.InvestmentSet.Find(id);
            if (investment == null)
            {
				return ErrorView("ID (" + id + ") Not Found");
				//return HttpNotFound();
            }
            return View(investment);
        }

        // POST: Investment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Log.Info("Delete Investment: " + id, Session);
                if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
                if (!Util.UserIsAdmin(Session)) return Redirect(Url.Content("~/Account/Login"));

                Investment investment = db.InvestmentSet.Find(id);
				investment.UpdateUser = int.Parse(Session["UserAccountId"].ToString());
				investment.UpdateDate = DateTime.Now;
				investment.Deleted = true;
                //db.Investments.Remove(investment);
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
