using System;
using System.Collections.Generic;
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
        private float zoomFactor = 1.0f; // Initial zoom factor
        private PointF offset = new PointF(0f, 0f); // Offset to maintain the zoom center
        private Timer traversalTimer;
        private List<Dvojisko_drevo<int>> traversalPath;
        private int currentTraversalIndex;
        private Dvojisko_drevo<int> highlightedNode;

        public Form1()
        {
            InitializeComponent();
            InitializeTree();
            InitializeTreePositions(); // Set initial positions
            InitializeTraversal(); // Initialize the traversal functionality
            this.Resize += Form1_Resize;
            this.MouseDown += Form1_MouseDown;
            this.MouseMove += Form1_MouseMove;
            this.MouseUp += Form1_MouseUp;
            this.MouseWheel += Form1_MouseWheel; // Add mouse wheel event handler

            // Enable double buffering to reduce flicker
            this.DoubleBuffered = true;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            InitializeTreePositions(); // Recalculate positions of the tree nodes when the form is resized
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

            InitializeTreePositions(); // Set initial positions
        }

        private void InitializeTreePositions()
        {
            SetInitialPositions(tree, this.ClientSize.Width / 2, 20, this.ClientSize.Width / 4, this.ClientSize.Height / 4);
        }

        private void SetInitialPositions(Dvojisko_drevo<int> node, float x, float y, float xOffset, float yOffset)
        {
            if (node == null || node.Prazno)
                return;

            node.PosX = x;
            node.PosY = y;

            if (node.Levo != null && !node.Levo.Prazno)
                SetInitialPositions(node.Levo, x - xOffset, y + yOffset, xOffset / 2, yOffset);

            if (node.Desno != null && !node.Desno.Prazno)
                SetInitialPositions(node.Desno, x + xOffset, y + yOffset, xOffset / 2, yOffset);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (tree != null)
            {
                e.Graphics.TranslateTransform(offset.X, offset.Y); // Translate to maintain zoom center
                e.Graphics.ScaleTransform(zoomFactor, zoomFactor); // Apply zoom factor
                DrawTree(e.Graphics, tree);
            }
        }

        private void DrawTree(Graphics g, Dvojisko_drevo<int> node)
        {
            if (node == null || node.Prazno)
                return;

            float nodeSize = Math.Min(this.ClientSize.Width, this.ClientSize.Height) / NodeSizeRatio;
            float scaledFontSize = nodeSize / 3; // Adjust font size relative to node size
            using (Font scaledFont = new Font(this.Font.FontFamily, scaledFontSize, this.Font.Style))
            {
                if (node.Levo != null && !node.Levo.Prazno)
                {
                    // Use red pen if the left child is the highlighted node
                    Pen pen = highlightedNode == node.Levo ? Pens.Red : Pens.Black;
                    g.DrawLine(pen, node.PosX, node.PosY, node.Levo.PosX, node.Levo.PosY);
                    DrawTree(g, node.Levo);
                }

                if (node.Desno != null && !node.Desno.Prazno)
                {
                    // Use red pen if the right child is the highlighted node
                    Pen pen = highlightedNode == node.Desno ? Pens.Red : Pens.Black;
                    g.DrawLine(pen, node.PosX, node.PosY, node.Desno.PosX, node.Desno.PosY);
                    DrawTree(g, node.Desno);
                }

                // Use orange brush if the current node is the highlighted node
                Brush brush = highlightedNode == node ? Brushes.Orange : Brushes.LightBlue;
                g.FillEllipse(brush, node.PosX - nodeSize / 2, node.PosY - nodeSize / 2, nodeSize, nodeSize);
                g.DrawEllipse(Pens.Black, node.PosX - nodeSize / 2, node.PosY - nodeSize / 2, nodeSize, nodeSize);

                // Measure the size of the text to center it correctly
                SizeF textSize = g.MeasureString(node.Podatek.ToString(), scaledFont);
                float textX = node.PosX - textSize.Width / 2;
                float textY = node.PosY - textSize.Height / 2;

                g.DrawString(node.Podatek.ToString(), scaledFont, Brushes.Black, textX, textY);
            }
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

            // Start panning
            if (e.Button == MouseButtons.Right)
            {
                dragStart = e.Location;
                Cursor = Cursors.SizeAll;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragStart.HasValue && draggedNode != null)
            {
                float dx = e.X - dragStart.Value.X;
                float dy = e.Y - dragStart.Value.Y;
                draggedNode.PosX = originalPosition.X + dx / zoomFactor;
                draggedNode.PosY = originalPosition.Y + dy / zoomFactor;
                this.Invalidate();
            }

            // Handle panning
            if (dragStart.HasValue && e.Button == MouseButtons.Right)
            {
                offset.X += e.X - dragStart.Value.X;
                offset.Y += e.Y - dragStart.Value.Y;
                dragStart = e.Location;
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
            {
                zoomFactor += zoomIncrement;
            }
            else if (e.Delta < 0 && zoomFactor > zoomIncrement)
            {
                zoomFactor -= zoomIncrement;
            }

            // Adjust the offset to ensure the zoom is centered around the mouse position
            float scale = zoomFactor / (zoomFactor + (e.Delta > 0 ? -zoomIncrement : zoomIncrement));
            offset.X = e.X - scale * (e.X - offset.X);
            offset.Y = e.Y - scale * (e.Y - offset.Y);

            this.Invalidate();
        }

        private Dvojisko_drevo<int> FindNodeAtPosition(Dvojisko_drevo<int> node, Point location)
        {
            if (node == null || node.Prazno)
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

        private void InitializeTraversal()
        {
            traversalTimer = new Timer();
            traversalTimer.Interval = 500; // Set the interval for the animation (in milliseconds)
            traversalTimer.Tick += TraversalTimer_Tick;
        }

        private void StartTraversal(List<Dvojisko_drevo<int>> path)
        {
            traversalPath = path;
            currentTraversalIndex = 0;
            traversalTimer.Start();
        }

        private void TraversalTimer_Tick(object sender, EventArgs e)
        {
            if (currentTraversalIndex < traversalPath.Count)
            {
                highlightedNode = traversalPath[currentTraversalIndex];
                currentTraversalIndex++;
                this.Invalidate();
            }
            else
            {
                traversalTimer.Stop();
                highlightedNode = null;
                this.Invalidate();
            }
        }

        private void PreOrderButton_Click(object sender, EventArgs e)
        {
            var path = new List<Dvojisko_drevo<int>>();
            PreOrderTraversal(tree, path);
            StartTraversal(path);
        }

        private void InOrderButton_Click(object sender, EventArgs e)
        {
            var path = new List<Dvojisko_drevo<int>>();
            InOrderTraversal(tree, path);
            StartTraversal(path);
        }

        private void PostOrderButton_Click(object sender, EventArgs e)
        {
            var path = new List<Dvojisko_drevo<int>>();
            PostOrderTraversal(tree, path);
            StartTraversal(path);
        }

        private void PreOrderTraversal(Dvojisko_drevo<int> node, List<Dvojisko_drevo<int>> path)
        {
            if (node == null || node.Prazno) return;
            path.Add(node);
            PreOrderTraversal(node.Levo, path);
            PreOrderTraversal(node.Desno, path);
        }

        private void InOrderTraversal(Dvojisko_drevo<int> node, List<Dvojisko_drevo<int>> path)
        {
            if (node == null || node.Prazno) return;
            InOrderTraversal(node.Levo, path);
            path.Add(node);
            InOrderTraversal(node.Desno, path);
        }

        private void PostOrderTraversal(Dvojisko_drevo<int> node, List<Dvojisko_drevo<int>> path)
        {
            if (node == null || node.Prazno) return;
            PostOrderTraversal(node.Levo, path);
            PostOrderTraversal(node.Desno, path);
            path.Add(node);
        }
    }
}
