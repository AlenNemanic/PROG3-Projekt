using System;
using System.Drawing;
using System.Windows.Forms;

namespace Projekt
{
    public partial class Form1 : Form
    {
        private Dvojisko_drevo<int> tree;
        private Point? dragStart = null;
        private Dvojisko_drevo<int> draggedNode;
        private PointF originalPosition;
        private const float NodeSizeRatio = 20;

        public Form1()
        {
            InitializeComponent();
            InitializeTree();
            this.Resize += Form1_Resize;
            this.MouseDown += Form1_MouseDown;
            this.MouseMove += Form1_MouseMove;
            this.MouseUp += Form1_MouseUp;

            // Enable double buffering to reduce flicker
            this.DoubleBuffered = true;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // Recalculate positions of the tree nodes when the form is resized
            SetInitialPositions(tree, this.ClientSize.Width / 2, 20, this.ClientSize.Width / 4, 30);
            this.Invalidate();
        }

        private void InitializeTree()
        {
            // Initialize a sample tree
            tree = new Dvojisko_drevo<int>(10);
            tree.Levo = new Dvojisko_drevo<int>(5);
            tree.Desno = new Dvojisko_drevo<int>(15);
            tree.Levo.Levo = new Dvojisko_drevo<int>(3);
            tree.Levo.Desno = new Dvojisko_drevo<int>(7);
            tree.Desno.Levo = new Dvojisko_drevo<int>(12);
            tree.Desno.Desno = new Dvojisko_drevo<int>(18);

            SetInitialPositions(tree, this.ClientSize.Width / 2, 20, this.ClientSize.Width / 4, 30);
        }

        private void SetInitialPositions(Dvojisko_drevo<int> node, float x, float y, float xOffset, float yOffset)
        {
            if (node.Prazno)
                return;

            node.PosX = x;
            node.PosY = y;

            if (!node.Levo.Prazno)
                SetInitialPositions(node.Levo, x - xOffset, y + yOffset, xOffset / 2, yOffset);

            if (!node.Desno.Prazno)
                SetInitialPositions(node.Desno, x + xOffset, y + yOffset, xOffset / 2, yOffset);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (tree != null)
            {
                DrawTree(e.Graphics, tree);
            }
        }

        private void DrawTree(Graphics g, Dvojisko_drevo<int> node)
        {
            if (node.Prazno)
                return;

            float nodeSize = Math.Min(this.ClientSize.Width, this.ClientSize.Height) / NodeSizeRatio;

            if (!node.Levo.Prazno)
            {
                g.DrawLine(Pens.Black, node.PosX, node.PosY, node.Levo.PosX, node.Levo.PosY);
                DrawTree(g, node.Levo);
            }

            if (!node.Desno.Prazno)
            {
                g.DrawLine(Pens.Black, node.PosX, node.PosY, node.Desno.PosX, node.Desno.PosY);
                DrawTree(g, node.Desno);
            }

            g.FillEllipse(Brushes.LightBlue, node.PosX - nodeSize / 2, node.PosY - nodeSize / 2, nodeSize, nodeSize);
            g.DrawEllipse(Pens.Black, node.PosX - nodeSize / 2, node.PosY - nodeSize / 2, nodeSize, nodeSize);
            g.DrawString(node.Podatek.ToString(), this.Font, Brushes.Black, node.PosX - nodeSize / 4, node.PosY - nodeSize / 4);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (tree != null)
            {
                draggedNode = FindNodeAtPosition(tree, e.Location);
                if (draggedNode != null)
                {
                    dragStart = e.Location;
                    originalPosition = new PointF(draggedNode.PosX, draggedNode.PosY);
                    // Provide visual feedback for the selected node
                    Cursor = Cursors.Hand;
                }
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragStart.HasValue && draggedNode != null)
            {
                float dx = e.X - dragStart.Value.X;
                float dy = e.Y - dragStart.Value.Y;
                draggedNode.PosX = originalPosition.X + dx;
                draggedNode.PosY = originalPosition.Y + dy;
                this.Invalidate();
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
                this.Invalidate();
            }
        }

        private Dvojisko_drevo<int> FindNodeAtPosition(Dvojisko_drevo<int> node, Point location)
        {
            if (node.Prazno)
                return null;

            float nodeSize = Math.Min(this.ClientSize.Width, this.ClientSize.Height) / NodeSizeRatio;
            var rect = new RectangleF(node.PosX - nodeSize / 2, node.PosY - nodeSize / 2, nodeSize, nodeSize);
            if (rect.Contains(location))
                return node;

            var found = FindNodeAtPosition(node.Levo, location);
            if (found == null)
                found = FindNodeAtPosition(node.Desno, location);
            return found;
        }
    }
}