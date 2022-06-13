using Newtonsoft.Json;
using Schedule.ModelTeachers;
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
    public partial class WindowScheduleTeachers : ContentPage, INotifyPropertyChanged
    {
        public WindowScheduleTeachers()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new MainPageViewModelTeachers(frame1, frame2, scroll1, stackLoad, labelOne);
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
        private int idTeachers;
        public int IdTeachers
        {
            get { return idTeachers; }
            set { idTeachers = value; }
        }

        private string nameTeachers;
        public string NameTeachers
        {
            get { return nameTeachers; }
            set { nameTeachers = value; }
        }

        public List<LessonTeachers> _uroki;
        public List<LessonTeachers> UROKIs
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
            if (Application.Current.Properties.ContainsKey("teachersName"))
            {
                if (!(String.IsNullOrEmpty(Application.Current.Properties["teachersName"].ToString())))
                {
                    // Вызов POST-запроса
                    SaveVariantTeachers(new VariantTeachers
                    {
                        Dates = this.Dates,
                        IdTeachers = Convert.ToInt32(Application.Current.Properties["teachersID"].ToString())
                    }); ;
                    picker1.Title = Application.Current.Properties["teachersName"].ToString();
                    picker1.TitleColor = Color.FromHex("#1F9FE2");
                    IdTeachers = Convert.ToInt32(Application.Current.Properties["teachersID"].ToString());
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

            if (Dates != null && IdTeachers != 0)
            {
                // Вызов POST-запроса
                SaveVariantTeachers(new VariantTeachers
                {
                    Dates = this.Dates,
                    IdTeachers = this.IdTeachers
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
                Teachers teach = selectedItem as Teachers;
                IdTeachers = teach.Id;
                NameTeachers = teach.Name;

                if (Dates != null && IdTeachers != 0)
                {
                    // Вызов POST-запроса
                    SaveVariantTeachers(new VariantTeachers
                    {
                        Dates = this.Dates,
                        IdTeachers = this.IdTeachers
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
        public async void SaveVariantTeachers(VariantTeachers variant)
        {
            try
            {
                if (variant.IdTeachers != 0 && variant.Dates != null)
                {
                    string url = $"http://192.168.1.5:1433/api/Masters/ReceiveResultTeachers";
                    HttpClient client = new HttpClient();
                    string jsonData = JsonConvert.SerializeObject(variant);
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    string result = await response.Content.ReadAsStringAsync();
                    var responseUROKI = JsonConvert.DeserializeObject<List<LessonTeachers>>(result);
                    UROKIs = new List<LessonTeachers>(responseUROKI);
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
                            stackLayoit1Label3.Text = $"Аудитория - {item.AUDITORI}";
                        }
                        if (item.ID == 2)
                        {
                            stackLayoit1Label4.Text = item.NaimUROKI;
                            stackLayoit1Label5.Text = $"Группа - {item.Groups}";
                            stackLayoit1Label6.Text = $"Аудитория - {item.AUDITORI}";
                        }
                        if (item.ID == 3)
                        {
                            stackLayoit1Label7.Text = item.NaimUROKI;
                            stackLayoit1Label8.Text = $"Группа - {item.Groups}";
                            stackLayoit1Label9.Text = $"Аудитория - {item.AUDITORI}";
                        }
                        if (item.ID == 4)
                        {
                            stackLayoit1Label10.Text = item.NaimUROKI;
                            stackLayoit1Label11.Text = $"Группа - {item.Groups}";
                            stackLayoit1Label12.Text = $"Аудитория - {item.AUDITORI}";
                        }
                        if (item.ID == 5)
                        {
                            stackLayoit1Label13.Text = item.NaimUROKI;
                            stackLayoit1Label14.Text = $"Группа - {item.Groups}";
                            stackLayoit1Label15.Text = $"Аудитория - {item.AUDITORI}";
                        }
                        if (item.ID == 6)
                        {
                            stackLayoit1Label16.Text = item.NaimUROKI;
                            stackLayoit1Label17.Text = $"Группа - {item.Groups}";
                            stackLayoit1Label18.Text = $"Аудитория - {item.AUDITORI}";
                        }
                        if (item.ID == 7)
                        {
                            stackLayoit1Label19.Text = item.NaimUROKI;
                            stackLayoit1Label20.Text = $"Группа - {item.Groups}";
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
                if (NameTeachers != null && IdTeachers != 0)
                {
                    Application.Current.Properties["teachersName"] = NameTeachers;
                    Application.Current.Properties["teachersID"] = IdTeachers;
                }
                else
                {
                    await DisplayAlert("Уведомление", $"Элемент из списка не был выбран", "OK");
                }

            }
        }
    }
    }
