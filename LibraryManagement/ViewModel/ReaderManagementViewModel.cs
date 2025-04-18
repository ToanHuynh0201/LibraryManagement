using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class ReaderManagementViewModel : BaseViewModel
    {
        #region Properties
        private string _SearchText;
        public string SearchText
        {
            get 
            { 
                return _SearchText; 
            }
            set
            {
                _SearchText = value;
                OnPropertyChanged();
            }
        }
        private string _SearchProperties;
        public string SearchProperties
        {
            get
            {
                return _SearchProperties;
            }
            set
            {
                _SearchProperties = value;
                OnPropertyChanged();
            }
        }
        private string _ID;
        public string ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
                OnPropertyChanged();
            }
        }
        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }
        private string _Phone;
        public string Phone
        {
            get
            {
                return _Phone;
            }
            set
            {
                _Phone = value;
                OnPropertyChanged();
            }
        }
        private string _Email;
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
                OnPropertyChanged();
            }
        }
        private string _Date;
        public string Date
        {
            get
            {
                return _Date;
            }
            set
            {
                _Date = value;
                OnPropertyChanged();
            }
        }
        private object _ReaderSeleted;
        public object ReaderSeleted
        {
            get
            {
                return _ReaderSeleted;
            }
            set
            {
                _ReaderSeleted = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> SearchList { get; set; }
        private ObservableCollection<object> _ListReaders;
        public ObservableCollection<object> ListReaders
        {
            get
            {
                return _ListReaders;
            }
            set
            {
                _ListReaders = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand LoadDataCustomerCM { get; set; }
        public ICommand SearchData { get; set; }
        public ICommand ResetData { get; set; }
        public ICommand AddCustomerCM { get; set; }
        public ICommand ExportExcelCM { get; set; }
        public ICommand ViewCustomerCM { get; set; }
        public ICommand UpdateCustomerCM { get; set; }
        public ICommand DeleteCustomerCM { get; set; }

        #endregion
        public ReaderManagementViewModel()
        {
            LoadDataCustomerCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            SearchData = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            ResetData = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            AddCustomerCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {

            });
            ExportExcelCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            ViewCustomerCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            UpdateCustomerCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            DeleteCustomerCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
        }
    }
}
