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
        private FileInfo[] originImageInfo = null;
        private BitmapImage[] loadedImages = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenWorkingFolderButton_Click(object sender, RoutedEventArgs e)
        {
            // Winform folder dialog 사용 하지 않음
            // Windows 7 이후 API 호출해서 사용
            // nuget package 설치함: WindowsAPICodePack
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                Debug.WriteLine(dialog.FileName);

                var dirInfo = new DirectoryInfo(dialog.FileName);
                
                originImageInfo = dirInfo.GetFiles("*.png").OrderBy(f => int.Parse( System.IO.Path.GetFileNameWithoutExtension (f.Name))).ToArray();

                var uriSource = new Uri(originImageInfo[0].FullName, UriKind.RelativeOrAbsolute);
                SourceImage.Source = new BitmapImage(uriSource);

                OriginSouceSlider.Maximum = originImageInfo.Length;

                loadedImages = new BitmapImage[originImageInfo.Length];
            }
        }

        private void OriginSourceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (originImageInfo != null && originImageInfo.Length > e.NewValue && (int)e.OldValue != (int)e.NewValue)
            {
                if (loadedImages[(int)e.NewValue] != null)
                {
                    SourceImage.Source = loadedImages[(int)e.NewValue];
                }
                else
                {
                    var uriSource = new Uri(originImageInfo[(int)e.NewValue].FullName, UriKind.RelativeOrAbsolute);
                    loadedImages[(int)e.NewValue] = new BitmapImage(uriSource);
                    SourceImage.Source = loadedImages[(int)e.NewValue];

                    Debug.WriteLine(uriSource);
                }
            }
        }
    }
}
