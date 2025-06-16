using LibraryManagement.DTO;
using Microsoft.Xaml.Behaviors.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.DAL;

namespace LibraryManagement.BLL
{
    public class NhomNguoiDungBLL
    {
        private static NhomNguoiDungBLL instance;
        public static NhomNguoiDungBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NhomNguoiDungBLL();
                }
                return instance;
            }
        }
        public async Task<List<NHOMNGUOIDUNG>> GetAllNhomNguoiDung()
        {
            return await NhomNguoiDungDAL.Instance.GetAllNhomNguoiDung();
        }
        public async Task<List<NHOMNGUOIDUNG>> GetNhomNguoiDungByTen(string tennhom)
        {
            return await NhomNguoiDungDAL.Instance.GetNhomNguoiDungByTen(tennhom);
        }
        public async Task<List<NHOMNGUOIDUNG>> GetNhomNguoiDungByMa(string manhom)
        {
            return await NhomNguoiDungDAL.Instance.GetNhomNguoiDungByMa(manhom);
        }
        public async Task<NHOMNGUOIDUNG> GetNhomNguoiDungById(int id)
        {
            return await NhomNguoiDungDAL.Instance.GetNhomNguoiDungById(id);
        }
        public async Task<(bool, string)> AddNhomNguoiDung(NHOMNGUOIDUNG nnd)
        {
            var res = CheckNhomND(nnd);
            if(res.Item1 == false)
            {
                return res;
            }
            return await NhomNguoiDungDAL.Instance.AddNhomNguoiDung(nnd);
        }
        public async Task<(bool, string)> UpdateNhomNguoiDung(NHOMNGUOIDUNG nnd)
        {
            var NND = await NhomNguoiDungDAL.Instance.GetNhomNguoiDungById(nnd.id);
            if(NND == null)
            {
                return(false, "Nhóm người dùng không tồn tại");
            }
            if(NND.id == 1)
            {
                return (false, "Không thể chỉnh sửa nhóm người dùng Admin");
            }
            var res = CheckNhomND(nnd);
            if (res.Item1 == false)
            {
                return res;
            }
            return await NhomNguoiDungDAL.Instance.UpdateNhomNguoiDung(nnd);
        }
        public async Task<(bool, string)> DeleteNhomNguoiDung(int id)
        {
            var NND = await NhomNguoiDungDAL.Instance.GetNhomNguoiDungById(id);
            if (NND == null)
            {
                return (false, "Nhóm người dùng không tồn tại");
            }
            if (id == 1)
            {
                return (false, "Không thể xoá nhóm người dùng Admin");
            }
            foreach (var cn in NND.CHUCNANGs)
            {
                if(cn.TenChucNang == "TTDG")
                {
                    return (false, "Không thể xoá nhóm người dùng Độc giả");
                }
            }
            var ND = await NguoiDungDAL.Instance.GetAllUsers();
            foreach (var nd in ND)
                if (nd.MaNhom == id && (nd.IsDeleted == false || nd.IsDeleted == null))
                    return (false, "Không thể xoá nhóm người dùng vẫn còn người dùng");
            return await NhomNguoiDungDAL.Instance.DeleteNhomNguoiDung(id);
        }
        private (bool, string) CheckNhomND(NHOMNGUOIDUNG nnd)
        {
            if (String.IsNullOrEmpty(nnd.TenNhom))
            {
                return (false, "Nhóm người dùng chưa có tên");
            }
            foreach (CHUCNANG cn in nnd.CHUCNANGs)
            {
                if (ChucNangDAL.Instance.GetChucNangById(cn.id) == null)
                {
                    return (false, "Nhóm người dùng có phân quyền không hợp lệ");
                }
                else if(cn.id == 1)
                {
                    return (false, "Chỉ nhóm người dùng Admin mới có quyền quản lý người dùng");
                }
                else if(cn.id == 7)
                {       
                    return (false, "Chỉ nhóm người dùng Admin mới có quyền thay đổi quy định");
                }
            }
            if(String.Equals("Admin", nnd.TenNhom))
            {
                return (false, "Tên nhóm người dùng không được là Admin");
            }
            return (true, "");
        }
    }
}
