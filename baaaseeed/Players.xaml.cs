using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
using static baaaseeed.Players;

namespace baaaseeed
{
    /// <summary>
    /// Логика взаимодействия для Players.xaml
    /// </summary>
    public partial class Players : Window
    {

        private string _connectionString = "Server=localhost\\sqlexpress; Database=spartakanddinamo; User=исп-34; Password=1234567890; Encrypt=false";
        private SpartakanddinamoContext _context; // Контекст базы данных
        private Player _player; // Объект игрокаа

        public Players(Player player = null)
        {
            InitializeComponent();


            // Инициализация контекста базы данных
            _context = new SpartakanddinamoContext();
            _player = player;

            //            if (_player != null)
            //            {
            //                // Заполнение формы данными игрока
            //                IDTextBox.Text = _player.Id.ToString();
            //                SurnameTextBox.Text = _player.Фамилия;
            //                NameTextBox.Text = _player.Имя;
            //                PatronymicTextBox.Text = _player.Отчество;
            //                TeamNameTextBox.Text = _player.НазваниеКоманды;
            //                JoinDatePicker.SelectedDate = _player.ДатаПриема?.ToDateTime(TimeOnly.MinValue); // Преобразование DateOnly? в DateTime?
            //                GoalsTextBox.Text = _player.ЗаброшенныеШайбы?.ToString();
            //                AssistsTextBox.Text = _player.ГолевыеПодачи?.ToString();
            //                PenaltyMinutesTextBox.Text = _player.ШтрафноеВремя?.ToString();
            //                GamesPlayedTextBox.Text = _player.СыгранныеМатчи?.ToString();
            //            }
            //
            //            // Сделать IDTextBox доступным только для чтения
            //            IDTextBox.IsEnabled = false;
            //        }
            //
            //        private void SavePlayer_Click(object sender, RoutedEventArgs e)
            //        {
            //            try
            //            {
            //                // Создание или обновление объекта игрока
            //                var player = _player ?? new Player(); // Новый игрок или существующий
            //
            //                // Заполнение свойств игрока
            //                player.Фамилия = SurnameTextBox.Text;
            //                player.Имя = NameTextBox.Text;
            //                player.Отчество = PatronymicTextBox.Text;
            //                player.НазваниеКоманды = TeamNameTextBox.Text;
            //                player.ДатаПриема = JoinDatePicker.SelectedDate.HasValue
            //                    ? DateOnly.FromDateTime(JoinDatePicker.SelectedDate.Value) // Преобразование DateTime? в DateOnly?
            //                    : null;
            //                player.ЗаброшенныеШайбы = int.TryParse(GoalsTextBox.Text, out int goals) ? goals : (int?)null;
            //                player.ГолевыеПодачи = int.TryParse(AssistsTextBox.Text, out int assists) ? assists : (int?)null;
            //                player.ШтрафноеВремя = int.TryParse(PenaltyMinutesTextBox.Text, out int penaltyMinutes) ? penaltyMinutes : (int?)null;
            //                player.СыгранныеМатчи = int.TryParse(GamesPlayedTextBox.Text, out int gamesPlayed) ? gamesPlayed : (int?)null;
            //
            //                if (_player == null)
            //                {
            //                    // Добавление нового игрока
            //                    _context.Players.Add(player);
            //                }
            //
            //                // Сохранение изменений в базе данных
            //                _context.SaveChanges();
            //
            //                MessageBox.Show("Данные успешно сохранены!");
            //                DialogResult = true;
            //            }
            //            catch (Exception ex)
            //            {
            //                MessageBox.Show("Произошла ошибка: " + ex.Message);
            //            }
            //        }
            //    }
            // }
            if (_player != null)
            {
                // Отладочная информация
                Console.WriteLine($"Editing player: ID={_player.Id}, Name={_player.Имя}");

                // Заполнение формы данными игрока
                IDTextBox.Text = _player.Id.ToString();
                SurnameTextBox.Text = _player.Фамилия ?? string.Empty;
                NameTextBox.Text = _player.Имя ?? string.Empty;
                PatronymicTextBox.Text = _player.Отчество ?? string.Empty;
                TeamNameTextBox.Text = _player.НазваниеКоманды ?? string.Empty;
                JoinDatePicker.SelectedDate = _player.ДатаПриема?.ToDateTime(TimeOnly.MinValue); // Преобразование DateOnly? в DateTime?
                GoalsTextBox.Text = _player.ЗаброшенныеШайбы?.ToString() ?? string.Empty;
                AssistsTextBox.Text = _player.ГолевыеПодачи?.ToString() ?? string.Empty;
                PenaltyMinutesTextBox.Text = _player.ШтрафноеВремя?.ToString() ?? string.Empty;
                GamesPlayedTextBox.Text = _player.СыгранныеМатчи?.ToString() ?? string.Empty;
            }

            // Сделать IDTextBox доступным только для чтения
            IDTextBox.IsEnabled = false;
        }

        private void SavePlayer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создание или обновление объекта игрока
                var player = _player ?? new Player(); // Новый игрок или существующий

                // Заполнение свойств игрока
                player.Фамилия = SurnameTextBox.Text;
                player.Имя = NameTextBox.Text;
                player.Отчество = PatronymicTextBox.Text;
                player.НазваниеКоманды = TeamNameTextBox.Text;
                player.ДатаПриема = JoinDatePicker.SelectedDate.HasValue
                    ? DateOnly.FromDateTime(JoinDatePicker.SelectedDate.Value) // Преобразование DateTime? в DateOnly?
                    : null;
                player.ЗаброшенныеШайбы = int.TryParse(GoalsTextBox.Text, out int goals) ? goals : (int?)null;
                player.ГолевыеПодачи = int.TryParse(AssistsTextBox.Text, out int assists) ? assists : (int?)null;
                player.ШтрафноеВремя = int.TryParse(PenaltyMinutesTextBox.Text, out int penaltyMinutes) ? penaltyMinutes : (int?)null;
                player.СыгранныеМатчи = int.TryParse(GamesPlayedTextBox.Text, out int gamesPlayed) ? gamesPlayed : (int?)null;

                if (_player == null)
                {
                    // Добавление нового игрока
                    _context.Players.Add(player);
                }

                // Сохранение изменений в базе данных
                _context.SaveChanges();

                MessageBox.Show("Данные успешно сохранены!");
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }
    }
}
