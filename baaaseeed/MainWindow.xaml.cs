using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace baaaseeed
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string connectionString = "Server=localhost\\sqlexpress; Database=spartakanddinamo; User=исп-34; Password=1234567890; Encrypt=false"; 
        private SqlConnection connection;
        private SqlDataAdapter adapter;
        private DataTable dataTable;
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                connection = new SqlConnection(connectionString);
                adapter = new SqlDataAdapter("SELECT * FROM Players", connection);
                dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGrid.ItemsSource = dataTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Players playerForm = new Players();
            bool? result = playerForm.ShowDialog();

            if (result == true)
            {
                LoadData();
            }
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;

                Players playerForm = new Players();

                if (playerForm.ShowDialog() == true)
                {

                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Выберите игрока для редактирования.");
            }
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("удалить эту запись?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string query = $"DELETE FROM Players WHERE ID = {selectedRow["ID"]}";
                            SqlCommand command = new SqlCommand(query, connection);
                            command.ExecuteNonQuery();
                        }
                        LoadData();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при удалении данных: " + ex.Message);
                    }
                }


            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.");
            }

        }

        private void SearchPlayer_Click(object sender, RoutedEventArgs e)
        {
            string searchText = searchTextBox.Text;

            if (!string.IsNullOrEmpty(searchText))
            {
                DataView dataView = dataTable.DefaultView;
                string filter = "";


                foreach (DataColumn column in dataTable.Columns)
                {

                    if (column.DataType == typeof(string))
                    {
                        filter += $"[{column.ColumnName}] LIKE '%{searchText}%' OR ";
                    }
                    else if (column.DataType == typeof(int))
                    {
                        if (int.TryParse(searchText, out _))
                        {
                            filter += $"[{column.ColumnName}] = {searchText} OR ";

                        }

                    }

                }

                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Substring(0, filter.Length - 4); // Удаление последнего "OR"

                    dataView.RowFilter = filter;
                    dataGrid.ItemsSource = dataView;
                }
            }
            else
            {
              dataGrid.ItemsSource = dataTable.DefaultView; // отображение всех данных при пустом поиске
            }
        }

    }

}
