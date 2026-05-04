using Microsoft.EntityFrameworkCore;

namespace PasientSimulator.lib.Models;

[Owned]
public class BloodPressure
{
    public int Systolic { get; set; }
    public int Diastolic { get; set; }
}