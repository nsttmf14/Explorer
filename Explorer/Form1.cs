﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Explorer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string path;

        private void Form1_Load(object sender, EventArgs e)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (var drive in drives)
            {
                MainTreeView.Nodes.Add(new TreeNode(drive.Name));

                if (Directory.Exists(drive.Name))
                {
                    ///Сделать рекурсию

                    //Подкаталоги
                    string[] directories = Directory.GetDirectories(drive.Name);
                    foreach (string directory in directories)
                    {
                        MainTreeView.Nodes[Array.IndexOf(drives, drive)].Nodes.Add(directory);

                    }

                    //Файлы
                    string[] files = Directory.GetFiles(drive.Name);
                    foreach (string file in files)
                    {
                        MainTreeView.Nodes[Array.IndexOf(drives, drive)].Nodes.Add(file);
                    }
                }
            }

            TreeNode userNode = new TreeNode() { Name = "User", Text = "Пользователь" };
            TreeNode desktopNode = new TreeNode() { Name = "Desktop", Text = "Рабочий стол" };
            TreeNode documentsNode = new TreeNode() { Name = "Documents", Text = "Мои документы" };

            MainTreeView.Nodes.Add(userNode);
            MainTreeView.Nodes.Add(desktopNode);
            MainTreeView.Nodes.Add(documentsNode);
        }

        private void MainTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            switch (MainTreeView.SelectedNode.Text)
            {
                case "Мои документы":
                    {
                        path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        PathTextBox.Text = path;
                        MainWebBrowser.Url = new Uri(path);
                        break;
                    }
                case "Рабочий стол":
                    {
                        path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        PathTextBox.Text = path;
                        MainWebBrowser.Url = new Uri(path);
                        break;
                    }
                case "Пользователь":
                    {
                        path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                        PathTextBox.Text = path;
                        MainWebBrowser.Url = new Uri(path);
                        break;
                    }
                default:
                    {
                        PathTextBox.Text = MainTreeView.SelectedNode.Text;
                        MainWebBrowser.Url = new Uri(MainTreeView.SelectedNode.Text);
                        break;
                    }
            }
        }

        private void BackToolStripButton_Click(object sender, EventArgs e)
        {
            if (MainWebBrowser.CanGoBack)
                MainWebBrowser.GoBack();
        }

        private void ForwardToolStripButton_Click(object sender, EventArgs e)
        {
            if (MainWebBrowser.CanGoForward)
                MainWebBrowser.GoForward();
        }

        private void PathTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                    MainWebBrowser.Url = new Uri(PathTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Такой путь отсутствует.", "Ошибка");
            }
        }

        private void excelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateFile(".xlsx");
        }

        private void CreateFile(string fileFormat)
        {
            if (PathTextBox.Text == "")
            {
                MessageBox.Show("Сначала откройте директорию", "Ошибка");
                return;
            }

            Form2 addform = new Form2();

            if (addform.ShowDialog() == DialogResult.OK)
            {
                Directory.CreateDirectory(addform.File);
                File.Create(PathTextBox.Text + "/" + addform.File + fileFormat).Close();

                MainWebBrowser.Url = new Uri(PathTextBox.Text);
            }
        }

        private void блокнотToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateFile(".txt");
        }

        private void wordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateFile(".docx");
        }

        private void OpenToolStripButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Выберите путь" })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    MainWebBrowser.Url = new Uri(fbd.SelectedPath);
                    PathTextBox.Text = fbd.SelectedPath;
                }
            }
        }


    }
}
