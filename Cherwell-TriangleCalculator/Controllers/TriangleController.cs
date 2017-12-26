using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CherwellTriangleCalculator.Controllers
{
    public class TriangleController : ApiController
    {
        // GET api/values/Triangle
        public Models.Coordinates Get(string t)
        {
            Models.Coordinates coordinates = Repositories.TriangleRepository.GenerateCoordinates(t);
            return coordinates;
        }

        // POST api/values
        public Models.GenerateRowAndColumnResult Post(Models.Coordinates coordinates)
        {
            Models.GenerateRowAndColumnResult result = Repositories.TriangleRepository.GenerateRowAndColumn(coordinates);
            return result;
        }
    }
}