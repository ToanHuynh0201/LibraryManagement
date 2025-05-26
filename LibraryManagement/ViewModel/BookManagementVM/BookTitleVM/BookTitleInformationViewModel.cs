using LibraryManagement.BLL;
using LibraryManagement.DTO;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls.Primitives;

namespace LibraryManagement.ViewModel
{
    public class BookTitleInformationViewModel : BaseViewModel
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
        private bool _isEnabled { get; set; } = false;
        public bool isEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnPropertyChanged(nameof(isEnabled));
                }
            }
        }

        private bool _isSearching = true;
        public Action OnSuccess { get; set; }

        public ICommand UpdateBookTitleCM { get; set; }
        public ICommand AddAuthorToListCM { get; set; }
        public ICommand DeleteAuthorFromListCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        public ICommand LoadOtherDataCM { get; set; }
        public ICommand EnableUpdateCM { get; set; }
        public ICommand GetSelectedDataCM { get; set; }
        public BookTitleInformationViewModel() { }
        public BookTitleInformationViewModel(DAUSACH bookTitleSelected)
        {
            GetSelectedDataCM = new RelayCommand<object>((p) => true, (p) =>
            {
                dausach = new DAUSACH
                {
                    id = bookTitleSelected.id,
                    MaDauSach = bookTitleSelected.MaDauSach,
                    TenDauSach = bookTitleSelected.TenDauSach,
                    MaTheLoai = bookTitleSelected.MaTheLoai,
                    TACGIAs = bookTitleSelected.TACGIAs,
                    THELOAI = bookTitleSelected.THELOAI
                };
                OnPropertyChanged(nameof(dausach));
                CategorySelected = bookTitleSelected.THELOAI;
                AuthorBookList = new ObservableCollection<TACGIA>(bookTitleSelected.TACGIAs);
            });
            LoadOtherDataCM = new RelayCommand<object>((p) => true, async (p) =>
            {
                var authorData = await TacGiaBLL.Instance.GetAllTacGia();
                var categoryData = await TheLoaiBLL.Instance.GetAllTheLoai();
                AllCategory = ListCategory = new ObservableCollection<THELOAI>(categoryData);
                AllAuthor = ListAuthor = new ObservableCollection<TACGIA>(authorData);
            });
            UpdateBookTitleCM = new RelayCommand<Window>((p) => true, async (p) =>
            {
                dausach.TACGIAs = new HashSet<TACGIA>(AuthorBookList);
                if (CategorySelected == null) dausach.MaTheLoai = 0;
                else dausach.MaTheLoai = CategorySelected.id;
                var res = await Task.Run(async () => await DauSachBLL.Instance.UpdateDauSach(dausach));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    OnSuccess?.Invoke();
                    p.Close();
                }
            });
            AddAuthorToListCM = new RelayCommand<object>((p) => true, (p) =>
            {
                if (AuthorSelected != null && !AuthorBookList.Any(author => author.id == AuthorSelected.id))
                    AuthorBookList.Add(AuthorSelected);
            });
            DeleteAuthorFromListCM = new RelayCommand<object>((p) => true, (p) =>
            {
                if (SelectedAuthorInList != null)
                    AuthorBookList.Remove(SelectedAuthorInList);
            });
            EnableUpdateCM = new RelayCommand<object>((p) => true, (p) =>
            {
                if(isEnabled)
                {
                    GetSelectedDataCM.Execute(null);
                }
                isEnabled = !isEnabled;
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