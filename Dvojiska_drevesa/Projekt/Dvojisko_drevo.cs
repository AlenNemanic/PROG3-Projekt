using System;
using System.Collections.Generic;
using System.Text;

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
            _levoPoddrevo = levo ?? new Dvojisko_drevo<T>();
            _desnoPoddrevo = desno ?? new Dvojisko_drevo<T>();
            _prazno = podatek == null && levo == null && desno == null;
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

        // Metoda za sestavljanje drevesa
        public static Dvojisko_drevo<T> Sestavi(T podatekVKorenu, Dvojisko_drevo<T> levoDrevo, Dvojisko_drevo<T> desnoDrevo)
        {
            return new Dvojisko_drevo<T>(podatekVKorenu, levoDrevo, desnoDrevo);
        }

        // Metoda za obhod drevesa
        public static string Obhod(Dvojisko_drevo<T> drevo, string vzorec)
        {
            if (drevo.Prazno)
                return "";

            StringBuilder vrni = new StringBuilder();
            foreach (char znak in vzorec)
            {
                if (znak == 'l')
                    vrni.Append(Obhod(drevo.Levo, vzorec));
                else if (znak == 'd')
                    vrni.Append(Obhod(drevo.Desno, vzorec));
                else if (znak == 'k')
                    vrni.Append(drevo.Podatek.ToString()).Append(",");
                else
                    throw new Exception($"Napačen znak v obhodu ({znak}). Dovoljeni znaki so 'd', 'k' in 'l'.");
            }
            return vrni.ToString();
        }

        // Metoda za sestavljanje drevesa iz tabele
        public static Dvojisko_drevo<T> SestaviIzTabele(T[] tabela, int polozajKorena = 1)
        {
            if (polozajKorena >= tabela.Length || EqualityComparer<T>.Default.Equals(tabela[polozajKorena], default(T)))
                return new Dvojisko_drevo<T>();

            Dvojisko_drevo<T> levo = SestaviIzTabele(tabela, 2 * polozajKorena);
            Dvojisko_drevo<T> desno = SestaviIzTabele(tabela, 2 * polozajKorena + 1);
            return Sestavi(tabela[polozajKorena], levo, desno);
        }

        // Metoda za pretvorbo drevesa v niz
        public override string ToString()
        {
            try
            {
                string izpis = Obhod(this, "lkd");
                return "[" + izpis.TrimEnd(',') + "]";
            }
            catch (Exception)
            {
                return "Interna napaka";
            }
        }
    }
}