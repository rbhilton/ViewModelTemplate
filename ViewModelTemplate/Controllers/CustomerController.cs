using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using ViewModelTemplate.Models;

namespace ViewModelTemplate.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult customer()
        {
            DBRepository dbr = new DBRepository();
            List<Customer> customers = dbr.getCustomers();
            return View("customer",customers);
        }

        public ActionResult Orders(string id)
        {
            DBRepository dbr = new DBRepository();
            CustomerOrders customerOrders = dbr.getCustomerOrders(id);

            return View(customerOrders);
        }

        // GET: Customer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Customer/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Customer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
