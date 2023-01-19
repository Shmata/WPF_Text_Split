using FileProcessing.Business;
using FileProcessing.Common;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FileProcessing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource _canceller = new CancellationTokenSource();
        Dictionary<string, Token> _tokens = new Dictionary<string, Token>();
        public MainWindow()
        {
            InitializeComponent();

        }
        private void TxtFile_DoubleClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? result = openFileDialog.ShowDialog();
            if (result.Value)
            {
                txtFile.Text = openFileDialog.FileName;
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            _canceller.Cancel();
            btnProcess.Content = "Process";
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            btnProcess.Content = "Processing...";
            btnProcess.IsEnabled = false;
            string file = txtFile.Text;
            _tokens.Clear();
            dgData.ItemsSource = new List<Token>() { };

            StartTask(file);

        }

        private void StartTask(string file)
        {
            _canceller = new CancellationTokenSource();
            Task Worker = Task.Factory.StartNew(() =>
            {
                try
                {
                    using (_canceller.Token.Register(Thread.CurrentThread.Interrupt))
                    {
                        var wordProcessor = new WordProcessor();
                        return wordProcessor.LoadCollectionData(File.ReadLines(file), _canceller, _tokens);
                    }
                }
                catch (ThreadInterruptedException)
                {
                    return false;
                }
                finally
                {
                    try
                    {
                        Dispatcher.Invoke(() =>
                                    {
                                        btnProcess.Content = "Process";
                                        btnProcess.IsEnabled = true; ;
                                    });
                    }
                    catch (System.Exception)
                    {
                        //ignore dead thread issue
                    }
                }
            }, _canceller.Token);

            Worker.ContinueWith(ShowData, _canceller);
        }

        private void ShowData(Task arg1, object? arg2)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    btnProcess.Content = "Process";
                    dgData.ItemsSource = _tokens.Values;
                });
            }
            catch (ThreadInterruptedException)
            {
                //Ignore interrupt exception

            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _canceller?.Cancel();
        }
    }
}
