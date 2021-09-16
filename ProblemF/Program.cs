using Cysterny;
using System;
using System.Globalization;
using System.IO;
using static Cysterny.Zadanie;

namespace ProblemF
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                foreach (var ksztalt in KsztaltyAttribute.DostepneKsztalty())
                {
                    Console.WriteLine($"{ksztalt.Nazwa}");
                }
                using (var sr = new StreamReader("..\\..\\..\\input.txt"))
                {
                    Zadanie zadanie = new Zadanie();
                    string linia = sr.ReadLine();
                    int n = int.Parse(linia);
                    for (int i = 0; i < n; i++)
                    {
                        zadanie.Wczytaj(sr);
                        try
                        {
                            zadanie.ObliczPoziomWody();
                            Console.WriteLine(zadanie.PoziomWody.ToString("F2", new CultureInfo("en-us")));
                        }
                        catch (Overflow e)
                        {
                            Console.WriteLine(e.Message);
                        }

                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }
    }
}
