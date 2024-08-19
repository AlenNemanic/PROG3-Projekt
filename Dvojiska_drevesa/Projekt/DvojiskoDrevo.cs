using System.Collections.Generic;

namespace Projekt
{
    public class DvojiskoDrevo
    {
        private int _podatek;
        private DvojiskoDrevo _levoPoddrevo;
        private DvojiskoDrevo _desnoPoddrevo;

        public bool Prazno { get; set; }
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

        public int Podatek
        {
            get { return _podatek; }
            set
            {
                if (value == int.MinValue)
                    Prazno = true;
                _podatek = value;
            }
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
                else _levoPoddrevo = null;
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
                else _desnoPoddrevo = null;
            }
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