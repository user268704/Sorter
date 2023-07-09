using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Sorter.Core;
using Sorter.Core.Interfaces;
using Sorter.Models.Models;
using Sorter.Models.Models.Events;

namespace Sorter.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IScanner Scanner { get; set; }

        private ObservableCollection<DirItem> DirItems { get; set; }
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Scanner = new Scanner();
            Scanner.OnFileIncrement += OnFileScan;
            Scanner.OnCoreDirectoryIncrement += OnRootDirectoryScan;
            
            DirItems = new();
        }

        private void OnRootDirectoryScan(FileIncrementArgs e)
        {
            float onePercent = Directory.GetDirectories(PathBox.Text).Length / 100;

            /*
            Dispatcher.Invoke(() =>
            {
                MainProgressBar.Value += onePercent;
            });
        */
        }

        private void OnFileScan(FileIncrementArgs e)
        {
            IncrementCounter();
        }

        /*
        private void FilesList_OnSelected(object sender, RoutedEventArgs e)
        {
            if (FilesList.SelectedItem == null || String.IsNullOrEmpty(FilesList.SelectedItem.ToString()))
                return;

            var path = FilesList.SelectedItem.ToString();

            FilesList.Items.Clear();
            PathBox.Text = path;
        }
        */
        
        

        /*
        private void PathBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            string path = ((TextBox)sender).Text;

            if (Directory.Exists(path))
                foreach (string directory in Directory.GetDirectories(path))
                    FilesList.Items.Add(directory);
            else
                FilesList.Items.Clear();
        }
        */

        private void DoneButton_OnClick(object sender, RoutedEventArgs e)
        {
            /*
            ResultsCountLabel.Content = 0;
            ResultPanel.Visibility = Visibility.Collapsed;
            */
            
            DirItems.Clear();
            string path = PathBox.Text;

            Task.Factory.StartNew(() =>
            {
                var result = Scanner.Scan(path);
                
                Dispatcher.Invoke(() =>
                {
                    result = ApplySettings(result);
                    
                    foreach (DirItem item in result)
                    {
                        DirItems.Add(item);
                    }
                });
            });
            
            // ResultPanel.Visibility = Visibility.Visible;
        }

        private void IncrementCounter(int count = 1)
        {
            /*
            Dispatcher.Invoke(() =>
            {
                if (ResultsCountLabel.Content == null)
                {
                    ResultsCountLabel.Content = 0;
                    return;
                }
                
                int currentCount = int.Parse(ResultsCountLabel.Content.ToString()) + count;
                ResultsCountLabel.Content = currentCount;
            });
        */
        }
        
        private List<DirItem> ApplySettings(List<DirItem> files)
        {
            if (this.OnlyDirectory.IsChecked == true) 
                files = OnlyDirectory(files);

            if (!String.IsNullOrEmpty(NoLessTextBox.Text)) 
                files = NoLess(files);

            return files;
            
            List<DirItem> OnlyDirectory(List<DirItem> items)
            {
                List<DirItem> result = new();
                
                var dirs = items.GroupBy(x => x.Directory);
                
                foreach (var dir in dirs)
                {
                    result.Add(new DirItem
                    {
                        Name = dir.Key,
                        Size = dir.Sum(x => x.Size / 1024),
                        FullPath = dir.Key,
                        Directory = dir.Key
                    });
                }

                return result;
            }

            List<DirItem> NoLess(List<DirItem> items)
            {
                try
                {
                    return items.Where(x => x.Size > float.Parse(NoLessTextBox.Text)).ToList();
                }
                catch (FormatException e)
                {
                    Debug.Print(e.Message);
                    throw;
                }
            }
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
        
        private void OpenPath_OnClick(object sender, RoutedEventArgs e)
        {
            var item = (DirItem)MainDataGrid.SelectedItem;

            Process.Start("explorer.exe", item.FullPath);
        }

        private void DeletePath_OnClick(object sender, RoutedEventArgs e)
        {
            
            
        }

        private void PathBox_OnTabUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string path = PathBox.Text[..(PathBox.Text.LastIndexOf('/') + 1)];
                string[] dirs = Directory.GetDirectories(path);
                
                foreach (string dir in dirs)
                    if (dir.StartsWith(PathBox.Text))
                    {
                        PathBox.Text = dir;
                        PathBox.Select(PathBox.Text.Length, 0);
                    }
            }
        }
    }
}