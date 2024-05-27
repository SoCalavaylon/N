using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace IT_Solutions.Windows
{
    /// <summary>
    /// Логика взаимодействия для Edit.xaml
    /// </summary>
    public partial class Edit : Window
    {
        private Requests _request;
        RepManageDBEntities1 _entities = new RepManageDBEntities1();

        public Edit(Requests request)
        {
            InitializeComponent();

            _request = request;

            // Заполняем поля формы данными из выбранной записи
            RequestNumberTxt.Text = _request.Request_Number.ToString();
            DatePickerAdded.SelectedDate = _request.Date_Added;
            IssueDescriptionTxt.Text = _request.Issue_Description;
            ComboBoxRequestStatus.SelectedItem = _request.Request_Status;
            ComboBoxAssignedUser.SelectedItem = _request.Users?.Last_Name;
            AdditionalInfoTxt.Text = _request.Additional_Information;

            EquipmentTypeTxt.Text = _request.Equipment?.Equipment_Name;
            ModelTxt.Text = _request.Equipment?.Model;
            NumberTxt.Text = _request.Equipment?.Serial_Number;

            LastNameTxt.Text = _request.Clients?.Last_Name;
            FirstNameTxt.Text = _request.Clients?.First_Name;
            MiddleNameTxt.Text = _request.Clients?.Middle_Name;
            PhoneNumberTxt.Text = _request.Clients?.Phone_Number;
            EmailTxt.Text = _request.Clients?.Email;

            ResponseTxt.Text = _request.Response;
            OrderedPartsTxt.Text = _request.Parts_Ordered;
            UsedMaterialsTxt.Text = _request.Materials_Used;
            WorkStartedDate.SelectedDate = _request.Start_Date;
            WorkEndedDate.SelectedDate = _request.Finish_Date;

            ComboBoxRequestStatus.ItemsSource = _entities.Requests.Select(r => r.Request_Status).Distinct().ToList();

            ComboBoxAssignedUser.ItemsSource = _entities.Users.Select(u => u.Last_Name).ToList();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _request.Equipment = _request.Equipment ?? new Equipment();
                _request.Equipment.Equipment_Name = EquipmentTypeTxt.Text;
                _request.Equipment.Model = ModelTxt.Text;
                _request.Equipment.Serial_Number = NumberTxt.Text;

                _request.Clients = _request.Clients ?? new Clients();
                _request.Clients.Last_Name = LastNameTxt.Text;
                _request.Clients.First_Name = FirstNameTxt.Text;
                _request.Clients.Middle_Name = MiddleNameTxt.Text;
                _request.Clients.Phone_Number = PhoneNumberTxt.Text;
                _request.Clients.Email = EmailTxt.Text;


                _request.Clients.Last_Name = LastNameTxt.Text;
                _request.Clients.First_Name = FirstNameTxt.Text;
                _request.Clients.Middle_Name = MiddleNameTxt.Text;
                _request.Clients.Phone_Number = PhoneNumberTxt.Text;
                _request.Clients.Email = EmailTxt.Text;

                _request.Response = ResponseTxt.Text;
                _request.Parts_Ordered = OrderedPartsTxt.Text;
                _request.Materials_Used = UsedMaterialsTxt.Text;
                _request.Start_Date = WorkStartedDate.SelectedDate;
                _request.Finish_Date = WorkEndedDate.SelectedDate;

                _entities.SaveChanges();
                MessageBox.Show("Запись успешно сохранена.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении записи: " + ex.Message);
            }
        }
    }
}
