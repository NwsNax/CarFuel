using CarFuel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Should;

namespace CarFuel.Tests.Models {
  public class CarTest {
    public class General {
      [Fact]
      public void InitialValues() {
        var c = new Car(name: "My Jazz");
        //Assert.Equal("My Jazz", c.Name);
        //Assert.Empty(c.FillUps);
        c.Name.ShouldEqual("My Jazz");
        c.FillUps.ShouldBeEmpty();
      }
    }

    public class AddFillUpMethod {

      private readonly ITestOutputHelper _output; 
      public AddFillUpMethod(ITestOutputHelper output) {
        _output = output;
      }

      [Fact]
      public void AddFirstFillUp() 
      {
        var c = new Car("Brio");
        FillUp f = c.AddFillUp(odometer: 1000, liters: 20.0);
        Assert.Equal(1, c.FillUps.Count());
        Assert.Equal(1000, f.Odometer);
        Assert.Equal(20.0, f.Liters);

      }

      [Fact]
      public void AddTwoFillUpsAndThemShouldBeChainedCorrectly() {
        var c1 = new Car("Brio");
        var f1 = c1.AddFillUp(1000, 30.0);
        var f2 = c1.AddFillUp(1500, 20.0);
        Assert.Same(f2, f1.NextFillUp);
      }

      [Fact]
      public void AddThreeFillUpsAndThemShouldBeChainedCorrectly() {
        var c1 = new Car("Brio");
        var f1 = c1.AddFillUp(1000, 30.0);
        var f2 = c1.AddFillUp(1500, 20.0);
        var f3 = c1.AddFillUp(2500, 50.0);

        Dump(c1);

        f1.NextFillUp.ShouldBeSameAs(f2);
        f2.NextFillUp.ShouldBeSameAs(f3);

        //Assert.Same(f2, f1.NextFillUp);
        //Assert.Same(f3, f2.NextFillUp);
      }

      //[Theory]
      ////[InlineData(1000, 50)]
      ////[InlineData(1000, 50)]    
      //[MemberData("RandomFillUpData", 50)]
      //public void AddSeveralFillUps(int odometer, double liters) {
      //  var c = new Car("Vios");
      //  c.AddFillUp(odometer, liters);
      //  c.FillUps.Count().ShouldEqual(1);

      //  Assert.IsType<Car>(c);
        
      //}

      public static IEnumerable<object[]> RandomFillUpData(int count) {
        var r = new Random();
        for (int i = 0; i < count; i++) {
          var odo = r.Next(0, 999999 + 1);
          var liters = r.Next(0, 9999 + 1) / 100.0;
          yield return new object[] { odo, liters };
        }
      }
      private void Dump(Car c) {
        _output.WriteLine("Car: {0}", c.Name);
        foreach(var f in c.FillUps) {
          _output.WriteLine($"{f.Odometer:000000} {f.Liters:n2} L. {f.KmL:n2} Km/L.");
        }
      }
    }

    public class AverageKmLProperty {

      [Fact]
      public void NoFillUp() {
        var c = new Car(name: "My Jazz");
        double? kml = c.AverageKmL;
        kml.ShouldBeNull();
      }

      [Fact]
      public void FirstFillUp() {
        var c = new Car(name: "My Jazz");
        c.AddFillUp(1000, 40.0);
        double? kml = c.AverageKmL;
        kml.ShouldBeNull();
        //var avg = c.AverageKmL;
        //Assert.Null(avg);
      }

      [Fact]
      public void TwoFillUp() {
        var c = new Car(name: "My Jazz");
        var f1 = c.AddFillUp(1000, 40.0);
        var f2 = c.AddFillUp(2000, 50.0);
        double? kml = c.AverageKmL;
        kml.ShouldEqual(f1.KmL);
        //Assert.Equal(20.0, kml);
      }

      [Fact]
      public void ThreeFillUp() {
        var c = new Car(name: "My Jazz");
        var f1 = c.AddFillUp(1000, 40.0);
        var f2 = c.AddFillUp(2000, 50.0);
        var f3 = c.AddFillUp(2500, 20.0);
        double? kml = c.AverageKmL;
        kml.ShouldEqual(21.43);        
      }
    }
  }

}
