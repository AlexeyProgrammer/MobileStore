using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MobileStore.Models;

namespace MobileStore.Controllers
{
    public class HomeController : Controller
    {
        MobileContext db;
        public HomeController(MobileContext context)
        {
            db = context;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View(db.Phones.ToList());
        }
        [HttpGet]
        public IActionResult Buy(int? id)
        {
            if (id == null) return RedirectToAction("Index");
            ViewBag.PhoneId = id;
            return View();
        }
        [HttpPost]
        public IActionResult Buy(Order order)
        {
            db.Orders.Add(order);
            Phone phone =  db.Phones.FirstOrDefault(p => p.Id == order.PhoneId);
            if (phone != null)
            { db.Phones.Remove(phone);
                db.SaveChanges();
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Phone phone)
        {
            db.Phones.Add(phone);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Change(int? id)
        {
            if (id != null)
            {
                Phone phone = db.Phones.FirstOrDefault(p => p.Id == id);
                if (phone != null)
                    return View(phone);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Change(Phone phone)
        {
            db.Phones.Update(phone);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(Phone phone)
        {
            db.Phones.Remove(phone);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Double (Phone phoneFromBody)
        {
            if(phoneFromBody.Id != null)
            {
                Phone phone = db.Phones.FirstOrDefault(p => p.Id == phoneFromBody.Id);
                if(phone != null)
                {   var phone1 = new Phone();
                    phone1.Name = phone.Name;
                    phone1.Company = phone.Company;
                    phone1.Price = phone.Price;
                    db.Phones.Add(phone1);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult Rand()
        {
            for (int i = 0; i < 3; i++)
            {
                var phone2 = new Phone();
                phone2.Name = RandomString(8);
                phone2.Company = RandomString(6);
                phone2.Price = random.Next(1000, 10000);
                db.Phones.Add(phone2);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        private static Random random = new Random();
        public static string RandomString (int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPRSTUVWXYZ0123456789@/";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
