using System;
using System.Collections.Generic;

namespace Projekt
{
    public class DvojiskoDrevo
    {
        private DvojiskoDrevo _levoPoddrevo;
        private DvojiskoDrevo _desnoPoddrevo;
        private bool _prazno;

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


        public bool Iskanje(int vrednost)
        {
            if (Prazno)
                return false;
            if (!Levo.Prazno && Levo.Iskanje(vrednost))
                return true;
            if (!Desno.Prazno && Desno.Iskanje(vrednost))
                return true;
            return false;
        }

        public static DvojiskoDrevo Sestavi(int podatekVKorenu, DvojiskoDrevo levoDrevo, DvojiskoDrevo desnoDrevo, string identifikator)
        {
            return new DvojiskoDrevo(podatekVKorenu, levoDrevo, desnoDrevo, identifikator);
        }

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

        public static DvojiskoDrevo SestaviIzTabele(int[] tabela, int polozajKorena = 1, string identifikator = "1")
        {
            if (polozajKorena >= tabela.Length || tabela[polozajKorena] == int.MinValue)
                return new DvojiskoDrevo();

            DvojiskoDrevo levo = SestaviIzTabele(tabela, 2 * polozajKorena, identifikator + "L");
            DvojiskoDrevo desno = SestaviIzTabele(tabela, 2 * polozajKorena + 1, identifikator + "R");
            return Sestavi(tabela[polozajKorena], levo, desno, identifikator);
        }

        public static DvojiskoDrevo SestaviIzSlovarja(Dictionary<string, int> slovar)
        {
            if (!slovar.TryGetValue("1", out int rootData))
                throw new Exception("Ne morem sestaviti drevesa: Koren '1' ni v slovarju.");

            DvojiskoDrevo koren = new DvojiskoDrevo(rootData, identifikator: "1");
            Queue<DvojiskoDrevo> vrsta = new Queue<DvojiskoDrevo>();
            vrsta.Enqueue(koren);

            while (vrsta.Count > 0)
            {
                DvojiskoDrevo trenutni = vrsta.Dequeue();
                string id = trenutni.Identifikator;

                if (slovar.TryGetValue(id + "L", out int leftData))
                {
                    trenutni.Levo = new DvojiskoDrevo(leftData, identifikator: id + "L");
                    vrsta.Enqueue(trenutni.Levo);
                }

                if (slovar.TryGetValue(id + "R", out int rightData))
                {
                    trenutni.Desno = new DvojiskoDrevo(rightData, identifikator: id + "R");
                    vrsta.Enqueue(trenutni.Desno);
                }
            }
            return koren;
        }

        public Dictionary<string, int> IzDrevesaVSlovar()
        {
            Dictionary<string, int> slovar = new Dictionary<string, int>();
            IzDrevesaVSlovar(this, "1", slovar);
            return slovar;
        }

        private void IzDrevesaVSlovar(DvojiskoDrevo vozlisce, string identifikator, Dictionary<string, int> slovar)
        {
            if (vozlisce == null || vozlisce.Prazno)
                return;

            slovar.Add(identifikator, vozlisce.Podatek);

            IzDrevesaVSlovar(vozlisce.Levo, identifikator + "L", slovar);
            IzDrevesaVSlovar(vozlisce.Desno, identifikator + "R", slovar);
        }

        public static DvojiskoDrevo IzSlovarja(Dictionary<string, int> slovar)
        {
            if (!slovar.ContainsKey("1"))
                return new DvojiskoDrevo(); // Če slovar ne vsebuje korena, vrnemo prazno drevo

            DvojiskoDrevo koren = new DvojiskoDrevo(slovar["1"]);
            GradnjaIzSlovarja(koren, "1", slovar);
            return koren;
        }

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