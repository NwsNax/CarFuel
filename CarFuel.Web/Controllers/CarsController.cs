using CarFuel.Models;
using CarFuel.Services;
using CarFuel.Services.Bases;
using CarFuel.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarFuel.Web.Controllers {
  public class CarsController : AppControllerBase {

    private readonly ICarService _carService;

    public CarsController(ICarService carService,
                          IUserService userService) : base(userService) {
      _carService = carService;
    }

    public ActionResult Index() {
      if (User.Identity.IsAuthenticated) {
        var id = new Guid(User.Identity.GetUserId());
        var cars = _carService.All();

        ViewBag.AppUser = _userService.CurrentUser;
        return View("IndexForMember",cars);
      }
      else {
        return View("IndexForAnonymous");
      }
    }

    public ActionResult Add() {     
      return View();
    }

    [HttpPost]
    public ActionResult Add(Car item) {
      ModelState.Remove("Owner");
      if (ModelState.IsValid) {

        User u = _userService.Find(new Guid(User.Identity.GetUserId()));
        item.Owner = u;

        _carService.Add(item);
        _carService.SaveChanges();
        return RedirectToAction("Index");
      }
      return View(item);
    }

    public ActionResult AddFillUp(int id) {
      var q = (from c in _carService.All()
               where c.Id == id
               select c.Name);
      var name = q.SingleOrDefault();


      ViewBag.CarName = name;
      return View();
    }

    [HttpPost]
    public ActionResult AddFillUp(int id, FillUp item) {
      if (ModelState.IsValid) {
        var c = _carService.Find(id);
        c.AddFillUp(item.Odometer, item.Liters);
        _carService.SaveChanges();

        return RedirectToAction("Index");
      }
      return View(item);
    }

  }
}