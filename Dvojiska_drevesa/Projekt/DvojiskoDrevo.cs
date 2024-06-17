using System;
using System.Collections.Generic;
using System.IO;

namespace Projekt
{
    public class DvojiskoDrevo<T> where T : IComparable<T>
    {
        private DvojiskoDrevo<T> _levoPoddrevo;
        private DvojiskoDrevo<T> _desnoPoddrevo;
        private bool _prazno;

        public T Podatek { get; set; }
        public string Identifikator { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }

        public DvojiskoDrevo(T podatek = default(T), DvojiskoDrevo<T> levo = null, DvojiskoDrevo<T> desno = null, string identifikator = "1")
        {
            if (EqualityComparer<T>.Default.Equals(podatek, default(T)) && levo == null && desno == null)
            {
                Podatek = podatek;
                Prazno = true; 
                Levo = null;
                Desno = null;
            }
            else
            {
                Podatek = podatek;
                Prazno = false;
                Levo = new DvojiskoDrevo<T>();
                Desno = new DvojiskoDrevo<T>();
            }
            Identifikator = identifikator;
        }

        public bool Prazno
        {
            get { return _prazno; }
            set
            {

            }
        }

        public DvojiskoDrevo<T> Levo
        {
            get { return _levoPoddrevo; }
            set
            {
                if (Prazno == false && value != null && !EqualityComparer<T>.Default.Equals(Podatek, default(T)))
                {
                    _levoPoddrevo = new DvojiskoDrevo<T>();   
                    _levoPoddrevo.Podatek = value.Podatek;
                    _levoPoddrevo.Identifikator = Identifikator + "L";
                }
                else
                {
                    _levoPoddrevo = value; //null
                }
            }
        }

        public DvojiskoDrevo<T> Desno
        {
            get { return _desnoPoddrevo; }
            set
            {
                if (Prazno == false && value != null && !EqualityComparer<T>.Default.Equals(Podatek, default(T)))
                {
                    _desnoPoddrevo = new DvojiskoDrevo<T>();
                    _desnoPoddrevo.Podatek = value.Podatek;
                    _desnoPoddrevo.Identifikator = Identifikator + "D";
                }
                else
                {
                    _levoPoddrevo = value; // null
                }
            }
        }

        /// <summary>
        /// Metoda pregleda ali podatek obstaja v drevesu.
        /// </summary>
        /// <param name="vrednost">Vrednost podatka</param>
        /// <returns>Vrne true če podatek obstaja v drevesu, sicer vrne false.</returns>
        public bool Iskanje(T vrednost)
        {
            if (Prazno)
                return false;
            if (!Levo.Prazno && Levo.Iskanje(vrednost))
                return true;
            if (!Desno.Prazno && Desno.Iskanje(vrednost))
                return true;
            return false;
        }

        /// <summary>
        /// Metoda za kosntruriranje drevesa.
        /// </summary>
        /// <param name="podatekVKorenu"></param>
        /// <param name="levoDrevo"></param>
        /// <param name="desnoDrevo"></param>
        /// <param name="identifikator"></param>
        /// <returns>Vrne novo dvojiško drevo.</returns>
        public static DvojiskoDrevo<T> Sestavi(T podatekVKorenu, DvojiskoDrevo<T> levoDrevo, DvojiskoDrevo<T> desnoDrevo, string identifikator)
        {
            return new DvojiskoDrevo<T>(podatekVKorenu, levoDrevo, desnoDrevo, identifikator);
        }

        /// <summary>
        /// Metoda za obhod drevesa. Dovoljeni znaki so 'l', 'k' in 'd'.
        /// </summary>
        /// <param name="drevo">Dvojiško drevo</param>
        /// <param name="vzorec">Vzorec po katerem bomo naredili obhod</param>
        /// <returns>Vrne obhod drevesa po vzorcu.</returns>
        public static string Obhod(DvojiskoDrevo<T> drevo, string vzorec)
        {
            if (drevo == null)
                return "";
            
            var vrni = new System.Text.StringBuilder();
            foreach (char znak in vzorec)
            {
                switch (znak)
                {
                    case 'l':
                        vrni.Append(Obhod(drevo.Levo, vzorec));
                        break;
                    case 'd':
                        vrni.Append(Obhod(drevo.Desno, vzorec));
                        break;
                    case 'k':
                        vrni.Append($"{drevo.Identifikator}:{drevo.Podatek},");
                        break;
                    default:
                        throw new Exception($"Napačen znak v obhodu ({znak}). Dovoljeni znaki so 'd', 'k' in 'l'.");
                }
            }
            return vrni.ToString();
        }

        /// <summary>
        /// Metoda za izpis drevesa v niz.
        /// </summary>
        /// <returns>Vrne vmesni pregled drevesa.</returns>
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

        /// <summary>
        /// Metoda za konstruriranje drevesa iz tabele.
        /// </summary>
        /// <param name="tabela"></param>
        /// <param name="polozajKorena"></param>
        /// <param name="identifikator"></param>
        /// <returns>Vrne novo dvojiško drevo.</returns>
        public static DvojiskoDrevo<T> SestaviIzTabele(T[] tabela, int polozajKorena = 1, string identifikator = "1")
        {
            if (polozajKorena >= tabela.Length || EqualityComparer<T>.Default.Equals(tabela[polozajKorena], default(T)))
                return new DvojiskoDrevo<T>();

            DvojiskoDrevo<T> levo = SestaviIzTabele(tabela, 2 * polozajKorena, identifikator + "L");
            DvojiskoDrevo<T> desno = SestaviIzTabele(tabela, 2 * polozajKorena + 1, identifikator + "R");
            return Sestavi(tabela[polozajKorena], levo, desno, identifikator);
        }

        /// <summary>
        /// Metoda za konstruriranje drevesa iz slovarja.
        /// </summary>
        /// <param name="slovar"></param>
        /// <returns>Vrne novo dvojiško drevo.</returns>
        public static DvojiskoDrevo<T> SestaviIzSlovarja(Dictionary<string, T> slovar)
        {
            if (!slovar.TryGetValue("1", out T rootData))
                throw new Exception("Ne morem sestaviti drevesa: Koren '1' ni v slovarju.");

            DvojiskoDrevo<T> koren = new DvojiskoDrevo<T>(rootData, identifikator: "1");
            Queue<DvojiskoDrevo<T>> vrsta = new Queue<DvojiskoDrevo<T>>();
            vrsta.Enqueue(koren);

            while (vrsta.Count > 0)
            {
                DvojiskoDrevo<T> trenutni = vrsta.Dequeue();
                string id = trenutni.Identifikator;

                if (slovar.TryGetValue(id + "L", out T leftData))
                {
                    trenutni.Levo = new DvojiskoDrevo<T>(leftData, identifikator: id + "L");
                    vrsta.Enqueue(trenutni.Levo);
                }

                if (slovar.TryGetValue(id + "R", out T rightData))
                {
                    trenutni.Desno = new DvojiskoDrevo<T>(rightData, identifikator: id + "R");
                    vrsta.Enqueue(trenutni.Desno);
                }
            }
            return koren;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public void IzDrevesaSlovar()
        {
            using (StreamWriter pisi = new StreamWriter("izvoz.txt"))
            {
                string slovarText = Obhod(this, "kld");
                pisi.WriteLine(slovarText);
            }
        }
    }
}