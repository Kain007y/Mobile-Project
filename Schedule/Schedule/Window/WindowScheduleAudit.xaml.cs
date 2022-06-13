using Newtonsoft.Json;
using Schedule.Model;
using Schedule.ModelAudit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Schedule
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WindowScheduleAudit : ContentPage, INotifyPropertyChanged
    {
        public WindowScheduleAudit()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new MainPageViewModelAudit(frame1, frame2, scroll1, stackLoad, labelOne);
        }

        private string dates;
        public string Dates
        {
            get
            {
                return dates;
            }
            set
            {
                dates = value;
            }
        }
        private int idaudit;
        public int IdAudit
        {
            get { return idaudit; }
            set { idaudit = value; }
        }

        private string nameAdaudit;
        public string NameAudit
        {
            get { return nameAdaudit; }
            set { nameAdaudit = value; }
        }

        public List<LessonAudit> _uroki;
        public List<LessonAudit> UROKIs
        {
            get
            {
                return _uroki;
            }
            set
            {
                _uroki = value;
                OnPropertyChanged();

            }
        }

        protected override void OnAppearing()
        {

            // Вывести дату на Label и сделать день недели начиная с большой буквы
            string strok = DateTime.Now.ToString("dddd");
            strok = char.ToUpper(strok[0]) + strok.Substring(1);
            labelDate.Text = $"{strok}, {DateTime.Now.ToString("M")} {DateTime.Now.ToString("yyy")} г.";
            CheckForSave();
            //MyDatePicker.MinimumDate = DateTime.Now.AddDays(-7);
        }
        private void CheckForSave()
        {
            // Проверка на наличие ключа в словаре
            if (Application.Current.Properties.ContainsKey("auditName"))
            {
                if (!(String.IsNullOrEmpty(Application.Current.Properties["auditName"].ToString())))
                {
                    // Вызов POST-запроса
                    SaveVariantAudit(new VariantAudit
                    {
                        Dates = this.Dates,
                        IdAudit = Convert.ToInt32(Application.Current.Properties["auditID"].ToString())
                    }); ;
                    picker1.Title = Application.Current.Properties["auditName"].ToString();
                    picker1.TitleColor = Color.FromHex("#1F9FE2");
                    IdAudit = Convert.ToInt32(Application.Current.Properties["auditID"].ToString());
                }
            }
        }

        private void MyDatePickers(object sender, EventArgs e)
        {
            MyDatePicker.Focus();
        }

        //  Удаляет верхний Page из стека навигации
        private async void PopMethod(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }


        private void MyDatePickerEvent(object sender, DateChangedEventArgs e)
        {
            // Перевод дня недели в русский формат
            string dayName = CultureInfo.CreateSpecificCulture("ru-RS").DateTimeFormat.GetDayName(e.NewDate.DayOfWeek);
            // Первый символ с большой буквы
            dayName = char.ToUpper(dayName[0]) + dayName.Substring(1);
            labelDate.Text = $"{dayName}, {e.NewDate.ToLongDateString()}";
            Dates = e.NewDate.ToShortDateString();

            if (Dates != null && IdAudit != 0)
            {
                // Вызов POST-запроса
                SaveVariantAudit(new VariantAudit
                {
                    Dates = this.Dates,
                    IdAudit = this.IdAudit
                });
            }

        }

        // Проверка на подключенную сеть
        private void picker1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var load = new MainViewModel();
            if (load.State == "false")
            {
                frame1.IsVisible = false;
                frame2.IsVisible = false;
                scroll1.IsVisible = false;
                stackLoad.IsVisible = true;
                labelOne.Text = "Отсутствует подключённая сеть";
            }
            else
            {
                //Преобразование объекта в тип Groups
                Picker picker = sender as Picker;
                var selectedItem = picker.SelectedItem;
                Audit audit = selectedItem as Audit;
                IdAudit = audit.Id;
                NameAudit = audit.Naim;
                if (Dates != null && IdAudit != 0)
                {
                    // Вызов POST-запроса
                    SaveVariantAudit(new VariantAudit
                    {
                        Dates = this.Dates,
                        IdAudit = this.IdAudit
                    });
                }
            }
        }

        // Проверка на подключенную сеть
        private async void ButtonLoading(object sender, EventArgs e)
        {
            var load = new MainViewModel();
            if (load.State == "true")
            {
                stackActivity.IsVisible = true;
                activityInd.IsRunning = true;
                stackLoad.IsVisible = false;
                await Task.Delay(1000);
                stackActivity.IsVisible = false;
                activityInd.IsRunning = false;
                stackLoad.IsVisible = false;
                frame1.IsVisible = true;
                frame2.IsVisible = true;
                scroll1.IsVisible = true;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        // Обработчик события
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //POST-запрос
        public async void SaveVariantAudit(VariantAudit variant)
        {
            try
            {
                if (variant.IdAudit != 0 && variant.Dates != null)
                {
                    string url = $"http://192.168.1.5:1433/api/Masters/ReceiveResultAudit";
                    HttpClient client = new HttpClient();
                    string jsonData = JsonConvert.SerializeObject(variant);
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    string result = await response.Content.ReadAsStringAsync();
                    var responseUROKI = JsonConvert.DeserializeObject<List<LessonAudit>>(result);
                    UROKIs = new List<LessonAudit>(responseUROKI);
                    stackLayoit1Label1.Text = "";
                    stackLayoit1Label2.Text = "";
                    stackLayoit1Label3.Text = "";
                    stackLayoit1Label4.Text = "";
                    stackLayoit1Label5.Text = "";
                    stackLayoit1Label6.Text = "";
                    stackLayoit1Label7.Text = "";
                    stackLayoit1Label8.Text = "";
                    stackLayoit1Label9.Text = "";
                    stackLayoit1Label10.Text = "";
                    stackLayoit1Label11.Text = "";
                    stackLayoit1Label12.Text = "";
                    stackLayoit1Label13.Text = "";
                    stackLayoit1Label14.Text = "";
                    stackLayoit1Label15.Text = "";
                    stackLayoit1Label16.Text = "";
                    stackLayoit1Label17.Text = "";
                    stackLayoit1Label18.Text = "";
                    stackLayoit1Label19.Text = "";
                    stackLayoit1Label20.Text = "";
                    stackLayoit1Label21.Text = "";
                    foreach (var item in UROKIs)
                    {
                        if (item.ID == 1)
                        {
                            stackLayoit1Label1.Text = item.NaimUROKI;
                            stackLayoit1Label2.Text = $"Группа - {item.Groups}";
                            stackLayoit1Label3.Text = item.FamioTeachers;
                        }
                        if (item.ID == 2)
                        {
                            stackLayoit1Label4.Text = item.NaimUROKI;
                            stackLayoit1Label5.Text = $"Группа - {item.Groups}";
                            stackLayoit1Label6.Text = item.FamioTeachers;
                        }
                        if (item.ID == 3)
                        {
                            stackLayoit1Label7.Text = item.NaimUROKI;
                            stackLayoit1Label8.Text = $"Группа - {item.Groups}";
                            stackLayoit1Label9.Text = item.FamioTeachers;
                        }
                        if (item.ID == 4)
                        {
                            stackLayoit1Label10.Text = item.NaimUROKI;
                            stackLayoit1Label11.Text = $"Группа - {item.Groups}";
                            stackLayoit1Label12.Text = item.FamioTeachers;
                        }
                        if (item.ID == 5)
                        {
                            stackLayoit1Label13.Text = item.NaimUROKI;
                            stackLayoit1Label14.Text = $"Группа - {item.Groups}";
                            stackLayoit1Label15.Text = item.FamioTeachers;
                        }
                        if (item.ID == 6)
                        {
                            stackLayoit1Label16.Text = item.NaimUROKI;
                            stackLayoit1Label17.Text = $"Группа - {item.Groups}";
                            stackLayoit1Label18.Text = item.FamioTeachers;
                        }
                        if (item.ID == 7)
                        {
                            stackLayoit1Label19.Text = item.NaimUROKI;
                            stackLayoit1Label20.Text = $"Группа - {item.Groups}";
                            stackLayoit1Label21.Text = item.FamioTeachers;
                        }
                    }
                }
            }
            catch (Exception)
            {
                frame1.IsVisible = false;
                frame2.IsVisible = false;
                scroll1.IsVisible = false;
                stackLoad.IsVisible = true;
                labelOne.Text = "Failed connection";
            }
        }

        // Сохранение выбранного элемента
        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert("Подтвердить действие", "Вы хотите cохранить элемент из списка?", "Да", "Нет");
            if (result == true)
            {
                //Сохранение данных локально под личными ключами
                if (NameAudit != null && IdAudit != 0)
                {
                    Application.Current.Properties["auditName"] = NameAudit;
                    Application.Current.Properties["auditID"] = IdAudit;
                }
                else
                {
                    await DisplayAlert("Уведомление", $"Элемент из списка не был выбран", "OK");
                }

            }
        }
    }
    
}