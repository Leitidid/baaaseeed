using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace baaaseeed
{
    /// <summary>
    /// Логика взаимодействия для Players.xaml
    /// </summary>
    public partial class Players : Window
    {
        private DataRow _playerRow;
        private string _connectionString;
        public Players()
        {
            InitializeComponent();


            if (_playerRow != null)
            {
                // Заполнение формы данными из строки
                IDTextBox.Text = _playerRow["ID"].ToString();
                SurnameTextBox.Text = _playerRow["Фамилия"].ToString();
                NameTextBox.Text = _playerRow["Имя"].ToString();
            }
        }
        private void SavePlayer_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();


                    string query;

                    if (_playerRow == null)
                    {
                        // Добавление новой записи
                        query = @"
                        INSERT INTO Players (Фамилия, Имя, Отчество, НазваниеКоманды, ДатаПриема, ЗаброшенныеШайбы, ГолевыеПодачи, ШтрафноеВремя, СыгранныеМатчи)
                        VALUES (@Surname, @Name, @Patronymic, @TeamName, @JoinDate, @Goals, @Assists, @PenaltyMinutes, @GamesPlayed)";
                    }

                    else
                    {
                        // Обновление существующей записи
                        query = @"
                       UPDATE Players SET 
                          Фамилия = @Surname, 
                          Имя = @Name, 
                          Отчество = @Patronymic,
                           НазваниеКоманды = @TeamName,
                            ДатаПриема = @JoinDate,
                           ЗаброшенныеШайбы = @Goals,
                            ГолевыеПодачи = @Assists,
                          ШтрафноеВремя = @PenaltyMinutes,
                           СыгранныеМатчи = @GamesPlayed
                       WHERE ID = @ID ";
                    }

                    SqlCommand command = new SqlCommand(query, connection);
                    if (_playerRow != null)
                    {
                        command.Parameters.AddWithValue("@ID", int.Parse(IDTextBox.Text));

                    }
                    command.Parameters.AddWithValue("@Surname", SurnameTextBox.Text);
                    command.Parameters.AddWithValue("@Name", NameTextBox.Text);
                    command.Parameters.AddWithValue("@Patronymic", PatronymicTextBox.Text);
                    command.ExecuteNonQuery();
                }
                DialogResult = true;
            }

            catch (FormatException)
            {
                MessageBox.Show("Неверный формат данных. Пожалуйста, введите корректные значения.");
            }
            catch (SqlException ex)
            {

                MessageBox.Show("Ошибка базы данных: " + ex.Message);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Произошла ошибка: " + ex.Message);

            }

        }
    }
}
