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
using LibraryManagement.DTO;
using LibraryManagement.BLL;
using LibraryManagement.View.Category;

namespace LibraryManagement.ViewModel
{
    public class CategoryViewModel : BaseViewModel
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
        private THELOAI _theloai { get; set; }
        public THELOAI theloai
        {
            get
            {
                return _theloai;
            }
            set
            {
                _theloai = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> SearchList { get; set; }
        private ObservableCollection<THELOAI> _ListCategorys;
        public ObservableCollection<THELOAI> ListCategorys
        {
            get
            {
                return _ListCategorys;
            }
            set
            {
                _ListCategorys = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<THELOAI> AllCategorys { get; set; }
        public THELOAI CategorySeleted { get; set; }
        #endregion

        #region Commands
        public ICommand GetCurrentWindowCM { get; set; }
        public ICommand LoadDataCategoryCM { get; set; }
        public ICommand SearchCategoryCM { get; set; }
        public ICommand ResetDataCM { get; set; }
        public ICommand OpenAddCategoryCM { get; set; }
        public ICommand OpenUpdateCategoryCM { get; set; }
        public ICommand AddNewCategoryCM { get; set; }
        public ICommand ViewCategoryCM { get; set; }
        public ICommand UpdateCategoryCM { get; set; }
        public ICommand DeleteCategoryCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        #endregion
        public CategoryViewModel()
        {
            SearchList = new ObservableCollection<string> { "Tên thể loại" };
            SearchProperties = SearchList.FirstOrDefault();

            LoadDataCategoryCM = new RelayCommand<THELOAI>((p) => { return true; }, async (p) =>
            {
                try
                {
                    var data = await Task.Run(async () => await TheLoaiBLL.Instance.GetAllTheLoai());
                    ListCategorys = new ObservableCollection<THELOAI>(data);
                    AllCategorys = new ObservableCollection<THELOAI>(data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
            });
            SearchCategoryCM = new RelayCommand<object>((p) => true, async (p) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(SearchText))
                    {
                        ListCategorys.Clear();
                        ListCategorys = AllCategorys;
                    }
                    else
                    {
                        if (SearchProperties == "Tên thể loại")
                        {
                            var res = await Task.Run(async () => await TheLoaiBLL.Instance.GetTheLoaiByTen(SearchText));
                            ListCategorys = new ObservableCollection<THELOAI>(res);
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
                ListCategorys = new ObservableCollection<THELOAI>(AllCategorys);
            });
            OpenAddCategoryCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                theloai = new THELOAI();
                var w1 = new AddCategoryWindow();
                w1.ShowDialog();
            });
            OpenUpdateCategoryCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                Window w1 = new EditCategoryInformationWindow();
                w1.ShowDialog();
            });
            AddNewCategoryCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                var res = await Task.Run(async () => await TheLoaiBLL.Instance.AddTheLoai(theloai));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    LoadDataCategoryCM.Execute(null);
                    p.Close();
                }
            });
            ViewCategoryCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Window w1 = new CategoryInformationWindow();
                w1.ShowDialog();
            });
            UpdateCategoryCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                var res = await Task.Run(async () => await TheLoaiBLL.Instance.UpdateTheLoai(CategorySeleted));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    LoadDataCategoryCM.Execute(null);
                    p.Close();
                }
            });
            DeleteCategoryCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                var result = MessageBox.Show("Bạn có chắc muốn xóa thể loại này không?", "Xác nhận xóa",
                                          MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var res = await Task.Run(async () => await TheLoaiBLL.Instance.DeleteTheLoai(CategorySeleted.id));
                    LoadDataCategoryCM.Execute(null);
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