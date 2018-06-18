using BrainTumorPredictViewer.Properties;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BrainTumorPredictViewer
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private FileInfo[] SourceImageInfo = null;
        private FileInfo[] ResultImageInfo = null;
        private BitmapImage[] LoadedSourceImages = null;
        private BitmapImage[] LoadedResultImages = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SourceImagePath.Text = BrainTumorPredictViewer.Properties.Settings.Default.SourceImagePath;

            if (!string.IsNullOrEmpty(SourceImagePath.Text))
            {
                OriginSouceSlider.Maximum = LoadImages(SourceImagePath.Text, ref LoadedSourceImages, ref SourceImageInfo, ref SourceImage);
            }

            ResultImagePath.Text = Settings.Default.ResultImagePath;

            if (!string.IsNullOrEmpty(ResultImagePath.Text))
            {
                ResultSouceSlider.Maximum = LoadImages(ResultImagePath.Text, ref LoadedResultImages, ref ResultImageInfo, ref ResultImage);
            }
        }

        private void OpenSourceImageFolderButton_Click(object sender, RoutedEventArgs e)
        {
            if (OpenFolderDialog(out string selectedPath) == CommonFileDialogResult.Ok)
            {
                Debug.WriteLine(selectedPath);
                SourceImagePath.Text = selectedPath;

                Settings.Default.SourceImagePath = selectedPath;
                Settings.Default.Save();

                OriginSouceSlider.Maximum = LoadImages(selectedPath, ref LoadedSourceImages, ref SourceImageInfo, ref SourceImage);
            }
        }

        private void OpenResultImageFolderButton_Click(object sender, RoutedEventArgs e)
        {
            //string startPath = "notepad.exe \"" + System.AppDomain.CurrentDomain.BaseDirectory + "111.txt\"";
            //Process.Start(startPath);

            if (OpenFolderDialog(out string selectedPath) == CommonFileDialogResult.Ok)
            {
                Debug.WriteLine(selectedPath);
                ResultImagePath.Text = selectedPath;

                Settings.Default.ResultImagePath = selectedPath;
                Settings.Default.Save();

                ResultSouceSlider.Maximum = LoadImages(selectedPath, ref LoadedResultImages, ref ResultImageInfo, ref ResultImage);
            }
        }

        private CommonFileDialogResult OpenFolderDialog(out string selectPath)
        {
            // Winform folder dialog 사용 하지 않음
            // Windows 7 이후 API 호출해서 사용
            // nuget package 설치함: WindowsAPICodePack
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };
            var result = dialog.ShowDialog();
            selectPath = dialog.FileName;
            return result;
        }

        private uint LoadImages(string path, ref BitmapImage[] images, ref FileInfo[] files, ref Image viewImage)
        {
            var dirInfo = new DirectoryInfo(path);

            files = dirInfo.GetFiles("*.png").OrderBy(f => int.Parse(System.IO.Path.GetFileNameWithoutExtension(f.Name))).ToArray();

            var uriSource = new Uri(files[0].FullName, UriKind.RelativeOrAbsolute);
            viewImage.Source = new BitmapImage(uriSource);

            images = new BitmapImage[files.Length];

            return (uint)files.Length;
        }

        private void SourceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (SourceImageInfo != null && SourceImageInfo.Length > e.NewValue && (int)e.OldValue != (int)e.NewValue)
            {
                if (LoadedSourceImages[(int)e.NewValue] != null)
                {
                    SourceImage.Source = LoadedSourceImages[(int)e.NewValue];
                }
                else
                {
                    var uriSource = new Uri(SourceImageInfo[(int)e.NewValue].FullName, UriKind.RelativeOrAbsolute);
                    LoadedSourceImages[(int)e.NewValue] = new BitmapImage(uriSource);
                    SourceImage.Source = LoadedSourceImages[(int)e.NewValue];

                    Debug.WriteLine(uriSource);
                }
            }
        }

        private void ResultSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ResultImageInfo != null && ResultImageInfo.Length > e.NewValue && (int)e.OldValue != (int)e.NewValue)
            {
                if (LoadedResultImages[(int)e.NewValue] != null)
                {
                    ResultImage.Source = LoadedResultImages[(int)e.NewValue];
                }
                else
                {
                    var uriSource = new Uri(ResultImageInfo[(int)e.NewValue].FullName, UriKind.RelativeOrAbsolute);
                    LoadedResultImages[(int)e.NewValue] = new BitmapImage(uriSource);
                    ResultImage.Source = LoadedResultImages[(int)e.NewValue];

                    Debug.WriteLine(uriSource);
                }
            }
        }
    }
}
