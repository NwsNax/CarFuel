﻿using CarFuel.Models;
using CarFuel.Services;
using CarFuel.Tests.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Should;
using CarFuel.DataAccess.Bases;
using CarFuel.Services.Bases;
using Moq;

namespace CarFuel.Tests.Services {
  public class CarServiceTest {

    public class AddMethodWithMoq {
      [Fact]
      public void AddFirstCar_Success() {
        var user = new User { Displayname = "user1" };
        var mock = new Mock<IRepository<Car>>();
        var mockUser = new Mock<IUserService>();
        var s = new CarService(mock.Object,mockUser.Object);
        var c = new Car("Jazz") { Owner = user};
        c.Owner = user;

        mockUser.Setup(u => u.CurrentUser).Returns(user);
        //mock.Setup(r => r.Add(It.IsAny<Car>()));
        s.Add(c);
        //mock.Verify(r => r.Add(It.IsAny<Car>()), Times.Once);
        mock.Verify(r => r.Add(c), Times.Once);
      }

      [Fact]
      public void AddTwoCarsWithDupName_Failed() {
        var user = new User { Displayname = "user1" };
        var mock = new Mock<IRepository<Car>>();
        var mockUser = new Mock<IUserService>();
        mockUser.Setup(u => u.CurrentUser).Returns(user);
        var s = new CarService(mock.Object,mockUser.Object);
        var c1 = new Car("Jazz") { Owner = user };
        var c2 = new Car("Jazz") { Owner = user };

        var collection = new HashSet<Car>();
        mock.Setup(repo => repo.Add(It.IsAny<Car>()))
          .Callback<Car>((c) => {
            collection.Add(c);
        });
        mock.Setup(repo => repo.Query(It.IsAny<Func<Car, bool>>()))
          .Returns(collection.AsQueryable());

        s.Add(c1);
        var ex = Assert.Throws<Exception>(() => {
          s.Add(c2);
        });

        ex.Message.ShouldEqual("This name has been used already.");
        mock.Verify(r => r.Add(c1), Times.Once);
      }
    }

    //public class AddMethod {
    //  private IRepository<Car> db;
    //  private IService<Car> s;

    //  public AddMethod() {
    //    db = new FakeRepository<Car>();
    //    s = new CarService(db);
    //  }

    //  [Fact]
    //  public void AddFirstCar_Success() {
    //    var c = new Car("Toyota");

    //    s.Add(c);

    //    var cars = s.All();
    //    cars.Count().ShouldEqual(1);
    //  }

    //  [Fact]
    //  public void AddTwoCarsWithDuplicateName_ShouldNotBeAdded() {
    //    var c1 = new Car("Car A");
    //    var c2 = new Car("Car A");

    //    s.Add(c1);

    //    var ex = Assert.Throws<Exception>(() => {
    //      s.Add(c2);
    //    });

    //    ex.Message.ShouldEqual("This name has been used already.");
    //    s.All().Count().ShouldEqual(1);
    //  }
      
    //}
  }
}
