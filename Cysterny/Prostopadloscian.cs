namespace Cysterny
{
    [Ksztalty(typeof(Prostopadloscian), "Prostopadloscian")]
    public class Prostopadloscian : Cysterna
    {
        public int Szerokosc { get; private set; }
        public int Wysokosc { get; private set; }
        public int Dlugosc { get; private set; }

        public Prostopadloscian(string linia)
        {
            string[] dane = linia.Split(' ');
            PoziomPodstawy = int.Parse(dane[1]);
            Wysokosc = int.Parse(dane[2]);
            Szerokosc = int.Parse(dane[3]);
            Dlugosc = int.Parse(dane[4]);
        }

        public override double MaxObjetosc()
        {
            return Szerokosc * Wysokosc * Dlugosc;
        }

        public override double Wypelnienie(double poziomWody)
        {
            if (poziomWody <= PoziomPodstawy)
                return 0;

            if (poziomWody > PoziomPodstawy && poziomWody < PoziomPodstawy + Wysokosc)
                return Szerokosc * (poziomWody - PoziomPodstawy) * Dlugosc;

            return MaxObjetosc();
        }

        public override int maxWysokosc()
        {
            return PoziomPodstawy + Wysokosc;
        }
    }
}
