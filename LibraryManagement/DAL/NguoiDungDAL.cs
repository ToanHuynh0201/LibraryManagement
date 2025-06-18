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
                return await context.NGUOIDUNGs.Include(nd => nd.NHOMNGUOIDUNG.CHUCNANGs).Where(nd => (nd.IsDeleted == false || nd.IsDeleted == null)).ToListAsync();
            }
        }
        public async Task<NGUOIDUNG> GetNguoiDungByTenDN(string tendangnhap)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.NGUOIDUNGs.Include(nd => nd.NHOMNGUOIDUNG.CHUCNANGs).FirstOrDefaultAsync(nd => nd.TenDangNhap == tendangnhap && (nd.IsDeleted == false || nd.IsDeleted == null));
            }
        }
        public async Task<List<NGUOIDUNG>> GetNguoiDungByMaNhom(int manhom)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.NGUOIDUNGs.Include(nd => nd.NHOMNGUOIDUNG.CHUCNANGs).Where(nd => nd.MaNhom == manhom && (nd.IsDeleted == false || nd.IsDeleted == null)).ToListAsync();
            }
        }
        public async Task<(bool, string)> AddNguoiDung(NGUOIDUNG nd, int manhom)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    nd.MaNhom = manhom;
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
                    NGUOIDUNG nguoidung = await context.NGUOIDUNGs.Where(ngd => ngd.IsDeleted == false || ngd.IsDeleted == null).FirstOrDefaultAsync(ngd => ngd.TenDangNhap == nd.TenDangNhap);
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
                    nguoidung.IsDeleted = true;
                    await context.SaveChangesAsync();
                    return (true, "Xoá người dùng thành công");
                }
                catch (Exception ex)
                {
                    return (false, "Xoá thất bại: " + ex.ToString());
                }
        }
        public async Task<(bool, string)> ChangePassword(NGUOIDUNG nd)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    NGUOIDUNG nguoidung = await context.NGUOIDUNGs.Where(ngd => ngd.IsDeleted == false || ngd.IsDeleted == null).FirstOrDefaultAsync(ngd => ngd.TenDangNhap == nd.TenDangNhap);
                    nguoidung.MatKhau = nd.MatKhau;
                    await context.SaveChangesAsync();
                    return (true, "Đổi mật khẩu thành công");
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
        }
    }
}
