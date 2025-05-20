using LibraryManagement.BLL;
using LibraryManagement.DTO;
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
    public class StatisticViewModel : BaseViewModel
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
        private THAMSO _thamso { get; set; }
        public THAMSO thamso
        {
            get
            {
                return _thamso;
            }
            set
            {
                _thamso = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> SearchList { get; set; }
        
        public ObservableCollection<THAMSO> AllStatistics { get; set; }
        public THAMSO StatisticSeleted { get; set; }
        #endregion

        #region Commands
        public ICommand GetCurrentWindowCM { get; set; }
        public ICommand LoadDataStatisticCM { get; set; }
        public ICommand SearchStatisticCM { get; set; }
        public ICommand ResetDataCM { get; set; }
        public ICommand OpenAddStatisticCM { get; set; }
        public ICommand OpenUpdateStatisticCM { get; set; }
        public ICommand AddNewStatisticCM { get; set; }
        public ICommand ViewStatisticCM { get; set; }
        public ICommand UpdateStatisticCM { get; set; }
        public ICommand DeleteStatisticCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        #endregion

        public StatisticViewModel()
        {
            SearchList = new ObservableCollection<string> { "Tên tham số" };
            SearchProperties = SearchList.FirstOrDefault();

            LoadDataStatisticCM = new RelayCommand<THAMSO>((p) => { return true; }, async (p) =>
            {
                try
                {
                    var data = await Task.Run(async () => await ThamSoBLL.Instance.GetThamSo());
                    thamso = data;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
            });
          
          
            OpenUpdateStatisticCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
               
            });
            
            ViewStatisticCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
              
            });
            UpdateStatisticCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                var res = await Task.Run(async () => await ThamSoBLL.Instance.UpdateThamSo(StatisticSeleted));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    LoadDataStatisticCM.Execute(null);
                    p.Close();
                }
            });
           
            CloseWindowCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
        }
    }
}
