using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace karimovautoservice
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Service _currentService = new Service();
        public AddEditPage(Service SelectedService)
        {
            InitializeComponent();

            if (SelectedService != null)
            {
                _currentService = SelectedService;
            }

            DataContext = _currentService;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            var context = karimov_autoserviceEntities.GetContext();
            if (string.IsNullOrWhiteSpace(_currentService.Title))
            {
                errors.AppendLine("Укажите название услуги");
            }
            else if (context.Service.Any(service => service.Title == _currentService.Title && service.ID != _currentService.ID))
            {
                errors.AppendLine("Уже существует такая услуга");
            }
            if (_currentService.Cost == 0)
            {
                errors.AppendLine("Укажите стоимость услуги");
            }

            if (_currentService.Duration == 0)
                errors.AppendLine("Укажите длительность услуги");
            if (_currentService.Duration > 240)
                errors.AppendLine("Длительность не может быть боьше 240 минут");
            if (_currentService.Duration < 0)
                errors.AppendLine("Длительность не может быть менее 0 минут");

            if (_currentService.Discount < 0 || _currentService.Discount > 100)
                errors.AppendLine("Укажите скидку от 0 до 100");


            string s = _currentService.Discount.ToString();
            if (string.IsNullOrWhiteSpace(s))
            {
                errors.AppendLine("Укажите скидку");
            }
      

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (_currentService.ID == 0)
                karimov_autoserviceEntities.GetContext().Service.Add(_currentService);

            try
            {
                karimov_autoserviceEntities.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                Class1.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
