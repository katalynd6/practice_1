using System;
using System.Windows;
using tebenkova_Services.DTOs;

namespace tebenkova_wpf
{
    public partial class PartnerEditWindow : Window
    {
        public string PartnerTypeName { get; private set; }
        public string PartnerName { get; private set; }
        public string PartnerPhone { get; private set; }
        public string PartnerEmail { get; private set; }
        public int PartnerRating { get; private set; }

        public PartnerEditWindow()
        {
            InitializeComponent();
            LoadPartnerTypes();
            TitleText.Text = "Добавление партнера";
        }

        public PartnerEditWindow(PartnerDto partnerDto)
        {
            InitializeComponent();
            LoadPartnerTypes();

            // Заполняем поля данными
            CmbPartnerType.Text = partnerDto.TypeName;
            TxtName.Text = partnerDto.Name;
            TxtPhone.Text = partnerDto.Phone;
            TxtEmail.Text = partnerDto.Email;
            TxtRating.Text = partnerDto.Rating.ToString();

            TitleText.Text = "Редактирование партнера";
        }

        private void LoadPartnerTypes()
        {
            var types = new[]
            {
                new { Id = 1, Name = "ООО" },
                new { Id = 2, Name = "ИП" },
                new { Id = 3, Name = "ЗАО" },
                new { Id = 4, Name = "АО" }
            };
            CmbPartnerType.ItemsSource = types;
            CmbPartnerType.DisplayMemberPath = "Name";
            CmbPartnerType.SelectedValuePath = "Id";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CmbPartnerType.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип партнера", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtName.Text))
            {
                MessageBox.Show("Введите наименование", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(TxtRating.Text, out int rating) || rating < 0 || rating > 10)
            {
                MessageBox.Show("Рейтинг должен быть целым числом от 0 до 10", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedType = (dynamic)CmbPartnerType.SelectedItem;

            PartnerTypeName = selectedType.Name;
            PartnerName = TxtName.Text.Trim();
            PartnerPhone = TxtPhone.Text?.Trim() ?? "";
            PartnerEmail = TxtEmail.Text?.Trim() ?? "";
            PartnerRating = rating;

            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}