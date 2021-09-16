using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cysterny
{
    public class Zadanie
    {
        public class Overflow : Exception
        {
            public Overflow(string message) : base(message)
            {
            }
        }
        public double PoziomWody { get; private set; }
        private int iloscWody;
        public List<Cysterna> cysterny { get; private set; }
        public Zadanie() { }

        public bool Wczytaj(TextReader tr)
        {
            try
            {
                int n = int.Parse(tr.ReadLine());
                cysterny = new List<Cysterna>(n);
                for (int i = 0; i < n; i++)
                {
                    DodajCysterne(tr);
                }
                iloscWody = int.Parse(tr.ReadLine());
                PoziomWody = -1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        private void DodajCysterne(TextReader tr)
        {
            string linia = tr.ReadLine();
            var ksztalty = KsztaltyAttribute.DostepneKsztalty();

            foreach (var ksztalt in ksztalty)
            {
                if(ksztalt.Nazwa.ToLower()[0] == linia[0])
                {
                    Cysterna cysterna = (Cysterna)Activator.CreateInstance(ksztalt.Type,linia);
                    cysterny.Add(cysterna);
                }
            }
        }

        public void ObliczPoziomWody()
        {
            double maxObj = 0;
            foreach (var cysterna in cysterny)
            {
                maxObj += cysterna.MaxObjetosc();
            }
            if (maxObj < iloscWody)
                throw new Overflow("OVERFLOW");

            double maxPoziomWody = cysterny.Max(x => x.maxWysokosc());
            PoziomWody = PolowieniePrzedzialow(0, maxPoziomWody, 0.01);
        }

        private double WykorzystanieWody(double poziomWody)
        {
            double sumaObj = 0;
            foreach (var cysterna in cysterny)
            {
                sumaObj += cysterna.Wypelnienie(poziomWody);
            }
            return sumaObj;
        }

        private double PolowieniePrzedzialow(double a, double b, double epsilon)
        {
            
            double srodek, fs, fa;
            fa = WykorzystanieWody(a);
            if (fa == iloscWody) return a;
            if (WykorzystanieWody(b) == iloscWody) return b;

            

            while (b - a > epsilon)
            {
                srodek = (a + b) / 2;
                fs = WykorzystanieWody(srodek);
                if ( fs == iloscWody)
                {
                    b = srodek;
                    continue;
                }
                if ((iloscWody - fa) * (iloscWody - fs) < 0)
                    b = srodek;
                else
                {
                    a = srodek;
                    fa = fs;
                }
            }
            return (a + b) / 2;
        }
    }
}