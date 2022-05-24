using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotePad
{
    public partial class Form1 : Form
    {

        string OpenFileName;
        bool TextChange;

        private PageSettings pageSettings = new PageSettings();  // 수정할 페이지 설정을 나타내는 값을 가져오거나 설정
        private PrinterSettings printerSettings = new PrinterSettings();

        public Form1()
        {
            InitializeComponent();
            OpenFileName = "";
            TextChange = false;  ///TextChang가 변경 된것이 없다.
        }

        private void 새로만들기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(TextChange) /// 텍스트가 변경이 되었다면
            {
                if(MessageBox.Show("변경 내용을 저장하시겠습니까","저장",MessageBoxButtons.YesNo)==DialogResult.Yes)
                {
                    if(OpenFileName=="")
                    {
                        saveFileDialog1.Filter = "텍스트파일|*.txt";
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            richTextBox1.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
                        }
                    }
                    else richTextBox1.SaveFile(OpenFileName,RichTextBoxStreamType.PlainText);
                }
            }
            richTextBox1.Text = "";
            OpenFileName = "";
            TextChange = false;
        }

        private void 열기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TextChange) /// 텍스트가 변경이 되었다면
            {
                DialogResult rs = MessageBox.Show("변경 내용을 저장하시겠습니까", "저장", MessageBoxButtons.YesNoCancel); 
                if (rs == DialogResult.Yes)
                {
                    if (OpenFileName == "")
                    {
                        saveFileDialog1.Filter = "텍스트파일|*.txt";
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            richTextBox1.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.RichText);
                        }
                    }
                    else richTextBox1.SaveFile(OpenFileName, RichTextBoxStreamType.RichText);
                }
                else if (rs == DialogResult.Cancel) return;
            }
            openFileDialog1.Filter = "텍스트파일|*.txt";
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                OpenFileName = openFileDialog1.FileName;
                richTextBox1.LoadFile(OpenFileName, RichTextBoxStreamType.RichText);
            }
            TextChange = false;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            TextChange = true; ///텍스트 박스의 값이 변경이 되면 나중에 저장 여부를 물어 본다.
        }

        private void 저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenFileName == "")
            {
                saveFileDialog1.Filter = "텍스트파일|*.txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    OpenFileName = saveFileDialog1.FileName;
                    richTextBox1.SaveFile(OpenFileName, RichTextBoxStreamType.RichText);
                }
            }
            else richTextBox1.SaveFile(OpenFileName, RichTextBoxStreamType.RichText);
            TextChange = false; ///저장했으면 TextChang를 False로 변경
        }

        private void 다른이름으로저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "텍스트파일|*.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                OpenFileName = saveFileDialog1.FileName;
                richTextBox1.SaveFile(OpenFileName, RichTextBoxStreamType.RichText);
            }
            TextChange = false;
        }

        private void 페이지설정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PageSetupDialog ps = new PageSetupDialog();
            ps.PageSettings = pageSettings;
            ps.PrinterSettings = printerSettings;
            ps.AllowPrinter = true; //프린터 버튼 사용
            ps.AllowOrientation = true; // 대화장자의 방향

            ps.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            String text = richTextBox1.Text;
            Font textFont = new Font(richTextBox1.Font.Name, richTextBox1.Font.Size); //폰트를 메모장 폰트로 설정
            int leftMargin = e.MarginBounds.Left;
            int topMargin = e.MarginBounds.Top;
            e.Graphics.DrawString(text, textFont, Brushes.Black, leftMargin, topMargin);
        }

        private void 인쇄ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            printDocument1.PrinterSettings = printerSettings;
            printDocument1.DefaultPageSettings = pageSettings;

            PrintDialog pd = new PrintDialog();
            pd.Document = printDocument1;
            if(pd.ShowDialog()==DialogResult.OK)
            {
                printDocument1.Print(); //인쇄 시작
            }
        }


        private void 끝내기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TextChange) /// 텍스트가 변경이 되었다면
            {
                DialogResult rs = MessageBox.Show("변경 내용을 저장하시겠습니까", "저장", MessageBoxButtons.YesNoCancel);
                if (rs == DialogResult.Yes)
                {
                    if (OpenFileName == "")
                    {
                        saveFileDialog1.Filter = "텍스트파일|*.txt";
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            richTextBox1.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.RichText);
                        }
                    }
                    else richTextBox1.SaveFile(OpenFileName, RichTextBoxStreamType.RichText);
                }
                else if (rs == DialogResult.Cancel) return;
            }

            Application.Exit();
        }

        private void 잘라내기ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void 복사ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void 붙여넣기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void 글꼴ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = richTextBox1.SelectionFont;
            if(fd.ShowDialog()==DialogResult.OK)
            {
                richTextBox1.SelectionFont = fd.Font;
            }
        }

        private void 색상ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = richTextBox1.SelectionColor;
            if (cd.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SelectionColor = cd.Color;
            }
        }
    }
}
