using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.DAL;
using LibraryManagement.DTO;
using LibraryManagement.View.BookCopy;

namespace LibraryManagement.BLL
{
    public class SachBLL
    {
        private static SachBLL instance;
        public static SachBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SachBLL();
                }
                return instance;
            }
        }

        public async Task<List<SACH>> GetAllSach()
        {
            return await SachDAL.Instance.GetAllSach();
        }
        public async Task<SACH> GetSachById(int id)
        {
            return await SachDAL.Instance.GetSachById(id);
        }
        public async Task<List<SACH>> GetSachByMaSach(string MaSach)
        {
            return await SachDAL.Instance.GetSachByMaSach(MaSach);
        }
        public async Task<List<SACH>> GetSachByMaDauSach(int MaDauSach)
        {
            return await SachDAL.Instance.GetSachByMaDauSach(MaDauSach);
        }
        public async Task<List<SACH>> GetSachByTenDauSach(string TenDauSach)
        {
            return await SachDAL.Instance.GetSachByTenDauSach(TenDauSach);
        }
        public async Task<List<SACH>> GetSachByNXB(string NhaXB)
        {
            return await SachDAL.Instance.GetSachByNXB(NhaXB);
        }
        public async Task<List<SACH>> GetSachByNamXB(int NamXB)
        {
            return await SachDAL.Instance.GetSachByNamXB(NamXB);
        }
        public async Task<bool> CheckExistingSach(SACH s)
        {
            return await SachDAL.Instance.CheckExistingSach(s);
        }
        public async Task<(bool, string)> AddNewSach(SACH s, PHIEUNHAPSACH pns)
        {
            if (CheckSach(s).Item1 == false)
            {
                return (false, CheckSach(s).Item2);
            }
            var thamso = await ThamSoBLL.Instance.GetThamSo();
            int kc = thamso.KhoangCachNamXB;
            if (s.NamXB + kc < DateTime.Now.Year)
            {
                return (false, "Chỉ nhận các sách xuất bản trong vòng " + kc + " năm.");
            }
            bool checkRes = await SachDAL.Instance.CheckExistingSach(s);
            if (checkRes)
                return (false, "Thư viện đã có sách này, hãy dùng tính năng nhập sách đã có để nhập");
            var phieunhapsach = await PhieuNhapSachDAL.Instance.GetPhieuNhapSachById(pns.id);
            if (phieunhapsach == null)
                return (false, "Phiếu nhập sách không hợp lệ");
            s.IsDeleted = false;
            s.SoLuongCon = (int)s.SoLuong;
            return await SachDAL.Instance.AddNewSach(s, pns);
        }
        public async Task<(bool, string)> AddExistingSach(List<(int, int)> dsnhap, PHIEUNHAPSACH pns)
        {
            foreach (var sachnhap in dsnhap)
            {
                SACH sach = await SachDAL.Instance.GetSachById(sachnhap.Item1);
                bool checkRes = await SachDAL.Instance.CheckExistingSach(sach);
                if (!checkRes)
                    return (false, "Thư viện chưa có sách này, hãy dùng tính năng nhập sách mới để nhập");
            }
            var phieunhapsach = await PhieuNhapSachDAL.Instance.GetPhieuNhapSachById(pns.id);
            if (phieunhapsach == null)
                return (false, "Phiếu nhập sách không hợp lệ");
            return await SachDAL.Instance.AddExistingSach(dsnhap, pns);
        }
        public async Task<(bool, string)> UpdateSach(SACH s)
        {
            var sach = await SachDAL.Instance.GetSachById(s.id);
            if (sach == null || sach.IsDeleted == true)
            {
                return (false, "Sách không hợp lệ"); 
            }
            var res = CheckSach(s);
            if(res.Item1 == false)
            {
                return res;
            }
            return await SachDAL.Instance.UpdateSach(s);
        }
        public async Task<(bool, string)> DeleteSach(int id)
        {
            SACH sach = await SachDAL.Instance.GetSachById(id);
            if (sach == null || sach.IsDeleted == true)
            {
                return (false, "Sách không hợp lệ");
            }
            foreach(CUONSACH cuonsach in sach.CUONSACHes)
            {
                if (cuonsach.TinhTrang == true && cuonsach.IsDeleted == false)
                {
                    return (false, "Không thể xoá sách vì đang có cuốn sách được mượn.");
                }
            }
            return await SachDAL.Instance.DeleteSach(id);
        }
        private (bool, string) CheckSach(SACH s)
        {
            var dausach = DauSachBLL.Instance.GetDauSachById(s.MaDauSach);
            if (dausach.Result == null)
            {
                return (false, "Đầu sách không hợp lệ");
            }
            if (string.IsNullOrEmpty(s.NhaXB))
            {
                return (false, "Sách không có nhà xuất bản.");
            }
            if (s.NamXB < 0)
            {
                return (false, "Năm xuất bản không hợp lệ.");
            }
            if (s.TriGia <= 0)
            {
                return (false, "Trị giá phải lớn hơn 0.");
            }
            if (s.SoLuong < 0 || s.SoLuongCon < 0)
            {
                return (false, "Số lượng không được âm.");
            }
            if (s.SoLuongCon > s.SoLuong)
            {
                return (false, "Số lượng còn không được lớn hơn số lượng");
            }
            return (true, "");
        }
    }
}
