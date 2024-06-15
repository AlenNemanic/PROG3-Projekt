using System;
using System.Collections.Generic;

namespace Projekt
{
    public class Dvojisko_drevo<T> where T : IComparable<T>
    {
        public T Podatek { get; set; }
        public Dvojisko_drevo<T> Levo { get; set; }
        public Dvojisko_drevo<T> Desno { get; set; }
        public bool Prazno { get; private set; }
        public string Identifikator { get; private set; }
        public float PosX { get; set; }
        public float PosY { get; set; }

        public Dvojisko_drevo(T podatek = default(T), Dvojisko_drevo<T> levo = null, Dvojisko_drevo<T> desno = null, string identifikator = "1")
        {
            if (EqualityComparer<T>.Default.Equals(podatek, default(T)) && levo == null && desno == null)
            {
                Prazno = true;
                Levo = null;
                Desno = null;
            }
            else
            {
                Podatek = podatek;
                Levo = levo ?? new Dvojisko_drevo<T>();
                Desno = desno ?? new Dvojisko_drevo<T>();
                Prazno = false;
            }
            Identifikator = identifikator;
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

        // Method for constructing a tree
        public static Dvojisko_drevo<T> Sestavi(T podatekVKorenu, Dvojisko_drevo<T> levoDrevo, Dvojisko_drevo<T> desnoDrevo, string identifikator)
        {
            return new Dvojisko_drevo<T>(podatekVKorenu, levoDrevo, desnoDrevo, identifikator);
        }

        // Method for traversing the tree
        public static string Obhod(Dvojisko_drevo<T> drevo, string vzorec)
        {
            if (drevo.Prazno)
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

        // Method for constructing a tree from an array
        public static Dvojisko_drevo<T> SestaviIzTabele(T[] tabela, int polozajKorena = 1, string identifikator = "1")
        {
            if (polozajKorena >= tabela.Length || EqualityComparer<T>.Default.Equals(tabela[polozajKorena], default(T)))
                return new Dvojisko_drevo<T>();

            Dvojisko_drevo<T> levo = SestaviIzTabele(tabela, 2 * polozajKorena, identifikator + "L");
            Dvojisko_drevo<T> desno = SestaviIzTabele(tabela, 2 * polozajKorena + 1, identifikator + "R");
            return Sestavi(tabela[polozajKorena], levo, desno, identifikator);
        }

        // Method for converting the tree to a string
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

        // Method for constructing a tree from a dictionary
        public static Dvojisko_drevo<T> SestaviIzSlovarja(Dictionary<string, T> slovar)
        {
            if (!slovar.TryGetValue("1", out T rootData))
            {
                throw new Exception("Ne morem sestaviti drevesa: Koren '1' ni v slovarju.");
            }

            Dvojisko_drevo<T> koren = new Dvojisko_drevo<T>(rootData, identifikator: "1");
            Queue<Dvojisko_drevo<T>> vrsta = new Queue<Dvojisko_drevo<T>>();
            vrsta.Enqueue(koren);

            while (vrsta.Count > 0)
            {
                Dvojisko_drevo<T> trenutni = vrsta.Dequeue();
                string id = trenutni.Identifikator;

                if (slovar.TryGetValue(id + "L", out T leftData))
                {
                    trenutni.Levo = new Dvojisko_drevo<T>(leftData, identifikator: id + "L");
                    vrsta.Enqueue(trenutni.Levo);
                }

                if (slovar.TryGetValue(id + "R", out T rightData))
                {
                    trenutni.Desno = new Dvojisko_drevo<T>(rightData, identifikator: id + "R");
                    vrsta.Enqueue(trenutni.Desno);
                }
            }

            return koren;
        }
    }
}
