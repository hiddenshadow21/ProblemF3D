namespace Cysterny
{
    [Ksztalty(typeof(Cylinder), "Cylinder")]
    public class Cylinder : Cysterna
    {
        public int Wysokosc { get; private set; }
        public int Promien { get; private set; }
        public Cylinder(string linia)
        {
            string[] dane = linia.Split(' ');
            PoziomPodstawy = int.Parse(dane[1]);
            Wysokosc = int.Parse(dane[2]);
            Promien = int.Parse(dane[3]);
        }

        public override double MaxObjetosc()
        {
            return 3.14 * Promien * Promien * Wysokosc;
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
                return 3.14 * Promien * Promien * (poziomWody - PoziomPodstawy);

            return MaxObjetosc();
        }
    }
}
