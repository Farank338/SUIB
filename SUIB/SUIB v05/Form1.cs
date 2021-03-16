using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SUIB_v05
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"Data\", "Data.txt");
            if (File.Exists(path))
            {
                // Create a file to write to.
                string Text = "";
                Text = File.ReadAllText(path, Encoding.GetEncoding(1251));
                parse_all(Text);
            }
            draw();
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.form_MouseWheel);
        }

        public struct skobochki
        {
            public int pos;
            public string frag;
        }

        private List<skobochki> a = new List<skobochki>();

        public void parse_all(string text)
        {
            skobochki temp0;
            temp0.pos = 0;
            temp0.frag = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '{')
                {
                    a.Add(temp0);
                    temp0.pos++;
                    temp0.frag = "";
                    i++;
                }
                if (text[i] == '}')
                {
                    a.Add(temp0);
                    temp0.pos--;
                    temp0.frag = "";
                    i++;
                }
                temp0.frag += text[i];
            }
            /*
            for (int i = 0; i < a.Count; i++)
            {
                richTextBox1.Text += "---->" + a[i].frag + "<---- " + Convert.ToString(a[i].pos) + "\n";
            }*/
            for (int i = 0; i < a.Count; i++)
            {
                if (a[i].pos == 0)
                {
                    course temp1 = new course();
                    c.Add(temp1);
                }
                if (i > 0)
                {
                    if (a[i - 1].pos == 1)
                    {
                        if (a[i].pos == 2)
                        {
                            if (a[i - 1].frag.LastIndexOf("Имя") != -1)
                            {
                                if (a[i - 1].frag.Substring(a[i - 1].frag.LastIndexOf("Имя")).Equals("Имя"))
                                {
                                    c.Last().name = a[i].frag;
                                }
                            }
                        }
                    }
                    if (a[i - 1].pos == 2)
                    {
                        if (a[i].pos == 3)
                        {
                            if (a[i - 1].frag.LastIndexOf("Имя") != -1)
                            {
                                if (a[i - 1].frag.Substring(a[i - 1].frag.LastIndexOf("Имя")).Equals("Имя"))
                                {
                                    course.page temp1 = new course.page();
                                    temp1.name = a[i].frag;
                                    c.Last().p.Add(temp1);
                                }
                            }
                            if (a[i - 1].frag.LastIndexOf("Текст") != -1)
                            {
                                if (a[i - 1].frag.Substring(a[i - 1].frag.LastIndexOf("Текст")).Equals("Текст"))
                                {
                                    c.Last().p.Last().text = a[i].frag;
                                }
                            }
                            if (a[i - 1].frag.LastIndexOf("Картинки") != -1)
                            {
                                if (a[i - 1].frag.Substring(a[i - 1].frag.LastIndexOf("Картинки")).Equals("Картинки"))
                                {
                                    c.Last().p.Last().picture = a[i].frag;
                                }
                            }
                            if (a[i - 1].frag.LastIndexOf("Ответы") != -1)
                            {
                                if (a[i - 1].frag.Substring(a[i - 1].frag.LastIndexOf("Ответы")).Equals("Ответы"))
                                {
                                    c.Last().p.Last().answer = a[i].frag;
                                }
                            }
                        }
                    }
                }
            }
            /*
            for (int i = 0; i < c.Count; i++)
            {
                richTextBox1.Text += c[i].name;
                for (int j = 0; j < c[i].p.Count; j++)
                {
                    richTextBox1.Text += c[i].p[j].name;
                    richTextBox1.Text += c[i].p[j].text;
                    richTextBox1.Text += c[i].p[j].picture;
                    richTextBox1.Text += c[i].p[j].answer;
                }
            }
            */
            analyze();
        }

        private List<course> c = new List<course>();

        private void analyze()
        {
            for (int i = 0; i < c.Count; i++)
            {
                treeView1.Nodes.Add(c[i].name);
                c[i].analyze(treeView1.Nodes[treeView1.Nodes.Count - 1]);
                c[i].add_rating();
            }
        }

        public int current_course = 0;
        public int image_diagonal = 200;
        public int textboxsize = 150;
        public int answerboxsize = 150;

        public class course : Form1
        {
            public string name = "";
            public List<page> p = new List<page>();
            public TreeNode this_tree_node;
            public int current_page = 0;
            public int current_rating = 0;
            public int max_rating = 0;

            public void analyze(TreeNode a)
            {
                this_tree_node = a;
                for (int i = 0; i < p.Count; i++)
                {
                    a.Nodes.Add(p[i].name);
                    p[i].analyze(a.Nodes[a.Nodes.Count - 1]);
                }
            }

            public void add_rating()
            {
                int sum = 0;
                for (int i = 0; i < p.Count; i++)
                {
                    sum += p[i].add_rating();
                }
                if (sum > 0)
                {
                    max_rating = sum;
                    this_tree_node.Text = name + " " + current_rating + "\\" + max_rating;
                }
            }

            public void draw(Panel a)
            {
                p[current_page].draw(a);
                if (max_rating > 0)
                {
                    this_tree_node.Text = name + " " + current_rating + "\\" + max_rating;
                }
                else
                {
                    this_tree_node.Text = name;
                }
            }

            public void draw(Panel a, int d, int b, int c)
            {
                p[current_page].draw(a, d, b, c);
                if (max_rating > 0)
                {
                    this_tree_node.Text = name + " " + current_rating + "\\" + max_rating;
                }
                else
                {
                    this_tree_node.Text = name;
                }
            }

            public void change_treeview_rating(Panel a, int b)
            {
                if (max_rating > 0)
                {
                    p[b].change_treeview_rating(a);
                    this_tree_node.Text = name + " " + current_rating + "\\" + max_rating;
                }
            }

            public void chech()
            {
                p[current_page].chech();
            }

            public void chech_all()
            {
                int sum = 0;
                for (int i = 0; i < p.Count; i++)
                {
                    sum += p[i].chech(); ;
                }
                if (sum != 0)
                {
                    current_rating = sum;
                    this_tree_node.Text = name + " " + current_rating + "\\" + max_rating;
                }
            }

            public void Save(ref string stream)
            {
                if (max_rating > 0)
                {
                    stream += "\n" + name;
                    stream += "\n" + "Общий рейтинг за курс =" + current_rating + "\\" + max_rating;
                    for (int i = 0; i < p.Count; i++)
                    {
                        p[i].Save(ref stream);
                    }
                }
            }

            public class page : course
            {
                public string name = "";
                public string text = "";
                public string picture = "";
                public List<string> picture_path = new List<string>();
                public string answer = "";
                public TreeNode this_tree_node;
                public Label nameLabel = new Label();
                public RichTextBox textBox = new RichTextBox();
                public List<PictureBox> pictureBox = new List<PictureBox>();
                public CheckedListBox answerCheckedListBox;

                public int current_rating = 0;
                public int max_rating = 0;
                public bool cheched = false;
                public List<skobochki> trueanswer = new List<skobochki>();

                public void analyze(TreeNode a)
                {
                    this_tree_node = a;
                    if (name != "")
                    {
                        nameLabel.Text = name;
                        nameLabel.Location = new Point(5, 10);
                        nameLabel.Font = new Font("Times new Roman", 20);
                        nameLabel.Height = 25;
                        nameLabel.AutoSize = true;
                    }
                    if (text != "")
                    {
                        textBox.Text = text;
                        textBox.Height = textboxsize;
                        textBox.Width = 300;
                        textBox.Font = new Font("Times new Roman", 14);
                        textBox.Location = new Point(5, ((name != "") ? (nameLabel.Location.Y + nameLabel.Height) : 0) + 10);
                    }
                    if (picture != "")
                    {
                        string[] temp0 = picture.Split(',');
                        int k = 0;
                        for (int i = 0; i < temp0.Length; i++)
                        {
                            picture_path.Add(Path.Combine(@"Data\" + temp0[i]));
                            Bitmap temp2 = new Bitmap(picture_path.Last());
                            PictureBox temp3 = new PictureBox();
                            temp3.Height = image_diagonal - 10;
                            temp3.Width = image_diagonal - 10;
                            temp3.SizeMode = PictureBoxSizeMode.StretchImage;
                            temp3.Image = temp2;
                            temp3.Location = new Point(5 + (i % 3) * image_diagonal, ((text != "") ? (textBox.Location.Y + textBox.Height) : ((name != "") ? (nameLabel.Location.Y + nameLabel.Height) : 0)) + ((i > 2) ? (pictureBox[i - 3].Location.Y) : (0)) + 10);
                            pictureBox.Add(temp3);
                            if (i >= 1)
                            {
                                if (k % 4 == 3)
                                {
                                    k++;
                                }
                            }
                        }
                    }
                    if (answer != "")
                    {
                        string[] temp0 = answer.Split(',');

                        answerCheckedListBox = new CheckedListBox();
                        answerCheckedListBox.Height = answerboxsize;

                        answerCheckedListBox.Width = 300;
                        answerCheckedListBox.Font = new Font("Times new Roman", 14);
                        answerCheckedListBox.Location = new Point(5, ((picture != "") ? (pictureBox.Last().Location.Y + pictureBox.Last().Height) : ((text != "") ? (textBox.Location.Y + textBox.Height) : ((name != "") ? (nameLabel.Location.Y + nameLabel.Height) : 0))) + 10);

                        for (int i = 0; i < temp0.Length; i++)
                        {
                            if (temp0[i].Contains("!") == true)
                            {
                                skobochki temp;
                                temp.pos = 1;
                                temp.frag = temp0[i].Replace("!", "");
                                trueanswer.Add(temp);
                            }
                            else
                            {
                                skobochki temp;
                                temp.pos = 0;
                                temp.frag = temp0[i];
                                trueanswer.Add(temp);
                            }
                            answerCheckedListBox.Items.Add(temp0[i].Replace("!", ""));
                        }
                    }
                    //a.Nodes.Add(text);
                }

                public void draw(Panel place)
                {
                    GC.Collect();
                    System.Drawing.Point CurrentPoint; CurrentPoint = place.AutoScrollPosition;
                    place.SuspendLayout();
                    place.Controls.Clear();
                    place.Location = new Point(0, 0);
                    if (name != "")
                    {
                        place.Controls.Add(nameLabel);
                    }
                    if (text != "")
                    {
                        textBox.Width = place.Width - 30;
                        //textBox.Height = textboxsize;
                        place.Controls.Add(textBox);
                    }
                    if (picture != "")
                    {
                        for (int i = 0; i < pictureBox.Count; i++)
                        {
                            //pictureBox[i].Width = image_diagonal;
                            //pictureBox[i].Height = image_diagonal;

                            place.Controls.Add(pictureBox[i]);
                        }
                    }
                    if (answer != "")
                    {
                        answerCheckedListBox.Width = place.Width - 30;
                        //answerCheckedListBox.Height = answerboxsize;

                        place.Controls.Add(answerCheckedListBox);
                    }
                    place.ResumeLayout();
                    place.AutoScrollPosition = new Point(Math.Abs(CurrentPoint.X), Math.Abs(place.AutoScrollPosition.Y));
                    if (max_rating > 0)
                    {
                        this_tree_node.Text = name + " " + current_rating + "\\" + max_rating;
                    }
                    else
                    {
                        this_tree_node.Text = name;
                    }
                }

                public void draw(Panel place, int a, int b, int c)
                {
                    GC.Collect();
                    System.Drawing.Point CurrentPoint; CurrentPoint = place.AutoScrollPosition;
                    place.SuspendLayout();
                    place.Controls.Clear();
                    place.Location = new Point(0, 0);
                    if (name != "")
                    {
                        place.Controls.Add(nameLabel);
                    }
                    if (text != "")
                    {
                        textBox.Width = place.Width - 30;
                        if (textBox.Height + a > 25)
                        {
                            textBox.Height += a;
                        }
                        textBox.Location = new Point(5, ((name != "") ? (nameLabel.Location.Y + nameLabel.Height) : 0) + 10);

                        place.Controls.Add(textBox);
                    }
                    if (picture != "")
                    {
                        for (int i = 0; i < pictureBox.Count; i++)
                        {
                            if (pictureBox[i].Height + a > 25)
                            {
                                pictureBox[i].Width += b;
                                pictureBox[i].Height += b;
                            }
                            pictureBox[i].Location = new Point(5 + (i % 3) * image_diagonal, ((text != "") ? (textBox.Location.Y + textBox.Height) : ((name != "") ? (nameLabel.Location.Y + nameLabel.Height) : 0)) + ((i > 2) ? (pictureBox[i - 3].Location.Y) : (0)) + 10);

                            place.Controls.Add(pictureBox[i]);
                        }
                    }
                    if (answer != "")
                    {
                        /*if (answerCheckedListBox.Height + a > 50)
                        {
                            answerCheckedListBox.ClientSize = new Size(place.Width - 30, answerCheckedListBox.Height + a);
                            answerCheckedListBox.Size = answerCheckedListBox.ClientSize;
                        }*/
                        //answerCheckedListBox.Width = place.Width - 30;
                        answerCheckedListBox.ClientSize = new Size(textBox.Size.Width, textBox.Size.Height);
                        answerCheckedListBox.Location = new Point(5, ((picture != "") ? (pictureBox.Last().Location.Y + pictureBox.Last().Height) : ((text != "") ? (textBox.Location.Y + textBox.Height) : ((name != "") ? (nameLabel.Location.Y + nameLabel.Height) : 0))) + 10);

                        place.Controls.Add(answerCheckedListBox);
                    }
                    place.ResumeLayout();
                    place.AutoScrollPosition = new Point(Math.Abs(CurrentPoint.X), Math.Abs(place.AutoScrollPosition.Y));
                    if (max_rating > 0)
                    {
                        this_tree_node.Text = name + " " + current_rating + "\\" + max_rating;
                    }
                    else
                    {
                        this_tree_node.Text = name;
                    }
                }

                public void change_treeview_rating(Panel a)
                {
                    if (max_rating > 0)
                    {
                        this_tree_node.Text = name + " " + current_rating + "\\" + max_rating;
                    }
                }

                public int add_rating()
                {
                    int sum = 0;
                    if (answer != "")
                    {
                        string[] temp0 = answer.Split(',');

                        for (int i = 0; i < temp0.Length; i++)
                        {
                            if (temp0[i].Contains("!") == true)
                            {
                                sum++;
                            }
                        }
                    }
                    if (sum != 0)
                    {
                        max_rating = sum;
                        this_tree_node.Text = name + " " + current_rating + "\\" + max_rating;
                    }
                    return sum;
                }

                public int chech()
                {
                    int true_ans = 0;
                    int false_ans = 0;
                    int res = 0;
                    if (max_rating != 0)
                    {
                        if (cheched == false)
                        {
                            for (int i = 0; i < trueanswer.Count; i++)
                            {
                                if (answerCheckedListBox.GetItemCheckState(i) == CheckState.Checked)
                                {
                                    if (trueanswer[i].pos == 1)
                                    {
                                        true_ans++;
                                    }
                                    else
                                    {
                                        false_ans++;
                                    }
                                }
                            }
                            cheched = true;
                            //DialogResult result = MessageBox.Show(Convert.ToString(true_ans) + " " + Convert.ToString(false_ans), "Сочувствие", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            if (true_ans == max_rating && false_ans == 0)
                            {
                                current_rating = max_rating;
                                this_tree_node.Text = name + " " + current_rating + "\\" + max_rating;
                            }
                            if (true_ans < max_rating && false_ans == 0)
                            {
                                current_rating = true_ans;
                                this_tree_node.Text = name + " " + current_rating + "\\" + max_rating;
                            }
                            if (false_ans > 0)
                            {
                                current_rating = 0;
                                this_tree_node.Text = name + " " + current_rating + "\\" + max_rating;
                            }
                        }
                    }
                    return current_rating;
                }

                public void Save(ref string stream)
                {
                    if (max_rating > 0)
                    {
                        stream += "\n\t" + name;
                        stream += "\n\t" + "Рейтинг за задание=" + current_rating + "\\" + max_rating;
                    }
                }
            }
        }

        public void draw()
        {
            c[current_course].draw(panel1);
            draw_button();
        }

        public void drawall()
        {
            for (int i = 0; i < c.Count; i++)
            {
                for (int j = 0; j < c[i].p.Count; j++)
                {
                    c[i].change_treeview_rating(panel1, j);
                }
            }
            c[current_course].draw(panel1);
            draw_button();
        }

        public void draw_button()
        {
            button1.Location = new Point(0, treeView1.Location.Y + treeView1.Height);
            button2.Location = new Point(button1.Location.X + button1.Width, treeView1.Location.Y + treeView1.Height);
            button3.Location = new Point(button2.Location.X + button2.Width, treeView1.Location.Y + treeView1.Height);
            button5.Location = new Point(button3.Location.X + button3.Width, treeView1.Location.Y + treeView1.Height);
            button6.Location = new Point(button5.Location.X + button5.Width, treeView1.Location.Y + treeView1.Height);

            button4.Location = new Point(0, button1.Location.Y + button1.Height);
            textBox1.Location = new Point(button4.Location.X + button4.Width, button1.Location.Y + button1.Height);
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode.Parent != null)
            {
                //c[treeView1.SelectedNode.Index].draw_all(panel1, 0);
                current_course = treeView1.SelectedNode.Parent.Index;
                c[current_course].current_page = treeView1.SelectedNode.Index;
                draw();
            }
            else
            {
                current_course = treeView1.SelectedNode.Index;
                draw();
            }
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            draw();
        }

        private void Form1_Enter(object sender, EventArgs e)
        {
            draw();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            draw();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (c[current_course].current_page > 0)
            {
                //c[current_course].chech();
                c[current_course].current_page--;
            }
            else
            {
                if (current_course > 0)
                {
                    c[current_course].chech_all();
                    current_course--;
                    c[current_course].current_page = c[current_course].p.Count - 1;
                }
            }
            draw();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (c[current_course].current_page < c[current_course].p.Count - 1)
            {
                //c[current_course].chech();
                c[current_course].current_page++;
            }
            else
            {
                if (current_course < c.Count - 1)
                {
                    c[current_course].chech_all();
                    //MessageBox.Show("Курс завершен\nРезультаты за курс\n" + Convert.ToString(c[current_course].current_rating) + "\\" + Convert.ToString(c[current_course].max_rating), "Курс завершен", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    current_course++;
                    c[current_course].current_page = 0;
                }
            }
            draw();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            c[current_course].chech_all();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "reset" || textBox1.Text == "Reset")
            {
                c[current_course].p[c[current_course].current_page].cheched = false;
                c[current_course].current_rating -= c[current_course].p[c[current_course].current_page].current_rating;
                c[current_course].p[c[current_course].current_page].current_rating = 0;
                drawall();
                textBox1.Text = "";
            }
            if (textBox1.Text == "reset all" || textBox1.Text == "Reset all")
            {
                for (int i = 0; i < c.Count; i++)
                {
                    for (int j = 0; j < c[i].p.Count; j++)
                    {
                        c[i].p[j].cheched = false;
                        c[i].current_rating -= c[i].p[j].current_rating;
                        c[i].p[j].current_rating = 0;
                    }
                }
                drawall();
                textBox1.Text = "";
            }
        }

        private int resize = 0;

        private void Button5_Click(object sender, EventArgs e)
        {
            if (resize == 0)
            {
                resize = 1;
            }
            else
            {
                resize = 0;
            }
        }

        private void form_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (resize == 1)
            {
                c[current_course].draw(panel1, e.Delta / 10, e.Delta / 10, e.Delta / 10);

                //draw();
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt|*.txt";
            saveFileDialog1.Title = "Сохранение результатов. \n Название файла должно содержать ФИО Дату Время";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                DateTime now = DateTime.Now;

                string save_to_file = now.ToString("F");
                for (int i = 0; i < c.Count; i++)
                {
                    c[i].Save(ref save_to_file);
                }
                string temp = saveFileDialog1.FileName.Substring(0, saveFileDialog1.FileName.Length - 4) + " " + now.ToString("dd MMM yyyy HH-mm-ss") + ".txt";

                File.WriteAllText(temp, save_to_file);
            }
        }
    }
}