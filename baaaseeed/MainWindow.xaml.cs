using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        private SpartakanddinamoContext _context; // Контекст базы данных
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            _context = new SpartakanddinamoContext();
        }
        private void LoadData()
        {
            try
            {
                using (SpartakanddinamoContext db = new SpartakanddinamoContext())
                {
                    dataGrid.ItemsSource = db.Players.ToList()    ;
                    //connection = new SqlConnection(connectionString);
                    //adapter = new SqlDataAdapter("SELECT * FROM Players", connection);
                    //dataTable = new DataTable();
                    //adapter.Fill(dataTable);

            
                }
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
            //  
            //    if (dataGrid.SelectedItem != null)
            //    {
            //       Player player = dataGrid.SelectedItem as Player;
            //  
            //       
            //        Players playerForm = new Players();
            //  
            //  
            //        if (playerForm.ShowDialog() == true)
            //        {
            //  
            //            LoadData();
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("Выберите игрока для редактирования.");
            //    }
            // Проверяем, выбран ли игрок в таблице
            // Проверяем, выбран ли игрок в таблице
            var selectedPlayer = dataGrid.SelectedItem as Player;
            if (selectedPlayer == null)
            {
                MessageBox.Show("Выберите игрока для редактирования.");
                return;
            }

            // Отладочная информация
            Console.WriteLine($"Selected player: ID={selectedPlayer.Id}, Name={selectedPlayer.Имя}");

            // Открываем окно редактирования с передачей выбранного игрока
            Players playerForm = new Players(selectedPlayer);
            if (playerForm.ShowDialog() == true)
            {
                LoadData(); // Обновляем данные в таблице после редактирования
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
                        Player selectedRow = (Player)dataGrid.SelectedItem;

                        using (SpartakanddinamoContext connection = new SpartakanddinamoContext())
                        {
                            connection.Players.Remove(selectedRow);
                            connection.SaveChanges();
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
            //     string searchText = searchTextBox.Text;
            //
            //     if (!string.IsNullOrEmpty(searchText))
            //     {
            //         DataView dataView = dataTable.DefaultView;
            //         string filter = "";
            //
            //
            //         foreach (DataColumn column in dataTable.Columns)
            //         {
            //
            //             if (column.DataType == typeof(string))
            //             {
            //                 filter += $"[{column.ColumnName}] LIKE '%{searchText}%' OR ";
            //             }
            //             else if (column.DataType == typeof(int))
            //             {
            //                 if (int.TryParse(searchText, out _))
            //                 {
            //                     filter += $"[{column.ColumnName}] = {searchText} OR ";
            //
            //                 }
            //
            //             }
            //
            //         }
            //
            //         if (!string.IsNullOrEmpty(filter))
            //         {
            //             filter = filter.Substring(0, filter.Length - 4); // Удаление последнего "OR"
            //
            //             dataView.RowFilter = filter;
            //             dataGrid.ItemsSource = dataView;
            //         }
            //     }
            //     else
            //     {
            //         dataGrid.ItemsSource = dataTable.DefaultView; // отображение всех данных при пустом поиске
            //     }
            string searchText = searchTextBox.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                try
                {
                    using (SpartakanddinamoContext db = new SpartakanddinamoContext())
                    {
                        var filteredPlayers = db.Players.Where(p =>
                            (p.Фамилия != null && EF.Functions.Like(p.Фамилия, $"%{searchText}%")) ||
                            (p.Имя != null && EF.Functions.Like(p.Имя, $"%{searchText}%")) ||
                            (p.Отчество != null && EF.Functions.Like(p.Отчество, $"%{searchText}%")) ||
                            (p.НазваниеКоманды != null && EF.Functions.Like(p.НазваниеКоманды, $"%{searchText}%")) ||
                            (p.ЗаброшенныеШайбы.HasValue && EF.Functions.Like(p.ЗаброшенныеШайбы.Value.ToString(), $"%{searchText}%")) ||
                            (p.ГолевыеПодачи.HasValue && EF.Functions.Like(p.ГолевыеПодачи.Value.ToString(), $"%{searchText}%")) ||
                            (p.ШтрафноеВремя.HasValue && EF.Functions.Like(p.ШтрафноеВремя.Value.ToString(), $"%{searchText}%")) ||
                            (p.СыгранныеМатчи.HasValue && EF.Functions.Like(p.СыгранныеМатчи.Value.ToString(), $"%{searchText}%"))
                        ).ToList();

                        dataGrid.ItemsSource = filteredPlayers;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при поиске: {ex.Message}");
                }
            }
            else
            {
                LoadData(); // Отображение всех данных при пустом поиске
            }
        }
        private void filter_Click(object sender, RoutedEventArgs e)
        {
            //         if (filterTextBox.Text.IsNullOrEmpty() == false)
            //         {
            //             using (SpartakanddinamoContext _db = new SpartakanddinamoContext())
            //             {
            //                 var filtered = _db = new SpartakanddinamoContext();
            //                 dataGrid.ItemsSource = filtered.dat();
            //             }
            //         }
            //         else
            //         {
            //             LoadData();
            //         }
        }

        private void dataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
           ;
        }

        private void FilterByGoals_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(searchTextBox.Text, out int goalCount))
            {
                try
                {
                    using (SpartakanddinamoContext db = new SpartakanddinamoContext())
                    {
                        var filteredPlayers = db.Players.Where(p => p.ЗаброшенныеШайбы == goalCount).ToList();
                        dataGrid.ItemsSource = filteredPlayers;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при фильтрации: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Введите корректное число забитых шайб.");
            }
        }
    } 

}
