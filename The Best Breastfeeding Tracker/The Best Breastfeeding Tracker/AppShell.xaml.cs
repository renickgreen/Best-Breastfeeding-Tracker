using Best_Breastfeeding_Tracker.Views;
using System;
using System.Collections.Generic;
using The_Best_Breastfeeding_Tracker.ViewModels;
using The_Best_Breastfeeding_Tracker.Views;
using Xamarin.Forms;

namespace The_Best_Breastfeeding_Tracker
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(EditPage), typeof(EditPage));
        }

    }
}
