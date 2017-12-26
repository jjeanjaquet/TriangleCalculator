using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CherwellTriangleCalculator.Models
{
    public class Coordinates
    {
        //The X-Coordinate of the first vertex
        public int V1x { get; set; }

        //The Y-Coordinate of the first vertex
        public int V1y { get; set; }

        //The X-Coordinate of the second vertex
        public int V2x { get; set; }

        //The Y-Coordinate of the second vertex
        public int V2y { get; set; }

        //The X-Coordinate of the third vertex
        public int V3x { get; set; }

        //The Y-Coordinate of the third vertex
        public int V3y { get; set; }
    }
}