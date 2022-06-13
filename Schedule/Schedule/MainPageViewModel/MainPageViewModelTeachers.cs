using Newtonsoft.Json;
using Schedule.ModelTeachers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace Schedule
{
    public class MainPageViewModelTeachers : INotifyPropertyChanged
    {
        public MainPageViewModelTeachers(Frame frame1, Frame frame2, ScrollView scroll, StackLayout stack, Label labeOne)
        {
            GetTeachers(frame1, frame2, scroll, stack, labeOne);
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<Teachers> _teachers;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Teachers> Teacherss
        {
            get
            {
                return _teachers;
            }
            set
            {
                _teachers = value;
                OnPropertyChanged();


            }
        }
        private async void GetTeachers(Frame frame1, Frame frame2, ScrollView scroll, StackLayout stack, Label labeOne)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = "http://192.168.1.5:1433/api/Masters/GetTeachers";
                    var result = await client.GetStringAsync(uri);
                    var groupsList = JsonConvert.DeserializeObject<List<Teachers>>(result);
                    Teacherss = new List<Teachers>(groupsList);
                }
            }
            catch (Exception)
            {
                frame1.IsVisible = false;
                frame2.IsVisible = false;
                scroll.IsVisible = false;
                stack.IsVisible = true;
                labeOne.Text = "Failed connection";
            }
        }
    }
}
