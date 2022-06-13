using DocumentFormat.OpenXml.Presentation;
using Newtonsoft.Json;
using Schedule.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Schedule
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WindowSchedule : ContentPage, INotifyPropertyChanged
    {
        public WindowSchedule()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModelGroups(frame1, frame2, scroll1, stackLoad, labelOne);
            NavigationPage.SetHasNavigationBar(this, false);
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
        private int idGroups;
        public int IdGroups
        {
            get { return idGroups; }
            set { idGroups = value; }
        }

        private string nameGroups;
        public string NameGroups
        {
            get { return nameGroups; }
            set { nameGroups = value; }
        }

        public List<LessonGroups> _uroki;
        public List<LessonGroups> UROKIs
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
            if (Application.Current.Properties.ContainsKey("groupsName"))
            {
                if (!(String.IsNullOrEmpty(Application.Current.Properties["groupsName"].ToString())))
                {
                    // Вызов POST-запроса
                    SaveVariantGROUPS(new VariantGroups
                    {
                        Dates = this.Dates,
                        IdGroups = Convert.ToInt32(Application.Current.Properties["groupsID"].ToString())
                    }); ;
                    picker1.Title = Application.Current.Properties["groupsName"].ToString();
                    picker1.TitleColor = Color.FromHex("#1F9FE2");
                    idGroups = Convert.ToInt32(Application.Current.Properties["groupsID"].ToString());
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

            if (Dates != null && IdGroups != 0)
            {
                // Вызов POST-запроса
                SaveVariantGROUPS(new VariantGroups
                {
                    Dates = this.Dates,
                    IdGroups = this.IdGroups
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
                Groups groups = selectedItem as Groups;
                IdGroups = groups.Id;
                NameGroups = groups.Naim;

                if (Dates != null && IdGroups != 0)
                {
                    // Вызов POST-запроса
                    SaveVariantGROUPS(new VariantGroups
                    {
                        Dates = this.Dates,
                        IdGroups = this.IdGroups
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
        public async void SaveVariantGROUPS(VariantGroups variant)
        {
            try
            {
                if (variant.IdGroups != 0 && variant.Dates != null)
                {
                    string url = $"http://192.168.1.5:1433/api/Masters/ReceiveResult";
                    HttpClient client = new HttpClient();
                    string jsonData = JsonConvert.SerializeObject(variant);
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    string result = await response.Content.ReadAsStringAsync();
                    var responseUROKI = JsonConvert.DeserializeObject<List<LessonGroups>>(result);
                    UROKIs = new List<LessonGroups>(responseUROKI);
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
                            stackLayoit1Label2.Text = item.FamioTeachers;
                            stackLayoit1Label3.Text = $"Аудитория - {item.AUDITORI}";
                        }
                        if (item.ID == 2)
                        {
                            stackLayoit1Label4.Text = item.NaimUROKI;
                            stackLayoit1Label5.Text = item.FamioTeachers;
                            stackLayoit1Label6.Text = $"Аудитория - {item.AUDITORI}";
                        }
                        if (item.ID == 3)
                        {
                            stackLayoit1Label7.Text = item.NaimUROKI;
                            stackLayoit1Label8.Text = item.FamioTeachers;
                            stackLayoit1Label9.Text = $"Аудитория - {item.AUDITORI}";
                        }
                        if (item.ID == 4)
                        {
                            stackLayoit1Label10.Text = item.NaimUROKI;
                            stackLayoit1Label11.Text = item.FamioTeachers;
                            stackLayoit1Label12.Text = $"Аудитория - {item.AUDITORI}";
                        }
                        if (item.ID == 5)
                        {
                            stackLayoit1Label13.Text = item.NaimUROKI;
                            stackLayoit1Label14.Text = item.FamioTeachers;
                            stackLayoit1Label15.Text = $"Аудитория - {item.AUDITORI}";
                        }
                        if (item.ID == 6)
                        {
                            stackLayoit1Label16.Text = item.NaimUROKI;
                            stackLayoit1Label17.Text = item.FamioTeachers;
                            stackLayoit1Label18.Text = $"Аудитория - {item.AUDITORI}";
                        }
                        if (item.ID == 7)
                        {
                            stackLayoit1Label19.Text = item.NaimUROKI;
                            stackLayoit1Label20.Text = item.FamioTeachers;
                            stackLayoit1Label21.Text = $"Аудитория - {item.AUDITORI}";
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
                if (NameGroups != null && IdGroups != 0)
                {
                    Application.Current.Properties["groupsName"] = NameGroups;
                    Application.Current.Properties["groupsID"] = IdGroups; 
                }
                else
                {
                    await DisplayAlert("Уведомление", $"Элемент из списка не был выбран", "OK");
                }

            }
        }
    }
}