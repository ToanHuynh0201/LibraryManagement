using LibraryManagement.BLL;
using LibraryManagement.DTO;
using LibraryManagement.View;
using LibraryManagement.View.Author;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class AuthorViewModel : BaseViewModel
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
        private TACGIA _tacgia { get; set; }
        public TACGIA tacgia
        {
            get
            {
                return _tacgia;
            }
            set
            {
                _tacgia = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> SearchList { get; set; }
        private ObservableCollection<TACGIA> _ListAuthors;
        public ObservableCollection<TACGIA> ListAuthors
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
        public ObservableCollection<TACGIA> AllAuthors { get; set; }
        private TACGIA _authorSelected;
        public TACGIA AuthorSelected
        {
            get => _authorSelected;
            set
            {
                _authorSelected = value;
                AuthorEdit = value != null ? new TACGIA
                {
                    MaTG = value.MaTG,
                    TenTG = value.TenTG
                } : null;
                OnPropertyChanged(nameof(AuthorSelected)); 
            }
        }

        private TACGIA _authorEdit;
        public TACGIA AuthorEdit
        {
            get => _authorEdit;
            set
            {
                _authorEdit = value;
                OnPropertyChanged(nameof(AuthorEdit));
            }
        }
        #endregion

        #region Commands
        public ICommand GetCurrentWindowCM { get; set; }
        public ICommand LoadDataAuthorCM { get; set; }
        public ICommand SearchAuthorCM { get; set; }
        public ICommand ResetDataCM { get; set; }
        public ICommand OpenAddAuthorCM { get; set; }
        public ICommand OpenUpdateAuthorCM { get; set; }
        public ICommand AddNewAuthorCM { get; set; }
        public ICommand ViewAuthorCM { get; set; }
        public ICommand UpdateAuthorCM { get; set; }
        public ICommand DeleteAuthorCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        #endregion

        public AuthorViewModel()
        {
            SearchList = new ObservableCollection<string> { "Tên tác giả" };
            SearchProperties = SearchList.FirstOrDefault();

            LoadDataAuthorCM = new RelayCommand<TACGIA>((p) => { return true; }, async (p) =>
            {
                try
                {
                    var data = await Task.Run(async () => await TacGiaBLL.Instance.GetAllTacGia());
                    ListAuthors = new ObservableCollection<TACGIA>(data);
                    AllAuthors = new ObservableCollection<TACGIA>(data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
            });
            SearchAuthorCM = new RelayCommand<object>((p) => true, async (p) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(SearchText))
                    {
                        ListAuthors.Clear();
                        ListAuthors = AllAuthors;
                    }
                    else
                    {
                        if (SearchProperties == "Tên tác giả")
                        {
                            var res = await Task.Run(async () => await TacGiaBLL.Instance.GetTacGiaByTen(SearchText));
                            ListAuthors = new ObservableCollection<TACGIA>(res);
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
                ListAuthors = new ObservableCollection<TACGIA>(AllAuthors);
            });
            OpenAddAuthorCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                tacgia = new TACGIA();
                var w1 = new AddAuthorWindow();
                w1.ShowDialog();
            });
            OpenUpdateAuthorCM = new RelayCommand<TextBox>((p) => { return true; }, (p) =>
            {
                if (p.IsEnabled) AuthorSelected = null;
                p.IsEnabled = !(p.IsEnabled);
            });
            AddNewAuthorCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                var res = await Task.Run(async () => await TacGiaBLL.Instance.AddTacGia(tacgia));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    LoadDataAuthorCM.Execute(null);
                    p.Close();
                }
            });
            UpdateAuthorCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                AuthorSelected.TenTG = AuthorEdit.TenTG;
                var res = await Task.Run(async () => await TacGiaBLL.Instance.UpdateTacGia(AuthorSelected));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    LoadDataAuthorCM.Execute(null);
                }
            });
            DeleteAuthorCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                var result = MessageBox.Show("Bạn có chắc muốn xóa tác giả này không?", "Xác nhận xóa",
                                            MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var res = await Task.Run(async () => await TacGiaBLL.Instance.DeleteTacGia(AuthorSelected.id));
                    LoadDataAuthorCM.Execute(null);
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
