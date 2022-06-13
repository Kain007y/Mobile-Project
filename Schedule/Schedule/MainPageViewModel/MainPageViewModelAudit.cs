using Newtonsoft.Json;
using Schedule.ModelAudit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace Schedule
{
    public class MainPageViewModelAudit : INotifyPropertyChanged
    {
        public MainPageViewModelAudit(Frame frame1, Frame frame2, ScrollView scroll, StackLayout stack, Label labeOne)
        {
            GetAudits(frame1, frame2, scroll, stack, labeOne);
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<Audit> _audit;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Audit> Auditt
        {
            get
            {
                return _audit;
            }
            set
            {
                _audit = value;
                OnPropertyChanged();


            }
        }
        private async void GetAudits(Frame frame1, Frame frame2, ScrollView scroll, StackLayout stack, Label labeOne)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = "http://192.168.1.5:1433/api/Masters/GetAudit";
                    var result = await client.GetStringAsync(uri);
                    var groupsList = JsonConvert.DeserializeObject<List<Audit>>(result);
                    Auditt = new List<Audit>(groupsList);
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
