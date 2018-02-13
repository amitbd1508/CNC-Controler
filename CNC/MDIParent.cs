using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CNC
{
    public partial class MDIParent : Form
    {
       

        private int childFormNumber = 0;
        public  int x = 0, y = 0;
        private List<Point> points;

        public MDIParent()
        {
            InitializeComponent();
            
            points=new List<Point>();
            
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void pnlDrawing_Paint(object sender, PaintEventArgs e)
        {

            
            // Create four Pen objects with red,
            // blue, green, and black colors and
            // different widths
            Pen redPen = new Pen(Color.Red, 1);
            Pen bluePen = new Pen(Color.Blue, 2);
            Pen greenPen = new Pen(Color.Green, 3);
            Pen blackPen = new Pen(Color.Black, 4);


            for (int i = 0; i < xy.Value; i += diff.Value)
            {
                e.Graphics.DrawLine(redPen, 0, i, xy.Value, i);
                e.Graphics.DrawLine(redPen, i, 0, i, xy.Value);
            }
            

            //Dispose of objects
            redPen.Dispose();
            bluePen.Dispose();
            greenPen.Dispose();
            blackPen.Dispose();
            foreach (Point p in points)
            {
                e.Graphics.FillRectangle(Brushes.Blue, new Rectangle(p.X * diff.Value, p.Y * diff.Value, diff.Value, diff.Value));
            }
            
            

        }

        private void pnlDrawing_MouseClick(object sender, MouseEventArgs e)
        {
             x = e.X;
             y = e.Y;

            if (x  > 7 && x< 11)
                x = 10;
            if (y > 7 && y < 11)
                y = 10;
            label1.Text = x/diff.Value + "==" + y/ diff.Value;
            points.Add(new Point(x/ diff.Value, y/ diff.Value));
            pnlDrawing.Invalidate();
            
        }

        

        private void xy_ValueChanged(object sender, EventArgs e)
        {
            pnlDrawing.Invalidate();
        }

        private void diff_ValueChanged(object sender, EventArgs e)
        {
            pnlDrawing.Invalidate();
        }

       

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            DialogResult D = colorDialog1.ShowDialog();
            if (D == DialogResult.OK)
            {
               
            }
        }

        
        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            points.Clear();
            pnlDrawing.Invalidate();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
           
        }

        private void MDIParent_Load(object sender, EventArgs e)
        {
            cmbPort.Items.AddRange(SerialPort.GetPortNames());

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            serialPort.BaudRate = 9600;
            serialPort.PortName = cmbPort.SelectedText;
            try
            {
                serialPort.Open();
                btnConnect.Text = "Connected";

            }
            catch
            {
                MessageBox.Show("Serial port Problem");
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            
            foreach (Point p in points)
            {
                send(p.X+","+p.Y);
                richTextBox1.AppendText("\nData sent: "+p.X+","+p.Y);
                Thread.Sleep(200);
                
            }
        }

        private void cmbPort_Click(object sender, EventArgs e)
        {
            cmbPort.Items.AddRange( SerialPort.GetPortNames());
        }

        private void btnXYValueSend_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort.WriteLine(txtX.Text + "," + txtY.Text);
            }
            catch (Exception ex)
            {
                
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            
        }

        void send(string msg)
        {
            try
            {
                serialPort.WriteLine(msg);
            }
            catch (Exception ex)
            {

            }
        }
    }


 
}
