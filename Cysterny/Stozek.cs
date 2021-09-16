namespace Cysterny
{
    [Ksztalty(typeof(Stozek), "Stozek")]
    public class Stozek : Cysterna
    {
        public int Promien { get; private set; }
        public int Wysokosc { get; private set; }
        public Stozek(string linia)
        {
            string[] dane = linia.Split(' ');
            PoziomPodstawy = int.Parse(dane[1]);
            Wysokosc = int.Parse(dane[2]);
            Promien = int.Parse(dane[3]);
        }

        public override double MaxObjetosc()
        {
            return 3.14 * Promien * Promien * Wysokosc / 3;
        }

        public override int maxWysokosc()
        {
            return PoziomPodstawy + Wysokosc;
        }

        public override double Wypelnienie(double poziomWody)
        {
            if (poziomWody <= PoziomPodstawy)
                return 0;

            if (poziomWody > PoziomPodstawy && poziomWody < PoziomPodstawy + Wysokosc)
            {
                double h = Wysokosc - poziomWody;
                double r = h * Promien / Wysokosc;
                return MaxObjetosc() - 3.14 * r * r * h / 3;
            }

            return MaxObjetosc();
        }
    }
}
