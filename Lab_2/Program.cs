﻿using System.Text.RegularExpressions;

namespace Lab_2;

internal class Program
{
    private static void Main(string[] args)
    {
        var values = new List<string[]>();
        var shapeCircle = new List<Circle>();
        var shapeSquare = new List<Square>();
        var hitSquare = new List<double>();
        var missSquare = new List<double>();
        var hitCircle = new List<double>();
        var missCircle = new List<double>();

        var startIndexHeader = 0;
        var lengthIndexHeader = 23;

        try
        {
            var readInputDot = args[0];
            var stringInput = args[1].ToUpper();
            var inputDot = string.Concat(readInputDot.Where(c => !char.IsWhiteSpace(c)));
            var trimmedInputDot = inputDot.Substring(1, inputDot.Length - 2);
            var concatString = string.Concat(stringInput.Where(c => !char.IsWhiteSpace(c)));
            var substringHeader = concatString.Substring(startIndexHeader, lengthIndexHeader);
            var dotValue = Regex.Split(trimmedInputDot, @",");
            var headerVariables = Regex.Split(substringHeader, @",|;");
            var listVariables = Regex.Split(concatString, @";");
            var inputDotX = Convert.ToInt32(dotValue[0]);
            var inputDotY = Convert.ToInt32(dotValue[1]);

            // Find index position for headers
            var indexShape = Array.FindIndex(headerVariables, row => row.Contains("SHAPE"));
            var indexX = Array.FindIndex(headerVariables, row => row.Contains("X"));
            var indexY = Array.FindIndex(headerVariables, row => row.Contains("Y"));
            var indexPoints = Array.FindIndex(headerVariables, row => row.Contains("POINTS"));
            var indexLength = Array.FindIndex(headerVariables, row => row.Contains("LENGTH"));

            for (var i = 1; i <= listVariables.Length - 1; i++)
            {
                var seperateValues = Regex.Split(listVariables[i], @",");
                values.Add(seperateValues);
            }

            // Remove last array
            values.RemoveAt(values.Count - 1);

            foreach (var row in values)
            {
                var x = Convert.ToInt32(row[indexX]);
                var y = Convert.ToInt32(row[indexY]);
                var length = Convert.ToInt32(row[indexLength]);
                var points = Convert.ToInt32(row[indexPoints]);

                if (row[indexShape] == "CIRCLE")
                    shapeCircle.Add(new Circle(x, y, length, points));
                else if (row[indexShape] == "SQUARE") shapeSquare.Add(new Square(x, y, length, points));
            }

            var dot = new Dot(inputDotX, inputDotY);

            foreach (var c in shapeCircle)
                if (c.Hit(dot.dotX, dot.dotY))
                    hitCircle.Add(c.shapeScore());
                else
                    missCircle.Add(c.shapeScore());

            foreach (var s in shapeSquare)
                if (s.Hit(dot.dotX, dot.dotY))
                    hitSquare.Add(s.shapeScore());
                else
                    missSquare.Add(s.shapeScore());

            // Output of the result
            var game = new Game(missCircle, missSquare, hitSquare, hitCircle);
            Console.WriteLine("Poängresultat: " + game.score);
        }
        catch (Exception)
        {
            Console.WriteLine("Du matade in felaktiga värden. Var god försök igen.");
        }
    }
}