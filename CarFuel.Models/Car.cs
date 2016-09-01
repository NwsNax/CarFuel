using CarFuel.Models;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace CarFuel.Models {
  public class Car {

    public Car(): this("Car") {      
    }

    public Car(string name) {
      FillUps = new HashSet<FillUp>();
      Name = name;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public double? AverageKmL {
      get {
        if (FillUps.Count <= 1) return null;

        if (FillUps.Count == 2) return FillUps.First().KmL;

        var first = FillUps.First();
        var last = FillUps.Last();
        var sumLiters = FillUps.Sum(f => f.Liters) - first.Liters;
        var kml = (last.Odometer - first.Odometer) / sumLiters;

        return Math.Round(kml, 2,MidpointRounding.AwayFromZero);       
      }
    }
    public virtual ICollection<FillUp> FillUps { get; set; }

    [Required]
    public virtual User Owner { get; set; }

    public FillUp AddFillUp(int odometer, double liters) {
      FillUp f = new FillUp(odometer, liters);
      if(FillUps.Count > 0) {        
        FillUps.Last().NextFillUp = f;
      }
      FillUps.Add(f);

      return f;
    }

   
  }
}