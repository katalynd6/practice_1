using System;
using System.Windows;
using System.Windows.Controls;
using tebenkova_Services.DTOs;

namespace tebenkova_wpf
{
    public partial class ProductEditWindow : Window
    {
        public string ProductArticle { get; private set; }
        public string ProductName { get; private set; }
        public string ProductCategory { get; private set; }
        public decimal ProductPrice { get; private set; }
        public int ProductStock { get; private set; }

        public ProductEditWindow()
        {
            InitializeComponent();
            CmbCategory.SelectedIndex = 0;
            TxtPrice.Text = "1000";
            TxtStock.Text = "10";
        }

        public ProductEditWindow(ProductDto productDto)
        {
            InitializeComponent();

            TxtArticle.Text = productDto.Article;
            TxtName.Text = productDto.Name;
            TxtPrice.Text = productDto.Price.ToString();
            TxtStock.Text = productDto.StockQuantity.ToString();

            // Выбираем категорию
            foreach (ComboBoxItem item in CmbCategory.Items)
            {
                if (item.Content.ToString() == productDto.Category)
                {
                    CmbCategory.SelectedItem = item;
                    break;
                }
            }

            TitleText.Text = "Редактирование товара";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtArticle.Text))
            {
                MessageBox.Show("Введите артикул", "Предупреждение");
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtName.Text))
            {
                MessageBox.Show("Введите наименование", "Предупреждение");
                return;
            }

            if (!decimal.TryParse(TxtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену", "Предупреждение");
                return;
            }

            if (!int.TryParse(TxtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Введите корректное количество", "Предупреждение");
                return;
            }

            ProductArticle = TxtArticle.Text.Trim();
            ProductName = TxtName.Text.Trim();
            ProductCategory = ((ComboBoxItem)CmbCategory.SelectedItem)?.Content.ToString() ?? "Прочее";
            ProductPrice = price;
            ProductStock = stock;

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