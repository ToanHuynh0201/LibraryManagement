using LibraryManagement.DAL;
using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryManagement.BLL
{
    public class DauSachBLL
    {
        private static DauSachBLL instance;
        public static DauSachBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DauSachBLL();
                }
                return instance;
            }
        }
        public async Task<List<DAUSACH>> GetAllDauSach()
        {
            return await DauSachDAL.Instance.GetAllDauSach();
        }
        public async Task<DAUSACH> GetDauSachById(int id)
        {
            return await DauSachDAL.Instance.GetDauSachById(id);
        }
        public async Task<List<DAUSACH>> GetDauSachByMa(string MaDauSach)
        {
            return await DauSachDAL.Instance.GetDauSachByMa(MaDauSach);
        }
        public async Task<List<DAUSACH>> GetDauSachByTen(string TenDauSach)
        {
            return await DauSachDAL.Instance.GetDauSachByTen(TenDauSach);
        }
        public async Task<List<DAUSACH>> GetDauSachByMaTheLoai(int MaTheLoai)
        {
            var theloai = await TheLoaiDAL.Instance.GetTheLoaiById(MaTheLoai);
            if (theloai == null)
            {
                return null;
            }
            else return await DauSachDAL.Instance.GetDauSachByMaTheLoai(MaTheLoai);
        }
        public async Task<List<DAUSACH>> GetDauSachByTacGia(List<TACGIA> dstg)
        {
            return await DauSachDAL.Instance.GetDauSachByTacGia(dstg);
        }
        public async Task<(bool, string)> AddDauSach(DAUSACH ds)
        {
            var res = CheckDauSach(ds);
            if (res.Item1 == false) return res;
            return await DauSachDAL.Instance.AddDauSach(ds);
        }
        public async Task<(bool, string)> UpdateDauSach(DAUSACH ds)
        {
            var res = CheckDauSach(ds);
            if (res.Item1 == false) return res;
            return await DauSachDAL.Instance.UpdateDauSach(ds);
        }
        public async Task<(bool, string)> DeleteDauSach(int id)
        {
            var dausach = await DauSachDAL.Instance.GetDauSachById(id);
            if (dausach == null)
            {
                return (false, "Đầu sách không hợp lệ");
            }
            return await DauSachDAL.Instance.DeleteDauSach(id);
        }
        private (bool, string) CheckDauSach(DAUSACH ds)
        {
            var theloai = TheLoaiBLL.Instance.GetTheLoaiById(ds.MaTheLoai);
            if (theloai.Result == null)
            {
                return (false, "Thể loại đầu sách không hợp lệ");
            }
            if (String.IsNullOrEmpty(ds.TenDauSach))
            {
                return (false, "Đầu sách không có tên");
            }
            return (true, "");
        }
    }
}
