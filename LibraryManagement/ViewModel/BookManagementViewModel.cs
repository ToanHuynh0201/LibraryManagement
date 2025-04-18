using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class BookManagementViewModel : BaseViewModel
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
        private object _BookSeleted;
        public object BookSeleted
        {
            get
            {
                return _BookSeleted;
            }
            set
            {
                _BookSeleted = value;
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
        private string _Author;
        public string Author
        {
            get
            {
                return _Author;
            }
            set
            {
                _Author = value;
                OnPropertyChanged();
            }
        }
        private string _PublishingHouse;
        public string PublishingHouse
        {
            get
            {
                return _PublishingHouse;
            }
            set
            {
                _PublishingHouse = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> SearchList { get; set; }
        private ObservableCollection<object> _ListBooks;
        public ObservableCollection<object> ListBooks
        {
            get
            {
                return _ListBooks;
            }
            set
            {
                _ListBooks = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand LoadDataBookCM { get; set; }
        public ICommand SearchData { get; set; }
        public ICommand ResetData { get; set; }
        public ICommand AddBookCM { get; set; }
        public ICommand ViewBookCM { get; set; }
        public ICommand UpdateBookCM { get; set; }
        public ICommand DeleteBookCM { get; set; }
        #endregion
        public BookManagementViewModel()
        {
            LoadDataBookCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            SearchData = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            ResetData = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            AddBookCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            ViewBookCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            UpdateBookCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            DeleteBookCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
        }
    }
}
