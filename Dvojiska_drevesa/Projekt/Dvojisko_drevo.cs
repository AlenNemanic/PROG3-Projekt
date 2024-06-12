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
        private string _identifikator;

        // Konstruktor
        public Dvojisko_drevo(T podatek = default(T), Dvojisko_drevo<T> levo = null, Dvojisko_drevo<T> desno = null, string identifikator = "1")
        {
            _podatek = podatek;
            _levoPoddrevo = levo ?? new Dvojisko_drevo<T>();
            _desnoPoddrevo = desno ?? new Dvojisko_drevo<T>();
            _prazno = EqualityComparer<T>.Default.Equals(podatek, default(T)) && levo == null && desno == null;
            _identifikator = identifikator;
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
            set { _levoPoddrevo = value ?? new Dvojisko_drevo<T>(); }
        }

        // Lastnost za pridobivanje in nastavljanje desnega poddrevesa
        public Dvojisko_drevo<T> Desno
        {
            get { return _desnoPoddrevo; }
            set { _desnoPoddrevo = value ?? new Dvojisko_drevo<T>(); }
        }

        // Lastnost za preverjanje ali je drevo prazno
        public bool Prazno
        {
            get { return _prazno; }
            private set { _prazno = value; }
        }

        // Lastnost za pridobivanje identifikatorja vozlišča
        public string Identifikator
        {
            get { return _identifikator; }
            private set { _identifikator = value; }
        }

        // Metoda za iskanje vrednosti v drevesu
        public bool Iskanje(T vrednost)
        {
            if (Prazno)
                return false;
            int comparison = Podatek.CompareTo(vrednost);
            if (comparison == 0)
                return true;
            if (comparison > 0 && !Levo.Prazno)
                return Levo.Iskanje(vrednost);
            if (comparison < 0 && !Desno.Prazno)
                return Desno.Iskanje(vrednost);
            return false;
        }

        // Metoda za sestavljanje drevesa
        public static Dvojisko_drevo<T> Sestavi(T podatekVKorenu, Dvojisko_drevo<T> levoDrevo, Dvojisko_drevo<T> desnoDrevo, string identifikator)
        {
            return new Dvojisko_drevo<T>(podatekVKorenu, levoDrevo, desnoDrevo, identifikator);
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
                    vrni.Append($"{drevo.Identifikator}:{drevo.Podatek.ToString()}").Append(",");
                else
                    throw new Exception($"Napačen znak v obhodu ({znak}). Dovoljeni znaki so 'd', 'k' in 'l'.");
            }
            return vrni.ToString();
        }

        // Metoda za sestavljanje drevesa iz tabele
        public static Dvojisko_drevo<T> SestaviIzTabele(T[] tabela, int polozajKorena = 1, string identifikator = "1")
        {
            if (polozajKorena >= tabela.Length || EqualityComparer<T>.Default.Equals(tabela[polozajKorena], default(T)))
                return new Dvojisko_drevo<T>();

            string levoIdentifikator = identifikator + "L";
            string desnoIdentifikator = identifikator + "R";

            Dvojisko_drevo<T> levo = SestaviIzTabele(tabela, 2 * polozajKorena, levoIdentifikator);
            Dvojisko_drevo<T> desno = SestaviIzTabele(tabela, 2 * polozajKorena + 1, desnoIdentifikator);
            return Sestavi(tabela[polozajKorena], levo, desno, identifikator);
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

        // Metoda za sestavljanje drevesa iz slovarja
        public static Dvojisko_drevo<T> SestaviIzSlovarja(Dictionary<string, T> slovar)
        {
            if (!slovar.ContainsKey("1"))
            {
                throw new Exception("Ne morem sestaviti drevesa: Koren '1' ni v slovarju.");
            }

            Dvojisko_drevo<T> koren = new Dvojisko_drevo<T>(slovar["1"], identifikator: "1");
            Queue<Dvojisko_drevo<T>> vrsta = new Queue<Dvojisko_drevo<T>>();
            vrsta.Enqueue(koren);

            while (vrsta.Count > 0)
            {
                Dvojisko_drevo<T> trenutni = vrsta.Dequeue();
                string id = trenutni.Identifikator;
                string levoId = id + "L";
                string desnoId = id + "R";

                if (slovar.ContainsKey(levoId))
                {
                    trenutni.Levo = new Dvojisko_drevo<T>(slovar[levoId], identifikator: levoId);
                    vrsta.Enqueue(trenutni.Levo);
                }

                if (slovar.ContainsKey(desnoId))
                {
                    trenutni.Desno = new Dvojisko_drevo<T>(slovar[desnoId], identifikator: desnoId);
                    vrsta.Enqueue(trenutni.Desno);
                }
            }

            return koren;
        }
    }
}
