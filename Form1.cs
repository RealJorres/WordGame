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

namespace WordGame
{
    public partial class Form1 : Form
    {
        Timer time;
        int t = 0;
        int left = 1;
        string word;
        int guessCount;
        List<char> wordChar = new List<char>();
        List<string> Words = new List<string>();
        List<TextBox> textboxes = new List<TextBox>();
        public Form1()
        {
            InitializeComponent();
            LoadWords();
        }

        public void DynamicText()
        {
            TextBox t = new TextBox();
            t.Top = (panel1.DisplayRectangle.Height/2) -15;
            t.Left = left * 32;
            t.Size = new Size(30, 10);
            t.Font = new Font("Monterrat", 18);
            t.ReadOnly = true;
            left++;
            panel1.Controls.Add(t);
            textboxes.Add(t);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            textBox1.Enabled = button2.Enabled = true;
            GenerateNewWord();
        }

        private void GenerateNewWord()
        {
            textboxes.Clear();
            wordChar.Clear();
            panel1.Controls.Clear();
            Random rnd = new Random();
            word = Words[rnd.Next(0, Words.Count)].TrimStart().ToLower();
            int wl = word.Length;
            int o = 0;
            left = 1;
            for (int i = 0; i < wl; i++)
            {
                DynamicText();
            }
            foreach (char ii in word)
            {
                wordChar.Add(ii);
            }
            foreach (char iii in wordChar)
            {
                if (iii == '\'')
                {
                    textboxes[o].Text = '\''.ToString();
                }
                else if (iii == '(')
                {
                    textboxes[o].Text = '('.ToString();
                }
                else if (iii == ')')
                {
                    textboxes[o].Text = ')'.ToString();
                }
                o++;
            }
            guessCount = word.Length / 2;
            //MessageBox.Show(word);
        }

        public void LoadWords()
        {
            string path = "words.txt";
            List<string> fs = File.ReadAllLines(path).ToList(); 
            foreach (var i in fs)
            {
                Words.Add(i);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Guess();
        }

        private void Guess()
        {
            try
            {
                int o = 0;
                char[] guesss = textBox1.Text.ToCharArray();
                char guess = guesss[0];
                foreach (char i in wordChar)
                {
                    if (i == guess)
                    {
                        textboxes[o].Text = i.ToString();
                    }
                    o++;
                }
            }
            catch
            {
                MessageBox.Show("Guess field is empty.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                Guess();
            }
            else if (e.KeyChar == (char)27)
            {
                DialogResult ans =  MessageBox.Show("Reveal the answer?", "", MessageBoxButtons.YesNo);
                if (ans.ToString().ToLower() == "yes")
                {
                    time = new Timer();
                    time.Interval = 100;
                    time.Tick += time_Tick;
                    int o = 0;
                    foreach (char i in wordChar)
                    {
                        textboxes[o].Text = i.ToString();
                        o++;
                    }
                    
                }
                else
                {
                    MessageBox.Show("No changes.");
                }
                
            }
        }

        void time_Tick(object sender, EventArgs e)
        {
            if (t == 5)
            {
                time.Stop();
                GenerateNewWord();
            }
            else
            {
                t++;
            }
        }
    }

}
