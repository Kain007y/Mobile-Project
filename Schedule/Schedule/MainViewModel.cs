using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
namespace Schedule
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string state;
        public string State
        {
            get { return state; }
            set
            {
                state = value;
                OnPropertyChanged(state);
            }
        }
        public MainViewModel()
        {
            CheckWifiOnStart();
            CheckWifiContinuously();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void CheckWifiOnStart()
        {
            State = CrossConnectivity.Current.IsConnected ? "true" : "false";
        }

        public void CheckWifiContinuously()
        {
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                State = args.IsConnected ? "true" : "false";
            };
        }

    }
}
