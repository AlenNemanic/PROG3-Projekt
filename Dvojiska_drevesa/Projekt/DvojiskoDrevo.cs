using System;
using System.Collections.Generic;

namespace Projekt
{
    public class DvojiskoDrevo
    {
        private DvojiskoDrevo _levoPoddrevo;
        private DvojiskoDrevo _desnoPoddrevo;

        public int Podatek { get; set; }
        public bool Prazno {get; set; }
        public string Identifikator { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }

        public DvojiskoDrevo(int podatek = int.MinValue, DvojiskoDrevo levo = null, DvojiskoDrevo desno = null, string identifikator = "1")
        {
            if (podatek == int.MinValue)
            {
                Prazno = true;
                Levo = null;
                Desno = null;
            }
            else
            {
                Prazno = false;
                Levo = new DvojiskoDrevo();
                Desno = new DvojiskoDrevo();
            }
            Podatek = podatek;
            Identifikator = identifikator;
        }

        public DvojiskoDrevo Levo
        {
            get { return _levoPoddrevo; }
            set
            {
                if (!Prazno)
                {
                    _levoPoddrevo = value;
                    if (_levoPoddrevo != null)
                        _levoPoddrevo.Identifikator = Identifikator + "L";
                }
                else
                    _levoPoddrevo = null;
            }
        }

        public DvojiskoDrevo Desno
        {
            get { return _desnoPoddrevo; }
            set
            {
                if (!Prazno)
                {
                    _desnoPoddrevo = value;
                    if (_desnoPoddrevo != null)
                        _desnoPoddrevo.Identifikator = Identifikator + "D";
                }
                else
                    _desnoPoddrevo = null;
            }
        }

        /// <summary>
        /// Preveri ali drevo vsebuje podatek.
        /// </summary>
        /// <param name="vrednost">Podatek, ki ga iščemo v drevesu</param>
        /// <returns>Vrne true če drevo vsebuje podatek, false sicer.</returns>
        public bool Iskanje(int vrednost)
        {
            if (Prazno)
                return false;
            if (vrednost == Podatek)
                return true;
            if (Levo.Iskanje(vrednost))
                return true;
            if (Desno.Iskanje(vrednost))
                return true;
            return false;
        }

        /// <summary>
        /// Sestavi novo dvojiško drevo.
        /// </summary>
        /// <param name="podatek">Podatek v korenu</param>
        /// <param name="levoDrevo">Levo poddrevo</param>
        /// <param name="desnoDrevo">Desno poddrevo</param>
        /// <param name="identifikator">Identifikator</param>
        /// <returns>Vrne novo dvojiško drevo.</returns>
        public static DvojiskoDrevo Sestavi(int podatek, DvojiskoDrevo levoDrevo, DvojiskoDrevo desnoDrevo, string identifikator)
        {
            return new DvojiskoDrevo(podatek, levoDrevo, desnoDrevo, identifikator);
        }

        /// <summary>
        /// Naredi obhod/pregled po drevesu. Možni vzorci so 'l', 'k' in 'd', ter vse njihove kombinacije.
        /// </summary>
        /// <param name="drevo">Dvojiško drevo</param>
        /// <param name="vzorec">Vzorec po katerem naredimo obhod</param>
        /// <returns>Vrne obhod drevesa</returns>
        public static string Obhod(DvojiskoDrevo drevo, string vzorec)
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
        /// Iz tabele sestavi novo dvojiško drevo.
        /// </summary>
        /// <param name="tabela">Tabela podatkov vozlišč</param>
        /// <param name="polozajKorena">Indeks korena v tabeli</param>
        /// <param name="identifikator">Identifikator korena</param>
        /// <returns>Vrne novo sestavljeno dvojiško drevo.</returns>
        public static DvojiskoDrevo SestaviIzTabele(int[] tabela, int polozajKorena = 1, string identifikator = "1")
        {
            if (polozajKorena >= tabela.Length || tabela[polozajKorena] == int.MinValue)
                return new DvojiskoDrevo();

            DvojiskoDrevo levo = SestaviIzTabele(tabela, 2 * polozajKorena, identifikator + "L");
            DvojiskoDrevo desno = SestaviIzTabele(tabela, 2 * polozajKorena + 1, identifikator + "R");
            return Sestavi(tabela[polozajKorena], levo, desno, identifikator);
        }

        /// <summary>
        /// Sestavi slovar iz drevesa.
        /// </summary>
        /// <returns>Vrne slovar, kjer so ključi identifikatorji in vrednosti so podatki vozlišč.</returns>
        public Dictionary<string, int> IzDrevesaVSlovar()
        {
            Dictionary<string, int> slovar = new Dictionary<string, int>();
            IzDrevesaVSlovar(this, "1", slovar);
            return slovar;
        }

        /// <summary>
        /// Sestavi slovar iz drevesa.
        /// </summary>
        /// <param name="vozlisce">Koren drevesa</param>
        /// <param name="identifikator">Identifikator korena</param>
        /// <param name="slovar">Slovar, kjer so ključi identifikatorji in vrednosti so podatki vozlišč</param>
        private void IzDrevesaVSlovar(DvojiskoDrevo vozlisce, string identifikator, Dictionary<string, int> slovar)
        {
            if (vozlisce == null || vozlisce.Prazno)
                return;

            slovar.Add(identifikator, vozlisce.Podatek);

            IzDrevesaVSlovar(vozlisce.Levo, identifikator + "L", slovar);
            IzDrevesaVSlovar(vozlisce.Desno, identifikator + "R", slovar);
        }

        /// <summary>
        /// Sestavi drevo iz slovarja.
        /// </summary>
        /// <param name="slovar">Slovar, kjer so ključi identifikatorji in vrednosti so podatki vozlišč</param>
        /// <returns>Vrne novo sestavljeno dvojiško drevo.</returns>
        public static DvojiskoDrevo IzSlovarja(Dictionary<string, int> slovar)
        {
            if (!slovar.ContainsKey("1"))
                return new DvojiskoDrevo(); // Če slovar ne vsebuje korena, vrnemo prazno drevo

            DvojiskoDrevo koren = new DvojiskoDrevo(slovar["1"]);
            GradnjaIzSlovarja(koren, "1", slovar);
            return koren;
        }

        /// <summary>
        /// Sestavi drevo iz slovarja.
        /// </summary>
        /// <param name="vozlisce">Koren drevesa</param>
        /// <param name="identifikator">Identifkator korena</param>
        /// <param name="slovar">Slovar, kjer so ključi identifikatorji in vrednosti so podatki vozlišč</param>
        private static void GradnjaIzSlovarja(DvojiskoDrevo vozlisce, string identifikator, Dictionary<string, int> slovar)
        {
            string levoId = identifikator + "L";
            string desnoId = identifikator + "R";

            if (slovar.ContainsKey(levoId))
            {
                vozlisce.Levo = new DvojiskoDrevo(slovar[levoId]);
                GradnjaIzSlovarja(vozlisce.Levo, levoId, slovar);
            }
            else vozlisce.Levo = null; // Ne dodaj praznega vozlišča

            if (slovar.ContainsKey(desnoId))
            {
                vozlisce.Desno = new DvojiskoDrevo(slovar[desnoId]);
                GradnjaIzSlovarja(vozlisce.Desno, desnoId, slovar);
            }
            else vozlisce.Desno = null; // Ne dodaj praznega vozlišča
        }
    }
}