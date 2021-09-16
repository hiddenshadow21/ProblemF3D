namespace Cysterny
{
    public abstract class Cysterna
    {
        public int PoziomPodstawy { get; protected set; }
        public abstract double MaxObjetosc();
        public abstract double Wypelnienie(double poziomWody);
        public abstract int maxWysokosc();
    }
}