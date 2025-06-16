using LibraryManagement.BLL;
using LibraryManagement.DAL;
using LibraryManagement.DTO;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class EditGroupUserViewModel : BaseViewModel
    {
        private NHOMNGUOIDUNG nhomnguoidung;
        public NHOMNGUOIDUNG NhomNguoiDung
        {
            get => nhomnguoidung;
            set
            {
                nhomnguoidung = value;
                OnPropertyChanged(nameof(NhomNguoiDung));
            }
        }

        public ObservableCollection<CHUCNANG> dsChucNang { get; set; } = new ObservableCollection<CHUCNANG>();

        public Action OnSuccess { get; set; }
        public ICommand SaveGroupUserCM { get; set; }
        public ICommand CloseWindowCM { get; set; }

        public EditGroupUserViewModel() { }
        public EditGroupUserViewModel(NHOMNGUOIDUNG selectedGroup)
        {
            NhomNguoiDung = selectedGroup;
            LoadChucNang();

            SaveGroupUserCM = new RelayCommand<Window>((p) => true, async (p) =>
            {
                try
                {
                    NhomNguoiDung.CHUCNANGs.Clear();
                    foreach (var cn in dsChucNang.Where(c => c.IsChecked))
                    {
                        var chucnang = await ChucNangBLL.Instance.GetChucNangById(cn.id);
                        if (chucnang != null)
                            NhomNguoiDung.CHUCNANGs.Add(chucnang);
                    }

                    var res = await NhomNguoiDungBLL.Instance.UpdateNhomNguoiDung(NhomNguoiDung);
                    MessageBox.Show(res.Item2);
                    if (res.Item1)
                    {
                        OnSuccess?.Invoke();
                        p.Close();
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật nhóm: " + ex.Message);
                }
            });

            CloseWindowCM = new RelayCommand<Window>((p) => true, (p) => p.Close());
        }

        private async void LoadChucNang()
        {
            var allCN = await ChucNangBLL.Instance.GetAllChucNang();
            var idCNHienTai = NhomNguoiDung.CHUCNANGs.Select(c => c.id).ToHashSet();

            dsChucNang.Clear();
            foreach (var cn in allCN)
            {
                cn.IsChecked = idCNHienTai.Contains(cn.id);
                dsChucNang.Add(cn);
            }
        }
    }
}
