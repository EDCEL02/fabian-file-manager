using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Fabian_FileManager
{
    public partial class Form1 : Form
    {
        private Stack<string> navigationHistory; // Stack to store navigation history
        private string baseDirectory = @"D:\"; // Base directory

        public Form1()
        {
            InitializeComponent();
            navigationHistory = new Stack<string>();
            LoadFilesAndDirectoriesFromDefaultLocation();
            listView1.ItemActivate += new EventHandler(listView1_ItemActivate);
            listView1.SelectedIndexChanged += new EventHandler(listView1_SelectedIndexChanged); // Handle item selection change
            goButton.Click += new EventHandler(goButton_Click); // Handle goButton click event
            backButton.Click += new EventHandler(backButton_Click); // Handle backButton click event
        }

        private void LoadFilesAndDirectoriesFromDefaultLocation()
        {
            if (Directory.Exists(baseDirectory))
            {
                LoadDirectory(baseDirectory);
                navigationHistory.Push(baseDirectory); // Initialize navigation history with base directory
            }
            else
            {
                MessageBox.Show($"The directory {baseDirectory} does not exist.");
            }
        }

        private void LoadDirectory(string path)
        {
            listView1.Items.Clear(); // Clear current items

            DirectoryInfo dirInfo = new DirectoryInfo(path);

            // Add directories to the list view
            DirectoryInfo[] directories = dirInfo.GetDirectories();
            foreach (DirectoryInfo directory in directories)
            {
                ListViewItem item = new ListViewItem(directory.Name);
                item.ImageIndex = GetIconIndex("Folder");
                item.Tag = directory.FullName; // Store full path in Tag property
                item.SubItems.Add("Directory");
                item.SubItems.Add(directory.FullName);
                listView1.Items.Add(item);
            }

            // Add files to the list view
            FileInfo[] files = dirInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                string fileExtension = file.Extension.ToUpper();
                int imageIndex = GetIconIndex(fileExtension);
                string fileType = GetFileType(fileExtension);

                ListViewItem item = new ListViewItem(file.Name);
                item.ImageIndex = imageIndex;
                item.Tag = file.FullName; // Store full path in Tag property
                item.SubItems.Add(fileType);
                item.SubItems.Add(file.FullName);
                listView1.Items.Add(item);
            }

            // Update the filePathTextBox to display the current path
            filePathTextBox.Text = path;
        }

        private int GetIconIndex(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".MP3":
                case ".MP2":
                    return 1; // Index of your MP3 icon in the ImageList
                case ".EXE":
                case ".COM":
                    return 2; // Index of your EXE icon in the ImageList
                case ".MP4":
                case ".AVI":
                case ".MKV":
                    return 3; // Index of your video file icon in the ImageList
                case ".PDF":
                    return 4; // Index of your PDF icon in the ImageList
                case ".DOC":
                case ".DOCX":
                    return 5; // Index of your DOC icon in the ImageList
                case ".PNG":
                case ".JPG":
                case ".JPEG":
                    return 6; // Index of your image file icon in the ImageList
                case "Folder":
                    return 0; // Index of your folder icon in the ImageList
                default:
                    return 7; // Index of your default icon in the ImageList
            }
        }

        private string GetFileType(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".MP3":
                case ".MP2":
                    return "Audio File";
                case ".EXE":
                case ".COM":
                    return "Executable File";
                case ".MP4":
                case ".AVI":
                case ".MKV":
                    return "Video File";
                case ".PDF":
                    return "PDF Document";
                case ".DOC":
                case ".DOCX":
                    return "Word Document";
                case ".PNG":
                case ".JPG":
                case ".JPEG":
                    return "Image File";
                case "Folder":
                    return "Directory";
                default:
                    return "Unknown File Type";
            }
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView1.SelectedItems[0];
                string selectedPath = selectedItem.Tag.ToString();

                if (Directory.Exists(selectedPath))
                {
                    navigationHistory.Push(selectedPath); // Add current path to navigation history
                    LoadDirectory(selectedPath); // Load the directory contents
                }
                else if (File.Exists(selectedPath))
                {
                    Process.Start(selectedPath); // Open the file with its default application
                }
                else
                {
                    MessageBox.Show($"The path {selectedPath} is not valid.");
                }
            }
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView1.SelectedItems[0];
                string selectedPath = selectedItem.Tag.ToString();

                if (Directory.Exists(selectedPath))
                {
                    navigationHistory.Push(selectedPath); // Add current path to navigation history
                    LoadDirectory(selectedPath); // Load the directory contents
                }
                else if (File.Exists(selectedPath))
                {
                    Process.Start(selectedPath); // Open the file with its default application
                }
                else
                {
                    MessageBox.Show($"The selected item is not valid.");
                }
            }
            else
            {
                MessageBox.Show("No item is selected.");
            }
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            if (navigationHistory.Count > 1)
            {
                navigationHistory.Pop(); // Remove the current directory
                string previousPath = navigationHistory.Peek(); // Get the previous directory
                LoadDirectory(previousPath); // Load the previous directory
            }
            else
            {
                MessageBox.Show("No previous directory to go back to.");
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView1.SelectedItems[0];
                string fileName = selectedItem.Text;
                string fileType = selectedItem.SubItems[1].Text;

                fileNameLabel.Text = fileName;
                fileTypeLabel.Text = fileType;
            }
        }
    }
}
