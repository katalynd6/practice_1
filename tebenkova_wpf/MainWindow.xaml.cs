using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using tebenkova_Services.DTOs;
using tebenkova_wpf.Converters;

namespace tebenkova_wpf
{
    public partial class MainWindow : Window
    {
        private List<PartnerDto> _currentPartners;
        private List<ProductDto> _currentProducts;
        private List<WarehouseItemDto> _warehouseItems;
        private List<SaleDto> _salesData;
        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();
            InitializeClock();
            LoadPartnersTab();
            MainTabControl.SelectionChanged += MainTabControl_SelectionChanged;
        }

        private void InitializeClock()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) =>
            {
                TxtCurrentDate.Text = DateTime.Now.ToString("dd.MM.yyyy");
                TxtCurrentTime.Text = DateTime.Now.ToString("HH:mm:ss");
            };
            _timer.Start();
        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedTab = MainTabControl.SelectedItem as TabItem;
            if (selectedTab == null) return;

            string header = selectedTab.Header.ToString();
            if (header.Contains("Партнеры"))
                LoadPartnersTab();
            else if (header.Contains("Товары"))
                LoadProductsTab();
            else if (header.Contains("Склад"))
                LoadWarehouseTab();
            else if (header.Contains("Отчеты"))
                LoadReportsTab();
        }

        // ==================== ОБРАБОТЧИКИ МЕНЮ ====================

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Выход",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }

        private void AddPartnerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowAddPartner();
        }

        private void EditPartnerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowEditPartner();
        }

        private void DeletePartnerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DeletePartner();
        }

        private void RefreshPartnersMenuItem_Click(object sender, RoutedEventArgs e)
        {
            RefreshPartners();
        }

        private void AddProductMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowAddProduct();
        }

        private void EditProductMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowEditProduct();
        }

        private void DeleteProductMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DeleteProduct();
        }

        private void RefreshProductsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            RefreshProducts();
        }

        private void SalesReportMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowReport("sales");
        }

        private void FinanceReportMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowReport("finance");
        }

        private void WarehouseReportMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowReport("warehouse");
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ООО Цветы Прикамья\nСистема учета партнеров\nВерсия 2.0\n\nПроизводственная практика 2026",
                "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // ==================== МЕТОД SHOWREPORT ====================

        private void ShowReport(string reportType)
        {
            switch (reportType)
            {
                case "sales":
                    if (_salesData == null) _salesData = GetTestSalesData();
                    var totalSales = _salesData.Sum(s => s.Amount);
                    MessageBox.Show($"📊 Отчет по продажам\n\nОбщая выручка: {totalSales:N2} ₽\nКоличество продаж: {_salesData.Count}",
                        "Отчет по продажам", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;

                case "finance":
                    if (_salesData == null) _salesData = GetTestSalesData();
                    var financeTotal = _salesData.Sum(s => s.Amount);
                    decimal profitMargin = 0.3m;
                    MessageBox.Show($"💰 Финансовый отчет\n\nВыручка: {financeTotal:N2} ₽\nПрибыль (30%): {financeTotal * profitMargin:N2} ₽\nРентабельность: 30%",
                        "Финансовый отчет", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;

                case "warehouse":
                    if (_warehouseItems == null) LoadWarehouseData();
                    var needToBuy = _warehouseItems.Count(w => w.Status == "Требуется закуп" || w.Status == "Отсутствует");
                    var totalStock = _warehouseItems.Sum(w => w.CurrentStock);
                    MessageBox.Show($"🏪 Складской отчет\n\nТребуется закупить: {needToBuy} позиций\nВсего товаров на складе: {totalStock} шт\nКритических запасов: {_warehouseItems.Count(w => w.CurrentStock == 0)}",
                        "Складской отчет", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }
        }

        // ==================== ОБЩИЕ МЕТОДЫ ====================

        private DataGrid GetCurrentDataGrid()
        {
            if (TabContentArea.Content is Grid grid)
            {
                foreach (var child in grid.Children)
                {
                    if (child is DataGrid dataGrid)
                        return dataGrid;
                }
            }
            return null;
        }

        // ==================== ВКЛАДКА ПАРТНЕРЫ ====================

        private void LoadPartnersTab()
        {
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var searchPanel = CreateSearchPanel("partners");
            Grid.SetRow(searchPanel, 0);
            grid.Children.Add(searchPanel);

            var dataGrid = CreatePartnersDataGrid();
            Grid.SetRow(dataGrid, 1);
            grid.Children.Add(dataGrid);

            TabContentArea.Content = grid;
            LoadPartnersData(dataGrid);
        }

        private StackPanel CreateSearchPanel(string target)
        {
            var panel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 10) };

            panel.Children.Add(new TextBlock
            {
                Text = "🔍 Поиск:",
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5),
                FontSize = 13,
                FontWeight = FontWeights.SemiBold
            });

            var searchBox = new TextBox
            {
                Width = 300,
                Height = 35,
                Margin = new Thickness(5),
                Padding = new Thickness(10, 0, 10, 0),
                VerticalContentAlignment = VerticalAlignment.Center,
                FontSize = 13,
                Tag = target
            };

            if (target == "partners")
                searchBox.TextChanged += (s, e) => SearchPartners(searchBox.Text);
            else if (target == "products")
                searchBox.TextChanged += (s, e) => SearchProducts(searchBox.Text);

            panel.Children.Add(searchBox);

            panel.Children.Add(new TextBlock
            {
                Text = "💡 Совет: выберите запись → меню → действие",
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(15, 0, 0, 0),
                FontSize = 11,
                Foreground = Brushes.Gray,
                FontStyle = FontStyles.Italic
            });

            return panel;
        }

        private DataGrid CreatePartnersDataGrid()
        {
            var grid = new DataGrid
            {
                AutoGenerateColumns = false,
                IsReadOnly = true,
                SelectionMode = DataGridSelectionMode.Single
            };

            grid.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Тип", Binding = new System.Windows.Data.Binding("TypeName"), Width = 80 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Наименование", Binding = new System.Windows.Data.Binding("Name"), Width = 250 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Телефон", Binding = new System.Windows.Data.Binding("Phone"), Width = 130 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Email", Binding = new System.Windows.Data.Binding("Email"), Width = 180 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Рейтинг", Binding = new System.Windows.Data.Binding("Rating"), Width = 70 });

            var discountColumn = new DataGridTextColumn
            {
                Header = "Скидка",
                Binding = new System.Windows.Data.Binding("Discount") { StringFormat = "{0}%" },
                Width = 80
            };

            var style = new Style(typeof(TextBlock));
            style.Setters.Add(new Setter(TextBlock.BackgroundProperty,
                new System.Windows.Data.Binding("Discount") { Converter = new DiscountToColorConverter() }));
            style.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.White));
            style.Setters.Add(new Setter(TextBlock.PaddingProperty, new Thickness(5, 2, 5, 2)));
            style.Setters.Add(new Setter(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center));
            style.Setters.Add(new Setter(TextBlock.FontWeightProperty, FontWeights.Bold));

            discountColumn.ElementStyle = style;
            grid.Columns.Add(discountColumn);

            return grid;
        }

        private void LoadPartnersData(DataGrid grid)
        {
            try
            {
                TxtStatus.Text = "⏳ Загрузка партнеров...";
                if (_currentPartners == null)
                {
                    _currentPartners = GetTestPartners();
                }
                grid.ItemsSource = null;
                grid.ItemsSource = _currentPartners;
                TxtStatus.Text = $"✅ Загружено партнеров: {_currentPartners.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
                TxtStatus.Text = "❌ Ошибка загрузки";
            }
        }

        private List<PartnerDto> GetTestPartners()
        {
            var data = new List<PartnerDto>();
            var rnd = new Random();
            string[] types = { "ООО", "ИП", "ЗАО", "АО" };
            string[] names = { "Цветочный рай", "Флора-Маркет", "Садовод", "Орхидея", "Роза", "Тюльпан", "Кактус", "Фиалка" };

            for (int i = 1; i <= 8; i++)
            {
                data.Add(new PartnerDto
                {
                    Id = i,
                    TypeName = types[rnd.Next(types.Length)],
                    Name = names[rnd.Next(names.Length)],
                    Phone = $"+7 (342) {rnd.Next(200, 999)}-{rnd.Next(10, 99)}-{rnd.Next(10, 99)}",
                    Email = $"partner{i}@mail.ru",
                    Rating = rnd.Next(1, 6),
                    Discount = new[] { 0, 5, 10, 15 }[rnd.Next(4)],
                    TotalSales = rnd.Next(10000, 600000)
                });
            }
            return data;
        }

        private void RefreshPartners()
        {
            var grid = GetCurrentDataGrid();
            if (grid != null)
            {
                grid.ItemsSource = null;
                grid.ItemsSource = _currentPartners;
                TxtStatus.Text = $"✅ Обновлено. Всего: {_currentPartners.Count}";
            }
        }

        private void SearchPartners(string searchText)
        {
            var grid = GetCurrentDataGrid();
            if (_currentPartners == null || grid == null) return;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                grid.ItemsSource = _currentPartners;
                TxtStatus.Text = $"✅ Всего: {_currentPartners.Count}";
            }
            else
            {
                var filtered = _currentPartners.Where(p =>
                    p.Name.ToLower().Contains(searchText.ToLower()) ||
                    (p.Phone != null && p.Phone.ToLower().Contains(searchText.ToLower())) ||
                    (p.Email != null && p.Email.ToLower().Contains(searchText.ToLower()))
                ).ToList();

                grid.ItemsSource = filtered;
                TxtStatus.Text = $"🔍 Найдено: {filtered.Count} из {_currentPartners.Count}";
            }
        }

        private void ShowAddPartner()
        {
            var dialog = new PartnerEditWindow();
            if (dialog.ShowDialog() == true)
            {
                var newPartner = new PartnerDto
                {
                    Id = _currentPartners.Count > 0 ? _currentPartners.Max(p => p.Id) + 1 : 1,
                    TypeName = dialog.PartnerTypeName,
                    Name = dialog.PartnerName,
                    Phone = dialog.PartnerPhone,
                    Email = dialog.PartnerEmail,
                    Rating = dialog.PartnerRating,
                    Discount = 0,
                    TotalSales = 0
                };
                _currentPartners.Add(newPartner);
                RefreshPartners();
                TxtStatus.Text = $"✅ Партнер '{newPartner.Name}' добавлен";
            }
        }

        private void ShowEditPartner()
        {
            var grid = GetCurrentDataGrid();
            if (grid?.SelectedItem == null)
            {
                MessageBox.Show("Сначала выберите партнера в таблице", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var selected = (PartnerDto)grid.SelectedItem;
            var dialog = new PartnerEditWindow(selected);
            if (dialog.ShowDialog() == true)
            {
                var index = _currentPartners.FindIndex(p => p.Id == selected.Id);
                if (index >= 0)
                {
                    _currentPartners[index].TypeName = dialog.PartnerTypeName;
                    _currentPartners[index].Name = dialog.PartnerName;
                    _currentPartners[index].Phone = dialog.PartnerPhone;
                    _currentPartners[index].Email = dialog.PartnerEmail;
                    _currentPartners[index].Rating = dialog.PartnerRating;
                }
                RefreshPartners();
                TxtStatus.Text = $"✅ Партнер '{selected.Name}' отредактирован";
            }
        }

        private void DeletePartner()
        {
            var grid = GetCurrentDataGrid();
            if (grid?.SelectedItem == null)
            {
                MessageBox.Show("Сначала выберите партнера в таблице", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var selected = (PartnerDto)grid.SelectedItem;
            var result = MessageBox.Show($"Удалить партнера '{selected.Name}'?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _currentPartners.Remove(selected);
                RefreshPartners();
                TxtStatus.Text = $"✅ Партнер '{selected.Name}' удален";
            }
        }

        // ==================== ВКЛАДКА ТОВАРЫ ====================

        private void LoadProductsTab()
        {
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var searchPanel = CreateSearchPanel("products");
            Grid.SetRow(searchPanel, 0);
            grid.Children.Add(searchPanel);

            var dataGrid = CreateProductsDataGrid();
            Grid.SetRow(dataGrid, 1);
            grid.Children.Add(dataGrid);

            TabContentArea.Content = grid;
            LoadProductsData(dataGrid);
        }

        private DataGrid CreateProductsDataGrid()
        {
            var grid = new DataGrid
            {
                AutoGenerateColumns = false,
                IsReadOnly = true,
                SelectionMode = DataGridSelectionMode.Single
            };

            grid.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Артикул", Binding = new System.Windows.Data.Binding("Article"), Width = 100 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Наименование", Binding = new System.Windows.Data.Binding("Name"), Width = 250 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Категория", Binding = new System.Windows.Data.Binding("Category"), Width = 120 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Цена", Binding = new System.Windows.Data.Binding("Price") { StringFormat = "{0:N2} ₽" }, Width = 100 });
            grid.Columns.Add(new DataGridTextColumn { Header = "На складе", Binding = new System.Windows.Data.Binding("StockQuantity"), Width = 100 });

            return grid;
        }

        private void LoadProductsData(DataGrid grid)
        {
            try
            {
                TxtStatus.Text = "⏳ Загрузка товаров...";
                if (_currentProducts == null)
                {
                    _currentProducts = GetTestProducts();
                }
                grid.ItemsSource = null;
                grid.ItemsSource = _currentProducts;
                TxtStatus.Text = $"✅ Загружено товаров: {_currentProducts.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
                TxtStatus.Text = "❌ Ошибка загрузки";
            }
        }

        private List<ProductDto> GetTestProducts()
        {
            var data = new List<ProductDto>();
            string[] categories = { "Цветы", "Горшки", "Удобрения", "Инструмент", "Декор" };
            string[] names = { "Роза красная", "Тюльпан желтый", "Горшок керамический", "Грунт универсальный",
                               "Лейка пластиковая", "Удобрение для роз", "Кашпо подвесное", "Семена газона" };

            for (int i = 1; i <= 8; i++)
            {
                var rnd = new Random();
                data.Add(new ProductDto
                {
                    Id = i,
                    Article = $"ART-{i:D4}",
                    Name = names[rnd.Next(names.Length)],
                    Category = categories[rnd.Next(categories.Length)],
                    Price = rnd.Next(100, 5000),
                    StockQuantity = rnd.Next(0, 100)
                });
            }
            return data;
        }

        private void ShowAddProduct()
        {
            // Проверяем, что список товаров существует
            if (_currentProducts == null)
            {
                _currentProducts = GetTestProducts();
            }

            var dialog = new ProductEditWindow();
            if (dialog.ShowDialog() == true)
            {
                var newProduct = new ProductDto
                {
                    Id = _currentProducts.Count > 0 ? _currentProducts.Max(p => p.Id) + 1 : 1,
                    Article = dialog.ProductArticle,
                    Name = dialog.ProductName,
                    Category = dialog.ProductCategory,
                    Price = dialog.ProductPrice,
                    StockQuantity = dialog.ProductStock
                };
                _currentProducts.Add(newProduct);
                RefreshProducts();
                TxtStatus.Text = $"✅ Товар '{newProduct.Name}' добавлен";
            }
        }

        private void ShowEditProduct()
        {
            var grid = GetCurrentDataGrid();
            if (grid?.SelectedItem == null)
            {
                MessageBox.Show("Сначала выберите товар в таблице", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var selected = (ProductDto)grid.SelectedItem;
            var dialog = new ProductEditWindow(selected);
            if (dialog.ShowDialog() == true)
            {
                var index = _currentProducts.FindIndex(p => p.Id == selected.Id);
                if (index >= 0)
                {
                    _currentProducts[index].Article = dialog.ProductArticle;
                    _currentProducts[index].Name = dialog.ProductName;
                    _currentProducts[index].Category = dialog.ProductCategory;
                    _currentProducts[index].Price = dialog.ProductPrice;
                    _currentProducts[index].StockQuantity = dialog.ProductStock;
                }
                RefreshProducts();
                TxtStatus.Text = $"✅ Товар '{selected.Name}' отредактирован";
            }
        }

        private void DeleteProduct()
        {
            var grid = GetCurrentDataGrid();
            if (grid?.SelectedItem == null)
            {
                MessageBox.Show("Сначала выберите товар в таблице", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var selected = (ProductDto)grid.SelectedItem;
            var result = MessageBox.Show($"Удалить товар '{selected.Name}'?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _currentProducts.Remove(selected);
                RefreshProducts();
                TxtStatus.Text = $"✅ Товар '{selected.Name}' удален";
            }
        }

        private void RefreshProducts()
        {
            var grid = GetCurrentDataGrid();
            if (grid != null)
            {
                grid.ItemsSource = null;
                grid.ItemsSource = _currentProducts;
                TxtStatus.Text = $"✅ Обновлено. Всего: {_currentProducts.Count}";
            }
        }

        private void SearchProducts(string searchText)
        {
            var grid = GetCurrentDataGrid();
            if (_currentProducts == null || grid == null) return;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                grid.ItemsSource = _currentProducts;
                TxtStatus.Text = $"✅ Всего: {_currentProducts.Count}";
            }
            else
            {
                var filtered = _currentProducts.Where(p =>
                    p.Name.ToLower().Contains(searchText.ToLower()) ||
                    p.Article.ToLower().Contains(searchText.ToLower())
                ).ToList();
                grid.ItemsSource = filtered;
                TxtStatus.Text = $"🔍 Найдено: {filtered.Count} из {_currentProducts.Count}";
            }
        }

        // ==================== ВКЛАДКА СКЛАД ====================

        private void LoadWarehouseTab()
        {
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            LoadWarehouseData();

            var summaryPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 15) };

            var totalItems = _warehouseItems.Sum(w => w.CurrentStock);
            var needToBuy = _warehouseItems.Count(w => w.Status == "Требуется закуп");
            var outOfStock = _warehouseItems.Count(w => w.Status == "Отсутствует");

            summaryPanel.Children.Add(CreateSummaryBox("📦 Всего товаров", totalItems.ToString(), "#2196F3"));
            summaryPanel.Children.Add(CreateSummaryBox("⚠️ Требуется закуп", needToBuy.ToString(), "#FF9800"));
            summaryPanel.Children.Add(CreateSummaryBox("❌ Отсутствует", outOfStock.ToString(), "#F44336"));

            Grid.SetRow(summaryPanel, 0);
            grid.Children.Add(summaryPanel);

            var dataGrid = CreateWarehouseDataGrid();
            Grid.SetRow(dataGrid, 1);
            grid.Children.Add(dataGrid);

            TabContentArea.Content = grid;
            dataGrid.ItemsSource = _warehouseItems;
        }

        private Border CreateSummaryBox(string title, string value, string color)
        {
            var brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));

            var border = new Border
            {
                Width = 160,
                Height = 75,
                Margin = new Thickness(5),
                Background = brush,
                CornerRadius = new CornerRadius(8)
            };

            var stack = new StackPanel { VerticalAlignment = VerticalAlignment.Center };
            stack.Children.Add(new TextBlock
            {
                Text = title,
                Foreground = Brushes.White,
                FontSize = 12,
                HorizontalAlignment = HorizontalAlignment.Center
            });
            stack.Children.Add(new TextBlock
            {
                Text = value,
                Foreground = Brushes.White,
                FontSize = 26,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center
            });

            border.Child = stack;
            return border;
        }

        private DataGrid CreateWarehouseDataGrid()
        {
            var grid = new DataGrid
            {
                AutoGenerateColumns = false,
                IsReadOnly = true,
                SelectionMode = DataGridSelectionMode.Single
            };

            grid.Columns.Add(new DataGridTextColumn { Header = "Товар", Binding = new System.Windows.Data.Binding("ProductName"), Width = 250 });
            grid.Columns.Add(new DataGridTextColumn { Header = "На складе", Binding = new System.Windows.Data.Binding("CurrentStock"), Width = 100 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Мин. запас", Binding = new System.Windows.Data.Binding("MinStock"), Width = 100 });

            var statusColumn = new DataGridTextColumn
            {
                Header = "Статус",
                Binding = new System.Windows.Data.Binding("Status"),
                Width = 150
            };

            var style = new Style(typeof(TextBlock));
            style.Setters.Add(new Setter(TextBlock.ForegroundProperty,
                new System.Windows.Data.Binding("StatusColor") { Converter = new StatusColorConverter() }));
            style.Setters.Add(new Setter(TextBlock.FontWeightProperty, FontWeights.Bold));

            statusColumn.ElementStyle = style;
            grid.Columns.Add(statusColumn);

            return grid;
        }

        private void LoadWarehouseData()
        {
            _warehouseItems = new List<WarehouseItemDto>
            {
                new WarehouseItemDto { ProductName = "Роза красная", CurrentStock = 150, MinStock = 50, Status = "Норма" },
                new WarehouseItemDto { ProductName = "Роза белая", CurrentStock = 120, MinStock = 50, Status = "Норма" },
                new WarehouseItemDto { ProductName = "Тюльпан желтый", CurrentStock = 30, MinStock = 100, Status = "Требуется закуп" },
                new WarehouseItemDto { ProductName = "Горшок керамический", CurrentStock = 15, MinStock = 30, Status = "Требуется закуп" },
                new WarehouseItemDto { ProductName = "Грунт универсальный", CurrentStock = 0, MinStock = 20, Status = "Отсутствует" },
                new WarehouseItemDto { ProductName = "Удобрение для роз", CurrentStock = 5, MinStock = 15, Status = "Требуется закуп" },
                new WarehouseItemDto { ProductName = "Лейка пластиковая", CurrentStock = 25, MinStock = 10, Status = "Норма" },
                new WarehouseItemDto { ProductName = "Кашпо подвесное", CurrentStock = 8, MinStock = 10, Status = "Требуется закуп" }
            };
        }

        // ==================== ВКЛАДКА ОТЧЕТЫ ====================

        private void LoadReportsTab()
        {
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var header = new TextBlock
            {
                Text = "📊 Отчеты и аналитика",
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E7D32")),
                Margin = new Thickness(0, 0, 0, 15)
            };
            Grid.SetRow(header, 0);
            grid.Children.Add(header);

            var infoBlock = new TextBlock
            {
                Text = "Для формирования отчетов используйте меню \"Отчеты\" в верхней панели",
                FontSize = 12,
                Foreground = Brushes.Gray,
                Margin = new Thickness(0, 0, 0, 20)
            };
            Grid.SetRow(infoBlock, 1);
            grid.Children.Add(infoBlock);

            // Предпросмотр последних продаж
            var previewBox = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(15),
                BorderBrush = Brushes.LightGray,
                BorderThickness = new Thickness(1)
            };

            var previewStack = new StackPanel();
            previewStack.Children.Add(new TextBlock
            {
                Text = "📋 Последние продажи",
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            });

            if (_salesData == null) _salesData = GetTestSalesData();

            var previewGrid = new DataGrid
            {
                AutoGenerateColumns = false,
                IsReadOnly = true,
                Height = 200,
                Margin = new Thickness(0, 0, 0, 10)
            };
            previewGrid.Columns.Add(new DataGridTextColumn { Header = "Дата", Binding = new System.Windows.Data.Binding("Date") { StringFormat = "dd.MM.yyyy" }, Width = 100 });
            previewGrid.Columns.Add(new DataGridTextColumn { Header = "Товар", Binding = new System.Windows.Data.Binding("ProductName"), Width = 200 });
            previewGrid.Columns.Add(new DataGridTextColumn { Header = "Кол-во", Binding = new System.Windows.Data.Binding("Quantity"), Width = 80 });
            previewGrid.Columns.Add(new DataGridTextColumn { Header = "Сумма", Binding = new System.Windows.Data.Binding("Amount") { StringFormat = "{0:N2} ₽" }, Width = 120 });
            previewGrid.ItemsSource = _salesData.Take(5);
            previewStack.Children.Add(previewGrid);

            var totalText = new TextBlock
            {
                Text = $"💰 Общая выручка за период: {_salesData.Sum(s => s.Amount):N2} ₽",
                FontSize = 13,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Green,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            previewStack.Children.Add(totalText);

            previewBox.Child = previewStack;
            Grid.SetRow(previewBox, 2);
            grid.Children.Add(previewBox);

            TabContentArea.Content = grid;
        }

        private List<SaleDto> GetTestSalesData()
        {
            var data = new List<SaleDto>();
            var rnd = new Random();
            string[] products = { "Роза красная", "Роза белая", "Тюльпан желтый", "Горшок керамический", "Грунт универсальный" };

            for (int i = 0; i < 15; i++)
            {
                data.Add(new SaleDto
                {
                    Date = DateTime.Now.AddDays(-rnd.Next(1, 60)),
                    ProductName = products[rnd.Next(products.Length)],
                    Quantity = rnd.Next(1, 20),
                    Amount = rnd.Next(1000, 20000)
                });
            }
            return data.OrderByDescending(s => s.Date).ToList();
        }
    }
}