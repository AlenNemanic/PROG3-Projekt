using System;

namespace Projekt
{
    public class Dvojisko_drevo<T> where T : IComparable<T>
    {
        public Dvojisko_drevo(T vrednost)
        {
            Vrednost = vrednost;
            Levo = null;
            Desno = null;
        }

        public T Vrednost { get; set; }
        public Dvojisko_drevo<T> Levo { get; set; }
        public Dvojisko_drevo<T> Desno { get; set; }

        public bool Iskanje(T vrednost)
        {
            if (Vrednost == null)
                return false;
            if (Vrednost.Equals(vrednost))
                return true;
            if (Levo != null && Levo.Iskanje(vrednost))
                return true;
            if (Desno != null && Desno.Iskanje(vrednost))
                return true;
            return false;
        }
    }
}