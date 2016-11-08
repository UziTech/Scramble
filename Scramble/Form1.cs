using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace Scramble
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _regKey.CreateSubKey(@"Software\Scramble");
            _regKey = _regKey.OpenSubKey(@"Software\Scramble", true);
            _speed = (int)_regKey.GetValue("Speed", 80);
            speedToolStripMenuItem.Text = "Speed = " + _speed.ToString();
            //int j = 0;
            //foreach (string i in Properties.Resources.wordList.Split('\n'))
            //{
            //    wor[j] = i.Split('\r')[0];
            //    j++;
            //}
            autoToolStripMenuItem.Text = "Manual";
        }
        #region Global Variables
        CaptureScreen _desktop = new CaptureScreen();
        Font _qu = new Font("Microsoft Sans Serif", 19);
        Font _norm = new Font("Microsoft Sans Serif", 28);
        bool _done = true;
        bool _start = false;
        bool _showNum = false;
        bool _E5 = true;
        int _speed = 80;
        RegistryKey _regKey = Registry.CurrentUser;
        //const int words = 178593;
        //string[] wor = new string[words];
        #endregion
        private void button_KeyPress(object sender, KeyPressEventArgs e)
        {
            //stopToolStripMenuItem.Text = "Start";
            timer1.Enabled = false;
            _done = true;
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                {
                    if (listBox1.Items.Count == 0 && sender.Equals(this.button[i, j]))
                    {
                        if (button[i, j].Text == "Q" && e.KeyChar.ToString().ToUpper() == "U")
                        {
                            button[i, j].Font = _qu;
                            button[i, j].Text = "Qu";
                            if (j == 4)
                            {
                                if (i == 4)
                                {
                                    button[0, 0].Focus();
                                }
                                else
                                {
                                    button[i + 1, 0].Focus();
                                }
                            }
                            else if (j == 3 && !_E5)
                            {
                                if (i == 3)
                                {
                                    button[0, 0].Focus();
                                }
                                else
                                {
                                    button[i + 1, 0].Focus();
                                }
                            }
                            else
                            {
                                button[i, j + 1].Focus();
                            }
                        }
                        else if (e.KeyChar == '')
                        {
                            if (j == 0)
                            {
                                if (_E5)
                                {
                                    if (i == 0)
                                    {
                                        button[4, 4].Focus();
                                    }
                                    else
                                    {
                                        button[i - 1, 4].Focus();
                                    }
                                }
                                else
                                {
                                    if (i == 0)
                                    {
                                        button[3, 3].Focus();
                                    }
                                    else
                                    {
                                        button[i - 1, 3].Focus();
                                    }
                                }
                            }
                            else
                            {
                                button[i, j - 1].Focus();
                            }
                        }
                        else
                        {
                            button[i, j].Font = _norm;
                            button[i, j].Text = e.KeyChar.ToString().ToUpper();
                            if (j == 4)
                            {
                                if (i == 4)
                                {
                                    button[0, 0].Focus();
                                }
                                else
                                {
                                    button[i + 1, 0].Focus();
                                }
                            }
                            else if (j == 3 && !_E5)
                            {
                                if (i == 3)
                                {
                                    button[0, 0].Focus();
                                }
                                else
                                {
                                    button[i + 1, 0].Focus();
                                }
                            }
                            else
                            {
                                button[i, j + 1].Focus();
                            }
                        }
                        return;
                    }
                }
        }
        private void button_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                if (stopToolStripMenuItem.Text == "Stop" && listBox1.Items.Count == 0)
                {
                    if (button[1, 3].Text == "")
                        CheckListsWT();
                    else if (_E5)
                        CheckLists(7);
                    else
                        CheckLists(6);
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                    for (int j = 0; j < 5; j++)
                    {
                        if (sender.Equals(this.button[i, j]))
                        {
                            if (e.KeyValue == 40)//down
                            {
                                if ((!_E5 && i == 3) || i == 4)
                                    if (j == 0)
                                        listBox1.Focus();
                                    else
                                        button[0, j - 1].Focus();
                                else
                                    if (j == 0)
                                        if (_E5)
                                            button[i, 4].Focus();
                                        else
                                            button[i, 3].Focus();
                                    else
                                        button[i + 1, j - 1].Focus();
                            }
                            else if (e.KeyValue == 38)//up
                            {
                                if ((!_E5 && j == 3) || j == 4)
                                    if (i == 0)
                                        listBox1.Focus();
                                    else
                                        button[i, 0].Focus();
                                else
                                    if (i == 0)
                                        if (_E5)
                                            button[4, j + 1].Focus();
                                        else
                                            button[3, j + 1].Focus();
                                    else
                                        button[i - 1, j + 1].Focus();
                            }
                            else if (e.KeyValue == 37)//left
                            {
                                if (j == 0)
                                    if ((!_E5 && i == 3) || i == 4)
                                        listBox1.Focus();
                                    else
                                        button[i + 1, j].Focus();
                            }
                            else if (e.KeyValue == 39)//Right
                            {
                                if ((!_E5 && j == 3) || j == 4)
                                    if (i == 0)
                                        listBox1.Focus();
                                    else
                                        button[i - 1, j].Focus();
                            }
                            return;
                        }
                    }
            }
        }
        private void button_Click(object sender, EventArgs e)
        {
            _showNum = !_showNum;
        }
        private void CheckLists(int size)
        {
            string[,] grid = new string[size, size];
            for(int i = 1; i < 5; i++)
                for (int j = 1; j < 5; j++)
                {
                    grid[i, j] = button[i - 1, j - 1].Text;
                }
            if (size == 7)
            {
                for (int i = 1; i < 6; i++)
                {
                    grid[i, 5] = button[i - 1, 4].Text;
                    grid[5, i] = button[4, i - 1].Text;
                }
            }
            int var;
            bool q1;
            bool q2;
            bool q3;
            bool q4;
            bool q5;
            bool q6;
            bool q7;
            bool q8;
            bool q9;
            bool q10;
            bool q11;
            bool q12;
            bool q13;
            bool q14;
            string word;
            foreach (string i in Properties.Resources.wordList.Split('\n'))
            {
                word = i.Split('\r')[0];
                var = 0;
                q1 = false;
                q2 = false;
                q3 = false;
                q4 = false;
                q5 = false;
                q6 = false;
                q7 = false;
                q8 = false;
                q9 = false;
                q10 = false;
                q11 = false;
                q12 = false;
                q13 = false;
                q14 = false;
                string letter1 = word.Substring(0, 1);
                for (int row1 = 1; row1 < size - 1; row1++)
                    for (int col1 = 1; col1 < size - 1; col1++)
                    {
                        if (q1)
                        {
                            var--;
                            q1 = false;
                        }
                        if (grid[row1, col1] == letter1 || (grid[row1, col1] == "Qu" && letter1 == "Q" && word.Substring(1, 1) == "U"))
                        {
                            if (grid[row1, col1] == "Qu" && letter1 == "Q" && word.Substring(1, 1) == "U")
                            {
                                var++;
                                q1 = true;
                            }
                            string letter2 = word.Substring(var + 1, 1);
                            for (int row2 = -1; row2 < 2; row2++)
                            {
                                for (int col2 = -1; col2 < 2; col2++)
                                {
                                    if (q2)
                                    {
                                        var--;
                                        q2 = false;
                                    }
                                    if (grid[row1 + row2, col1 + col2] == letter2 && (row2 != 0 || col2 != 0) || (grid[row1 + row2, col1 + col2] == "Qu" && letter2 == "Q" && word.Length > var + 2 && word.Substring(var + 2, 1) == "U"))
                                    {
                                        if (grid[row1 + row2, col1 + col2] == "Qu" && letter2 == "Q" && word.Substring(var + 2, 1) == "U")
                                        {
                                            var++;
                                            q2 = true;
                                        }
                                        if (word.Length > var + 2)
                                        {
                                            string letter3 = word.Substring(var + 2, 1);
                                            for (int row3 = -1; row3 < 2; row3++)
                                            {
                                                for (int col3 = -1; col3 < 2; col3++)
                                                {
                                                    if (q3)
                                                    {
                                                        var--;
                                                        q3 = false;
                                                    }
                                                    if (grid[row1 + row2 + row3, col1 + col2 + col3] == letter3 && (row3 != 0 || col3 != 0) && (row2 + row3 != 0 || col2 + col3 != 0) || (grid[row1 + row2 + row3, col1 + col2 + col3] == "Qu" && letter3 == "Q" && word.Length > var + 3 && word.Substring(var + 3, 1) == "U"))
                                                    {
                                                        if (grid[row1 + row2 + row3, col1 + col2 + col3] == "Qu" && letter3 == "Q" && word.Substring(var + 3, 1) == "U")
                                                        {
                                                            var++;
                                                            q3 = true;
                                                        }
                                                        if (word.Length > var + 3)
                                                        {
                                                            string letter4 = word.Substring(var + 3, 1);
                                                            for (int row4 = -1; row4 < 2; row4++)
                                                            {
                                                                for (int col4 = -1; col4 < 2; col4++)
                                                                {
                                                                    if (q4)
                                                                    {
                                                                        var--;
                                                                        q4 = false;
                                                                    }
                                                                    if (grid[row1 + row2 + row3 + row4, col1 + col2 + col3 + col4] == letter4 && (row4 != 0 || col4 != 0) && (row3 + row4 != 0 || col3 + col4 != 0) && (row2 + row3 + row4 != 0 || col2 + col3 + col4 != 0) || (grid[row1 + row2 + row3 + row4, col1 + col2 + col3 + col4] == "Qu" && letter4 == "Q" && word.Length > var + 4 && word.Substring(var + 4, 1) == "U"))
                                                                    {
                                                                        if (grid[row1 + row2 + row3 + row4, col1 + col2 + col3 + col4] == "Qu" && letter4 == "Q" && word.Substring(var + 4, 1) == "U")
                                                                        {
                                                                            var++;
                                                                            q4 = true;
                                                                        }
                                                                        if (word.Length > var + 4)
                                                                        {
                                                                            string letter5 = word.Substring(var + 4, 1);
                                                                            for (int row5 = -1; row5 < 2; row5++)
                                                                            {
                                                                                for (int col5 = -1; col5 < 2; col5++)
                                                                                {
                                                                                    if (q5)
                                                                                    {
                                                                                        var--;
                                                                                        q5 = false;
                                                                                    }
                                                                                    if (grid[row1 + row2 + row3 + row4 + row5, col1 + col2 + col3 + col4 + col5] == letter5 && (row5 != 0 || col5 != 0) && (row4 + row5 != 0 || col4 + col5 != 0) && (row3 + row4 + row5 != 0 || col3 + col4 + col5 != 0) && (row2 + row3 + row4 + row5 != 0 || col2 + col3 + col4 + col5 != 0) || (grid[row1 + row2 + row3 + row4 + row5, col1 + col2 + col3 + col4 + col5] == "Qu" && letter5 == "Q" && word.Length > var + 5 && word.Substring(var + 5, 1) == "U"))
                                                                                    {
                                                                                        if (grid[row1 + row2 + row3 + row4 + row5, col1 + col2 + col3 + col4 + col5] == "Qu" && letter5 == "Q" && word.Substring(var + 5, 1) == "U")
                                                                                        {
                                                                                            var++;
                                                                                            q5 = true;
                                                                                        }
                                                                                        if (word.Length > var + 5)
                                                                                        {
                                                                                            string letter6 = word.Substring(var + 5, 1);
                                                                                            for (int row6 = -1; row6 < 2; row6++)
                                                                                            {
                                                                                                for (int col6 = -1; col6 < 2; col6++)
                                                                                                {
                                                                                                    if (q6)
                                                                                                    {
                                                                                                        var--;
                                                                                                        q6 = false;
                                                                                                    }
                                                                                                    if (grid[row1 + row2 + row3 + row4 + row5 + row6, col1 + col2 + col3 + col4 + col5 + col6] == letter6 && (row6 != 0 || col6 != 0) && (row5 + row6 != 0 || col5 + col6 != 0) && (row4 + row5 + row6 != 0 || col4 + col5 + col6 != 0) && (row3 + row4 + row5 + row6 != 0 || col3 + col4 + col5 + col6 != 0) && (row2 + row3 + row4 + row5 + row6 != 0 || col2 + col3 + col4 + col5 + col6 != 0) || (grid[row1 + row2 + row3 + row4 + row5 + row6, col1 + col2 + col3 + col4 + col5 + col6] == "Qu" && letter6 == "Q" && word.Length > var + 6 && word.Substring(var + 6, 1) == "U"))
                                                                                                    {
                                                                                                        if (grid[row1 + row2 + row3 + row4 + row5 + row6, col1 + col2 + col3 + col4 + col5 + col6] == "Qu" && letter6 == "Q" && word.Substring(var + 6, 1) == "U")
                                                                                                        {
                                                                                                            var++;
                                                                                                            q6 = true;
                                                                                                        }
                                                                                                        if (word.Length > var + 6)
                                                                                                        {
                                                                                                            string letter7 = word.Substring(var + 6, 1);
                                                                                                            for (int row7 = -1; row7 < 2; row7++)
                                                                                                            {
                                                                                                                for (int col7 = -1; col7 < 2; col7++)
                                                                                                                {
                                                                                                                    if (q7)
                                                                                                                    {
                                                                                                                        var--;
                                                                                                                        q7 = false;
                                                                                                                    }
                                                                                                                    if (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7, col1 + col2 + col3 + col4 + col5 + col6 + col7] == letter7 && (row7 != 0 || col7 != 0) && (row6 + row7 != 0 || col6 + col7 != 0) && (row5 + row6 + row7 != 0 || col5 + col6 + col7 != 0) && (row4 + row5 + row6 + row7 != 0 || col4 + col5 + col6 + col7 != 0) && (row3 + row4 + row5 + row6 + row7 != 0 || col3 + col4 + col5 + col6 + col7 != 0) && (row2 + row3 + row4 + row5 + row6 + row7 != 0 || col2 + col3 + col4 + col5 + col6 + col7 != 0) || (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7, col1 + col2 + col3 + col4 + col5 + col6 + col7] == "Qu" && letter7 == "Q" && word.Length > var + 7 && word.Substring(var + 7, 1) == "U"))
                                                                                                                    {
                                                                                                                        if (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7, col1 + col2 + col3 + col4 + col5 + col6 + col7] == "Qu" && letter7 == "Q" && word.Substring(var + 7, 1) == "U")
                                                                                                                        {
                                                                                                                            var++;
                                                                                                                            q7 = true;
                                                                                                                        }
                                                                                                                        if (word.Length > var + 7)
                                                                                                                        {
                                                                                                                            string letter8 = word.Substring(var + 7, 1);
                                                                                                                            for (int row8 = -1; row8 < 2; row8++)
                                                                                                                            {
                                                                                                                                for (int col8 = -1; col8 < 2; col8++)
                                                                                                                                {
                                                                                                                                    if (q8)
                                                                                                                                    {
                                                                                                                                        var--;
                                                                                                                                        q8 = false;
                                                                                                                                    }
                                                                                                                                    if (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7 + row8, col1 + col2 + col3 + col4 + col5 + col6 + col7 + col8] == letter8 && (row8 != 0 || col8 != 0) && (row7 + row8 != 0 || col7 + col8 != 0) && (row6 + row7 + row8 != 0 || col6 + col7 + col8 != 0) && (row5 + row6 + row7 + row8 != 0 || col5 + col6 + col7 + col8 != 0) && (row4 + row5 + row6 + row7 + row8 != 0 || col4 + col5 + col6 + col7 + col8 != 0) && (row3 + row4 + row5 + row6 + row7 + row8 != 0 || col3 + col4 + col5 + col6 + col7 + col8 != 0) && (row2 + row3 + row4 + row5 + row6 + row7 + row8 != 0 || col2 + col3 + col4 + col5 + col6 + col7 + col8 != 0) || (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7 + row8, col1 + col2 + col3 + col4 + col5 + col6 + col7 + col8] == "Qu" && letter8 == "Q" && word.Length > var + 8 && word.Substring(var + 8, 1) == "U"))
                                                                                                                                    {
                                                                                                                                        if (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7 + row8, col1 + col2 + col3 + col4 + col5 + col6 + col7 + col8] == "Qu" && letter8 == "Q" && word.Substring(var + 8, 1) == "U")
                                                                                                                                        {
                                                                                                                                            var++;
                                                                                                                                            q8 = true;
                                                                                                                                        }
                                                                                                                                        if (word.Length > var + 8)
                                                                                                                                        {
                                                                                                                                            string letter9 = word.Substring(var + 8, 1);
                                                                                                                                            for (int row9 = -1; row9 < 2; row9++)
                                                                                                                                            {
                                                                                                                                                for (int col9 = -1; col9 < 2; col9++)
                                                                                                                                                {
                                                                                                                                                    if (q9)
                                                                                                                                                    {
                                                                                                                                                        var--;
                                                                                                                                                        q9 = false;
                                                                                                                                                    }
                                                                                                                                                    if (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9, col1 + col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9] == letter9 && (row9 != 0 || col9 != 0) && (row8 + row9 != 0 || col8 + col9 != 0) && (row7 + row8 + row9 != 0 || col7 + col8 + col9 != 0) && (row6 + row7 + row8 + row9 != 0 || col6 + col7 + col8 + col9 != 0) && (row5 + row6 + row7 + row8 + row9 != 0 || col5 + col6 + col7 + col8 + col9 != 0) && (row4 + row5 + row6 + row7 + row8 + row9 != 0 || col4 + col5 + col6 + col7 + col8 + col9 != 0) && (row3 + row4 + row5 + row6 + row7 + row8 + row9 != 0 || col3 + col4 + col5 + col6 + col7 + col8 + col9 != 0) && (row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9 != 0 || col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9 != 0) || (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9, col1 + col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9] == "Qu" && letter9 == "Q" && word.Length > var + 9 && word.Substring(var + 9, 1) == "U"))
                                                                                                                                                    {
                                                                                                                                                        if (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9, col1 + col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9] == "Qu" && letter9 == "Q" && word.Substring(var + 9, 1) == "U")
                                                                                                                                                        {
                                                                                                                                                            var++;
                                                                                                                                                            q9 = true;
                                                                                                                                                        }
                                                                                                                                                        if (word.Length > var + 9)
                                                                                                                                                        {
                                                                                                                                                            string letter10 = word.Substring(var + 9, 1);
                                                                                                                                                            for (int row10 = -1; row10 < 2; row10++)
                                                                                                                                                            {
                                                                                                                                                                for (int col10 = -1; col10 < 2; col10++)
                                                                                                                                                                {
                                                                                                                                                                    if (q10)
                                                                                                                                                                    {
                                                                                                                                                                        var--;
                                                                                                                                                                        q10 = false;
                                                                                                                                                                    }
                                                                                                                                                                    if (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10, col1 + col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10] == letter10 && (row10 != 0 || col10 != 0) && (row9 + row10 != 0 || col9 + col10 != 0) && (row8 + row9 + row10 != 0 || col8 + col9 + col10 != 0) && (row7 + row8 + row9 + row10 != 0 || col7 + col8 + col9 + col10 != 0) && (row6 + row7 + row8 + row9 + row10 != 0 || col6 + col7 + col8 + col9 + col10 != 0) && (row5 + row6 + row7 + row8 + row9 + row10 != 0 || col5 + col6 + col7 + col8 + col9 + col10 != 0) && (row4 + row5 + row6 + row7 + row8 + row9 + row10 != 0 || col4 + col5 + col6 + col7 + col8 + col9 + col10 != 0) && (row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 != 0 || col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 != 0) && (row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 != 0 || col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 != 0) || (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10, col1 + col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10] == "Qu" && letter10 == "Q" && word.Length > var + 10 && word.Substring(var + 10, 1) == "U"))
                                                                                                                                                                    {
                                                                                                                                                                        if (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10, col1 + col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10] == "Qu" && letter10 == "Q" && word.Substring(var + 10, 1) == "U")
                                                                                                                                                                        {
                                                                                                                                                                            var++;
                                                                                                                                                                            q10 = true;
                                                                                                                                                                        }
                                                                                                                                                                        if (word.Length > var + 10)
                                                                                                                                                                        {
                                                                                                                                                                            string letter11 = word.Substring(var + 10, 1);
                                                                                                                                                                            for (int row11 = -1; row11 < 2; row11++)
                                                                                                                                                                            {
                                                                                                                                                                                for (int col11 = -1; col11 < 2; col11++)
                                                                                                                                                                                {
                                                                                                                                                                                    if (q11)
                                                                                                                                                                                    {
                                                                                                                                                                                        var--;
                                                                                                                                                                                        q11 = false;
                                                                                                                                                                                    }
                                                                                                                                                                                    if (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11, col1 + col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11] == letter11 && (row11 != 0 || col11 != 0) && (row10 + row11 != 0 || col10 + col11 != 0) && (row9 + row10 + row11 != 0 || col9 + col10 + col11 != 0) && (row8 + row9 + row10 + row11 != 0 || col8 + col9 + col10 + col11 != 0) && (row7 + row8 + row9 + row10 + row11 != 0 || col7 + col8 + col9 + col10 + col11 != 0) && (row6 + row7 + row8 + row9 + row10 + row11 != 0 || col6 + col7 + col8 + col9 + col10 + col11 != 0) && (row5 + row6 + row7 + row8 + row9 + row10 + row11 != 0 || col5 + col6 + col7 + col8 + col9 + col10 + col11 != 0) && (row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 != 0 || col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 != 0) && (row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 != 0 || col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 != 0) && (row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 != 0 || col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 != 0) || (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11, col1 + col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11] == "Qu" && letter11 == "Q" && word.Length > var + 11 && word.Substring(var + 11, 1) == "U"))
                                                                                                                                                                                    {
                                                                                                                                                                                        if (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11, col1 + col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11] == "Qu" && letter11 == "Q" && word.Substring(var + 11, 1) == "U")
                                                                                                                                                                                        {
                                                                                                                                                                                            var++;
                                                                                                                                                                                            q11 = true;
                                                                                                                                                                                        }
                                                                                                                                                                                        if (word.Length > var + 11)
                                                                                                                                                                                        {
                                                                                                                                                                                            string letter12 = word.Substring(var + 11, 1);
                                                                                                                                                                                            for (int row12 = -1; row12 < 2; row12++)
                                                                                                                                                                                            {
                                                                                                                                                                                                for (int col12 = -1; col12 < 2; col12++)
                                                                                                                                                                                                {
                                                                                                                                                                                                    if (q12)
                                                                                                                                                                                                    {
                                                                                                                                                                                                        var--;
                                                                                                                                                                                                        q12 = false;
                                                                                                                                                                                                    }
                                                                                                                                                                                                    if (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12, col1 + col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12] == letter12 && (row12 != 0 || col12 != 0) && (row11 + row12 != 0 || col11 + col12 != 0) && (row10 + row11 + row12 != 0 || col10 + col11 + col12 != 0) && (row9 + row10 + row11 + row12 != 0 || col9 + col10 + col11 + col12 != 0) && (row8 + row9 + row10 + row11 + row12 != 0 || col8 + col9 + col10 + col11 + col12 != 0) && (row7 + row8 + row9 + row10 + row11 + row12 != 0 || col7 + col8 + col9 + col10 + col11 + col12 != 0) && (row6 + row7 + row8 + row9 + row10 + row11 + row12 != 0 || col6 + col7 + col8 + col9 + col10 + col11 + col12 != 0) && (row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12 != 0 || col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12 != 0) && (row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12 != 0 || col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12 != 0) && (row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12 != 0 || col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12 != 0) && (row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12 != 0 || col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12 != 0) || (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12, col1 + col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12] == "Qu" && letter12 == "Q" && word.Length > var + 12 && word.Substring(var + 12, 1) == "U"))
                                                                                                                                                                                                    {
                                                                                                                                                                                                        if (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12, col1 + col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12] == "Qu" && letter12 == "Q" && word.Substring(var + 12, 1) == "U")
                                                                                                                                                                                                        {
                                                                                                                                                                                                            var++;
                                                                                                                                                                                                            q12 = true;
                                                                                                                                                                                                        }
                                                                                                                                                                                                        if (word.Length > var + 12)
                                                                                                                                                                                                        {
                                                                                                                                                                                                            string letter13 = word.Substring(var + 12, 1);
                                                                                                                                                                                                            for (int row13 = -1; row13 < 2; row13++)
                                                                                                                                                                                                            {
                                                                                                                                                                                                                for (int col13 = -1; col13 < 2; col13++)
                                                                                                                                                                                                                {
                                                                                                                                                                                                                    if (q13)
                                                                                                                                                                                                                    {
                                                                                                                                                                                                                        var--;
                                                                                                                                                                                                                        q13 = false;
                                                                                                                                                                                                                    }
                                                                                                                                                                                                                    if (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12 + row13, col1 + col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12 + col13] == letter13 && (row13 != 0 || col13 != 0) && (row12 + row13 != 0 || col12 + col13 != 0) && (row11 + row12 + row13 != 0 || col11 + col12 + col13 != 0) && (row10 + row11 + row12 + row13 != 0 || col10 + col11 + col12 + col13 != 0) && (row9 + row10 + row11 + row12 + row13 != 0 || col9 + col10 + col11 + col12 + col13 != 0) && (row8 + row9 + row10 + row11 + row12 + row13 != 0 || col8 + col9 + col10 + col11 + col12 + col13 != 0) && (row7 + row8 + row9 + row10 + row11 + row12 + row13 != 0 || col7 + col8 + col9 + col10 + col11 + col12 + col13 != 0) && (row6 + row7 + row8 + row9 + row10 + row11 + row12 + row13 != 0 || col6 + col7 + col8 + col9 + col10 + col11 + col12 + col13 != 0) && (row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12 + row13 != 0 || col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12 + col13 != 0) && (row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12 + row13 != 0 || col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12 + col13 != 0) && (row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12 + row13 != 0 || col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12 + col13 != 0) && (row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12 + row13 != 0 || col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12 + col13 != 0) || (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12 + row13, col1 + col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12 + col13] == "Qu" && letter13 == "Q" && word.Length > var + 13 && word.Substring(var + 13, 1) == "U"))
                                                                                                                                                                                                                    {
                                                                                                                                                                                                                        if (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12 + row13, col1 + col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12 + col13] == "Qu" && letter13 == "Q" && word.Substring(var + 13, 1) == "U")
                                                                                                                                                                                                                        {
                                                                                                                                                                                                                            var++;
                                                                                                                                                                                                                            q13 = true;
                                                                                                                                                                                                                        }
                                                                                                                                                                                                                        if (word.Length > var + 13)
                                                                                                                                                                                                                        {
                                                                                                                                                                                                                            string letter14 = word.Substring(var + 13, 1);
                                                                                                                                                                                                                            for (int row14 = -1; row14 < 2; row14++)
                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                for (int col14 = -1; col14 < 2; col14++)
                                                                                                                                                                                                                                {
                                                                                                                                                                                                                                    if (q14)
                                                                                                                                                                                                                                    {
                                                                                                                                                                                                                                        var--;
                                                                                                                                                                                                                                        q14 = false;
                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                    if (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12 + row13 + row14, col1 + col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12 + col13 + col14] == letter14 && (row14 != 0 || col14 != 0) && (row13 + row14 != 0 || col13 + col14 != 0) && (row12 + row13 + row14 != 0 || col12 + col13 + col14 != 0) && (row11 + row12 + row13 + row14 != 0 || col11 + col12 + col13 + col14 != 0) && (row10 + row11 + row12 + row13 + row14 != 0 || col10 + col11 + col12 + col13 + col14 != 0) && (row9 + row10 + row11 + row12 + row13 + row14 != 0 || col9 + col10 + col11 + col12 + col13 + col14 != 0) && (row8 + row9 + row10 + row11 + row12 + row13 + row14 != 0 || col8 + col9 + col10 + col11 + col12 + col13 + col14 != 0) && (row7 + row8 + row9 + row10 + row11 + row12 + row13 + row14 != 0 || col7 + col8 + col9 + col10 + col11 + col12 + col13 + col14 != 0) && (row6 + row7 + row8 + row9 + row10 + row11 + row12 + row13 + row14 != 0 || col6 + col7 + col8 + col9 + col10 + col11 + col12 + col13 + col14 != 0) && (row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12 + row13 + row14 != 0 || col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12 + col13 + col14 != 0) && (row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12 + row13 + row14 != 0 || col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12 + col13 + col14 != 0) && (row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12 + row13 + row14 != 0 || col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12 + col13 + col14 != 0) && (row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12 + row13 + row14 != 0 || col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12 + col13 + col14 != 0) || (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12 + row13 + row14, col1 + col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12 + col13 + col14] == "Qu" && word.Length > var + 14 && letter14 == "Q" && word.Substring(var + 14, 1) == "U"))
                                                                                                                                                                                                                                    {
                                                                                                                                                                                                                                        if (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12 + row13 + row14, col1 + col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12 + col13 + col14] == "Qu" && letter14 == "Q" && word.Substring(var + 14, 1) == "U")
                                                                                                                                                                                                                                        {
                                                                                                                                                                                                                                            var++;
                                                                                                                                                                                                                                            q14 = true;
                                                                                                                                                                                                                                        }
                                                                                                                                                                                                                                        if (word.Length > var + 14)
                                                                                                                                                                                                                                        {
                                                                                                                                                                                                                                            string letter15 = word.Substring(var + 14, 1);
                                                                                                                                                                                                                                            for (int row15 = -1; row15 < 2; row15++)
                                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                                for (int col15 = -1; col15 < 2; col15++)
                                                                                                                                                                                                                                                {
                                                                                                                                                                                                                                                    if (grid[row1 + row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12 + row13 + row14 + row15, col1 + col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12 + col13 + col14 + col15] == letter15 && (row15 != 0 || col15 != 0) && (row14 + row15 != 0 || col14 + col15 != 0) && (row13 + row14 + row15 != 0 || col13 + col14 + col15 != 0) && (row12 + row13 + row14 + row15 != 0 || col12 + col13 + col14 + col15 != 0) && (row11 + row12 + row13 + row14 + row15 != 0 || col11 + col12 + col13 + col14 + col15 != 0) && (row10 + row11 + row12 + row13 + row14 + row15 != 0 || col10 + col11 + col12 + col13 + col14 + col15 != 0) && (row9 + row10 + row11 + row12 + row13 + row14 + row15 != 0 || col9 + col10 + col11 + col12 + col13 + col14 + col15 != 0) && (row8 + row9 + row10 + row11 + row12 + row13 + row14 + row15 != 0 || col8 + col9 + col10 + col11 + col12 + col13 + col14 + col15 != 0) && (row7 + row8 + row9 + row10 + row11 + row12 + row13 + row14 + row15 != 0 || col7 + col8 + col9 + col10 + col11 + col12 + col13 + col14 + col15 != 0) && (row6 + row7 + row8 + row9 + row10 + row11 + row12 + row13 + row14 + row15 != 0 || col6 + col7 + col8 + col9 + col10 + col11 + col12 + col13 + col14 + col15 != 0) && (row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12 + row13 + row14 + row15 != 0 || col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12 + col13 + col14 + col15 != 0) && (row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12 + row13 + row14 + row15 != 0 || col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12 + col13 + col14 + col15 != 0) && (row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12 + row13 + row14 + row15 != 0 || col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12 + col13 + col14 + col15 != 0) && (row2 + row3 + row4 + row5 + row6 + row7 + row8 + row9 + row10 + row11 + row12 + row13 + row14 + row15 != 0 || col2 + col3 + col4 + col5 + col6 + col7 + col8 + col9 + col10 + col11 + col12 + col13 + col14 + col15 != 0))
                                                                                                                                                                                                                                                    {
                                                                                                                                                                                                                                                        listBox1.Items.Add(word);
                                                                                                                                                                                                                                                        listBox1.Update();
                                                                                                                                                                                                                                                        goto found;
                                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                            }
                                                                                                                                                                                                                                        }
                                                                                                                                                                                                                                        else
                                                                                                                                                                                                                                        {
                                                                                                                                                                                                                                            listBox1.Items.Add(word);
                                                                                                                                                                                                                                            listBox1.Update();
                                                                                                                                                                                                                                            goto found;
                                                                                                                                                                                                                                        }
                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                }
                                                                                                                                                                                                                            }
                                                                                                                                                                                                                        }
                                                                                                                                                                                                                        else
                                                                                                                                                                                                                        {
                                                                                                                                                                                                                            listBox1.Items.Add(word);
                                                                                                                                                                                                                            listBox1.Update();
                                                                                                                                                                                                                            goto found;
                                                                                                                                                                                                                        }
                                                                                                                                                                                                                    }
                                                                                                                                                                                                                }
                                                                                                                                                                                                            }
                                                                                                                                                                                                        }
                                                                                                                                                                                                        else
                                                                                                                                                                                                        {
                                                                                                                                                                                                            listBox1.Items.Add(word);
                                                                                                                                                                                                            listBox1.Update();
                                                                                                                                                                                                            goto found;
                                                                                                                                                                                                        }
                                                                                                                                                                                                    }
                                                                                                                                                                                                }
                                                                                                                                                                                            }
                                                                                                                                                                                        }
                                                                                                                                                                                        else
                                                                                                                                                                                        {
                                                                                                                                                                                            listBox1.Items.Add(word);
                                                                                                                                                                                            listBox1.Update();
                                                                                                                                                                                            goto found;
                                                                                                                                                                                        }
                                                                                                                                                                                    }
                                                                                                                                                                                }
                                                                                                                                                                            }
                                                                                                                                                                        }
                                                                                                                                                                        else
                                                                                                                                                                        {
                                                                                                                                                                            listBox1.Items.Add(word);
                                                                                                                                                                            listBox1.Update();
                                                                                                                                                                            goto found;
                                                                                                                                                                        }
                                                                                                                                                                    }
                                                                                                                                                                }
                                                                                                                                                            }
                                                                                                                                                        }
                                                                                                                                                        else
                                                                                                                                                        {
                                                                                                                                                            listBox1.Items.Add(word);
                                                                                                                                                            listBox1.Update();
                                                                                                                                                            goto found;
                                                                                                                                                        }
                                                                                                                                                    }
                                                                                                                                                }
                                                                                                                                            }
                                                                                                                                        }
                                                                                                                                        else
                                                                                                                                        {
                                                                                                                                            listBox1.Items.Add(word);
                                                                                                                                            listBox1.Update();
                                                                                                                                            goto found;
                                                                                                                                        }
                                                                                                                                    }
                                                                                                                                }
                                                                                                                            }
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            listBox1.Items.Add(word);
                                                                                                                            listBox1.Update();
                                                                                                                            goto found;
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            listBox1.Items.Add(word);
                                                                                                            listBox1.Update();
                                                                                                            goto found;
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            listBox1.Items.Add(word);
                                                                                            listBox1.Update();
                                                                                            goto found;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            listBox1.Items.Add(word);
                                                                            listBox1.Update();
                                                                            goto found;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            listBox1.Items.Add(word);
                                                            listBox1.Update();
                                                            goto found;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            listBox1.Items.Add(word);
                                            listBox1.Update();
                                            goto found;
                                        }
                                    }
                                }
                            }
                        }
                    }
            found: ;
            }
            if (stopToolStripMenuItem.Text == "Stop")
            {
                _done = false;
                timer1.Enabled = true;
            }
        }
        private void CheckListsWT()
        {
            string[] grid = new string[7];
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 5; j++)
                {
                    if (i * 5 + j > 6)
                    {
                        break;
                    }
                    else
                    {
                        grid[i * 5 + j] = button[i, j].Text;
                    }
                }
            int var;
            bool q1;
            bool q2;
            bool q3;
            bool q4;
            bool q5;
            bool q6;
            bool q7;
            string word;
            foreach (string i in Properties.Resources.wordList.Split('\n'))
            {
                word = i.Split('\r')[0];
                var = 0;
                q1 = false;
                q2 = false;
                q3 = false;
                q4 = false;
                q5 = false;
                q6 = false;
                q7 = false;
                string letter1 = word.Substring(0, 1);
                for (int let1 = 0; let1 < 7; let1++)
                {
                    if (q1)
                    {
                        var--;
                        q1 = false;
                    }
                    if (grid[let1] == letter1 || (grid[let1] == "Qu" && letter1 == "Q" && word.Substring(1, 1) == "U"))
                    {
                        if (grid[let1] == "Qu" && letter1 == "Q" && word.Substring(1, 1) == "U")
                        {
                            var++;
                            q1 = true;
                        }
                        string letter2 = word.Substring(var + 1, 1);
                        for (int let2 = 0; let2 < 7; let2++)
                        {
                            if (q2)
                            {
                                var--;
                                q2 = false;
                            }
                            if (grid[let2] == letter2 && let2 != let1 || (grid[let2] == "Qu" && letter2 == "Q" && word.Length > var + 2 && word.Substring(var + 2, 1) == "U"))
                            {
                                if (grid[let2] == "Qu" && letter2 == "Q" && word.Substring(var + 2, 1) == "U")
                                {
                                    var++;
                                    q2 = true;
                                }
                                if (word.Length > var + 2)
                                {
                                    string letter3 = word.Substring(var + 2, 1);
                                    for (int let3 = 0; let3 < 7; let3++)
                                    {
                                        if (q3)
                                        {
                                            var--;
                                            q3 = false;
                                        }
                                        if (grid[let3] == letter3 && let3 != let1 && let3 != let2 || (grid[let3] == "Qu" && letter3 == "Q" && word.Length > var + 3 && word.Substring(var + 3, 1) == "U"))
                                        {
                                            if (grid[let3] == "Qu" && letter3 == "Q" && word.Substring(var + 3, 1) == "U")
                                            {
                                                var++;
                                                q3 = true;
                                            }
                                            if (word.Length > var + 3)
                                            {
                                                string letter4 = word.Substring(var + 3, 1);
                                                for (int let4 = 0; let4 < 7; let4++)
                                                {
                                                    if (q4)
                                                    {
                                                        var--;
                                                        q4 = false;
                                                    }
                                                    if (grid[let4] == letter4 && let4 != let1 && let4 != let2 && let4 != let3 || (grid[let4] == "Qu" && letter4 == "Q" && word.Length > var + 4 && word.Substring(var + 4, 1) == "U"))
                                                    {
                                                        if (grid[let4] == "Qu" && letter4 == "Q" && word.Substring(var + 4, 1) == "U")
                                                        {
                                                            var++;
                                                            q4 = true;
                                                        }
                                                        if (word.Length > var + 4)
                                                        {
                                                            string letter5 = word.Substring(var + 4, 1);
                                                            for (int let5 = 0; let5 < 7; let5++)
                                                            {
                                                                if (q5)
                                                                {
                                                                    var--;
                                                                    q5 = false;
                                                                }
                                                                if (grid[let5] == letter5 && let5 != let1 && let5 != let2 && let5 != let3 && let5 != let4 || (grid[let5] == "Qu" && letter5 == "Q" && word.Length > var + 5 && word.Substring(var + 5, 1) == "U"))
                                                                {
                                                                    if (grid[let5] == "Qu" && letter5 == "Q" && word.Substring(var + 5, 1) == "U")
                                                                    {
                                                                        var++;
                                                                        q5 = true;
                                                                    }
                                                                    if (word.Length > var + 5)
                                                                    {
                                                                        string letter6 = word.Substring(var + 5, 1);
                                                                        for (int let6 = 0; let6 < 7; let6++)
                                                                        {
                                                                            if (q6)
                                                                            {
                                                                                var--;
                                                                                q6 = false;
                                                                            }
                                                                            if (grid[let6] == letter6 && let6 != let1 && let6 != let2 && let6 != let3 && let6 != let4 && let6 != let5 || (grid[let6] == "Qu" && letter6 == "Q" && word.Length > var + 6 && word.Substring(var + 6, 1) == "U"))
                                                                            {
                                                                                if (grid[let6] == "Qu" && letter6 == "Q" && word.Substring(var + 6, 1) == "U")
                                                                                {
                                                                                    var++;
                                                                                    q6 = true;
                                                                                }
                                                                                if (word.Length > var + 6)
                                                                                {
                                                                                    string letter7 = word.Substring(var + 6, 1);
                                                                                    for (int let7 = 0; let7 < 7; let7++)
                                                                                    {
                                                                                        if (q7)
                                                                                        {
                                                                                            var--;
                                                                                            q7 = false;
                                                                                        }
                                                                                        if (grid[let7] == letter7 && let7 != let1 && let7 != let2 && let7 != let3 && let7 != let4 && let7 != let5 && let7 != let6 || (grid[let7] == "Qu" && letter7 == "Q" && word.Length > var + 7 && word.Substring(var + 7, 1) == "U"))
                                                                                        {
                                                                                            if (grid[let7] == "Qu" && letter7 == "Q" && word.Substring(var + 7, 1) == "U")
                                                                                            {
                                                                                                var++;
                                                                                                q7 = true;
                                                                                            }
                                                                                            if (word.Length > var + 7)
                                                                                            {
                                                                                                string letter8 = word.Substring(var + 7, 1);
                                                                                                for (int let8 = 0; let8 < 7; let8++)
                                                                                                {
                                                                                                    if (grid[let8] == letter8 && let8 != let1 && let8 != let2 && let8 != let3 && let8 != let4 && let8 != let5 && let8 != let6 && let8 != let7)
                                                                                                    {
                                                                                                        listBox1.Items.Add(word);
                                                                                                        listBox1.Update();
                                                                                                        goto found;
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                listBox1.Items.Add(word);
                                                                                                listBox1.Update();
                                                                                                goto found;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    listBox1.Items.Add(word);
                                                                                    listBox1.Update();
                                                                                    goto found;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        listBox1.Items.Add(word);
                                                                        listBox1.Update();
                                                                        goto found;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            listBox1.Items.Add(word);
                                                            listBox1.Update();
                                                            goto found;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                listBox1.Items.Add(word);
                                                listBox1.Update();
                                                goto found;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    listBox1.Items.Add(word);
                                    listBox1.Update();
                                    goto found;
                                }
                            }
                        }
                    }
                }
            found: ;
            }
            if (stopToolStripMenuItem.Text == "Stop")
            {
                _done = false;
                timer1.Enabled = true;
            }
        }
        private void x4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                button[i, 4].Enabled = false;
                button[4, i].Enabled = false;
                button[i, 4].Text = "";
                button[4, i].Text = "";
            }
            _E5 = false;
        }
        private void x5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                button[i, 4].Enabled = true;
                button[4, i].Enabled = true;
            }
            _E5 = true;
        }
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    button[i, j].Text = "";
                }
            }
            stopToolStripMenuItem.Text = "Start";
            _start = false;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SendKeys.Send("{Enter}");
            /*for(int i = 0; i < 10; i++)
            {
                if (_desktop.pic.GetPixel(_x, _y).G < 50)
                {
                    listBox1.SetSelected(listBox1.SelectedIndex - 1, true);
                }
            }*/
            if (!_done)
            {
                SendKeys.SendWait(listBox1.SelectedItem.ToString());
                timer1.Enabled = true;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (listBox1.SelectedIndex + 1 < listBox1.Items.Count)
            {
                if (!_done && stopToolStripMenuItem.Text == "Stop")
                {
                    if (_start)
                    {
                        SendKeys.SendWait(listBox1.SelectedItem.ToString());
                        _start = false;
                    }
                    listBox1.SetSelected(listBox1.SelectedIndex + 1, true);

                }
            }
            else
            {
                if (listBox1.Items.Count > 0)
                {
                    _done = true;
                    listBox1.SetSelected(0, true);
                    stopToolStripMenuItem.Text = "Start";
                }
            }
        }
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stopToolStripMenuItem.Text == "Start")
            {
                stopToolStripMenuItem.Text = "Stop";
                if (listBox1.Items.Count > 0)
                {
                    System.Threading.Thread.Sleep(2000);
                    _start = true;
                    _done = false;
                    timer1.Enabled = true;
                }
                else if(autoToolStripMenuItem.Text == "Automatic")
                {
                    getGrid();
                }
            }
            else
            {
                stopToolStripMenuItem.Text = "Start";
                timer1.Enabled = false;
                _done = true;
            }
        }
        private void autoToolStripMenuItem_MouseDown(object sender, EventArgs e)
        {
            if (autoToolStripMenuItem.Text == "Automatic")
                autoToolStripMenuItem.Text = "Manual";
            //else
            //    autoToolStripMenuItem.Text = "Automatic";
        }
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help help = new Help();
            help.ShowDialog();
        }
        private void listBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            button[0, 0].Focus();
        }
        private void speedToolStripMenuItem_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _speed--;
            }
            else if (e.Button == MouseButtons.Right)
            {
                _speed++;
            }
            speedToolStripMenuItem.Text = "Speed  = " + _speed.ToString();
            timer1.Interval = _speed;
        }
        private void getGrid()
        {
            _desktop = new CaptureScreen();
            int xx = 0;
            int yy = 0;
            for (xx = 0; xx < _desktop.pic.Width; xx++)
            {
                for (yy = 166; yy < _desktop.pic.Height; yy++)
                {
                    if (_desktop.pic.GetPixel(xx, yy).Name == ((_E5) ? "ffe5d5c6" : "ffe7d5d1"))
                    {
                        xx--;
                        goto a;
                    }
                    if (_desktop.pic.GetPixel(xx, yy).Name == ((_E5) ? "ffe7d5d1" : "ffe5d5c6"))
                    {
                        xx--;
                        object sender = null;
                        EventArgs e = null;
                        if (_E5)
                        {
                            x4ToolStripMenuItem_Click(sender, e);
                        }
                        else
                        {
                            x5ToolStripMenuItem_Click(sender, e);
                        }
                        goto a;
                    }
                }
            }
            MessageBox.Show("Can't Find Board");
            stopToolStripMenuItem.Text = "Start";
            return;
        a:
            for (int i = 0; i < ((_E5) ? 5 : 4); i++)
            {
                for (int j = 0; j < ((_E5) ? 5 : 4); j++)
                {
                    OCR cr = new OCR(_desktop.pic);
                    button[i, j].Font = _norm;
                    button[i, j].Text = cr.String;
                    int x = j * ((_E5) ? 50 : 62) + xx;
                    int y = i * ((_E5) ? 50 : 62) + yy;
                    if (!_E5)
                    {
                        if (j == 2)
                            x++;
                        else if (j == 3)
                            x += 2;
                        if (i > 1)
                            y++;
                    }
                    if (x + 50 > _desktop.pic.Width || y + 50 > _desktop.pic.Height)
                        return;
                    int num = 0;
                    for (int a = 0; a < ((_E5) ? 50 : 50); a++)
                    {
                        for (int b = 0; b < ((_E5) ? 50 : 50); b++)
                        {
                            if (_desktop.pic.GetPixel(x + a, y + b).R < 100)
                            {
                                num++;
                            }
                        }
                    }
                    if (_E5)
                    {
                        //MessageBox.Show(_desktop.pic.GetPixel(x + 16, y + 31).R.ToString() + " " + _desktop.pic.GetPixel(x + 17, y + 28).R.ToString() + " " + _desktop.pic.GetPixel(x + 18, y + 25).R.ToString() + " " + _desktop.pic.GetPixel(x + 19, y + 22).R.ToString() + " " + _desktop.pic.GetPixel(x + 20, y + 19).R.ToString() + " " + _desktop.pic.GetPixel(x + 21, y + 16).R.ToString() + " " + _desktop.pic.GetPixel(x + 23, y + 14).R.ToString() + " " + _desktop.pic.GetPixel(x + 25, y + 16).R.ToString() + " " + _desktop.pic.GetPixel(x + 26, y + 19).R.ToString() + " " + _desktop.pic.GetPixel(x + 27, y + 22).R.ToString() + " " + _desktop.pic.GetPixel(x + 28, y + 25).R.ToString() + " " + _desktop.pic.GetPixel(x + 29, y + 28).R.ToString() + " " + _desktop.pic.GetPixel(x + 30, y + 31).R.ToString() + " " + _desktop.pic.GetPixel(x + 25, y + 25).R.ToString() + " " + _desktop.pic.GetPixel(x + 23, y + 25).R.ToString() + " " + _desktop.pic.GetPixel(x + 21, y + 25).R.ToString() + " ");
                        /*if (num < 20)
                        {
                            button[i, j].Text = "I";
                        }
                        else if (num < 30)
                        {
                            button[i, j].Text = "L";
                        }
                        else if (num < 33)
                        {
                            if (_desktop.pic.GetPixel(x + 19, y + 27).R < 96)
                            {
                                button[i, j].Text = "J";
                            }
                            else if (_desktop.pic.GetPixel(x + 24, y + 13).R < 96)
                            {
                                button[i, j].Text = "T";
                            }
                        }
                        else if (num < 38)
                        {
                            if (_desktop.pic.GetPixel(x + 21, y + 24).R < 96)
                            {
                                button[i, j].Text = "V";
                            }
                            else if (_desktop.pic.GetPixel(x + 24, y + 22).R < 96)
                            {
                                button[i, j].Text = "Y";
                            }
                        }
                        else if (num < 45)
                        {
                            button[i, j].Text = "F";
                        }
                        else if (num < 55)
                        {
                            if (_desktop.pic.GetPixel(x + 22, y + 15).R < 96)
                            {
                                button[i, j].Text = "A";
                            }
                            else if (_desktop.pic.GetPixel(x + 28, y + 21).R < 96 && _desktop.pic.GetPixel(x + 30, y + 30).R < 96)
                            {
                                button[i, j].Text = "H";
                            }
                            else if (_desktop.pic.GetPixel(x + 28, y + 26).R < 96 && _desktop.pic.GetPixel(x + 21, y + 16).R < 96)
                            {
                                button[i, j].Text = "X";
                            }
                            else if (_desktop.pic.GetPixel(x + 20, y + 14).R < 96 && _desktop.pic.GetPixel(x + 29, y + 29).R < 96)
                            {
                                button[i, j].Text = "C";
                            }
                            else if (_desktop.pic.GetPixel(x + 23, y + 20).R < 96)
                            {
                                button[i, j].Text = "K";
                            }
                            else if (_desktop.pic.GetPixel(x + 20, y + 23).R < 96)
                            {
                                button[i, j].Text = "P";
                            }
                            else if (_desktop.pic.GetPixel(x + 18, y + 19).R < 96)
                            {
                                button[i, j].Text = "U";
                            }
                            else
                            {
                                button[i, j].Text = "Z";
                            }
                        }
                        else if (num < 63)
                        {
                            if (_desktop.pic.GetPixel(x + 30, y + 18).R < 96)
                            {
                                button[i, j].Text = "D";
                            }
                            else if (_desktop.pic.GetPixel(x + 31, y + 16).R < 96)
                            {
                                button[i, j].Text = "O";
                            }
                            else if (_desktop.pic.GetPixel(x + 35, y + 13).R < 96)
                            {
                                button[i, j].Text = "W";
                            }
                            else
                            {
                                button[i, j].Text = "E";
                            }
                        }
                        else if (num < 67)
                        {
                            button[i, j].Text = "S";
                        }
                        else if (num < 80)
                        {
                            if (_desktop.pic.GetPixel(x + 18, y + 22).R < 96 && _desktop.pic.GetPixel(x + 22, y + 31).R < 96)
                            {
                                button[i, j].Text = "B";
                            }
                            else if (_desktop.pic.GetPixel(x + 23, y + 13).R < 96 && _desktop.pic.GetPixel(x + 24, y + 31).R < 96)
                            {
                                button[i, j].Text = "G";
                            }
                            else if (_desktop.pic.GetPixel(x + 24, y + 30).R < 96)
                            {
                                button[i, j].Text = "M";
                            }
                            else if (_desktop.pic.GetPixel(x + 21, y + 18).R < 96)
                            {
                                button[i, j].Text = "N";
                            }
                            else
                            {
                                button[i, j].Text = "R";
                            }
                        }
                        else
                        {
                            button[i, j].Font = _qu;
                            button[i, j].Text = "Qu";
                        }*/
                        if (_showNum)
                        {
                            button[i, j].Font = new Font("Microsoft Sans Serif", 10);
                            button[i, j].Text += num.ToString();
                        }
                    }
                    else
                    {
                        /*if (num < 50)
                        {
                            button[i, j].Text = "I";
                        }
                        else if (num < 61)
                        {

                            button[i, j].Text = "L";
                        }
                        else if (num < 67)
                        {
                            button[i, j].Text = "T";
                        }
                        else if (num < 75)
                        {
                            button[i, j].Text = "J";
                        }
                        else if (num < 82)
                        {
                                button[i, j].Text = "Y";
                        }
                        else if (num < 95)
                        {
                            if (_desktop.pic.GetPixel(x + 32, y + 22).R < 96)
                            {
                                button[i, j].Text = "Z";
                            }
                            else if (_desktop.pic.GetPixel(x + 35, y + 39).R < 96)
                            {
                                button[i, j].Text = "V";
                            }
                            else
                            {
                                button[i, j].Text = "F";
                            }
                        }
                        else if (num < 95)
                        {
                            if (_desktop.pic.GetPixel(x + 29, y + 40).R < 96)
                            {
                                button[i, j].Text = "U";
                            }
                            else if (_desktop.pic.GetPixel(x + 37, y + 26).R < 96)
                            {
                                button[i, j].Text = "P";
                            }
                            else if (_desktop.pic.GetPixel(x + 24, y + 30).R < 96)
                            {
                                button[i, j].Text = "K";
                            }
                            else
                            {
                                button[i, j].Text = "X";
                            }
                        }
                        else if (num < 105)
                        {
                            if (_desktop.pic.GetPixel(x + 28, y + 32).R < 96)
                            {
                                button[i, j].Text = "A";
                            }
                            else if (_desktop.pic.GetPixel(x + 32, y + 32).R < 96 || _desktop.pic.GetPixel(x + 34, y + 30).R < 96)
                            {
                                button[i, j].Text = "R";
                            }
                            else if (_desktop.pic.GetPixel(x + 27, y + 27).R < 96)
                            {
                                button[i, j].Text = "H";
                            }
                            else if (_desktop.pic.GetPixel(x + 38, y + 28).R < 96)
                            {
                                button[i, j].Text = "D";
                            }
                            else
                            {
                                button[i, j].Text = "C";
                            }
                        }
                        else if (num < 130)
                        {
                            if (_desktop.pic.GetPixel(x + 40, y + 39).R < 96)
                            {
                                button[i, j].Text = "M";
                            }
                            else if (_desktop.pic.GetPixel(x + 15, y + 17).R < 96)
                            {
                                button[i, j].Text = "W";
                            }
                            else if (_desktop.pic.GetPixel(x + 35, y + 26).R < 96)
                            {
                                button[i, j].Text = "B";
                            }
                            else if (_desktop.pic.GetPixel(x + 20, y + 39).R < 96)
                            {
                                button[i, j].Text = "N";
                            }
                            else if (_desktop.pic.GetPixel(x + 28, y + 27).R < 96)
                            {
                                button[i, j].Text = "S";
                            }
                            else
                            {
                                button[i, j].Text = "O";
                            }
                        }
                        else if (num < 150)
                        {
                            button[i, j].Text = "G";
                        }
                        else
                        {
                            button[i, j].Font = _qu;
                            button[i, j].Text = "Qu";
                        }*/
                        if (_showNum)
                        {
                            button[i, j].Text += num.ToString();
                            button[i, j].Font = new Font("Microsoft Sans Serif", 10);
                        }
                    }
                }
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _regKey.SetValue("Speed", _speed);
        }
    }
}