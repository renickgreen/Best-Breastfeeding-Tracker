using The_Best_Breastfeeding_Tracker.Models;
using The_Best_Breastfeeding_Tracker.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Best_Breastfeeding_Tracker.Views;

namespace The_Best_Breastfeeding_Tracker.ViewModels
{
    class LogPageModel : BindableObject
    {
        private string _breastText;
        public string BreastText
        {
            get => _breastText;
            set
            {
                _breastText = value;
                OnPropertyChanged(nameof(BreastText));
            }
        }
        private string _timeFed;
        public string TimeFed
        {
            get => _timeFed;
            set
            {
                _timeFed = value;
                OnPropertyChanged(nameof(TimeFed));
            }
        }
        private bool _hadDiaperChange;
        public bool HadDiaperChange
        {
            get => _hadDiaperChange;
            set
            {
                _hadDiaperChange = value;
                OnPropertyChanged(nameof(HadDiaperChange));
            }
        }
        private bool _wasFed;
        public bool WasFed
        {
            get => _wasFed;
            set
            {
                _wasFed = value;
                OnPropertyChanged(nameof(WasFed));
            }
        }
        private FeedingRecord _selectedItem;
        public FeedingRecord SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        private ICommand _appearingCommand;
        public ICommand AppearingCommand
        {
            get => _appearingCommand;
            set
            {
                _appearingCommand = value;
                OnPropertyChanged(nameof(AppearingCommand));
            }
        }

        private ICommand _selectBreastCommand;
        public ICommand SelectBreastCommand
        {
            get => _selectBreastCommand;
            set
            {
                _selectBreastCommand = value;
                OnPropertyChanged(nameof(SelectBreastCommand));
            }
        }
        private ICommand _saveFeedingCommand;
        public ICommand SaveFeedingCommand
        {
            get => _saveFeedingCommand;
            set
            {
                _saveFeedingCommand = value;
                OnPropertyChanged(nameof(SaveFeedingCommand));
            }
        }
        private ICommand _hadDiaperChangeCommand;
        public ICommand HadDiaperChangeCommand
        {
            get => _hadDiaperChangeCommand;
            set
            {
                _hadDiaperChangeCommand = value;
                OnPropertyChanged(nameof(HadDiaperChangeCommand));
            }
        }
        private ICommand _wasFedCommand;
        public ICommand WasFedCommand
        {
            get => _wasFedCommand;
            set
            {
                _wasFedCommand = value;
                OnPropertyChanged(nameof(WasFedCommand));
            }
        }
        private ICommand _onDeleteCommand;
        public ICommand OnDeleteCommand
        {
            get => _onDeleteCommand;
            set
            {
                _onDeleteCommand = value;
                OnPropertyChanged(nameof(OnDeleteCommand));
            }
        }
        private ICommand _selectCommand;
        public ICommand SelectCommand
        {
            get => _selectCommand;
            set
            {
                _selectCommand = value;
                OnPropertyChanged(nameof(SelectCommand));
            }
        }
        private ObservableCollection<FeedingRecord> _records;
        public ObservableCollection<FeedingRecord> Records
        {
            get => _records;
            set
            {
                _records = value;
                OnPropertyChanged(nameof(Records));
            }
        }

        public LogPageModel()
        {
            Records = new ObservableCollection<FeedingRecord>();
            BreastText = "Left";
            WasFed = true;
            AppearingCommand = new Command(OnAppearingCommand);
            SelectCommand = new Command<FeedingRecord>(OnSelectCommand);
            SelectBreastCommand = new Command(OnSelectBreastAction);
            SaveFeedingCommand = new Command(OnSaveFeedingAction);
            HadDiaperChangeCommand = new Command(OnHadDiaperChangeAction);
            WasFedCommand = new Command(OnWasFedCommand);
            OnDeleteCommand = new Command<FeedingRecord>(OnDelete);
        }

        private void OnAppearingCommand(object obj)
        {
            GetRecords();
        }

        private async void OnSelectCommand(FeedingRecord item)
        {
            await Shell.Current.GoToAsync($"{nameof(EditPage)}?ItemId={item.ID}");
            SelectedItem = null;
        }

        private void OnWasFedCommand(object obj)
        {
            WasFed = true;
            HadDiaperChange = false;
            BreastText = "Left";
        }

        private void OnHadDiaperChangeAction(object obj)
        {
            HadDiaperChange = true;
            WasFed = false;
            BreastText = "Pee";
            TimeFed = "";
        }

        private async void GetRecords()
        {
            Records.Clear();
            FeedingRecordDatabase db = await FeedingRecordDatabase.Instance;
            var records = await db.GetItemsDAsync();
            foreach (FeedingRecord r in records)
            {
                if (r.Breast == "Left") r.ColorCode = Color.LightSkyBlue;
                if (r.Breast == "Right") r.ColorCode = Color.LightPink;
                if (r.Breast == "Both") r.ColorCode = Color.LightGreen;
                if (r.Breast == "Pee") r.ColorCode = Color.LightGoldenrodYellow;
                if (r.Breast == "Poop") r.ColorCode = Color.DarkGoldenrod;
                if (r.Breast == "Pee and Poop") r.ColorCode = Color.Goldenrod;
                Records.Add(r);
            }
        }
        public async void OnDelete(FeedingRecord item)
        {
            FeedingRecordDatabase db = await FeedingRecordDatabase.Instance;
            await db.DeleteItemAsync(item);
            GetRecords();
        }
        private async void OnSaveFeedingAction(object obj)
        {
            if (string.IsNullOrWhiteSpace(TimeFed) && WasFed == true)
            {
                return;
            }
            var item = new FeedingRecord(BreastText, TimeFed, DateTime.Now);
            Records.Insert(0, item);
            TimeFed = "";
            FeedingRecordDatabase db = await FeedingRecordDatabase.Instance;
            await db.SaveItemAsync(item);
            GetRecords();
        }

        private void OnSelectBreastAction(object obj)
        {
            if (WasFed)
            {
                switch (BreastText)
                {
                    case "Left":
                        BreastText = "Right";
                        break;
                    case "Right":
                        BreastText = "Both";
                        break;
                    default:
                        BreastText = "Left";
                        break;
                }
            }
            else
            {
                switch (BreastText)
                {
                    case "Pee":
                        BreastText = "Poop";
                        break;
                    case "Poop":
                        BreastText = "Pee and Poop";
                        break;
                    default:
                        BreastText = "Pee";
                        break;
                }
            }
        }
    }
}
