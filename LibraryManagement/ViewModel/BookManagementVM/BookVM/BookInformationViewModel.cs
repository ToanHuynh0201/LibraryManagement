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
    public class BookInformationViewModel : BaseViewModel
    {
        public SACH sach { get; set; } = new SACH();
        private int _triGia;
        public int TriGia
        {
            get => _triGia;
            set
            {
                _triGia = value;
                OnPropertyChanged();
                sach.TriGia = value; // Đồng bộ với đối tượng SACH
            }
        }

        public ICommand CloseWindowCM { get; set; }
        public ICommand SaveNewPriceCM { get; set; }
        public BookInformationViewModel() { }
        public BookInformationViewModel(SACH bookSelected)
        {
            sach = bookSelected;
            TriGia = sach.TriGia;
            CloseWindowCM = new RelayCommand<Window>((p) => true, (p) =>
            {
                p.Close();
            });
            SaveNewPriceCM = new RelayCommand<Window>((p) => true, async (p) =>
            {
                sach.TriGia = TriGia;
                var res = await SachBLL.Instance.UpdateGiaSach(sach.id, TriGia);
                MessageBox.Show(res.Item2);
                if(res.Item1)
                {
                    p.Close();
                }
            });
        }
    }
}