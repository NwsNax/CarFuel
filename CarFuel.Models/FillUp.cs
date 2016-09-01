using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarFuel.Models {

  //change table name
  [Table("tblFillUp")]
  public class FillUp {
    public FillUp() {
    }

    [Key]
    public int Id { get; set; }

    //bring math is column
    //if(change columnname) make below
    [Column("IS_FULL")]
    public bool IsFull { get; set; }

    [Range(0.0, 100.0)]
    public double Liters { get; set; }

    //Navigation Properties
    //make it "virtual" to enable lazy-loading.
    public virtual FillUp NextFillUp { get; set; }

    [Range(0,999999)]
    public int Odometer { get; set; }

    public FillUp(int odometer, double liters, bool isFull = true) {
      Odometer = odometer;
      Liters = liters;
      IsFull = isFull;
    }

    public double? KmL {
      get {
        if (NextFillUp == null)
          return null;
        if (Odometer > NextFillUp.Odometer)
          throw new Exception("Odermeter should be grater than the previous one.");


          return (NextFillUp.Odometer - Odometer) / NextFillUp.Liters;
      }
    }
  }
}