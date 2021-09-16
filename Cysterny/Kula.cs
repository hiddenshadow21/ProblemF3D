namespace Cysterny
{
    [Ksztalty(typeof(Kula), "Kula")]
    public class Kula : Cysterna
    {
        public int Promien { get; private set; }
        public Kula(string linia)
        {
            string[] dane = linia.Split(' ');
            PoziomPodstawy = int.Parse(dane[1]);
            Promien = int.Parse(dane[2]);
        }

        public override double MaxObjetosc()
        {
            return 4 * 3.14 * Promien * Promien * Promien / 3;
        }

        public override int maxWysokosc()
        {
            return PoziomPodstawy + Promien * 2;
        }

        public override double Wypelnienie(double poziomWody)
        {
            if (poziomWody <= PoziomPodstawy)
                return 0;

            if (poziomWody > PoziomPodstawy && poziomWody < PoziomPodstawy + Promien * 2)
            {
                double h = poziomWody - PoziomPodstawy;
                return 3.14 * h * h * Promien - 3.14 * h * h * h / 3;
            }

            return MaxObjetosc();
        }
    }
}
