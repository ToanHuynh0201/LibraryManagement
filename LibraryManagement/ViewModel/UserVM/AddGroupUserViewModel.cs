using LibraryManagement.BLL;
using LibraryManagement.DAL;
using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class AddGroupUserViewModel : BaseViewModel
    {
        private NHOMNGUOIDUNG nhomnguoidung { get; set; } = new NHOMNGUOIDUNG();
        public NHOMNGUOIDUNG NhomNguoiDung
        {
            get
            {
                return nhomnguoidung;
            }
            set
            {
                if (nhomnguoidung != value)
                    nhomnguoidung = value;
                OnPropertyChanged(nameof(NHOMNGUOIDUNG));
            }
        }
        public ObservableCollection<CHUCNANG> dsChucNang { get; set; }

        public ICommand AddGroupUserCM { get; set; }
        public ICommand CloseWindowCM { get; set; }

        public Action OnSuccess { get; set; }

        public AddGroupUserViewModel()
        {
            dsChucNang = new ObservableCollection<CHUCNANG>();
            LoadChucNang();
            AddGroupUserCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                try
                {
                    NhomNguoiDung.CHUCNANGs.Clear();
                    foreach (var cn in dsChucNang.Where(c => c.IsChecked == true))
                    {
                        var chucnang = await ChucNangBLL.Instance.GetChucNangById(cn.id);
                        if (chucnang != null)
                            NhomNguoiDung.CHUCNANGs.Add(chucnang);
                    }
                    var res = await NhomNguoiDungBLL.Instance.AddNhomNguoiDung(NhomNguoiDung);
                    MessageBox.Show(res.Item2);
                    if(res.Item1)
                    {
                        OnSuccess?.Invoke();
                        p.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm nhóm: " + ex.Message);
                }
            });
            CloseWindowCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
        }

        private async void LoadChucNang()
        {
            var data = await ChucNangBLL.Instance.GetAllChucNang();
            dsChucNang.Clear();
            foreach (var cn in data)
            {
                cn.IsChecked = false;
                dsChucNang.Add(cn);
            }
        }
    }
}
