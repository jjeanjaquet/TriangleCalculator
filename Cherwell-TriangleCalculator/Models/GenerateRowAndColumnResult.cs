using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CherwellTriangleCalculator.Models
{
    public class GenerateRowAndColumnResult
    {
        public string Row { get; set; }

        public int Column { get; set; }

        public bool IsSuccessful { get; set; }
    }
}