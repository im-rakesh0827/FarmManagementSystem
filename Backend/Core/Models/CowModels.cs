using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmSystem.Core.Models
{
    public class Cow
{
    public int Id { get; set; }
    public string TagNumber { get; set; } = string.Empty;
    public string Breed { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public bool IsMilking { get; set; }
}

}