using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.DTO;
using LibraryManagement.DAL;
using ControlzEx;
using System.Windows;

namespace LibraryManagement.BLL
{
    public class NguoiDungBLL
    {
        private static NguoiDungBLL instance;
        public static NguoiDungBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NguoiDungBLL();
                }
                return instance;
            }
        }
        public async Task<List<NGUOIDUNG>> GetAllUsers()
        {
            return await NguoiDungDAL.Instance.GetAllUsers();
        }
        public async Task<NGUOIDUNG> GetNguoiDungByTenDN(string tendangnhap)
        {
            return await NguoiDungDAL.Instance.GetNguoiDungByTenDN(tendangnhap);
        }
        public async Task<List<NGUOIDUNG>> GetNguoiDungByMaNhom(int manhom)
        {
            return await NguoiDungDAL.Instance.GetNguoiDungByMaNhom(manhom);
        }
        public async Task<(bool, string)> AddNguoiDung(NGUOIDUNG nd)
        {
            var nguoidung = await NguoiDungDAL.Instance.GetNguoiDungByTenDN(nd.TenDangNhap);
            if (nguoidung != null)
            {
                return (false, "Tên đăng nhập đã tồn tại");
            }
            NHOMNGUOIDUNG nnd = await NhomNguoiDungBLL.Instance.GetNhomNguoiDungById(nd.MaNhom);
            foreach(var cn in nnd.CHUCNANGs)
            if(cn.id == 8)
            {
                return (false, "Người dùng là độc giả phải được đăng ký thẻ qua trang quản lý độc giả.");
            }
            if (nd.MaNhom == 3)
            {
                return (false, "Người dùng là độc giả phải được đăng ký thẻ qua trang quản lý độc giả.");
            }
            var res = CheckNguoiDung(nd);
            if (!res.Item1)
            {
                return (false, res.Item2);
            }
            int manhom;
            manhom = nd.MaNhom;
            return await NguoiDungDAL.Instance.AddNguoiDung(nd, manhom);
        }
        public async Task<(bool, string)> UpdateNguoiDung(NGUOIDUNG nd)
        {
            var nguoidung = await NguoiDungDAL.Instance.GetNguoiDungByTenDN(nd.TenDangNhap);
            if (nguoidung == null)
            {
                return (false, "Người dùng không tồn tại");
            }
            if(nguoidung.TenDangNhap == "admin")
            {
                return (false, "Không thể sửa thông tin người dùng Admin");
            }
            var res = CheckNguoiDung(nd);
            if(!res.Item1)
            {
                return (false, res.Item2);
            }
            if(nd.MaNhom == 3)
            {
                return (false, "Nhóm độc giả phải được đăng ký thẻ, không thể đổi nhóm");
            }
            return await NguoiDungDAL.Instance.UpdateNguoiDung(nd);
        }
        public async Task<(bool, string)> DeleteNguoiDung(string tendangnhap)
        {
            var nguoidung = await NguoiDungDAL.Instance.GetNguoiDungByTenDN(tendangnhap);
            if (nguoidung == null)
            {
                return (false, "Người dùng không tồn tại");
            }
            if(nguoidung.TenDangNhap == "admin")
            {
                return (false, "Không thể xóa người dùng Admin");
            }
            var docgia = DocGiaBLL.Instance.GetDocGiaByTenDangNhap(tendangnhap);
            if (docgia.Result != null)
            {
                return (false, "Người dùng là độc giả, hãy xoá thẻ của người dùng trước");
            }
            return await NguoiDungDAL.Instance.DeleteNguoiDung(tendangnhap);
        }
        private (bool, string) CheckNguoiDung(NGUOIDUNG nd)
        {
            if (nd.TenDangNhap.Length > 64)
            {
                return (false, "Tên đăng nhập không được quá 64 ký tự");
            }
            if (nd.MatKhau.Length > 256)
            {
                return (false, "Mật khẩu không được quá 256 ký tự");
            }
            var nhom = NhomNguoiDungBLL.Instance.GetNhomNguoiDungById(nd.MaNhom);
            if (nhom.Result == null)
            {
                return (false, "Người dùng không thuộc nhóm người dùng nào/nhóm người dùng sai");
            }
            return (true, "");
        }
    }
}
