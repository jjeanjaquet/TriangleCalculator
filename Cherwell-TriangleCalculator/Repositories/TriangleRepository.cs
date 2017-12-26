using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace CherwellTriangleCalculator.Repositories
{
    public static class TriangleRepository
    {
        #region Public Methods
        // Generate the coordinates based on a string location of the triangle
        public static Models.Coordinates GenerateCoordinates(string triangle)
        {
            int pixelMultiplier = int.Parse(WebConfigurationManager.AppSettings["pixelMultiplier"]); 
            try
            {
                Models.Coordinates coordinates = new Models.Coordinates();

                //
                // Parse the inbound triangle row (A-F) to determine its vertical boundaries
                //
                string row = triangle.Substring(0, 1);
                int verticalLowerBoundary = CharacterIntCalculator(row);
                int verticalUpperBoundary = verticalLowerBoundary - pixelMultiplier;

                //
                // Use the column to determine the horizontal boundaries
                //
                int columnNumber = 0;
                int actualColumnNumber = 0;
                int.TryParse(triangle.Substring(1), out columnNumber);
                int horizontalRightBoundary = 0;
                int horizontalLeftBoundary = 0;

                //
                // Since the triangles are mirroed at the hypotenus, V1x and V1y depend on whether the column is even or odd
                //
                if (columnNumber > 0 && columnNumber % 2 == 0) //even
                {
                    actualColumnNumber = columnNumber / 2;
                    horizontalRightBoundary = actualColumnNumber * pixelMultiplier;
                    horizontalLeftBoundary = horizontalRightBoundary - pixelMultiplier;

                    coordinates.V1x = horizontalRightBoundary;
                    coordinates.V1y = verticalUpperBoundary;
                }
                else //odd, hopefully
                {
                    actualColumnNumber = (columnNumber + 1) / 2;
                    horizontalRightBoundary = actualColumnNumber * pixelMultiplier;
                    horizontalLeftBoundary = horizontalRightBoundary - pixelMultiplier;

                    coordinates.V1x = horizontalLeftBoundary;
                    coordinates.V1y = verticalLowerBoundary;
                }

                //
                // Set model values
                //
                coordinates.V2x = horizontalLeftBoundary;
                coordinates.V3x = horizontalRightBoundary;
                coordinates.V2y = verticalUpperBoundary;
                coordinates.V3y = verticalLowerBoundary;

                return coordinates;
            }
            catch (Exception ex)
            {
                // Don't do anything with the exception at this time - normally I would wrap my return object in a RepositoryResult object and return
                // a failed status message but I'll just return an empty object instead.
                return new Models.Coordinates();
            }
        } 

        public static Models.GenerateRowAndColumnResult GenerateRowAndColumn(Models.Coordinates coordinates)
        {
            int pixelMultiplier = int.Parse(WebConfigurationManager.AppSettings["pixelMultiplier"]); 
            try
            {
                //
                // All we need to determine which triangle is being referenced is by (V1x,V1Y) and one of the other values
                // I'm choosing (V3x,V3y) because I'm going to need to confirm the lower Horizontal axis to determine the row
                //
                string row;
                int column;

                //
                // Calculate the column, If V1y == V3y, then it's odd. Otherwise the column is even.
                //
                bool isOdd = coordinates.V1y == coordinates.V3y ? true : false;
                //
                // Make sure we are dealing with a right triangle
                //
                bool shouldProceed = ValidateRightTriangle(coordinates, isOdd);
                if (!shouldProceed)
                    throw new ApplicationException();

                if (isOdd)
                {
                    column = ((coordinates.V3x / pixelMultiplier) * 2) - 1;
                }
                else
                {
                    column = (coordinates.V3x / pixelMultiplier) * 2;
                }

                //
                // Calculate the row
                //
                row = CalculateRowFromCoordinate(coordinates.V3y);

                Models.GenerateRowAndColumnResult result = new Models.GenerateRowAndColumnResult()
                {
                    Column = column,
                    Row = row,
                    IsSuccessful = true
                };
                return result;
            }
            catch (Exception ex)
            {
                // Don't do anything with the exception at this time, just return an error
                return new Models.GenerateRowAndColumnResult()
                {
                    IsSuccessful = false
                };
            }
        }
        #endregion

        #region Private Methods
        // A repository method to take an inbound row and convert it to the proper integer (based on a 10X10 pixel right triangle)
        private static int CharacterIntCalculator(string row)
        {
            int pixelMultiplier = int.Parse(WebConfigurationManager.AppSettings["pixelMultiplier"]); 
            //
            // Convert to its character value, and make sure that we are using uppercase as "A" = 65 but "a" = 97, figured out the hard way
            // Then, multiply the value by 10. Ex. Row F = 60
            //
            int coordinate = 0;
            try
            {
                char character = Convert.ToChar(row.ToUpper());
                coordinate = (character - 64) * pixelMultiplier;
            }
            catch (Exception ex)
            {
                // Don't do anything with the exception at this time, just return 0
                return 0;
            }

            return coordinate;
        }

        // A repository method to take an inbound coordinate and convert it to the proper alphabetical value (based on a 10X10 pixel right triangle)
        private static string CalculateRowFromCoordinate(int coordinate)
        {
            int pixelMultiplier = int.Parse(WebConfigurationManager.AppSettings["pixelMultiplier"]); 
            //
            // Convert to its character value, and make sure that we are using uppercase as "A" = 65 but "a" = 97, figured out the hard way
            // Then, multiply the value by 10. Ex. Row F = 60
            //
            try
            {
                int integer = (coordinate / pixelMultiplier) + 64;
                char character = Convert.ToChar(integer);
                if (character < 'A' || character > 'F')
                    throw new ApplicationException();

                return Convert.ToString(character);
            }
            catch (Exception ex)
            {
                // Don't do anything with the exception at this time
                return "Could not calculate the row from the coordinate";
            }
        } 

        private static bool ValidateRightTriangle(Models.Coordinates coordinates, bool isOdd)
        {
            //
            // Use pythagorean theorem to determine if the triangle is a right triangle
            //
            double side1;
            double side2;
            double pixelMultiplier = double.Parse(WebConfigurationManager.AppSettings["pixelMultiplier"]);
            double correctSquaredHypotenus = (pixelMultiplier * pixelMultiplier) + (pixelMultiplier * pixelMultiplier);

            if (isOdd)
            {
                side1 = Math.Abs(coordinates.V3x - coordinates.V1x);
                side2 = Math.Abs(coordinates.V1y - coordinates.V2y);
            }
            else
            {
                side1 = Math.Abs(coordinates.V1x - coordinates.V2x);
                side2 = Math.Abs(coordinates.V3y - coordinates.V1y);
            }

            //
            // Validate all sides are divisible by the multiplier
            //
            if (side1 != pixelMultiplier || side2 != pixelMultiplier)
                return false;

            // Confirm right triangle
            if ((side1 * side1 + side2 * side2) <= correctSquaredHypotenus)
                return true;
            else
                return false;
        }
        #endregion
    }
}