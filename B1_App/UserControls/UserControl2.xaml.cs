using B1_App.Models.Model2;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace B1_App.UserControls
{
    /// <summary>
    /// Логика взаимодействия для UserControl2.xaml
    /// </summary>
    public partial class UserControl2 : UserControl
    {
        public UserControl2()
        {
            InitializeComponent();

            this.Height = SystemParameters.PrimaryScreenHeight * 0.9;
            this.Width = SystemParameters.PrimaryScreenWidth * 0.9;

            comboBox_Refresh(comboBox);
        }

        private void comboBox_Refresh(System.Windows.Controls.ComboBox comboBox)
        {
            using (Model2 model2 = new Model2())
            {
                List<Files> files = model2.Files.ToList();
                List<string> comboBoxItems = files.Select(file => file.Path + " + " + file.Name + " + " + file.Date_create).ToList();

                comboBox.ItemsSource = comboBoxItems;
            }
        }

        private string[] Create_account_result(string signs)
        {
            string[] result = new string[7];
            result[0] = signs;

            if (signs.Length != 2)
                throw new ArgumentOutOfRangeException();

            List<Opening_balances> opening_Balances = new List<Opening_balances>();
            List<Cash_turnover> cash_Turnover = new List<Cash_turnover>();
            List<Closing_balances> closing_Balances = new List<Closing_balances>();

            using (Model2 model2 = new Model2())
            {
                var accounts = model2.Bank_accounts.Where(b => b.Number.ToString().Substring(0, 2) == signs).ToArray();

                foreach (var acc in accounts)
                {
                    opening_Balances.Add(model2.Opening_balances.First(o_b => o_b.id == acc.id));
                    cash_Turnover.Add(model2.Cash_turnover.First(c_t => c_t.id == acc.id));
                    closing_Balances.Add(model2.Closing_balances.First(c_b => c_b.id == acc.id));
                }
            }

            result[1] = opening_Balances.Select(o_b=>o_b.Active).Sum().ToString();
            result[2] = opening_Balances.Select(o_b=>o_b.Passive).Sum().ToString();
            result[3] = cash_Turnover.Select(c_t => c_t.Debited).Sum().ToString();
            result[4] = cash_Turnover.Select(c_t => c_t.Credited).Sum().ToString();
            result[5] = closing_Balances.Select(c_b => c_b.Active).Sum().ToString();
            result[6] = closing_Balances.Select(c_b => c_b.Passive).Sum().ToString();

            return result;
        }

        private string[] Create_class_result(string account_class)
        {
            string[] result = new string[7];
            
            List<Opening_balances> opening_Balances = new List<Opening_balances>();
            List<Cash_turnover> cash_Turnover = new List<Cash_turnover>();
            List<Closing_balances> closing_Balances = new List<Closing_balances>();

            using (Model2 model2 = new Model2())
            {
                var accounts = model2.Bank_accounts.Where(b => b.Class == account_class).ToArray();

                foreach (var acc in accounts)
                {
                    opening_Balances.Add(model2.Opening_balances.First(o_b => o_b.id == acc.id));
                    cash_Turnover.Add(model2.Cash_turnover.First(c_t => c_t.id == acc.id));
                    closing_Balances.Add(model2.Closing_balances.First(c_b => c_b.id == acc.id));
                }
            }

            result[0] = "По классу:";
            result[1] = opening_Balances.Select(o_b => o_b.Active).Sum().ToString();
            result[2] = opening_Balances.Select(o_b => o_b.Passive).Sum().ToString();
            result[3] = cash_Turnover.Select(c_t => c_t.Debited).Sum().ToString();
            result[4] = cash_Turnover.Select(c_t => c_t.Credited).Sum().ToString();
            result[5] = closing_Balances.Select(c_b => c_b.Active).Sum().ToString();
            result[6] = closing_Balances.Select(c_b => c_b.Passive).Sum().ToString();

            return result;
        }

        private string[] Create_file_result(int file_id)
        {
            string[] result = new string[7];

            List<Opening_balances> opening_Balances = new List<Opening_balances>();
            List<Cash_turnover> cash_Turnover = new List<Cash_turnover>();
            List<Closing_balances> closing_Balances = new List<Closing_balances>();

            using (Model2 model2 = new Model2())
            {
                var accounts = model2.Bank_accounts.Where(b => b.id_File == file_id).ToArray();

                foreach (var acc in accounts)
                {
                    opening_Balances.Add(model2.Opening_balances.First(o_b => o_b.id == acc.id));
                    cash_Turnover.Add(model2.Cash_turnover.First(c_t => c_t.id == acc.id));
                    closing_Balances.Add(model2.Closing_balances.First(c_b => c_b.id == acc.id));
                }
            }

            result[0] = "ИТОГО:";
            result[1] = opening_Balances.Select(o_b => o_b.Active).Sum().ToString("F2");
            result[2] = opening_Balances.Select(o_b => o_b.Passive).Sum().ToString("F2");
            result[3] = cash_Turnover.Select(c_t => c_t.Debited).Sum().ToString("F2");
            result[4] = cash_Turnover.Select(c_t => c_t.Credited).Sum().ToString("F2");
            result[5] = closing_Balances.Select(c_b => c_b.Active).Sum().ToString("F2");
            result[6] = closing_Balances.Select(c_b => c_b.Passive).Sum().ToString("F2");

            return result;
        }


        private DataTable Create_DataTable(Files file)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Счёт");
            dt.Columns.Add("Входящее сальдо (Актив)");
            dt.Columns.Add("Входящее сальдо (Пассив)");
            dt.Columns.Add("Обороты (Дебет)");
            dt.Columns.Add("Обороты (Кредит)");
            dt.Columns.Add("Исходящее сальдо (Актив)");
            dt.Columns.Add("Исходящее сальдо (Пассив)");

            Bank_accounts[] accounts;

            DataRow dataRow = null;

            using (Model2 model2 = new Model2())
            {
                accounts = model2.Bank_accounts.Where(b_a => b_a.id_File == file.id).ToArray();
            }

            string account_signs = accounts[0].Number.ToString().Substring(0,2);
            string class_name = accounts[0].Class;

            dataRow = dt.NewRow();
            dataRow[0] = class_name;
            dt.Rows.Add(dataRow);

            for (int i = 0; i < accounts.Length; i++)
            {
                Bank_accounts b_a = accounts[i];
                
                if (accounts[i].Number.ToString().Substring(0, 2) != account_signs)
                {
                    dataRow = dt.NewRow();
                    
                    var row = Create_account_result(account_signs);
                    for (int j = 0; j < row.Length; j++)
                    {
                        dataRow[j] = row[j];
                    }

                    dt.Rows.Add(dataRow);

                    account_signs = accounts[i].Number.ToString().Substring(0, 2);
                }

                if (b_a.Class != class_name)
                {
                    dataRow = dt.NewRow();
                    var row = Create_class_result(class_name);
                    for (int j = 0; j < row.Length; j++)
                    {
                        dataRow[j] = row[j];
                    }
                    dt.Rows.Add(dataRow);

                    dataRow = dt.NewRow();
                    class_name = b_a.Class;
                    dataRow[0] = class_name;
                    dt.Rows.Add(dataRow);
                }

                using (Model2 model2 = new Model2())
                {
                    Opening_balances opening = model2.Opening_balances.First(o => o.id == b_a.id);
                    Cash_turnover cash = model2.Cash_turnover.First(c => c.id == b_a.id);
                    Closing_balances closing = model2.Closing_balances.First(c => c.id == b_a.id);

                    DataRow dataRow2 = dt.NewRow();
                    dataRow2[0] = b_a.Number;
                    dataRow2[1] = opening.Active;
                    dataRow2[2] = opening.Passive;
                    dataRow2[3] = cash.Debited;
                    dataRow2[4] = cash.Credited;
                    dataRow2[5] = closing.Active;
                    dataRow2[6] = closing.Passive;

                    dt.Rows.Add(dataRow2);
                }
            }

            dataRow = dt.NewRow();
            var row_last_class = Create_class_result(accounts.Last().Class);
            for (int j = 0; j < row_last_class.Length; j++)
            {
                dataRow[j] = row_last_class[j];
            }
            dt.Rows.Add(dataRow);

            dataRow = dt.NewRow();
            var row_file = Create_file_result(file.id);
            for (int j = 0; j < row_file.Length; j++)
            {
                dataRow[j] = row_file[j];
            }
            dt.Rows.Add(dataRow);

            return dt;
        }

        private bool Row_Is_Account(string[,] arr, int row)
        {
            return (arr[row, 0].Length == 4 && int.TryParse(arr[row, 0], out int val));
        }

        private bool Row_Is_Class_Name(string[,] arr, int row)
        {
            return (arr[row, 0].ToLower().Contains("класс") && arr[row, 1] == String.Empty);
        }

        private void upload_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

                if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                string filename = openFileDialog.FileName;

                Excel.Application ObjWorkExcel = new Excel.Application();
                Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(filename);
                Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1];
                var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);

                string[,] list = new string[lastCell.Row, 7];

                for (int i = 0; i < lastCell.Row; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        list[i, j] = ObjWorkSheet.Cells[i + 1, j + 1].Text.ToString();
                    }
                }

                ObjWorkBook.Close(false);
                ObjWorkExcel.Quit();

                string[] splited_date = list[2, 0].Split(' ');

                using (Model2 model2 = new Model2())
                {
                    Files file = new Files()
                    {
                        Name = list[1, 0],
                        Path = filename,
                        Date_start = Convert.ToDateTime(splited_date[3]),
                        Date_finish = Convert.ToDateTime(splited_date[5]),
                        Date_create = Convert.ToDateTime(list[5, 0]),
                        Bank_name = list[0, 0],
                    };

                    model2.Files.Add(file);
                    model2.SaveChanges();

                    int file_id = file.id;

                    string class_name = "";

                    for (int i = 0; i < list.GetLength(0); i++)
                    {
                        if (Row_Is_Class_Name(list, i))
                        {
                            class_name = list[i, 0];
                        }

                        if (Row_Is_Account(list, i))
                        {
                            Bank_accounts bank_Account = new Bank_accounts()
                            {
                                Number = Convert.ToInt32(list[i, 0]),
                                id_File = file_id,
                                Class = class_name,
                            };

                            model2.Bank_accounts.Add(bank_Account);
                            model2.SaveChanges();

                            int account_id = bank_Account.id;

                            Opening_balances opening_Balance = new Opening_balances()
                            {
                                id = account_id,
                                Active = Convert.ToDouble(list[i, 1]),
                                Passive = Convert.ToDouble(list[i, 2]),
                            };

                            model2.Opening_balances.Add(opening_Balance);

                            Cash_turnover cash_Turnover = new Cash_turnover()
                            {
                                id = account_id,
                                Debited = Convert.ToDouble(list[i, 3]),
                                Credited = Convert.ToDouble(list[i, 4]),
                            };

                            model2.Cash_turnover.Add(cash_Turnover);

                            Closing_balances closing_Balances = new Closing_balances()
                            {
                                id = account_id,
                                Active = Convert.ToDouble(list[i, 5]),
                                Passive = Convert.ToDouble(list[i, 6]),
                            };

                            model2.Closing_balances.Add(closing_Balances);
                            model2.SaveChanges();
                        }
                    }
                }

                comboBox_Refresh(comboBox);

                MessageBox.Show("Загружено");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] param = comboBox.SelectedItem.ToString().Split('+');
            var path = param[0].Trim();
            var name = param[1].Trim();
            var date = Convert.ToDateTime(param[2].Trim());

            Files file;
            using (Model2 model2 = new Model2())
            {
                file = model2.Files.Where(f => f.Path == path && f.Name == name && f.Date_create == date).First();
            }

            dataGrid.ItemsSource = Create_DataTable(file).DefaultView;
        }
    }
}
