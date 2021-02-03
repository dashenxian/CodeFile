using Microsoft.Win32;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CodeFile.Annotations;

namespace CodeFile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public string fileStr;

        public string FileStr
        {
            get => fileStr;
            set
            {
                fileStr = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private async void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FileStr))
            {
                MessageBox.Show("内容不能为空");
                return;
            }

            var bytes = Convert.FromBase64String(FileStr);
            var dialog = new SaveFileDialog()
            {
                Title = "保存文件",
            };
            if (dialog.ShowDialog() != true)
            {
                return;
            }

            var filePath = dialog.FileName;
            await File.WriteAllBytesAsync(filePath, bytes);
        }

        private async void ReadFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Title = "选择文件",
                Multiselect = false,
            };
            if (dialog.ShowDialog() != true)
            {
                return;
            }
            var file = dialog.FileName;
            var bytes = await File.ReadAllBytesAsync(file);
            FileStr = Convert.ToBase64String(bytes);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
