using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Schedule
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var load = new MainViewModel();
            if (load.State == "false")
            {
                stackImage.IsVisible = false;
                stackFrame1.IsVisible = false;
                stackFrame2.IsVisible = false;
                stackFrame3.IsVisible = false;
                stackLoad.IsVisible = true;
                labelOne.Text = "Отсутствует подключённая сеть";
            }
            else
            {
                await Navigation.PushAsync(new WindowSchedule());
            }
        }

        private async void ButtonLoading(object sender, EventArgs e)
        {
            var load = new MainViewModel();
            if (load.State == "true")
            {
                stackLoad.IsVisible = false;
                stackActivity.IsVisible = true;
                activityInd.IsRunning = true;
                await Navigation.PushAsync(new WindowSchedule());
                stackImage.IsVisible = true;
                stackFrame1.IsVisible = true;
                stackFrame2.IsVisible = true;
                stackFrame3.IsVisible = true;
                stackActivity.IsVisible = false;
                stackLoad.IsVisible = false;
                activityInd.IsRunning = false;
            }
        }

        private async void Button_SheduleTeachers(object sender, EventArgs e)
        {
            var load = new MainViewModel();
            if (load.State == "false")
            {
                stackImage.IsVisible = false;
                stackFrame1.IsVisible = false;
                stackFrame2.IsVisible = false;
                stackFrame3.IsVisible = false;
                stackLoad.IsVisible = true;
                labelOne.Text = "Отсутствует подключённая сеть";
            }
            else
            {
                await Navigation.PushAsync(new WindowScheduleTeachers());
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var load = new MainViewModel();
            if (load.State == "false")
            {
                stackImage.IsVisible = false;
                stackFrame1.IsVisible = false;
                stackFrame2.IsVisible = false;
                stackFrame3.IsVisible = false;
                stackLoad.IsVisible = true;
                labelOne.Text = "Отсутствует подключённая сеть";
            }
            else
            {
                await Navigation.PushAsync(new WindowScheduleAudit());
            }
        }
    }
}
