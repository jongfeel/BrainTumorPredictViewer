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
            if (!string.IsNullOrEmpty(Settings.Default.SourceImagePath) &&
                Directory.Exists(Settings.Default.SourceImagePath))
            {
                DirectoryInfo di = new DirectoryInfo(Settings.Default.SourceImagePath);
                DirectoryInfo[] childDI = di.GetDirectories();
                SourceImagePathComboBox.ItemsSource = childDI;
                SourceImagePathComboBox.SelectedIndex = 0;

                OriginSouceSlider.Maximum = LoadImages(childDI[0].FullName, ref LoadedSourceImages, ref SourceImageInfo, ref SourceImage);
            }

            if (!string.IsNullOrEmpty(ResultImagePath.Text) &&
                File.Exists(Settings.Default.ResultImagePath))
            {
                ResultImagePath.Text = Settings.Default.ResultImagePath;

                ResultSouceSlider.Maximum = LoadImages(ResultImagePath.Text, ref LoadedResultImages, ref ResultImageInfo, ref ResultImage);
            }
        }

        private void OpenSourceImageFolderButton_Click(object sender, RoutedEventArgs e)
        {
            if (OpenFolderDialog(out string selectedPath) == CommonFileDialogResult.Ok)
            {
                Debug.WriteLine(selectedPath);

                DirectoryInfo di = new DirectoryInfo(selectedPath);
                DirectoryInfo[] childDI = di.GetDirectories();
                SourceImagePathComboBox.ItemsSource = childDI;
                SourceImagePathComboBox.SelectedIndex = 0;

                if (childDI.Length > 0)
                {
                    Settings.Default.SourceImagePath = selectedPath;
                    Settings.Default.Save();

                    OriginSouceSlider.Maximum = LoadImages(childDI[0].FullName, ref LoadedSourceImages, ref SourceImageInfo, ref SourceImage);
                }
                else
                {
                    MessageBox.Show("이미지 폴더가 있는 상위 폴더를 선택해야 함");
                }
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
            selectPath = result == CommonFileDialogResult.Ok ? dialog.FileName : "";
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (OpenFolderDialog(out string selectedPath) == CommonFileDialogResult.Ok)
            {
                DirectoryInfo di = new DirectoryInfo(selectedPath);
                DirectoryInfo[] childDI = di.GetDirectories();
                foreach (var item in childDI)
                {
                    Debug.WriteLine(item.Name);
                }
            }
        }
    }
}
