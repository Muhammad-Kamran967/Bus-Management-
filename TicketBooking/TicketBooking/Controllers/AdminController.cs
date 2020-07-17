using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketBooking.Models;

namespace TicketBooking.Controllers
{
    public class AdminController : Controller
    {
        TicketBookingEntities _context = new TicketBookingEntities();
        // GET: Admin
        public ActionResult Index(int? Id)
        {
            if (Session["Id"]  != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }
        public ActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(Admin admin)
        {
            var result = _context.Admins.FirstOrDefault(o => o.UserName.Equals(admin.UserName) && o.Password.Equals(admin.Password));
            if (result != null)
            {
                Session["Id"] = result.Id;
                Session["UserName"] = result.UserName;
                return RedirectToAction("Index");
            }
            TempData["Message"] = "Invalid User Details";
            return View();
        }
        public ActionResult LogOut()
        {
            Session.RemoveAll();
            return RedirectToAction("LogIn");
        }
        public ActionResult BusService(BusService busService, HttpPostedFileBase Image)
        {
            if (Session["Id"] != null)
            {
                return View(_context.BusServices.ToList());
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }
        public ActionResult AddBusService(int? Id, HttpPostedFileBase Image)
        {
            if (Session["Id"] != null)
            {
                var busService = _context.BusServices.Where(o => o.Id == Id).FirstOrDefault();
                if (busService != null)
                {
                    var find = _context.BusServices.Find(Id);
                    return View(find);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }
        [HttpPost]
        public ActionResult AddBusService(BusService busService,HttpPostedFileBase UpdateImage, HttpPostedFileBase Image)
        {
            if (busService.Id > 0)
            {
                if (UpdateImage != null)
                {
                    string filename = UpdateImage.FileName;
                    busService.Image = filename;
                    string path = Path.Combine(Server.MapPath("/Images"), filename);
                    UpdateImage.SaveAs(path);
                }
                _context.Entry(busService).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("BusService");
            }
            else
            {
                string filename = Image.FileName;
                busService.Image = filename;
                string path = Path.Combine(Server.MapPath("/Images"), filename);
                Image.SaveAs(path);
                _context.BusServices.Add(busService);
                _context.SaveChanges();
                return RedirectToAction("BusService");
            }
        }
        public ActionResult DeleteBusService(int Id)
        {
            var delete = _context.BusServices.Where(o=>o.Id==Id).FirstOrDefault();
            _context.BusServices.Remove(delete);
            _context.SaveChanges();
            return RedirectToAction("BusService");
        }
        public ActionResult BusDetails(Bus bus,HttpPostedFileBase Image)
        {
            if (Session["Id"] != null)
            {
                return View(_context.Buses.ToList());
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }
        public ActionResult AddBusDetails(int? Id,HttpPostedFileBase Image)
        {
            if (Session["Id"] != null)
            {
                TempData["BusService"] = _context.BusServices.ToList();
                var bus = _context.Buses.Where(o => o.Id == Id).FirstOrDefault();
                if (bus != null)
                {
                    var find = _context.Buses.Find(Id);
                    return View(find);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }
        [HttpPost]
        public ActionResult AddBusDetails(Bus bus, HttpPostedFileBase UpdateImage,HttpPostedFileBase Image)
        {
            TempData["BusService"] = _context.BusServices.ToList();
            if (bus.Id > 0)
            {
                if(UpdateImage != null)
                {
                    string filename = UpdateImage.FileName;
                    bus.Image = filename;
                    string path = Path.Combine(Server.MapPath("/Images"), filename);
                    UpdateImage.SaveAs(path);
                }
                _context.Entry(bus).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("BusDetails");
            }
            else
            {
                string filename = Image.FileName;
                bus.Image = filename;
                string path = Path.Combine(Server.MapPath("/Images"), filename);
                Image.SaveAs(path);
                _context.Buses.Add(bus);
                _context.SaveChanges();
                return RedirectToAction("BusDetails");
            }
        }
        public ActionResult DeleteBusDetails(int Id)
        {
            var deletebus = _context.Buses.Where(o => o.Id == Id).FirstOrDefault();
            _context.Buses.Remove(deletebus);
            _context.SaveChanges();
            return RedirectToAction("BusDetails");
        }
        public ActionResult BusRoutes(int? Id)
        {
            if (Session["Id"] != null)
            {
                return View(_context.BusRoutes.ToList());
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }
        public ActionResult AddBusRoutes(int? Id)
        {
            if (Session["Id"] != null)
            {
                TempData["Route"] = _context.Routes.ToList();
                TempData["Bus"] = _context.Buses.ToList();
                var busRoutes = _context.BusRoutes.Where(o => o.Id == Id).FirstOrDefault();
                if (busRoutes != null)
                {
                    var find = _context.BusRoutes.Find(Id);
                    return View(find);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }
        [HttpPost]
        public ActionResult AddBusRoutes(int? Id,BusRoute busRoute)
        {
            TempData["Route"] = _context.Routes.ToList();
            TempData["Bus"] = _context.Buses.ToList();
            if (busRoute.Id > 0)
            {
                _context.Entry(busRoute).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("BusRoutes");
            }
            else
            {
                _context.BusRoutes.Add(busRoute);
                _context.SaveChanges();
                return RedirectToAction("BusRoutes");
            }
        }
        public ActionResult DeleteBusRoutes(int Id)
        {
            var deleteroute = _context.BusRoutes.Where(o => o.Id == Id).FirstOrDefault();
            _context.BusRoutes.Remove(deleteroute);
            _context.SaveChanges();
            return RedirectToAction("BusRoutes");
        }
        public ActionResult Routes()
        {
            if (Session["Id"] != null)
            {
                return View(_context.Routes.ToList());
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }
        public ActionResult AddRoutes(int? Id)
        {
            if (Session["Id"] != null)
            {
                TempData["Cities"] = GetCities();
                var routes = _context.Routes.Where(o => o.Id == Id).FirstOrDefault();
                if (routes != null)
                {
                    var find = _context.Routes.Find(Id);
                    return View(find);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }
        [HttpPost]
        public ActionResult AddRoutes(Route route)
        {
            TempData["Cities"] = GetCities();
            if (route.Id > 0)
            {
                _context.Entry(route).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Routes");
            }
            else
            {
                _context.Routes.Add(route);
                _context.SaveChanges();
                return RedirectToAction("Routes");
            }
        }
        public ActionResult DeleteRoutes(int Id)
        {
            var deleteroute = _context.Routes.Where(o => o.Id == Id).FirstOrDefault();
            _context.Routes.Remove(deleteroute);
            _context.SaveChanges();
            return RedirectToAction("Routes");
        }
        public List<Cities> GetCities()
        {
            var list = new List<Cities>();
            var city1 = new Cities { Text = "Lahore", Value = "Lahore" };
            var city2 = new Cities { Text = "Islamabad", Value = "Islamabad" };
            var city3 = new Cities { Text = "Peshawar", Value = "Peshawar" };
            var city4 = new Cities { Text = "Karachi", Value = "Karachi" };
            var city5 = new Cities { Text = "Multan", Value = "Multan" };
            var city6 = new Cities { Text = "Sahiwal", Value = "Sahiwal" };
            var city7 = new Cities { Text = "Bahawalpur", Value = "Bahawalpur" };
            var city8 = new Cities { Text = "Gujrat", Value = "Gujrat" };
            var city9 = new Cities { Text = "Gujranwala", Value = "Gujranwala" };
            var city10 = new Cities { Text = "Sialkot", Value = "Sialkot" };
            list.Add(city1);
            list.Add(city2);
            list.Add(city3);
            list.Add(city4);
            list.Add(city5);
            list.Add(city6);
            list.Add(city7);
            list.Add(city8);
            list.Add(city9);
            list.Add(city10);
            return list;
        }
    }
}