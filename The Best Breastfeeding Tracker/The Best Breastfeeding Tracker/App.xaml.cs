using System;
using The_Best_Breastfeeding_Tracker.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace The_Best_Breastfeeding_Tracker
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
