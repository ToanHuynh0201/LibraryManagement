using LibraryManagement.BLL;
using LibraryManagement.DTO;
using LibraryManagement.View.BookTitle;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class BookTitleViewModel : BaseViewModel
    {
        public ObservableCollection<string> SearchList { get; set; }
        public string SearchText { get; set; }
        public string SearchProperties { get; set; }

        public ObservableCollection<DAUSACH> ListBookTitles { get; set; }
        public ObservableCollection<DAUSACH> AllBookTitles { get; set; }
        public DAUSACH BookTitleSelected { get; set; }

        public ICommand LoadDataBookTitleCM { get; set; }
        public ICommand LoadOtherDataCM { get; set; }
        public ICommand SearchBookTitleCM { get; set; }
        public ICommand OpenAddBookTitleCM { get; set; }
        public ICommand OpenUpdateBookTitleCM { get; set; }

        public BookTitleViewModel()
        {
            SearchList = new ObservableCollection<string> { "Tên đầu sách" };
            SearchProperties = SearchList.FirstOrDefault();
            LoadDataBookTitleCM = new RelayCommand<object>((p) => true, async (p) =>
            {
                var data = await DauSachBLL.Instance.GetAllDauSach();
                ListBookTitles = new ObservableCollection<DAUSACH>(data);
                AllBookTitles = new ObservableCollection<DAUSACH>(data);
                OnPropertyChanged(nameof(ListBookTitles));
            });

            SearchBookTitleCM = new RelayCommand<object>((p) => true, async (p) =>
            {
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    ListBookTitles = AllBookTitles;
                }
                else
                {
                    var res = new List<DAUSACH>();
                    if(SearchProperties == "Tên đầu sách") res = await DauSachBLL.Instance.GetDauSachByTen(SearchText);
                    ListBookTitles = new ObservableCollection<DAUSACH>(res);
                }
                OnPropertyChanged(nameof(ListBookTitles));
            });

            OpenAddBookTitleCM = new RelayCommand<object>((p) => true, (p) =>
            {
                var vm1 = new AddBookTitleViewModel();
                vm1.OnSuccess = () => LoadDataBookTitleCM.Execute(null);
                var w1 = new AddBookTitleWindow { DataContext = vm1 };
                w1.ShowDialog();
            });

            OpenUpdateBookTitleCM = new RelayCommand<object>((p) => BookTitleSelected != null, (p) =>
            {
                var vm1 = new BookTitleInformationViewModel(BookTitleSelected);
                vm1.OnSuccess = () => LoadDataBookTitleCM.Execute(null);
                var w1 = new BookTitleInformtationWindow() { DataContext = vm1 };
                w1.ShowDialog();
            });
        }
    }
}