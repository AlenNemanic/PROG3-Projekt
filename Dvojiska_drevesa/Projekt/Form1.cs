using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Projekt
{
    public partial class Form1: Form
    {
        private int trenutniIndeksPrehoda;
        private const float velikostVozlisca = 20;
        private DvojiskoDrevo drevo = new DvojiskoDrevo();
        private DvojiskoDrevo izbranoVozlisce;
        private List<DvojiskoDrevo> potPrehoda;
        private Stack<Dictionary<string, int>> prejsnja_stanja = new Stack<Dictionary<string, int>>();
        private Stack<Dictionary<string, int>> naslednja_stanja = new Stack<Dictionary<string, int>>();
        private PointF zamik = new PointF(0f, 0f); // Zamik za ohranjanje središèa poveèave
        private Timer casovnikPrehoda;
        private ContextMenuStrip meniDodajanja; // Meni za dodajanje levega ali desnega sina

        public Form1()
        {
            InitializeComponent();
            InitializeTrees();
            InitializeTreePositions();
            InicializacijaPrehoda();
            meniDodajanja = new ContextMenuStrip();
            meniDodajanja.Items.Add("Dodaj levega sina", null, DodajLevegaSina_Click);
            meniDodajanja.Items.Add("Dodaj desnega sina", null, DodajDesnegaSina_Click);
            DoubleBuffered = true;
        }

        /// <summary>
        /// Ob spremembi velikosti okna prilagodi položaje vozlišè drevesa in osveži risanje.
        /// </summary>
        private void Form1_Resize(object sender, EventArgs e)
        {
            InitializeTreePositions();
            Invalidate();
        }

        /// <summary>
        /// Inicializira primer drevesa z vnaprej doloèenimi vozlišèi in nastavi zaèetne pozicije vsakega vozlišèa.
        /// </summary>
        private void InitializeTrees()
        {
            drevo = new DvojiskoDrevo(10);
            drevo.Levo = new DvojiskoDrevo(5);
            drevo.Desno = new DvojiskoDrevo(15);
            drevo.Levo.Levo = new DvojiskoDrevo(3);
            drevo.Levo.Desno = new DvojiskoDrevo(7);
            drevo.Desno.Levo = new DvojiskoDrevo(12);
            drevo.Desno.Desno = new DvojiskoDrevo(18);

            SetInitialPositions(drevo, 100, 50, 50, 50);
        }

        /// <summary>
        /// Ob spremembi velikosti okna ponovno izraèuna in nastavi položaje vozlišè drevesa.
        /// </summary>
        private void InitializeTreePositions()
        {
            int sirina = pictureBox.Width;
            SetInitialPositions(drevo, (sirina + Width) / 2, 50, (Width - sirina) / 4, (Height - sirina) / 4);
        }

        /// <summary>
        /// Nastavi zaèetne pozicije vozlišè drevesa glede na dane koordinate in zamike.
        /// </summary>
        /// <param name="vozlisce">Trenutno vozlišèe drevesa.</param>
        /// <param name="x">Zaèetna x-koordinata.</param>
        /// <param name="y">Zaèetna y-koordinata.</param>
        /// <param name="xOffset">Odmik na osi x za leve in desne sinove.</param>
        /// <param name="yOffset">Odmik na osi y za leve in desne sinove.</param>
        private void SetInitialPositions(DvojiskoDrevo vozlisce, float x, float y, float xOffset, float yOffset)
        {
            if (vozlisce == null || vozlisce.Prazno)
                return;

            vozlisce.PosX = x;
            vozlisce.PosY = y;

            if (vozlisce.Levo != null && !vozlisce.Levo.Prazno)
                SetInitialPositions(vozlisce.Levo, x - xOffset, y + yOffset, xOffset / 2, yOffset);
            if (vozlisce.Desno != null && !vozlisce.Desno.Prazno)
                SetInitialPositions(vozlisce.Desno, x + xOffset, y + yOffset, xOffset / 2, yOffset);
        }

        /// <summary>
        /// Prilagodi središèe poveèave in nariše celotno drevo na zaslonu.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.TranslateTransform(zamik.X, zamik.Y);    // Prilagoditev za ohranjanje središèe poveèave
            DrawTree(e.Graphics, drevo);
        }

        /// <summary>
        /// Rekurzivno nariše vozlišèa drevesa in povezave med njimi na zaslonu.
        /// </summary>
        /// <param name="g">Grafièni objekt za risanje.</param>
        /// <param name="vozlisce">Trenutno vozlišèe drevesa.</param>
        private void DrawTree(Graphics g, DvojiskoDrevo vozlisce)
        {
            if (vozlisce == null || vozlisce.Prazno)
                return;

            float velikostVozlisca = Math.Min(Width, Height) / Form1.velikostVozlisca;
            float scaledFontSize = velikostVozlisca / 3;    // Prilagodi velikost pisave glede na velikost vozlišèa
            using (Font scaledFont = new Font(Font.FontFamily, scaledFontSize, Font.Style))
            {
                if (vozlisce.Levo != null && !vozlisce.Levo.Prazno)
                {
                    Pen pen = izbranoVozlisce == vozlisce.Levo ? Pens.Red : Pens.Black;
                    g.DrawLine(pen, vozlisce.PosX, vozlisce.PosY, vozlisce.Levo.PosX, vozlisce.Levo.PosY);
                    DrawTree(g, vozlisce.Levo);
                }

                if (vozlisce.Desno != null && !vozlisce.Desno.Prazno)
                {
                    Pen pen = izbranoVozlisce == vozlisce.Desno ? Pens.Red : Pens.Black;
                    g.DrawLine(pen, vozlisce.PosX, vozlisce.PosY, vozlisce.Desno.PosX, vozlisce.Desno.PosY);
                    DrawTree(g, vozlisce.Desno);
                }

                Brush brush = izbranoVozlisce == vozlisce ? Brushes.Orange : Brushes.LightBlue;
                g.FillEllipse(brush, vozlisce.PosX - velikostVozlisca / 2, vozlisce.PosY - velikostVozlisca / 2, velikostVozlisca, velikostVozlisca);
                g.DrawEllipse(Pens.Black, vozlisce.PosX - velikostVozlisca / 2, vozlisce.PosY - velikostVozlisca / 2, velikostVozlisca, velikostVozlisca);

                SizeF textSize = g.MeasureString(vozlisce.Podatek.ToString(), scaledFont);
                float textX = vozlisce.PosX - textSize.Width / 2;
                float textY = vozlisce.PosY - textSize.Height / 2;

                g.DrawString(vozlisce.Podatek.ToString(), scaledFont, Brushes.Black, textX, textY);
            }
        }

        /// <summary>
        /// Ob kliku miške na vozlišèe doda novo vozlišèe ali odstrani obstojeèe vozlišèe na podlagi izbranega naèina.
        /// </summary>
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (radioButtonDodaj.Checked)
            {
                // Izvede se, kadar drevo še nima korena. Dodamo koren brez sinov.
                if (drevo.Prazno && int.TryParse(textBox.Text, out int vrednost))
                {
                    DvojiskoDrevo novoDrevo = new DvojiskoDrevo(vrednost);
                    Stanje(drevo);
                    drevo = novoDrevo;
                    InitializeTreePositions();
                    Invalidate();
                }
                else
                {
                    izbranoVozlisce = NajdiVozlisce(drevo, e.Location);
                    if (izbranoVozlisce != null)
                        meniDodajanja.Show(this, e.Location);
                }
            }
            else if (radioButtonOdstrani.Checked)
            {
                DvojiskoDrevo vozlisceZaOdstranit = NajdiVozlisce(drevo, e.Location);
                if (vozlisceZaOdstranit != null)
                {
                    Stanje(drevo);
                    IzbrisiVozlisce(drevo, vozlisceZaOdstranit);
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Shrani trenutno stanje drevesa na sklad prejšnjih stanj in poèisti sklad naslednjih stanj.
        /// </summary>
        /// <param name="drevo">Trenutno dvojiško drevo.</param>
        private void Stanje(DvojiskoDrevo drevo)
        {
            prejsnja_stanja.Push(drevo.IzDrevesaVSlovar());
            naslednja_stanja.Clear();
        }

        /// <summary>
        /// Dogodek, ki se izvede ob kliku na meni "Dodaj levega sina". Doda novega levega sina k izbranemu vozlišèu.
        /// </summary>
        private void DodajLevegaSina_Click(object sender, EventArgs e)
        {
            if (izbranoVozlisce != null && int.TryParse(textBox.Text, out int vrednost))
            {
                if (izbranoVozlisce.Levo == null || izbranoVozlisce.Levo.Prazno)
                {
                    Stanje(drevo);
                    izbranoVozlisce.Levo = new DvojiskoDrevo(vrednost);
                    InitializeTreePositions();
                    izbranoVozlisce = null; // Ponastavi izbrano vozlišèe, da prepreèi obarvanje
                    Invalidate();
                }
                else MessageBox.Show("Izbrano vozlišèe že ima levega sina.");
            }
        }

        /// <summary>
        /// Dogodek, ki se izvede ob kliku na meni "Dodaj desnega sina". Doda novega desnega sina k izbranemu vozlišèu.
        /// </summary>
        private void DodajDesnegaSina_Click(object sender, EventArgs e)
        {
            if (izbranoVozlisce != null && int.TryParse(textBox.Text, out int vrednost))
            {
                if (izbranoVozlisce.Desno == null || izbranoVozlisce.Desno.Prazno)
                {
                    Stanje(drevo);
                    izbranoVozlisce.Desno = new DvojiskoDrevo(vrednost);
                    InitializeTreePositions();
                    izbranoVozlisce = null; // Ponastavi izbrano vozlišèe, da prepreèi obarvanje
                    Invalidate();
                }
                else MessageBox.Show("Izbrano vozlišèe že ima desnega sina.");
            }
        }

        /// <summary>
        /// Izbriše podano vozlišèe iz drevesa skupaj z njegovim celotnim poddrevesom.
        /// </summary>
        /// <param name="vozlisce">Vozlišèe drevesa.</param>
        /// <param name="vozlisceZaOdstranit">Vozlišèe, ki ga želimo odstraniti.</param>
        private void IzbrisiVozlisce(DvojiskoDrevo vozlisce, DvojiskoDrevo vozlisceZaOdstranit)
        {
            if (vozlisce != null)
            {
                if (vozlisceZaOdstranit == vozlisce)
                    drevo.Podatek = int.MinValue;
                else if (vozlisce.Levo == vozlisceZaOdstranit)
                    vozlisce.Levo = null;
                else if (vozlisce.Desno == vozlisceZaOdstranit)
                    vozlisce.Desno = null;

                IzbrisiVozlisce(vozlisce.Levo, vozlisceZaOdstranit);
                IzbrisiVozlisce(vozlisce.Desno, vozlisceZaOdstranit);
            }
        }

        /// <summary>
        /// Poišèe vozlišèe v drevesu, ki je najbližje podani toèki.
        /// </summary>
        /// <param name="vozlisce">Koren drevesa.</param>
        /// <param name="lokacija">Toèka klika.</param>
        /// <returns>Vrne najbližje vozlišèe, èe obstaja, sicer vrne null.</returns>
        private DvojiskoDrevo NajdiVozlisce(DvojiskoDrevo vozlisce, Point lokacija)
        {
            if (vozlisce == null || vozlisce.Prazno)
                return null;

            float velikostVozlisca = Math.Min(Width, Height) / Form1.velikostVozlisca;
            RectangleF pravokotnik = new RectangleF(vozlisce.PosX - velikostVozlisca / 2, vozlisce.PosY - velikostVozlisca / 2, velikostVozlisca, velikostVozlisca);
            if (pravokotnik.Contains(lokacija))
                return vozlisce;

            DvojiskoDrevo najdenoVozlisce = NajdiVozlisce(vozlisce.Levo, lokacija);
            if (najdenoVozlisce == null)
                najdenoVozlisce = NajdiVozlisce(vozlisce.Desno, lokacija);
            return najdenoVozlisce;
        }

        /// <summary>
        /// Inicializira prehod z nastavitvijo èasovnika za animacijo.
        /// </summary>
        private void InicializacijaPrehoda()
        {
            casovnikPrehoda = new Timer();
            casovnikPrehoda.Interval = 500;
            casovnikPrehoda.Tick += CasovnikPrehoda_Tick;
        }

        /// <summary>
        /// Zaène prehod skozi vozlišèa drevesa glede na podano pot.
        /// </summary>
        /// <param name="pot">Seznam vozlišè, ki predstavlja pot prehoda.</param>
        private void ZacniPrehod(List<DvojiskoDrevo> pot)
        {
            potPrehoda = pot;
            trenutniIndeksPrehoda = 0;
            casovnikPrehoda.Start();
        }

        /// <summary>
        /// Dogodek, ki se sproži ob vsakem intervalu èasovnika.
        /// Posodobi trenutno izbrano vozlišèe in ustavi èasovnik, èe je prehod konèan.
        /// </summary>
        private void CasovnikPrehoda_Tick(object sender, EventArgs e)
        {
            if (trenutniIndeksPrehoda < potPrehoda.Count)
                izbranoVozlisce = potPrehoda[trenutniIndeksPrehoda++];
            else
            {
                casovnikPrehoda.Stop();
                izbranoVozlisce = null;
            }
            Invalidate();
        }

        /// <summary>
        /// Pregleda drevo glede na podani vzorec in zgradi pot prehoda.
        /// </summary>
        /// <param name="vozlisce">Trenutno vozlišèe v drevesu.</param>
        /// <param name="pot">Seznam vozlišè za pot prehoda.</param>
        /// <param name="vzorec">Vrstni red prehoda ("k" za koren, "l" za levo, "d" za desno).</param>
        private void PregledDrevesa(DvojiskoDrevo vozlisce, List<DvojiskoDrevo> pot, string vzorec)
        {
            if (vozlisce == null || vozlisce.Prazno) return;
            foreach (char znak in vzorec)
            {
                switch (znak)
                {
                    case 'k':
                        pot.Add(vozlisce);
                        break;
                    case 'l':
                        PregledDrevesa(vozlisce.Levo, pot, vzorec);
                        break;
                    case 'd':
                        PregledDrevesa(vozlisce.Desno, pot, vzorec);
                        break;
                }
            }
        }

        /// <summary>
        /// Naredi premi pregled drevesa.
        /// </summary>
        private void PremiPregledGumb_Click(object sender, EventArgs e)
        {
            List<DvojiskoDrevo> pot = new List<DvojiskoDrevo>();
            PregledDrevesa(drevo, pot, "kld");
            ZacniPrehod(pot);
        }

        /// <summary>
        /// Naredi vmesni pregled drevesa.
        /// </summary>
        private void VmesniPregledGumb_Click(object sender, EventArgs e)
        {
            List<DvojiskoDrevo> pot = new List<DvojiskoDrevo>();
            PregledDrevesa(drevo, pot, "lkd");
            ZacniPrehod(pot);
        }

        /// <summary>
        /// Naredi obratni pregled drevesa.
        /// </summary>
        private void ObratniPregledGumb_Click(object sender, EventArgs e)
        {
            List<DvojiskoDrevo> pot = new List<DvojiskoDrevo>();
            PregledDrevesa(drevo, pot, "ldk");
            ZacniPrehod(pot);
        }

        /// <summary>
        /// Odpre meni za uvoz datoteke in prebere drevo iz izbrane datoteke.
        /// </summary>
        private void UvoziGumb_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                DefaultExt = "txt"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                PreberiDrevoIzDatoteke(openFileDialog.FileName);
        }

        /// <summary>
        /// Odpre meni za izvoz datoteke in zapiše drevo v izbrano datoteko.
        /// </summary>
        private void IzvoziGumb_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                DefaultExt = "txt"
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                ZapisiDrevoVDatoteko(drevo, saveFileDialog.FileName); // Zapiši drevo v datoteko
        }

        /// <summary>
        /// Razveljavi zadnjo spremembo drevesa s povrnitvijo na prejšnje stanje.
        /// </summary>
        private void RazveljaviGumb_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, int> stanje = prejsnja_stanja.Pop();
                naslednja_stanja.Push(drevo.IzDrevesaVSlovar());
                drevo = DvojiskoDrevo.IzSlovarja(stanje);
            }
            catch (Exception)
            {
                MessageBox.Show("Prejšnje stanje ne obstaja!");
            }
            InitializeTreePositions();
            Invalidate();
        }

        /// <summary>
        /// Povrne drevo na naslednje stanje v zgodovini sprememb.
        /// </summary>
        private void ObnoviGumb_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, int> stanje = naslednja_stanja.Pop();
                prejsnja_stanja.Push(drevo.IzDrevesaVSlovar());
                drevo = DvojiskoDrevo.IzSlovarja(stanje);
            }
            catch (Exception)
            {
                MessageBox.Show("Naslednje stanje ne obstaja!");
            }
            InitializeTreePositions();
            Invalidate();
        }

        /// <summary>
        /// Prebere drevo iz datoteke, obnovi njegovo stanje in ga nariše na zaslon.
        /// </summary>
        /// <param name="filePath">Pot do datoteke.</param>
        private void PreberiDrevoIzDatoteke(string filePath)
        {
            Dictionary<string, int> slovar = new Dictionary<string, int>();
            using (StreamReader beri = new StreamReader(filePath))
            {
                string vrstica;
                while ((vrstica = beri.ReadLine()) != null)
                {
                    string[] identifikatorInVrednost = vrstica.Split(':');
                    if (int.TryParse(identifikatorInVrednost[1], out int value))
                        slovar.Add(identifikatorInVrednost[0], value);
                }
            }
            DvojiskoDrevo novoDrevo = DvojiskoDrevo.IzSlovarja(slovar);
            drevo = novoDrevo;
            prejsnja_stanja.Push(novoDrevo.IzDrevesaVSlovar());
            InitializeTreePositions();
            Invalidate();
        }

        /// <summary>
        /// Zapiše trenutno stanje binarnega drevesa v datoteko.
        /// </summary>
        /// <param name="drevo">Drevo za zapis.</param>
        /// <param name="filePath">Pot do datoteke.</param>
        private void ZapisiDrevoVDatoteko(DvojiskoDrevo drevo, string filePath)
        {
            Dictionary<string, int> slovar = drevo.IzDrevesaVSlovar();
            using (StreamWriter pisi = new StreamWriter(filePath))
            {
                foreach (KeyValuePair<string, int> element in slovar)
                {
                    pisi.WriteLine($"{element.Key}:{element.Value}");
                }
            }
        }
    }
}