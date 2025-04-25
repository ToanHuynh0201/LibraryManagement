using LibraryManagement.View;
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
    public class AuthorManagementViewModel : BaseViewModel
    {
        #region Properties
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
        private object _AuthorSeleted;
        public object AuthorSeleted
        {
            get
            {
                return _AuthorSeleted;
            }
            set
            {
                _AuthorSeleted = value;
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
        private string _DetailID;
        public string DetailID
        {
            get
            {
                return _DetailID;
            }
            set
            {
                _DetailID = value;
                OnPropertyChanged();
            }
        }
        private string _DetailName;
        public string DetailName
        {
            get
            {
                return _DetailName;
            }
            set
            {
                _DetailName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> SearchList { get; set; }
        private ObservableCollection<object> _ListAuthors;
        public ObservableCollection<object> ListAuthors
        {
            get
            {
                return _ListAuthors;
            }
            set
            {
                _ListAuthors = value;
                OnPropertyChanged();
            }
        }

        private Window mainWindow { get; set; }
        #endregion

        #region Commands
        public ICommand ExitCM { get; set; }
        public ICommand ResetData { get; set; }
        public ICommand SearchData { get; set; }
        public ICommand AddAuthorCM { get; set; }
        public ICommand DeleteAuthorCM { get; set; }
        public ICommand EditCM { get; set; }
        public ICommand ViewDetailCM { get; set; }
        public ICommand SaveCM { get; set; }
        public ICommand GetThisWindowCM { get; set; }
        #endregion

        public AuthorManagementViewModel()
        {
            GetThisWindowCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                mainWindow = p;
            });
            ExitCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                mainWindow.Close();
            });
            ResetData = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            SearchData = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            DeleteAuthorCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            SaveCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            EditCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            ViewDetailCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            AddAuthorCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddAuthorWindow addAuthorWindow = new AddAuthorWindow();
                addAuthorWindow.ShowDialog();
            });
        }

    }
}
