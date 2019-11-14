using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Pathfinding
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string choice = "1";
            int speed = 50;

            while (choice !=  "0")
            {
                Console.WriteLine("Which maze would you like to show? (1,2,3,4,5  or 0 to exit");
                choice = Console.ReadLine().ToString();
                Console.Clear();

                Console.WriteLine("What speed to draw with? ( 1,5,50,100");
                speed = Convert.ToInt32(Console.ReadLine());
                Console.Clear();

                Maze1 maze1 = new Maze1();
                switch (choice)
                {
                    case "1":
                        maze1 = defineMaze1();
                        break;
                    case "2":
                        maze1 = defineMaze2();
                        break;
                    case "3":
                        maze1 = defineMaze3();
                        break;
                    case "4":
                        maze1 = defineMaze4();
                        break;
                    case "5":
                        maze1 = defineMaze5();
                        break;
                    case "6":
                        maze1 = defineMaze6();
                        break;
                }


                Stopwatch stopwatch = new Stopwatch();


                // Teken het doolhof
                
                string[] map = maze1.map;
                foreach (var line in map)
                    Console.WriteLine(line);
                Console.ReadLine().ToString();
                // algorithm

                stopwatch.Start();

                Location current = null;
                Location start = new Location { X = maze1.XA, Y = maze1.YA };
                Location target = new Location { X = maze1.XB, Y = maze1.YB };
                List<Location> considerations = new List<Location>();
                List<Location> visited = new List<Location>();
                int g = 0;
                // voeg startpositie toe aan mogelijke opties
                considerations.Add(start);
                int maxListSize = 0;
                while (considerations.Count > 0)
                {
                    if (considerations.Count > maxListSize)
                    {
                        maxListSize = considerations.Count;
                    }
                    // Zoek de volgende positie met de laagste F score
                    var lowest = considerations.Min(l => l.F);
                    current = considerations.First(l => l.F == lowest);

                    // Voeg huidige locatie toe aan visited
                    visited.Add(current);

                    // Laat huidige positie zien in console
                    Console.SetCursorPosition(current.X, current.Y);
                    Console.Write('.');
                    Console.SetCursorPosition(current.X, current.Y);
                    System.Threading.Thread.Sleep(speed);                                                                                                     //sleeps

                    // verwijder location uit mogelijke posities
                    considerations.Remove(current);

                    // Als huidige positie hetzelfde is als target positie zijn we klaar
                    if (visited.FirstOrDefault(l => l.X == target.X && l.Y == target.Y) != null)
                        break;

                    IEnumerable<Location> walkableLocations = GetWalkableAdjacentSquares(current.X, current.Y, map);
                    g++;

                    foreach (Location location in walkableLocations)
                    {
                        // Als de mogelijke location als bezocht is, negeer deze.
                        if (visited.FirstOrDefault(l => l.X == location.X
                                && l.Y == location.Y) != null)
                            continue;

                        // Als deze niet in de lijst van mogelijke opties staat, bereken de score en set de parent.
                        // en voeg deze toe aan de lijst van mogelijke opties.
                        if (considerations.FirstOrDefault(l => l.X == location.X
                                && l.Y == location.Y) == null)
                        {
                            location.G = g;
                            location.H = ComputeHScore(location.X, location.Y, target.X, target.Y);
                            location.F = location.G + location.H;
                            location.Parent = current;

                            considerations.Insert(0, location);
                        }
                        else
                        {
                            // Controleer dat als je de huidige G score gebruikt om de mogelijke locaties F score te berekenen, deze score lager is,
                            // Zo ja, dan is dit een betere route.
                            if (g + location.H >= location.F) continue;
                            location.G = g;
                            location.F = location.G + location.H;
                            location.Parent = current;
                        }
                    }
                }

                while (current != null)
                {
                    Console.SetCursorPosition(current.X, current.Y);
                    Console.Write('*');
                    Console.SetCursorPosition(current.X, current.Y);
                    current = current.Parent;
                    System.Threading.Thread.Sleep(speed);                                                                                                    //sleep
                }

                stopwatch.Stop();
                var total = stopwatch.ElapsedMilliseconds;
                Console.SetCursorPosition(1, 20);
                Console.WriteLine("Total Time taken : " + total + "ms");
                Console.WriteLine("Max number of items in storage is : " + maxListSize);
                Console.ReadLine();
            }
        }

        private static IEnumerable<Location> GetWalkableAdjacentSquares(int x, int y, string[] map)
        {
            List<Location> proposedLocations = new List<Location>()
            {
                new Location { X = x, Y = y - 1 },
                new Location { X = x, Y = y + 1 },
                new Location { X = x - 1, Y = y },
                new Location { X = x + 1, Y = y },
            };

            return proposedLocations.Where(l => map[l.Y][l.X] == ' ' || map[l.Y][l.X] == 'B').ToList();
        }

        private static int ComputeHScore(int x, int y, int targetX, int targetY)
        {
            return Math.Abs(targetX - x) + Math.Abs(targetY - y);
        }

        private static Maze1 defineMaze1()
        {
            Maze1 maze1 = new Maze1
            {
                XA = 2,
                XB = 2,
                YA = 2,
                YB = 5,
                map = new string[]
                {
                    "+-------------------+",
                    "|       X   X      X|",
                    "|A XXXX X X X XX   X|",
                    "|XXX    X     XX   X|",
                    "|   XX  X XXX XX   X|",
                    "| BX        X      X|",
                    "|  XXXXXXXXXXX XXXXX|",
                    "|                   |",
                    "+-------------------+",
                }
            };
            return maze1;
        }
        private static Maze1 defineMaze2()
        {
            Maze1 maze1 = new Maze1
            {
                XA = 1,
                XB = 3,
                YA = 1,
                YB = 1,
                map = new string[]
                {
                    "+---+",
                    "|AXB|",
                    "|   |",
                    "+---+"
                }
            };
            return maze1;
        }
        private static Maze1 defineMaze3()
        {
            Maze1 maze1 = new Maze1
            {
                XA = 1,
                XB = 2,
                YA = 2,
                YB = 5,
                map = new string[]
                {
                    "+------------------------------+",
                    "|       X   X           X     X|",
                    "|A XXXX X X X XXXXX  XXXX  X  X|",
                    "|XXX    X     XX           X  X|",
                    "|   XX  X XXX XX X  XXXXXXXX  X|",
                    "| BX        X    X  X         X|",
                    "|  XXXXXXXXXXXXXXXXXX  X X X XX|",
                    "|   XX  X     XX X  X         X|",
                    "|          X           X X X XX|",
                    "+------------------------------+",
                }
            };
            return maze1;
        }
        private static Maze1 defineMaze4()
        {
            Maze1 maze1 = new Maze1
            {
                XA = 1,
                XB = 2,
                YA = 2,
                YB = 13,
                map = new string[]
                {
                    "+------------------------------+",
                    "|       X   X           X     X|",
                    "|A XXXX X X X XXXXX  XXXX  X  X|",
                    "|XXX    X     XX           X  X|",
                    "|   XX  X XXX XX X  XXXXXXXX  X|",
                    "|  X        X    X  X         X|",
                    "|  XXXXXXXXXXXXXXXXXX  X X X XX|",
                    "|   XX  X     XX X  X         X|",
                    "|          X          XX XXXXXX|",
                    "|       X   X           X     X|",
                    "|  XXX  X X X XXXXXXXXXXX  X  X|",
                    "|XXX    X     XX           X  X|",
                    "|   XX  X XXX XX X  XXXXXXXX  X|",
                    "| BX        X    X  X         X|",
                    "|  XXXXXXXXXXXXXXXXXX  X X X XX|",
                    "|   XX  X     XX X  X         X|",
                    "|          X          XX X X XX|",
                    "+------------------------------+",
                }
            };
            return maze1;
        }
        private static Maze1 defineMaze5()
        {
            Maze1 maze1 = new Maze1
            {
                XA = 1,
                XB = 2,
                YA = 2,
                YB = 13,
                map = new string[]
                {
                    "+-----------------------------------------------+",
                    "|       X   X           X     X          X     X|",
                    "|A XXXX X X X XXXXX  XXXX  X  XXXXX  XXXX  X  XZ|",
                    "|XXX    X     XX           X  X           X  XXX|",
                    "|   XX  X XXX XX X  XXXXXXXX  X X  XXXXXXXX  XXX|",
                    "|  X        X    X  X         X   X  X         X|",
                    "|  XXXXXXXXXXXXXXXXXX  X X X XXXXXXX  X XXX X XX|",
                    "|   XX  X     XX X  X         X X  X         XXX|",
                    "|          X          XX XXXXX      XX XXXXX XXX|",
                    "|       X   X           X              X     XXX|",
                    "|  XXX  X X X XXXXXXXXXXX  XXXXXXXXXXXXX  X  XXX|",
                    "|XXX    X     XX           X  X           X  XXX|",
                    "|   XX  X XXX XX X  XXXXXXXX  XXX X  XXXXXXXX  X|",
                    "| BX        X    X  X         X   X           XX|",
                    "|  XXXXXXXXXXXXXXXXXX  X X X XXXXXXXXX  X X X XX|",
                    "|   XX  X     XX X  X         XXX X  X         X|",
                    "|          X          XX X X           XX X X XX|",
                    "+-----------------------------------------------+",
                }
            };
            return maze1;
        }

        private static Maze1 defineMaze6()
        {
            Maze1 maze1 = new Maze1
            {
                XA = 1,
                XB = 5,
                YA = 25,
                YB = 3,
                map = new string[]
                {
                    "+-----------------------------------------------+",
                    "|       X   X           X     X          X     X|",
                    "|  XXXX X X X XXXXX  XXXX  X  XXXXX  XXXX  X  XX|",
                    "| XX B  X     XX           X  X           X  XXX|",
                    "|   XX  X XXX XX X  XXXXXXXX  X X  XXXXXXXX  XXX|",
                    "|  X        X    X  X         X   X  X         X|",
                    "|  XXXXXXXXXXXXXXXXXX  X X X XXXXXXX  X XXX X XX|",
                    "|  XXXXXXXXXXXXXXXXXX  X X X XXXXXXX  X XXX X XX|",
                    "|   XX  X     XX X  X         X X  X         XXX|",
                    "|          X          XX XXXXX      XX XXXXX XXX|",
                    "|       X   X           X              X     XXX|",
                    "|  XXX  X X X XXXXXXXXXXX  XXXXXXXXXXXXX  X  XXX|",
                    "|XXX    X     XX           X  X           X  XXX|",
                    "|   XX  XXXXXXXX X  XXXXXXXX  XXX X  XXXXXXXX  X|",
                    "|  X             X  X         X   X           XX|",
                    "|  XXXXXXXXXXXXXXXXXX  X X X XXXXXXXXX  X X X XX|",
                    "|   XX  X     XX X  X         X X  X         XXX|",
                    "|          X          XX XXXXX      XX XXXXX XXX|",
                    "|       X   X           X              X     XXX|",
                    "|  XXX  X X X XXXXXXXXXXX  XXXXXXXXXXXXX  X  XXX|",
                    "|XXX    X     XX           X  X           X  XXX|",
                    "|   XX  X XXX XX X  XXXXXXXX  XXX X  XXXXXXXX  X|",
                    "|  X        X    X  X         X   X           XX|",
                    "|  XXXXXXXXXXXXXXXXXX  X X X XXXXXXXXX  X X X XX|",
                    "|   XX  X     XX X  X         XXX X  X         X|",
                    "|A         X          XX X X           XX X X XX|",
                    "+-----------------------------------------------+",
                }
            };
            return maze1;
        }

    }
}