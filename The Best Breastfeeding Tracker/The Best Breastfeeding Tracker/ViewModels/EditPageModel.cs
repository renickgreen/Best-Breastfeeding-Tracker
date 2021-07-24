using System;
using System.Collections.Generic;
using System.Text;
using The_Best_Breastfeeding_Tracker.ViewModels;
using Xamarin.Forms;
using The_Best_Breastfeeding_Tracker.Services;
using System.Windows.Input;
using The_Best_Breastfeeding_Tracker.Models;

namespace Best_Breastfeeding_Tracker.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class EditPageModel : BaseViewModel
    {
        private int _itemId;
        public int ItemId
        {
            get => _itemId;
            set
            {
                SetProperty(ref _itemId, value);
                LoadItem();
            }
        }
        private string _breast;
        public string Breast
        {
            get => _breast;
            set => SetProperty(ref _breast, value);
        }
        private string _timeFed;
        public string TimeFed
        {
            get => _timeFed;
            set => SetProperty(ref _timeFed, value);
        }
        private FeedingRecord _editItem;
        public FeedingRecord EditItem
        {
            get => _editItem;
            set => SetProperty(ref _editItem, value);
        }
        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }
        private TimeSpan _time;
        public TimeSpan Time
        {
            get => _time;
            set => SetProperty(ref _time, value);
        }
        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get => _saveCommand;
            set => SetProperty(ref _saveCommand, value);
        }
        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get => _deleteCommand;
            set => SetProperty(ref _deleteCommand, value);
        }
        public EditPageModel() 
        {
            SaveCommand = new Command(OnSaveCommand);
            DeleteCommand = new Command<FeedingRecord>(OnDeleteCommand);
        }

        private async void OnSaveCommand(object obj)
        {
            if (!Validate())
            {
                await Shell.Current.DisplayAlert("Form Incorrect", "Cannot save", "Ok");
            }
            else
            {
                var item = new FeedingRecord()
                {
                    ID = ItemId,
                    Breast = Breast,
                    Minutes = "",
                    Date = new DateTime(Date.Year, Date.Month, Date.Day, Time.Hours, Time.Minutes, 0)
                };
                if(item.Breast == "Left" || item.Breast == "Right" || item.Breast == "Both")
                {
                    item.Minutes = TimeFed + " Minutes";
                }
                FeedingRecordDatabase db = await FeedingRecordDatabase.Instance;
                await db.SaveItemAsync(item);
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void OnDeleteCommand(FeedingRecord item)
        {
           bool answer = await Shell.Current.DisplayAlert("Warning",
                "Do you want to Permanently Delete this record?", "Yes, Delete", "Cancel");
            if (answer)
            {
                FeedingRecordDatabase db = await FeedingRecordDatabase.Instance;
                await db.DeleteItemAsync(EditItem);
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void LoadItem()
        {
            FeedingRecordDatabase db = await FeedingRecordDatabase.Instance;
            var item = await db.GetItemAsync(ItemId);
            Breast = item.Breast;
            switch (Breast)
            {
                case "Left":
                case "Right":
                case "Both":
                    TimeFed = item.Minutes.Substring(0, item.Minutes.LastIndexOf(" "));
                    break;
                default:
                    TimeFed = "";
                    break;
            }
            
            Date = item.Date;
            Time = item.Date.TimeOfDay;
            EditItem = item;
        }
        private bool Validate()
        {

            if(Breast == null || Date == null || Time == null ||
                (string.IsNullOrWhiteSpace(TimeFed) && Breast == "Left" || Breast == "Right" || Breast == "Both"))
            {
                return false;
            }
            return true;
        }
    }
}
