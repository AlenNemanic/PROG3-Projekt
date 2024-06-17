using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Projekt
{
    public partial class Form1: Form
    {
        private List<Dvojisko_drevo<int>> drevesa;
        private Point? dragStart = null;
        private Dvojisko_drevo<int> draggedNode;
        private PointF zacetnaPozicija;
        private const float NodeSizeRatio = 20;
        private float zoomFactor = 1.0f; // Initial zoom factor
        private PointF offset = new PointF(0f, 0f); // Offset to maintain the zoom center
        private Timer casovnikPrehoda;
        private List<Dvojisko_drevo<int>> potPrehoda;
        private int trenutniIndeksPrehoda;
        private Dvojisko_drevo<int> izbranoVozlisce;
        private Dvojisko_drevo<int> prvoIzbranoVozlisce = null; // First selected node for connecting

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
            MouseWheel += Form1_MouseWheel; // Add mouse wheel event handler

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
            drevesa = new List<Dvojisko_drevo<int>>();

            // Initialize a sample tree
            Dvojisko_drevo<int> drevo1 = new Dvojisko_drevo<int>(10);
            drevo1.Levo = new Dvojisko_drevo<int>(5);
            drevo1.Desno = new Dvojisko_drevo<int>(15);
            drevo1.Levo.Levo = new Dvojisko_drevo<int>(3);
            drevo1.Levo.Desno = new Dvojisko_drevo<int>(7);
            drevo1.Desno.Levo = new Dvojisko_drevo<int>(12);
            drevo1.Desno.Desno = new Dvojisko_drevo<int>(18);

            SetInitialPositions(drevo1, 100, 50, 50, 50);

            drevesa.Add(drevo1);
        }

        private void InitializeTreePositions()
        {
            int sirina = pictureBox.Width;
            foreach (Dvojisko_drevo<int> drevo in drevesa)
            {
                SetInitialPositions(drevo, (sirina + Width) / 2, 50, (Width - sirina) / 4, (Height - sirina) / 4);
            }
        }

        private void SetInitialPositions(Dvojisko_drevo<int> vozlisce, float x, float y, float xOffset, float yOffset)
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
                foreach (Dvojisko_drevo<int> drevo in drevesa)
                {
                    e.Graphics.TranslateTransform(offset.X, offset.Y); // Translate to maintain zoom center
                    e.Graphics.ScaleTransform(zoomFactor, zoomFactor); // Apply zoom factor
                    DrawTree(e.Graphics, drevo);
                }
            }
        }

        private void DrawTree(Graphics g, Dvojisko_drevo<int> vozlisce)
        {
            if (vozlisce == null || vozlisce.Prazno)
                return;

            float velikostVozlisca = Math.Min(Width, Height) / NodeSizeRatio;
            float scaledFontSize = velikostVozlisca / 3; // Adjust font size relative to node size
            using (Font scaledFont = new Font(Font.FontFamily, scaledFontSize, Font.Style))
            {
                if (vozlisce.Levo != null && !vozlisce.Levo.Prazno)
                {
                    // Use red pen if the left child is the highlighted node
                    Pen pen = izbranoVozlisce == vozlisce.Levo ? Pens.Red : Pens.Black;
                    g.DrawLine(pen, vozlisce.PosX, vozlisce.PosY, vozlisce.Levo.PosX, vozlisce.Levo.PosY);
                    DrawTree(g, vozlisce.Levo);
                }

                if (vozlisce.Desno != null && !vozlisce.Desno.Prazno)
                {
                    // Use red pen if the right child is the highlighted node
                    Pen pen = izbranoVozlisce == vozlisce.Desno ? Pens.Red : Pens.Black;
                    g.DrawLine(pen, vozlisce.PosX, vozlisce.PosY, vozlisce.Desno.PosX, vozlisce.Desno.PosY);
                    DrawTree(g, vozlisce.Desno);
                }

                // Use orange brush if the current node is the highlighted node
                Brush brush = izbranoVozlisce == vozlisce ? Brushes.Orange : Brushes.LightBlue;
                g.FillEllipse(brush, vozlisce.PosX - velikostVozlisca / 2, vozlisce.PosY - velikostVozlisca / 2, velikostVozlisca, velikostVozlisca);
                g.DrawEllipse(Pens.Black, vozlisce.PosX - velikostVozlisca / 2, vozlisce.PosY - velikostVozlisca / 2, velikostVozlisca, velikostVozlisca);

                // Measure the size of the text to center it correctly
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
                int sirina = pictureBox.Width;
                Dvojisko_drevo<int> novoDrevo = new Dvojisko_drevo<int>(1);
                drevesa.Add(novoDrevo);
                SetInitialPositions(novoDrevo, e.Location.X, e.Location.Y, (Width - sirina) / 4, (Height - sirina) / 4);
                Invalidate();
            }
            else if (radioButtonPremakni.Checked)
            {
                foreach (Dvojisko_drevo<int> drevo in drevesa)
                {
                    draggedNode = FindNodeAtPosition(drevo, e.Location);
                    if (draggedNode != null)
                    {
                        dragStart = e.Location;
                        zacetnaPozicija = new PointF(draggedNode.PosX, draggedNode.PosY);
                        // Provide visual feedback for the selected node
                        Cursor = Cursors.Hand;
                        break;
                    }
                }

                // Start panning
                if (e.Button == MouseButtons.Right)
                {
                    dragStart = e.Location;
                    Cursor = Cursors.SizeAll;
                }
            }
            else if (radioButtonOdstrani.Checked)
            {
                foreach (Dvojisko_drevo<int> drevo in drevesa)
                {
                    Dvojisko_drevo<int> nodeToRemove = FindNodeAtPosition(drevo, e.Location);
                    if (nodeToRemove != null)
                    {
                        IzbrisiVozlisce(drevo, nodeToRemove);
                        Invalidate();
                        break;
                    }
                }
            }
            else if (radioButtonPovezi.Checked)
            {
                foreach (Dvojisko_drevo<int> drevo in drevesa)
                {
                    Dvojisko_drevo<int> selectedNode = FindNodeAtPosition(drevo, e.Location);
                    if (selectedNode != null)
                    {
                        if (prvoIzbranoVozlisce == null)
                        {
                            prvoIzbranoVozlisce = selectedNode;
                        }
                        else
                        {
                            PoveziVozlisci(prvoIzbranoVozlisce, selectedNode);
                            prvoIzbranoVozlisce = null;
                        }
                        break;
                    }
                }
            }
        }

        private void PoveziVozlisci(Dvojisko_drevo<int> prvoVozlisce, Dvojisko_drevo<int> drugoVozlisce)
        {
            // Remove secondNode from its current tree
            foreach (Dvojisko_drevo<int> drevo in drevesa)
            {
                if (IzbrisiVozlisce(drevo, drugoVozlisce))
                {
                    break;
                }
            }

            // Connect secondNode to firstNode
            if (prvoVozlisce.Levo == null || prvoVozlisce.Levo.Prazno)
            {
                prvoVozlisce.Levo = drugoVozlisce;
            }
            else if (prvoVozlisce.Desno == null || prvoVozlisce.Desno.Prazno)
            {
                prvoVozlisce.Desno = drugoVozlisce;
            }
            else
            {
                MessageBox.Show("The selected node already has two children.");
            }

            // Maintain secondNode's position
            drugoVozlisce.PosX = prvoVozlisce.PosX;
            drugoVozlisce.PosY = prvoVozlisce.PosY;

            InitializeTreePositions();
            Invalidate();
        }

        private bool IzbrisiVozlisce(Dvojisko_drevo<int> koren, Dvojisko_drevo<int> nodeToRemove)
        {
            if (koren == null || koren.Prazno)
                return false;

            if (koren == nodeToRemove)
            { 
                koren = new Dvojisko_drevo<int>(); // Assign an empty node
                return true;
            }
            else if (koren.Levo == nodeToRemove)
            {
                koren.Levo = new Dvojisko_drevo<int>(); // Assign an empty node
                return true;
            }
            else if (koren.Desno == nodeToRemove)
            {
                koren.Desno = new Dvojisko_drevo<int>(); // Assign an empty node
                return true;
            }

            return IzbrisiVozlisce(koren.Levo, nodeToRemove) || IzbrisiVozlisce(koren.Desno, nodeToRemove);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragStart.HasValue && draggedNode != null)
            {
                float dx = e.X - dragStart.Value.X;
                float dy = e.Y - dragStart.Value.Y;
                draggedNode.PosX = zacetnaPozicija.X + dx / zoomFactor;
                draggedNode.PosY = zacetnaPozicija.Y + dy / zoomFactor;
                Invalidate();
            }

            // Handle panning
            if (dragStart.HasValue && e.Button == MouseButtons.Right)
            {
                offset.X += e.X - dragStart.Value.X;
                offset.Y += e.Y - dragStart.Value.Y;
                dragStart = e.Location;
                Invalidate();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (dragStart.HasValue && draggedNode != null)
            {
                draggedNode = null;
                dragStart = null;
                // Reset the cursor after dragging
                Cursor = Cursors.Default;
                Invalidate();
            }

            // End panning
            if (e.Button == MouseButtons.Right)
            {
                dragStart = null;
                Cursor = Cursors.Default;
            }
        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            const float zoomIncrement = 0.1f;

            if (e.Delta > 0)
                zoomFactor += zoomIncrement;
            else if (e.Delta < 0 && zoomFactor > zoomIncrement)
                zoomFactor -= zoomIncrement;

            // Adjust the offset to ensure the zoom is centered around the mouse position
            float scale = zoomFactor / (zoomFactor + (e.Delta > 0 ? -zoomIncrement : zoomIncrement));
            offset.X = e.X - scale * (e.X - offset.X);
            offset.Y = e.Y - scale * (e.Y - offset.Y);

            Invalidate();
        }

        private Dvojisko_drevo<int> FindNodeAtPosition(Dvojisko_drevo<int> vozlisce, Point lokacija)
        {
            if (vozlisce == null || vozlisce.Prazno)
                return null;

            float nodeSize = Math.Min(Width, Height) / NodeSizeRatio;
            RectangleF rect = new RectangleF(vozlisce.PosX - nodeSize / 2, vozlisce.PosY - nodeSize / 2, nodeSize, nodeSize);
            if (rect.Contains(lokacija))
                return vozlisce;

            Dvojisko_drevo<int> found = FindNodeAtPosition(vozlisce.Levo, lokacija);
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

        private void ZacniPrehod(List<Dvojisko_drevo<int>> pot)
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
            List<Dvojisko_drevo<int>> pot = new List<Dvojisko_drevo<int>>();
            foreach (Dvojisko_drevo<int> drevo in drevesa)
            {
                PregledDrevesa(drevo, pot, "kld");
            }
            ZacniPrehod(pot);
        }

        private void VmesniPregledGumb_Click(object sender, EventArgs e)
        {
            List<Dvojisko_drevo<int>> pot = new List<Dvojisko_drevo<int>>();
            foreach (Dvojisko_drevo<int> drevo in drevesa)
            {
                PregledDrevesa(drevo, pot, "lkd");
            }
            ZacniPrehod(pot);
        }

        private void ObratniPregledGumb_Click(object sender, EventArgs e)
        {
            List<Dvojisko_drevo<int>> pot = new List<Dvojisko_drevo<int>>();
            foreach (Dvojisko_drevo<int> drevo in drevesa)
            {
                PregledDrevesa(drevo, pot, "ldk");
            }
            ZacniPrehod(pot);
        }

        /// <summary>
        /// Metoda za pregled drevesa. Dovoljeni znaki so 'l', 'k' in 'd'.
        /// </summary>
        /// <param name="vozlisce">Dvojiško drevo</param>
        /// <param name="pot">Pot vozlišè</param>
        /// <param name="vzorec">Vzorec po katerem bomo naredili pregled</param>
        private void PregledDrevesa(Dvojisko_drevo<int> vozlisce, List<Dvojisko_drevo<int>> pot, string vzorec)
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
    }
}