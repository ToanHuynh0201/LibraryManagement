using LibraryManagement.BLL;
using LibraryManagement.DAL;
using LibraryManagement.DTO;
using LibraryManagement.View;
using LibraryManagement.View.Reader;
using MaterialDesignThemes.Wpf;
using Microsoft.Xaml.Behaviors.Core;
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
    public class ReaderViewModel : BaseViewModel
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
        private DOCGIA _docgia { get; set; }
        public DOCGIA docgia
        {
            get
            {
                return _docgia;
            }
            set
            {
                _docgia = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> SearchList { get; set; }
        private ObservableCollection<DOCGIA> _ListReaders;
        public ObservableCollection<DOCGIA> ListReaders
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
        public ObservableCollection<DOCGIA> AllReaders { get; set; }
        public DOCGIA ReaderSeleted { get; set; }
        #endregion

        #region Commands
        public ICommand GetCurrentWindowCM { get; set; }
        public ICommand LoadDataReaderCM { get; set; }
        public ICommand SearchReaderCM { get; set; }
        public ICommand ResetDataCM { get; set; }
        public ICommand OpenAddReaderCM { get; set; }
        public ICommand OpenUpdateReaderCM { get; set; }
        public ICommand AddNewReaderCM { get; set; }
        public ICommand ViewReaderCM { get; set; }
        public ICommand UpdateReaderCM { get; set; }
        public ICommand DeleteReaderCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        #endregion
        public ReaderViewModel()
        {
            SearchList = new ObservableCollection<string> { "Tên độc giả" };
            SearchProperties = SearchList.FirstOrDefault();

            LoadDataReaderCM = new RelayCommand<DOCGIA>((p) => { return true; }, async (p) =>
            {
                try
                {
                    var data = await Task.Run(async () => await DocGiaBLL.Instance.GetAllDocGia());
                    ListReaders = new ObservableCollection<DOCGIA>(data);
                    AllReaders = new ObservableCollection<DOCGIA>(data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
            });
            SearchReaderCM = new RelayCommand<object>((p) => true, async (p) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(SearchText))
                    {
                        ListReaders.Clear();
                        ListReaders = AllReaders;
                    }
                    else
                    {
                        if (SearchProperties == "Tên độc giả")
                        {
                            var res = await Task.Run(async () => await DocGiaBLL.Instance.GetDocGiaByTenDG(SearchText));
                            ListReaders = new ObservableCollection<DOCGIA>(res);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message);
                }
            });
            ResetDataCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                SearchText = "";
                ListReaders = new ObservableCollection<DOCGIA>(AllReaders);
            });
            OpenAddReaderCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                docgia = new DOCGIA()
                {
                    NgayLapThe = DateTime.Now,
                    NgaySinhDG = DateTime.Now,
                    MaLoaiDG = 1
                };
                var w1 = new AddReaderWindow();
                w1.ShowDialog();
            });
            OpenUpdateReaderCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                Window w1 = new EditReaderInformationWindow();
                w1.ShowDialog();
            });
            AddNewReaderCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                var res = await Task.Run(async () => await DocGiaBLL.Instance.AddDocGia(docgia));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    LoadDataReaderCM.Execute(null);
                    p.Close();
                }
            });
            ViewReaderCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Window w1 = new ReaderInformationWindow();
                w1.ShowDialog();
            });
            UpdateReaderCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                var res = await Task.Run(async () => await DocGiaBLL.Instance.UpdateDocGia(ReaderSeleted));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    LoadDataReaderCM.Execute(null);
                    p.Close();
                }
            });
            DeleteReaderCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                var result = MessageBox.Show("Bạn có chắc muốn xóa độc giả không?","Xác nhận xóa",
                                          MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(result == MessageBoxResult.Yes)
                {
                    var res = await Task.Run(async () => await DocGiaBLL.Instance.DeleteDocGia(ReaderSeleted.id));
                    LoadDataReaderCM.Execute(null);
                    MessageBox.Show(res.Item2);
                }
            });
            CloseWindowCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
        }
    }
}
