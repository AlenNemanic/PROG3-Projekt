using System;

namespace Projekt
{
    public class Dvojisko_drevo<T> where T : IComparable<T>
    {
        private T _podatek;
        private Dvojisko_drevo<T> _levoPoddrevo;
        private Dvojisko_drevo<T> _desnoPoddrevo;
        private bool _prazno;

        // Konstruktor
        public Dvojisko_drevo(T podatek = default(T), Dvojisko_drevo<T> levo = null, Dvojisko_drevo<T> desno = null)
        {
            _podatek = podatek;
            _levoPoddrevo = levo;
            _desnoPoddrevo = desno;
            _prazno = false;
            if (podatek == null && levo == null && desno == null)
                _prazno = true;
        }

        // Lastnost za pridobivanje in nastavljanje podatka v korenu
        public T Podatek
        {
            get { return _podatek; }
            set { _podatek = value; }
        }

        // Lastnost za pridobivanje in nastavljanje levega poddrevesa
        public Dvojisko_drevo<T> Levo
        {
            get { return _levoPoddrevo; }
            set { _levoPoddrevo = value; }
        }

        // Lastnost za pridobivanje in nastavljanje desnega poddrevesa
        public Dvojisko_drevo<T> Desno
        {
            get { return _desnoPoddrevo; }
            set { _desnoPoddrevo = value; }
        }

        // Lastnost za preverjanje ali je drevo prazno
        public bool Prazno
        {
            get { return _prazno; }
            private set { _prazno = value; }
        }

        // Metoda za iskanje vrednosti v drevesu
        public bool Iskanje(T vrednost)
        {
            if (Prazno)
                return false;
            if (Podatek.Equals(vrednost))
                return true;
            if (!Levo.Prazno && Levo.Iskanje(vrednost))
                return true;
            if (!Desno.Prazno && Desno.Iskanje(vrednost))
                return true;
            return false;
        }
    }
}