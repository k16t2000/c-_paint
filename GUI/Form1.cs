using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class Form1 : Form
    {
        bool drawing;//global peremennie dlja risovanika
        int historyCounter;// S4et4ik istorii

        GraphicsPath currentPath;
        Point oldLocation;
        Point CurrentPoint;
        
        public Pen currentPen;
        Color historyColor;//sohranenie tekushego zveta pered ispolzovanie Lastika
        List<Image> History;// Spisok dlja istorii

        Color CurrentColor = Color.Black;//zvet dobavila************
        Graphics g;//*******************************


        //v panel2 aktivirovala autoscroll=true, dlja prosmotra bolshih kartinok
        public Form1()
        {
            InitializeComponent();

            

            drawing = false;//peremennaja otvetstvena za risovanie
            currentPen = new Pen(Color.Black);//inizilizoravili pero -4ernoe
            currentPen.Width = trackBar1.Value;//inizializazija tolshini pera
            History = new List<Image>();// inizializazija Spiska dlja istorii

            BackColor = Color.FromArgb(224, 224, 224);
            toolStrip1.BackColor= Color.FromArgb(224, 224, 224);
            menuStrip1.BackColor=Color.FromArgb(224, 224, 224);
            //this.picDrawingSurface.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);vizivaet srazu sohranenie faila, posle risovanika


            g = picDrawingSurface.CreateGraphics();//********************************************
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)//menu panel, button new
        {
            History.Clear();//o4ishaem istoriju,kak tolko polzovatel sozdaet novij fail
            historyCounter = 0;
            Bitmap pic = new Bitmap(686, 322);//razmeri PictureBox
            picDrawingSurface.Image = pic;
            History.Add(new Bitmap(picDrawingSurface.Image));
            //if user in one time want create 2 files, we make possibility to save first file
            if (picDrawingSurface.Image != null)
            {
                var result = MessageBox.Show("Save current picture before creating new?", "Alert", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.No:break;
                    case DialogResult.Yes:saveToolStripMenuItem_Click(sender, e);break;
                    case DialogResult.Cancel: return;
                }
            }

        }

        

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)//menu panel, button save
        {
            /*proveraem, est li voobshe v Pictureox risunok, esli est, to sohranaem, esli net, to net*/
            if (picDrawingSurface.Image != null)
            {
                SaveFileDialog SaveDlg = new SaveFileDialog();
                SaveDlg.Title = "Save an Image File";
                SaveDlg.OverwritePrompt = true;//perezapisat, dialogovoe soobshenie
                SaveDlg.CheckPathExists = true;//put k failu, dialogovoe soobshenie
                SaveDlg.Filter = "Image Files (*BMP)|*.BMP|Image Files (*JPG)|*.JPG|Image Files (*GIF)|*.GIF|Image Files (*PNG)|*PNG|All files(*.*)|*.*";
                SaveDlg.ShowHelp = true;//spravka

                if (SaveDlg.ShowDialog() == DialogResult.OK) //esli soranaem s pomoshju knopki ok
                {
                    try
                    {
                        picDrawingSurface.Image.Save(SaveDlg.FileName);//FileName-put kuda sohranaem
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Not possible save picture","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);//knopka ok i ikonka
                    }
                }
                
            }
            
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)//menu panel, button open
        {
            OpenFileDialog OP = new OpenFileDialog();
            OP.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|GIF Image|*.gif|PNG Image|*png";
            OP.Title = "Open an Image File";
            OP.FilterIndex = 1;//by default choosed .jpg

            //download pic in PictureBox
            if (OP.ShowDialog() != DialogResult.Cancel)
                picDrawingSurface.Load(OP.FileName);
            picDrawingSurface.AutoSize = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)//ToolStrip, new
        {
            History.Clear();//o4ishaem istoriju,kak tolko polzovatel sozdaet novij fail
            historyCounter = 0;
            Bitmap pic = new Bitmap(686, 322);//razmeri PictureBox
            picDrawingSurface.Image = pic;
            History.Add(new Bitmap(picDrawingSurface.Image));
            //if user in one time want create 2 files, we make possibility to save first file
            if (picDrawingSurface.Image != null)
            {
                var result = MessageBox.Show("Save current picture before creating new?", "Alert", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.No: break;
                    case DialogResult.Yes: saveToolStripMenuItem_Click(sender, e); break;
                    case DialogResult.Cancel: return;
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)//ToolStrip, save
        {
            /*proveraem, est li voobshe v Pictureox risunok, esli est, to sohranaem, esli net, to net*/
            if (picDrawingSurface.Image != null)
            {
                SaveFileDialog SaveDlg = new SaveFileDialog();
                SaveDlg.Title = "Save an Image File";
                SaveDlg.OverwritePrompt = true;//perezapisat, dialogovoe soobshenie
                SaveDlg.CheckPathExists = true;//put k failu, dialogovoe soobshenie
                SaveDlg.Filter = "Image Files (*BMP)|*.BMP|Image Files (*JPG)|*.JPG|Image Files (*GIF)|*.GIF|Image Files (*PNG)|*PNG|All files(*.*)|*.*";
                SaveDlg.ShowHelp = true;//spravka

                if (SaveDlg.ShowDialog() == DialogResult.OK) //esli soranaem s pomoshju knopki ok
                {
                    try
                    {
                        picDrawingSurface.Image.Save(SaveDlg.FileName);//FileName-put kuda sohranaem
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Not possible save picture", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);//knopka ok i ikonka
                    }
                }

            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)//ToolStrip, open
        {
            OpenFileDialog OP = new OpenFileDialog();
            OP.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|GIF Image|*.gif|PNG Image|*png";
            OP.Title = "Open an Image File";
            OP.FilterIndex = 1;//by default choosed .jpg

            //download pic in PictureBox
            if (OP.ShowDialog() != DialogResult.Cancel)
                picDrawingSurface.Load(OP.FileName);
            picDrawingSurface.AutoSize = true;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)//ToolStrip, exit
        {
            Application.Exit();
        }

        private void picDrawingSurface_MouseDown_1(object sender, MouseEventArgs e)//MouseDown from PictureBox, otve4aet za nazatuju knopku, drawing=true or false
        {

            if (picDrawingSurface.Image == null)//if PictureBox not initialized,then shows message, the programs does't go wrong
            {
                MessageBox.Show("Create a new fail!");
                return;
            }
            
            if (e.Button == MouseButtons.Left)
            {
                drawing = true;
                oldLocation = e.Location;
                CurrentPoint = e.Location;//********************************
                currentPath = new GraphicsPath();
                currentPen = new Pen(CurrentColor, trackBar1.Value);//*Color.Black***************//
                currentPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;//nakone4nik pen
                currentPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                //currentPen.Width = 5;
                //currentPen.Color = historyColor;dobavit if esli historycolor ne null, to priravnivaem k tekushemu, esli  0, to 4ernij zvet
                if (solidToolStripMenuItem.Checked)
                {
                    currentPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;//zadali stil pera
                }

                if (dotToolStripMenuItem.Checked)
                {
                    currentPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;//zadali stil pera
                    //currentPen.Color = System.Drawing.Color.RoyalBlue;   
                }
                if (dashDotToolStripMenuItem.Checked)
                {
                    currentPen.DashStyle= System.Drawing.Drawing2D.DashStyle.DashDotDot;//zadali stil pera
                }

            }
            if (e.Button == MouseButtons.Right)//LASTIK, esli nazata pravaja knopka,
            {
                drawing = true;
                oldLocation = e.Location;
                currentPath = new GraphicsPath();
                historyColor = currentPen.Color;//historycolor zapisivaet tekushij zvet 
                currentPen = new Pen(Color.White);//menaem tekushij zvet na belij
                currentPen.Width=5;
            }
        }

        

        private void picDrawingSurface_MouseUp_1(object sender, MouseEventArgs e)//MouseUp-otve4aet za otpushennuju levuju knopku
        {
            //o4istka nenuznoj istorii
            History.RemoveRange(historyCounter + 1, History.Count - historyCounter - 1);
            History.Add(new Bitmap(picDrawingSurface.Image));//zanosim v spisok
            if (historyCounter + 1 < 10) historyCounter++;
            if (historyCounter - 1 == 10) History.RemoveAt(0);//o4ishaem 1 element, eli bolee 9 elementov
            drawing = false;
            try
            {
                currentPath.Dispose();
                
            }
            catch { };
        }
        private void picDrawingSurface_MouseMove_1(object sender, MouseEventArgs e)//MouseMove-otve4aet za peremeshenie,risovanie
        {
            label1.Text = e.X.ToString() + ", " + e.Y.ToString();//Label, MouseEventArgs e -peredast koordinati X,Y, esli obratitsa-e.X, e.Y,
            //razdelaem zapatoj koordinati polozenija mishi v PictureBox

            if (drawing)
            {
                
                Graphics g = Graphics.FromImage(picDrawingSurface.Image);
              
                
                currentPath.AddLine(oldLocation, e.Location);
                g.DrawPath(currentPen, currentPath);
                oldLocation = e.Location;
                g.Dispose();
                picDrawingSurface.Invalidate();
            }
            
        }

        private void trackBar1_Scroll(object sender, EventArgs e)//TrackBar menaet zna4enija X i Y peremeshenie mishi
        {
            currentPen.Width = trackBar1.Value;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)//knopka Undo
        {
            if (History.Count != 0 && historyCounter != 0)
            {
                picDrawingSurface.Image = new Bitmap(History[--historyCounter]);
            }
            else MessageBox.Show("History is empty!");
        }

        private void renoToolStripMenuItem_Click(object sender, EventArgs e)// knopka Reno
        {
            if (historyCounter < History.Count - 1)
            {
                picDrawingSurface.Image = new Bitmap(History[++historyCounter]);
            }
            else MessageBox.Show("History is empty!");
        }

        private void solidToolStripMenuItem_Click(object sender, EventArgs e)//knopka Solid dlja pen
        {
            currentPen.DashStyle = DashStyle.Solid;//zadali stil pera
            solidToolStripMenuItem.Checked = true;
            dotToolStripMenuItem.Checked = false;
            dashDotToolStripMenuItem.Checked = false;
        }

        private void dotToolStripMenuItem_Click(object sender, EventArgs e)//knopka Dot dlja pen
        {
            currentPen.DashStyle = DashStyle.Dot;//zadali stil pera
            solidToolStripMenuItem.Checked = false;
            dotToolStripMenuItem.Checked = true;
            dashDotToolStripMenuItem.Checked = false;
                
           
           
        }

        private void dashDotToolStripMenuItem_Click(object sender, EventArgs e)//knopka DashDotDot dlja pen
        {
            currentPen.DashStyle = DashStyle.DashDotDot;
            solidToolStripMenuItem.Checked = false;
            dotToolStripMenuItem.Checked = false;
            dashDotToolStripMenuItem.Checked = true;
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)//panel,button color
        {
            DialogResult D = colorDialog1.ShowDialog();
            if (D == System.Windows.Forms.DialogResult.OK)
            {
                CurrentColor = colorDialog1.Color;
            }
        }
        private void for_paint()
        {
            Pen p = new Pen(CurrentColor);//**********************************************
            g.DrawLine(p, oldLocation, CurrentPoint);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)//button toolstrip colors
        {
            DialogResult D = colorDialog1.ShowDialog();
            if (D == System.Windows.Forms.DialogResult.OK)
            {
                CurrentColor = colorDialog1.Color;
            }
        }
    }
}
