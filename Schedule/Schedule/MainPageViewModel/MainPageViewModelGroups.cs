using Newtonsoft.Json;
using Schedule.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace Schedule
{
    public class MainPageViewModelGroups : INotifyPropertyChanged
    {
        public MainPageViewModelGroups(Frame frame1, Frame frame2, ScrollView scroll, StackLayout stack, Label labeOne)
        {
            GetGroups(frame1, frame2, scroll, stack, labeOne);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<Groups> _groups;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Groups> Groupss
        {
            get
            {
                return _groups;
            }
            set
            {
                _groups = value;
                OnPropertyChanged();


            }
        }
        private async void GetGroups(Frame frame1, Frame frame2, ScrollView scroll, StackLayout stack, Label labeOne)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = "http://192.168.1.5:1433/api/Masters/GetGroups";
                    var result = await client.GetStringAsync(uri);
                    var groupsList = JsonConvert.DeserializeObject<List<Groups>>(result);
                    Groupss = new List<Groups>(groupsList);
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
