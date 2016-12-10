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
using System.Collections; 

namespace DirectorySaver
{
    public class Dir
    {
        public DirectoryInfo directoryInfo { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public bool isOpen { get; set; }
        public int level { get; set; }
        public Dir(DirectoryInfo _direcotryInfo)
        {
            this.directoryInfo = _direcotryInfo;
            this.FullName = _direcotryInfo.FullName;
            this.Name = _direcotryInfo.Name;
            this.isOpen = false;
            this.level = 0;
        }
        public Dir(DirectoryInfo _direcotryInfo, int _level)
        {
            this.directoryInfo = _direcotryInfo;
            this.FullName = _direcotryInfo.FullName;
            this.Name = _direcotryInfo.Name;
            this.isOpen = false;
            this.level = _level;
        }
    }
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            lstDirectoriesAndFiles.ValueMember = "FullName";
            lstDirectoriesAndFiles.DisplayMember = "Name";
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About abt = new About();
            abt.Show(); 
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Application.MessageLoop)
            {
                System.Windows.Forms.Application.Exit(); 
            }
            else
            {
                System.Environment.Exit(1);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lstDirectoriesAndFiles.Items.Clear();

            FolderBrowserDialog fbd = new FolderBrowserDialog(); 

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                lstDirectoriesAndFiles.Items.Add(fbd.SelectedPath.ToString()); 
            }

            Dir directory = new Dir(new DirectoryInfo(fbd.SelectedPath.ToString()));
            DirectoryInfo[] dirInfo = directory.directoryInfo.GetDirectories();

            foreach (DirectoryInfo info in dirInfo)
            {
                Dir temp = new Dir(info);
                lstDirectoriesAndFiles.Items.Add(temp);
            }

            FileInfo[] fileInfo = directory.directoryInfo.GetFiles();
            foreach (FileInfo info in fileInfo)
            {
                lstDirectoriesAndFiles.Items.Add(info);
            }
        }

        private void lstDirectoriesAndFiles_DoubleClick(object sender, EventArgs e)
        {
            if (lstDirectoriesAndFiles.Items[lstDirectoriesAndFiles.SelectedIndex].GetType() == typeof(FileInfo))
                return;

            Dir dir = (Dir)lstDirectoriesAndFiles.Items[lstDirectoriesAndFiles.SelectedIndex];
            DirectoryInfo[] subDir = dir.directoryInfo.GetDirectories();
            int count = 0;
            int startPos = lstDirectoriesAndFiles.SelectedIndex; 
            for (int i = startPos + 1; i < startPos + subDir.Count() + 1; i++)
            {
                lstDirectoriesAndFiles.Items.Insert(i, new Dir(subDir[count], dir.level + 1));
                count++; 
            }
            FileInfo[] fi = dir.directoryInfo.GetFiles();
            int newStart = startPos + subDir.Count();
            int newCount = 0;  
            for (int i = newStart + 1; i < newStart + fi.Count() + 1; i++)
            {
                lstDirectoriesAndFiles.Items.Insert(i, fi[newCount]);
                newCount++; 
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lstDirectoriesAndFiles.Items.Clear(); 
        }
    }
}
