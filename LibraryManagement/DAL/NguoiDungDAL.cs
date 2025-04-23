using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DAL
{
    public class NguoiDungDAL
    {
        private static NguoiDungDAL instance;
        public static NguoiDungDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NguoiDungDAL();
                }
                return instance;
            }
        }
        public async Task<List<NGUOIDUNG>> GetAllUsers()
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.NGUOIDUNGs.ToListAsync();
            }
        }
        public async Task<NGUOIDUNG> GetNguoiDungByTenDN(string tendangnhap)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.NGUOIDUNGs.FirstOrDefaultAsync(nd => nd.TenDangNhap == tendangnhap);
            }
        }
        public async Task<List<NGUOIDUNG>> GetNguoiDungByMaNhom(int manhom)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.NGUOIDUNGs.Where(nd => nd.MaNhom == manhom).ToListAsync();
            }
        }
        public async Task<(bool, string)> AddNguoiDung(NGUOIDUNG nd)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    context.NGUOIDUNGs.Add(nd);
                    await context.SaveChangesAsync();
                    return (true, "Thêm người dùng thành công");
                }
                catch(Exception ex)
                {
                    return (false, "Thêm người dùng thất bại: " + ex.Message);
                }
        }
        public async Task<(bool, string)> UpdateNguoiDung(NGUOIDUNG nd)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    NGUOIDUNG nguoidung = await context.NGUOIDUNGs.FindAsync(nd.TenDangNhap);
                    nguoidung.MatKhau = nd.MatKhau;
                    nguoidung.TenNguoiDung = nd.TenNguoiDung;
                    nguoidung.MaNhom = nd.MaNhom;
                    await context.SaveChangesAsync();
                    return (true, "Cập nhật người dùng thành công");
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
        }
        public async Task<(bool, string)> DeleteNguoiDung(string tendangnhap)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    NGUOIDUNG nguoidung = await context.NGUOIDUNGs.FindAsync(tendangnhap);
                    await context.SaveChangesAsync();
                    return (true, "Xoá người dùng thành công");
                }
                catch (Exception ex)
                {
                    return (false, "Xoá thất bại: " + ex.Message);
                }
        }
    }
}
