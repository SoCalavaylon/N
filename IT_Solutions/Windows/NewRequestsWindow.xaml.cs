using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Linq;

namespace IT_Solutions.Windows
{
    /// <summary>
    /// Логика взаимодействия для NewRequestsWindow.xaml
    /// </summary>
    public partial class NewRequestsWindow : Window
    {
        RepManageDBEntities1 _entities = new RepManageDBEntities1();
        private readonly List<string> _requestStatuses;
        private List<string> _assignedUsers;

        private string newRequestNumber;

        public NewRequestsWindow()
        {
            InitializeComponent();

            _requestStatuses = new List<string>() { "В ожидании", "В работе", "Выполнено" };
            ComboBoxRequestStatus.ItemsSource = _requestStatuses;
            ComboBoxRequestStatus.SelectedIndex = 0;

            _assignedUsers = _entities.Users.Select(u => u.Last_Name).ToList();
            ComboBoxAssignedUser.ItemsSource = _assignedUsers;

            var users = _entities.Users.ToList();
            var usertList = users.Select(c => $"{c.Last_Name} {c.Name} {c.Middle_Name}").ToList();
            ComboBoxAssignedUser.ItemsSource = usertList;

            int maxRequestNumber = _entities.Requests.Any() ? _entities.Requests.Max(r => r.Request_Number) : 0;
            maxRequestNumber++;
            newRequestNumber = maxRequestNumber.ToString();
            RequestNumberTxt.Text = newRequestNumber;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получение данных из полей формы
                string requestNumber = RequestNumberTxt.Text;
                string issueDescription = IssueDescriptionTxt.Text;
                string additionalInfo = AdditionalInfoTxt.Text;
                string equipmentType = EquipmentTypeTxt.Text;
                string model = ModelTxt.Text;
                string number = NumberTxt.Text;
                string lastName = LastNameTxt.Text;
                string firstName = FirstNameTxt.Text;
                string middleName = MiddleNameTxt.Text;
                string phoneNumber = PhoneNumberTxt.Text;
                string email = EmailTxt.Text;
                string selectedUserName = ComboBoxAssignedUser.SelectedItem.ToString();
                string selectedRequestStatus = ComboBoxRequestStatus.SelectedItem.ToString();

                // Поиск существующего или создание нового клиента
                Clients client = _entities.Clients.FirstOrDefault(c => c.Last_Name == lastName && c.First_Name == firstName && c.Middle_Name == middleName && c.Phone_Number == phoneNumber);
                if (client == null)
                {
                    client = new Clients
                    {
                        Last_Name = lastName,
                        First_Name = firstName,
                        Middle_Name = middleName,
                        Phone_Number = phoneNumber,
                        Email = email
                    };
                    _entities.Clients.Add(client);
                    _entities.SaveChanges(); // Сохранение изменений для получения ID клиента
                }

                // Поиск существующего или создание нового оборудования
                Equipment equipment = _entities.Equipment.FirstOrDefault(eq => eq.Equipment_Name == equipmentType && eq.Model == model && eq.Serial_Number == number);
                if (equipment == null)
                {
                    equipment = new Equipment
                    {
                        Equipment_Name = equipmentType,
                        Model = model,
                        Serial_Number = number
                    };
                    _entities.Equipment.Add(equipment);
                    _entities.SaveChanges(); // Сохранение изменений для получения ID оборудования
                }

                // Получение ID исполнителя
                var assignedUser = _entities.Users.FirstOrDefault(u => u.Last_Name + " " + u.Name + " " + u.Middle_Name == selectedUserName);

                // Создание новой заявки и заполнение данными
                Requests newRequest = new Requests
                {
                    Request_Number = int.Parse(requestNumber),
                    Date_Added = DateTime.Now,
                    Issue_Description = issueDescription,
                    Request_Status = selectedRequestStatus,
                    Additional_Information = additionalInfo,
                    User_ID = assignedUser.User_ID,
                    Client_ID = client.Client_ID,
                    Equipment_ID = equipment.Equipment_ID
                };

                // Добавление новой заявки
                _entities.Requests.Add(newRequest);
                _entities.SaveChanges();

                MessageBox.Show("Заявка успешно создана!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении заявки: {ex.Message}");
            }
        }

    }
}
