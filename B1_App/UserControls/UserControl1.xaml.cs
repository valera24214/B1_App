using B1_App.Models.Model1;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace B1_App.UserControls
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        private string eng_alphabet;
        private string ru_alphabet;

        private int union_count;

        private string path;

        private static Random rnd;

        public class PathException : Exception
        {
            public PathException(string message) : base(message) { }
        }

        public UserControl1()
        {
            InitializeComponent();

            this.Height = SystemParameters.PrimaryScreenHeight * 0.9;
            this.Width = SystemParameters.PrimaryScreenWidth * 0.9;

            rnd = new Random();

            eng_alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            ru_alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

            label1.Visibility = Visibility.Hidden;
            textBox1.Visibility = Visibility.Hidden;
            accept_button.Visibility = Visibility.Hidden;
        }

        private void Create_file(string path)
        {
            File.Create(path).Close();

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                for (int i = 0; i < 100000; i++)
                {
                    writer.WriteLine(Random_Date() + "||" + Random_Signs(eng_alphabet) + "||" + Random_Signs(ru_alphabet) + "||" + Random_Even_Int() + "||" + Random_Double());
                }
            }
        }

        private string Random_Date()
        {
            DateTime start = DateTime.Today.AddYears(-5);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(rnd.Next(range)).ToShortDateString();
        }

        private string Random_Signs(string origin)
        {
            string s = "";
            for (int i = 0; i < 10; i++)
            {
                s += origin[rnd.Next(origin.Length - 1)];
            }

            return s;
        }

        private string Random_Even_Int()
        {
            int x = rnd.Next(1, 50001);
            return (x * 2).ToString();
        }

        private string Random_Double()
        {
            double x = rnd.NextDouble() * 19 + 1;
            return Math.Round(x, 8).ToString("F8");
        }

        private void task_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (path == null)
                {
                    throw new PathException("Путь не указан");
                }

                for (int i = 0; i < 100; i++)
                {
                    Create_file(path + "\\" + (i + 1) + ".txt");
                }

                MessageBox.Show("Файлы созданы");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void union_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (path == null)
                {
                    throw new PathException("Путь не указан");
                }

                label1.Visibility = Visibility.Visible;
                textBox1.Visibility = Visibility.Visible;
                accept_button.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void accept_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (path == null)
                {
                    throw new PathException("Путь не указан");
                }

                File.Create(path + "\\union.txt").Close();
                int deleted = 0;

                using (StreamWriter writer = new StreamWriter(path + "\\union.txt"))
                {
                    for (int i = 0; i < 100; i++)
                    {
                        var lines = File.ReadAllLines(path + "\\" + (i + 1) + ".txt").ToList();
                        if (textBox1.Text != String.Empty)
                        {
                            deleted += lines.RemoveAll(l => l.Contains(textBox1.Text));
                        }

                        foreach (var l in lines)
                        {
                            writer.WriteLine(l);
                        }
                    }
                }

                MessageBox.Show("Файлы успешно объединены, кол-во удалённых:" + deleted);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void path_button_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                path = folderBrowser.SelectedPath;
                label_Path.Content = "Путь: " + path;
            }
        }

        private void load_button_Click(object sender, RoutedEventArgs e)
        {
            path_button.IsEnabled = false;
            accept_button.IsEnabled = false;
            load_button.IsEnabled = false;
            task_button.IsEnabled = false;
            union_button.IsEnabled = false;

            label2.Content = "Пожалуйста, подождите...";

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Перенос в БД завершён");

            load_button.IsEnabled = true;
            path_button.IsEnabled = true;
            accept_button.IsEnabled = true;
            load_button.IsEnabled = true;
            task_button.IsEnabled = true;
            union_button.IsEnabled = true;

            label2.Content = String.Empty;
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            label2.Content = e.UserState.ToString() + " из " + union_count + "; осталось: " + (union_count - (int)e.UserState);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (path == null)
                {
                    throw new PathException("Путь не указан");
                }

                using (var streamReader = new StreamReader(path + "\\union.txt"))
                {
                    while (streamReader.ReadLine() != null)
                    {
                        union_count++;
                    }
                }

                using (var streamReader = new StreamReader(path + "\\union.txt"))
                {
                    string line;
                    int processedCount = 0;

                    Model1 model = new Model1();

                    while ((line = streamReader.ReadLine()) != null)
                    {
                        string[] parts = line.Replace("||", "|").Split('|');

                        if (parts.Length != 5)
                        {
                            throw new ArgumentOutOfRangeException();
                        }

                        Notes note = new Notes()
                        {
                            Date = Convert.ToDateTime(parts[0]),
                            English_signs = parts[1],
                            Russian_signs = parts[2],
                            Even_int = Convert.ToInt32(parts[3]),
                            Decimal = Convert.ToDecimal(parts[4])
                        };

                        model.Notes.Add(note);
                        processedCount++;

                        if (processedCount % (union_count/1000) == 0)
                        {
                            model.SaveChanges();
                            model.Dispose();
                            model = new Model1();
                        }

                        int progress = (int)(((float)(processedCount + 1) / union_count) * 100);
                        (sender as BackgroundWorker).ReportProgress(progress, processedCount + 1);
                        Thread.Sleep(1);
                    }

                    model.SaveChanges();
                    model.Dispose();
                }

                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                GC.Collect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
