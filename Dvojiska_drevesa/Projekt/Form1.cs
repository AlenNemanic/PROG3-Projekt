using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Projekt
{
    public partial class Form1: Form
    {
        private int trenutniIndeksPrehoda;
        private const float NodeSizeRatio = 20;
        private List<DvojiskoDrevo> drevesa;
        private List<DvojiskoDrevo> potPrehoda;
        private Stack<Dictionary<string, int>> prejsnja_stanja = new Stack<Dictionary<string, int>>();
        private Stack<Dictionary<string, int>> naslednja_stanja = new Stack<Dictionary<string, int>>();
        private DvojiskoDrevo draggedNode;
        private DvojiskoDrevo izbranoVozlisce;
        private PointF offset = new PointF(0f, 0f); // Offset to maintain the zoom center
        private PointF zacetnaPozicija;
        private Point? dragStart = null;
        private Timer casovnikPrehoda;
        private ContextMenuStrip addNodeMenu; // Context menu for adding left or right child

        public Form1()
        {
            InitializeComponent();
            InitializeTrees();
            InitializeTreePositions();
            InicializacijaPrehoda(); // Initialize the traversal functionality
            Resize += Form1_Resize;
            MouseDown += Form1_MouseDown;
            MouseMove += Form1_MouseMove;
            MouseUp += Form1_MouseUp;

            // Initialize the context menu for adding nodes
            addNodeMenu = new ContextMenuStrip();
            addNodeMenu.Items.Add("Add Left", null, AddLeftNode_Click);
            addNodeMenu.Items.Add("Add Right", null, AddRightNode_Click);

            // Enable double buffering to reduce flicker
            DoubleBuffered = true;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            InitializeTreePositions(); // Recalculate positions of the tree nodes when the form is resized
            Invalidate();
        }

        private void InitializeTrees()
        {
            drevesa = new List<DvojiskoDrevo>();

            // Initialize a sample tree
            DvojiskoDrevo drevo1 = new DvojiskoDrevo(10);
            drevo1.Levo = new DvojiskoDrevo(5);
            drevo1.Desno = new DvojiskoDrevo(15);
            drevo1.Levo.Levo = new DvojiskoDrevo(3);
            drevo1.Levo.Desno = new DvojiskoDrevo(7);
            drevo1.Desno.Levo = new DvojiskoDrevo(12);
            drevo1.Desno.Desno = new DvojiskoDrevo(18);

            SetInitialPositions(drevo1, 100, 50, 50, 50);

            drevesa.Add(drevo1);
        }

        private void InitializeTreePositions()
        {
            int sirina = pictureBox.Width;
            foreach (DvojiskoDrevo drevo in drevesa)
            {
                SetInitialPositions(drevo, (sirina + Width) / 2, 50, (Width - sirina) / 4, (Height - sirina) / 4);
            }
        }

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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (drevesa.Count != 0)
            {
                foreach (DvojiskoDrevo drevo in drevesa)
                {
                    e.Graphics.TranslateTransform(offset.X, offset.Y); // Translate to maintain zoom center
                    DrawTree(e.Graphics, drevo);
                }
            }
        }

        private void DrawTree(Graphics g, DvojiskoDrevo vozlisce)
        {
            if (vozlisce == null || vozlisce.Prazno)
                return;

            float velikostVozlisca = Math.Min(Width, Height) / NodeSizeRatio;
            float scaledFontSize = velikostVozlisca / 3; // Adjust font size relative to node size
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

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (radioButtonDodaj.Checked)
            {
                // Se izvede v primeru, da smo zbrisali celotno drevo oziroma drevesa sploh ni. Dodamo koren brez sinov.
                if (drevesa.Count == 0 && int.TryParse(textBox.Text, out int vrednost))
                {
                    // dodamo stanje pred novo operacijo
                    //Stanje(drevo);
                    DvojiskoDrevo novoDrevo = new DvojiskoDrevo(vrednost);
                    drevesa.Add(novoDrevo);
                    // Set initial position for the root node
                    InitializeTreePositions();
                    Invalidate();
                }
                // v primeru, da je drevo obstajalo, zbrišemo tisto na katero smo kliknili
                else {  
                    foreach (DvojiskoDrevo drevo in drevesa)
                {
                    izbranoVozlisce = FindNodeAtPosition(drevo, e.Location);
                    if (izbranoVozlisce != null)
                    {
                        Stanje(drevo);
                        addNodeMenu.Show(this, e.Location);                        break;
                    }
                }
                }
            }
            else if (radioButtonOdstrani.Checked)
            {
                for (int i = 0; i < drevesa.Count; i++)
                {
                    foreach (DvojiskoDrevo drevo in drevesa)
                    {
                        DvojiskoDrevo nodeToRemove = FindNodeAtPosition(drevo, e.Location);
                        if (nodeToRemove != null)
                        {
                            Stanje(drevo);
                            if (drevo == nodeToRemove)
                            {
                                drevesa.Remove(drevo);
                            }
                            else
                            {
                                IzbrisiVozlisce(drevo, nodeToRemove);
                            }
                            Invalidate();
                            break;
                        }
                    }
                }
            }
        }

        private void Stanje(DvojiskoDrevo drevo)
        {
            prejsnja_stanja.Push(drevo.IzDrevesaVSlovar());
            naslednja_stanja.Clear();
        }

        private void AddLeftNode_Click(object sender, EventArgs e)
        {
            if (izbranoVozlisce != null && int.TryParse(textBox.Text, out int vrednost))
            {
                if (izbranoVozlisce.Levo == null || izbranoVozlisce.Levo.Prazno)
                {
                    izbranoVozlisce.Levo = new DvojiskoDrevo(vrednost);
                    InitializeTreePositions();
                    izbranoVozlisce = null; // Reset the selected node to avoid coloring
                    Invalidate();
                }
                else
                {
                    MessageBox.Show("The selected node already has a left child.");
                }
            }
        }

        private void AddRightNode_Click(object sender, EventArgs e)
        {
            if (izbranoVozlisce != null && int.TryParse(textBox.Text, out int vrednost))
            {
                if (izbranoVozlisce.Desno == null || izbranoVozlisce.Desno.Prazno)
                {
                    izbranoVozlisce.Desno = new DvojiskoDrevo(vrednost);
                    InitializeTreePositions();
                    izbranoVozlisce = null; // Reset the selected node to avoid coloring
                    Invalidate();
                }
                else
                {
                    MessageBox.Show("The selected node already has a right child.");
                }
            }
        }

        private bool IzbrisiVozlisce(DvojiskoDrevo parent, DvojiskoDrevo nodeToRemove)
        {
            if (parent == null || parent.Prazno)
                return false;

            if (parent.Levo == nodeToRemove)
            {
                parent.Levo = null;
                return true;
            }
            else if (parent.Desno == nodeToRemove)
            {
                parent.Desno = null;
                return true;
            }

            return IzbrisiVozlisce(parent.Levo, nodeToRemove) || IzbrisiVozlisce(parent.Desno, nodeToRemove);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragStart.HasValue && draggedNode != null)
            {
                float dx = e.X - dragStart.Value.X;
                float dy = e.Y - dragStart.Value.Y;
                draggedNode.PosX = zacetnaPozicija.X + dx;
                draggedNode.PosY = zacetnaPozicija.Y + dy;
                Invalidate();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (dragStart.HasValue && draggedNode != null)
            {
                draggedNode = null;
                Cursor = Cursors.Default;
                Invalidate();
            }
        }

        private DvojiskoDrevo FindNodeAtPosition(DvojiskoDrevo vozlisce, Point lokacija)
        {
            if (vozlisce == null || vozlisce.Prazno)
                return null;

            float nodeSize = Math.Min(Width, Height) / NodeSizeRatio;
            RectangleF rect = new RectangleF(vozlisce.PosX - nodeSize / 2, vozlisce.PosY - nodeSize / 2, nodeSize, nodeSize);
            if (rect.Contains(lokacija))
                return vozlisce;

            DvojiskoDrevo found = FindNodeAtPosition(vozlisce.Levo, lokacija);
            if (found == null)
                found = FindNodeAtPosition(vozlisce.Desno, lokacija);
            return found;
        }

        private void InicializacijaPrehoda()
        {
            casovnikPrehoda = new Timer();
            casovnikPrehoda.Interval = 500;
            casovnikPrehoda.Tick += CasovnikPrehoda_Tick;
        }

        private void ZacniPrehod(List<DvojiskoDrevo> pot)
        {
            potPrehoda = pot;
            trenutniIndeksPrehoda = 0;
            casovnikPrehoda.Start();
        }

        private void CasovnikPrehoda_Tick(object sender, EventArgs e)
        {
            if (trenutniIndeksPrehoda < potPrehoda.Count)
            {
                izbranoVozlisce = potPrehoda[trenutniIndeksPrehoda];
                trenutniIndeksPrehoda++;
            }
            else
            {
                casovnikPrehoda.Stop();
                izbranoVozlisce = null;
            }
            Invalidate();
        }

        private void PremiPregledGumb_Click(object sender, EventArgs e)
        {
            List<DvojiskoDrevo> pot = new List<DvojiskoDrevo>();
            foreach (DvojiskoDrevo drevo in drevesa)
            {
                PregledDrevesa(drevo, pot, "kld");
            }
            ZacniPrehod(pot);
        }

        private void VmesniPregledGumb_Click(object sender, EventArgs e)
        {
            List<DvojiskoDrevo> pot = new List<DvojiskoDrevo>();
            foreach (DvojiskoDrevo drevo in drevesa)
            {
                PregledDrevesa(drevo, pot, "lkd");
            }
            ZacniPrehod(pot);
        }

        private void ObratniPregledGumb_Click(object sender, EventArgs e)
        {
            List<DvojiskoDrevo> pot = new List<DvojiskoDrevo>();
            foreach (DvojiskoDrevo drevo in drevesa)
            {
                PregledDrevesa(drevo, pot, "ldk");
            }
            ZacniPrehod(pot);
        }

        private void PregledDrevesa(DvojiskoDrevo vozlisce, List<DvojiskoDrevo> pot, string vzorec)
        {
            if (vozlisce == null || vozlisce.Prazno) return;
            foreach (char znak in vzorec)
            {
                switch (znak)
                {
                    case 'l':
                        PregledDrevesa(vozlisce.Levo, pot, vzorec);
                        break;
                    case 'd':
                        PregledDrevesa(vozlisce.Desno, pot, vzorec);
                        break;
                    case 'k':
                        pot.Add(vozlisce);
                        break;
                }
            }
        }

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

        private void IzvoziGumb_Click(object sender, EventArgs e)
        {
            if (drevesa.Count > 0)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                    DefaultExt = "txt"
                };
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    ZapisiDrevoVDatoteko(drevesa[0], saveFileDialog.FileName); // Zapisi prvo drevo v seznamu v datoteko
            }
            else MessageBox.Show("Ni dreves za izvoz.");
        }

        private void RazveljaviGumb_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, int> stanje = prejsnja_stanja.Pop();
                // ce smo izbrisali celotno drevo
                if (drevesa.Count == 0)
                {
                    DvojiskoDrevo praznoDrevo = new DvojiskoDrevo(); // prazno dvojisko drevo
                    naslednja_stanja.Push(praznoDrevo.IzDrevesaVSlovar());
                }
                // ce smo naredili operacijo na drevesu in drevo še vedno obstaja
                else { 
                    naslednja_stanja.Push(drevesa[0].IzDrevesaVSlovar());
                }
                drevesa.Clear();
                drevesa.Add(DvojiskoDrevo.IzSlovarja(stanje));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Prejšnje stanje ne obstaja!");
            }
            InitializeTreePositions();
            Invalidate();
        }

        private void ObnoviGumb_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, int> stanje = naslednja_stanja.Pop();
                prejsnja_stanja.Push(drevesa[0].IzDrevesaVSlovar());
                drevesa.Clear();
                drevesa.Add(DvojiskoDrevo.IzSlovarja(stanje));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Naslednje stanje ne obstaja!");
            }
            InitializeTreePositions();
            Invalidate();
        }

        private void PreberiDrevoIzDatoteke(string filePath)
        {
            Dictionary<string, int> slovar = new Dictionary<string, int>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(':');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int value))
                    {
                        slovar.Add(parts[0], value);
                    }
                }
            }

            DvojiskoDrevo novoDrevo = DvojiskoDrevo.IzSlovarja(slovar);

            drevesa.Clear();
            drevesa.Add(novoDrevo);
            prejsnja_stanja.Push(novoDrevo.IzDrevesaVSlovar());
            InitializeTreePositions();
            Invalidate();
        }

        private void ZapisiDrevoVDatoteko(DvojiskoDrevo drevo, string filePath)
        {
            Dictionary<string, int> slovar = drevo.IzDrevesaVSlovar();
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (KeyValuePair<string, int> element in slovar)
                {
                    writer.WriteLine($"{element.Key}:{element.Value}");
                }
            }
        }
    }
}