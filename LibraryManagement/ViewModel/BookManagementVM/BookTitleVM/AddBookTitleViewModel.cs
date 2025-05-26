using LibraryManagement.BLL;
using LibraryManagement.DTO;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class AddBookTitleViewModel : BaseViewModel
    {
        public DAUSACH dausach { get; set; } = new DAUSACH();
        private string _categoryText { get; set; }
        public string CategoryText
        {
            get => _categoryText;
            set
            {
                if (_categoryText != value)
                {
                    _categoryText = value;
                    OnPropertyChanged(nameof(CategoryText));
                    if (_isSearching) _ = CategorySearch();
                }
            }
        }
        private string _authorText { get; set; }
        public string AuthorText
        {
            get => _authorText;
            set
            {
                if (_authorText != value)
                {
                    _authorText = value;
                    OnPropertyChanged(nameof(AuthorText));
                    if (_isSearching) _ = AuthorSearch();
                }
            }
        }
        public ObservableCollection<THELOAI> AllCategory { get; set; }
        public ObservableCollection<TACGIA> AllAuthor { get; set; }
        private ObservableCollection<THELOAI> _listCategory { get; set; }
        public ObservableCollection<THELOAI> ListCategory
        {
            get => _listCategory;
            set
            {
                if (_listCategory != value)
                {
                    _listCategory = value;
                    OnPropertyChanged(nameof(ListCategory));
                }
            }
        }
        private ObservableCollection<TACGIA> _listAuthor { get; set; }
        public ObservableCollection<TACGIA> ListAuthor
        {
            get => _listAuthor;
            set
            {
                if (_listAuthor != value)
                {
                    _listAuthor = value;
                    OnPropertyChanged(nameof(ListAuthor));
                }
            }
        }
        private THELOAI _categorySelected;
        public THELOAI CategorySelected
        {
            get => _categorySelected;
            set
            {
                _categorySelected = value;
                OnPropertyChanged(nameof(CategorySelected));
                if (_categorySelected != null)
                {
                    _isSearching = false;
                    CategoryText = _categorySelected.DisplayName;
                    _isSearching = true;
                }
            }
        }
        private TACGIA _authorSelected;
        public TACGIA AuthorSelected
        {
            get => _authorSelected;
            set
            {
                _authorSelected = value;
                OnPropertyChanged(nameof(AuthorSelected));
                if (_authorSelected != null)
                {
                    _isSearching = false;
                    AuthorText = _authorSelected.DisplayName;
                    _isSearching = true;
                }
            }
        }
        private ObservableCollection<TACGIA> _authorBookList { get; set; } = new ObservableCollection<TACGIA>();
        public ObservableCollection<TACGIA> AuthorBookList
        {
            get
            {
                return _authorBookList;
            }
            set
            {
                _authorBookList = value;
                OnPropertyChanged(nameof(AuthorBookList));
            }
        }
        public TACGIA SelectedAuthorInList { get; set; }

        private bool _isSearching = true;
        public Action OnSuccess { get; set; }

        public ICommand AddNewBookTitleCM { get; set; }
        public ICommand AddAuthorToListCM { get; set; }
        public ICommand DeleteAuthorFromListCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        public ICommand LoadOtherDataCM { get; set; }
        public AddBookTitleViewModel()
        {
            LoadOtherDataCM = new RelayCommand<Window>((p) => true, async (p) =>
            {
                var authorData = await TacGiaBLL.Instance.GetAllTacGia();
                var categoryData = await TheLoaiBLL.Instance.GetAllTheLoai();
                AllCategory = ListCategory = new ObservableCollection<THELOAI>(categoryData);
                AllAuthor = ListAuthor = new ObservableCollection<TACGIA>(authorData);
            });
            AddNewBookTitleCM = new RelayCommand<Window>((p) => true, async (p) =>
            {
                dausach.TACGIAs = new HashSet<TACGIA>(AuthorBookList);
                if (CategorySelected == null) dausach.MaTheLoai = 0;
                else dausach.MaTheLoai = CategorySelected.id;
                var res = await Task.Run(async () => await DauSachBLL.Instance.AddDauSach(dausach));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    OnSuccess?.Invoke();
                    p.Close();
                }
            });
            AddAuthorToListCM = new RelayCommand<object>((p) => true, (p) =>
            {
                if (AuthorSelected != null && !AuthorBookList.Any(author => author == AuthorSelected))
                    AuthorBookList.Add(AuthorSelected);
            });
            DeleteAuthorFromListCM = new RelayCommand<object>((p) => true, (p) =>
            {
                if (SelectedAuthorInList != null)
                    AuthorBookList.Remove(SelectedAuthorInList);
            });
            CloseWindowCM = new RelayCommand<Window>((p) => true, (p) =>
            {
                p.Close();
            });
        }
        private async Task CategorySearch()
        {
            var res = await Task.Run(async () => await TheLoaiBLL.Instance.GetTheLoaiByTen(CategoryText));
            ListCategory = new ObservableCollection<THELOAI>(res);
        }
        private async Task AuthorSearch()
        {
            var res = await Task.Run(async () => await TacGiaBLL.Instance.GetTacGiaByTen(AuthorText));
            ListAuthor = new ObservableCollection<TACGIA>(res);
        }
    }
}