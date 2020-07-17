using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketBooking.Models;

namespace TicketBooking.Controllers
{
    public class HomeController : Controller
    {
        TicketBookingEntities _context = new TicketBookingEntities();
        public ActionResult Index()
        {
            return View(_context.BusServices.ToList());
        }
        public ActionResult BusServices(int? Id)
        {
            var busService = _context.BusServices.FirstOrDefault();
            var bus = _context.Buses.Where(o => o.BusServiceId == Id).ToList();
            return View(bus);
        }
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return View();
        }
        public ActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(User user)
        {
            var result = _context.Users.FirstOrDefault(o => o.Email.Equals(user.Email) && o.Password.Equals(user.Password));
            if (result != null)
            {
                Session["Id"] = result.Id;
                return RedirectToAction("BookingDetails");
            }
            TempData["Message"] = "Invalid User Details";
            return View();
        }
        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Index");
        }
        public ActionResult BookingServices()
        {
            return View(_context.BusRoutes.ToList());
        }
        public ActionResult BookingDetails()
        {
            TempData["Cities"] = GetCities();
            TempData["BusRoute"] = _context.BusRoutes.ToList();
            return View(_context.BusRoutes.ToList());
        }
        [HttpPost]
        public ActionResult BookingDetails(Route route,BusRoute busRoute)
        {
            TempData["Cities"] = GetCities();
            TempData["BusRoute"] = _context.BusRoutes.ToList();
            var search = _context.BusRoutes.Where(o => o.Route.FromRoute.Contains(route.FromRoute) && o.Route.ToRoute.Contains(route.ToRoute) && o.DepartureTime == busRoute.DepartureTime).ToList();
            if (search.Count() != 0)
            { 
            return View(search);
            }
            else
            {
                return RedirectToAction("BookingDetails");
            }
        }
        public ActionResult Book(int? Id)
        {
            var check = _context.BusRoutes.Where(o => o.Id == Id).FirstOrDefault();
            return View(check);
        }
        [HttpPost]
        public ActionResult Book(int? Id,int BusRouteId, BusRoute busRoute,UserTicket userTicket)
        {
            var route = _context.BusRoutes.Where(o => o.Id == userTicket.BusRouteId).FirstOrDefault();
            var fare = _context.Buses.Where(o => o.Id == route.BusId).FirstOrDefault();
            List<string> tickets = new List<string>();
            for(var i = 1; i <= userTicket.ReservedSeats; i++)
            {
                var randomTicket = Guid.NewGuid().ToString().Substring(0, 6);
                tickets.Add(randomTicket);
            }
            string multipleTickets = string.Join(",", tickets);
            userTicket.TicketId = multipleTickets;
            _context.SaveChanges();
            if (userTicket.ReservedSeats >= 1)
            { 
                userTicket.PaidFare = fare.Price * userTicket.ReservedSeats;
            }
            busRoute.Id = route.Id;
            busRoute.BusId = route.BusId;
            busRoute.RouteId = route.RouteId;
            busRoute.ArrivalTime = route.ArrivalTime;
            busRoute.DepartureTime = route.DepartureTime;
            busRoute.ReservedSeats = route.ReservedSeats + userTicket.ReservedSeats;
            _context.Set<BusRoute>().AddOrUpdate(busRoute);
            _context.SaveChanges();
            _context.UserTickets.Add(userTicket);
            _context.SaveChanges();
            return RedirectToAction("Ticket", new { userTicket.Id });
        }
        public ActionResult Ticket(int Id)

        {
            var ticket = _context.UserTickets.Find(Id);
            return View(ticket);
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