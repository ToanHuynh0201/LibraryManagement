using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using LibraryManagement.View;

namespace LibraryManagement.ViewModel
{
    public class CategoryManagementViewModel : BaseViewModel
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
        private object _CategorySelected;
        public object CategorySelected
        {
            get
            {
                return _CategorySelected;
            }
            set
            {
                _CategorySelected = value;
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
        private ObservableCollection<object> _ListCategory;
        public ObservableCollection<object> ListCategory
        {
            get
            {
                return _ListCategory;
            }
            set
            {
                _ListCategory = value;
                OnPropertyChanged();
            }
        }

        private Window mainWindow { get; set; }
        #endregion

        #region Commands
        public ICommand ExitCM { get; set; }
        public ICommand ResetData { get; set; }
        public ICommand SearchData { get; set; }
        public ICommand AddCategoryCM { get; set; }
        public ICommand SaveCM { get; set; }
        public ICommand EditCM { get; set; }
        public ICommand DeleteCategoryCM { get; set; }
        public ICommand GetThisWindowCM { get; set; }
        #endregion

        public CategoryManagementViewModel()
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
            DeleteCategoryCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            SaveCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            EditCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            AddCategoryCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
        }
    }
}
