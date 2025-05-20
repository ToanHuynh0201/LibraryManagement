using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlzEx;
using LibraryManagement.DTO;

namespace LibraryManagement.DAL
{
    public class SachDAL
    {
        private static SachDAL instance;
        public static SachDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SachDAL();
                }
                return instance;
            }
        }
        public async Task<List<SACH>> GetAllSach()
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.SACHes.AsNoTracking().Where(s => s.IsDeleted == false).ToListAsync();
            }
        }
        public async Task<SACH> GetSachById(int id)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.SACHes.AsNoTracking().FirstOrDefaultAsync(s => s.id == id && s.IsDeleted == false);
            }
        }
        public async Task<List<SACH>> GetSachByMaDauSach(int madausach)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.SACHes.AsNoTracking()
                .Where(s => s.MaDauSach == madausach && s.IsDeleted == false).ToListAsync();
            }
        }
        public async Task<List<SACH>> GetSachByTenDauSach(string tendausach)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.SACHes.AsNoTracking()
                .Where(s => s.DAUSACH.TenDauSach.Contains(tendausach) && s.IsDeleted == false).ToListAsync();
            }
        }
        public async Task<List<SACH>> GetSachByNXB(string nhaxb)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.SACHes.AsNoTracking()
                .Where(s => s.NhaXB.Contains(nhaxb) && s.IsDeleted == false).ToListAsync();
            }
        }
        public async Task<List<SACH>> GetSachByNamXB(int namxb)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.SACHes.AsNoTracking()
                .Where(s => s.NamXB == namxb && s.IsDeleted == false).ToListAsync();
            }
        }
        public async Task<SACH> CheckExistingSach(SACH s)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.SACHes.AsNoTracking()
                .FirstOrDefaultAsync(sach => sach.MaDauSach == s.MaDauSach
                && sach.NhaXB == s.NhaXB && sach.NamXB == s.NamXB);
            }
        }
        public async Task<(bool, string)> AddNewSach(SACH s)
        {
            using (var context = new LibraryManagementEntities())
            {
                try
                {
                    context.SACHes.Add(s);
                    await context.SaveChangesAsync();
                    return (true, "Thêm sách thành công");
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
            }
        }
        public async Task<(bool, string)> AddExistingSach(int id, int soluong)
        {
            using (var context = new LibraryManagementEntities())
            {
                try
                {
                    var sach = await context.SACHes.FindAsync(id);
                    sach.IsDeleted = false;
                    sach.SoLuong += soluong;
                    sach.SoLuongCon += soluong;
                    for(int i = 1; i <= soluong; i++)
                    {
                        var cuonsach = new CUONSACH
                        {
                            MaSach = sach.id,
                            TinhTrang = false,
                            IsDeleted = false
                        };
                        context.CUONSACHes.Add(cuonsach);
                    }
                    await context.SaveChangesAsync();
                    return (true, "Thêm sách thành công");
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
            }
        }
        public async Task<(bool, string)> DeleteSach(int id)
        {
            using (var context = new LibraryManagementEntities())
            {
                try
                {
                    var sach = await context.SACHes.FindAsync(id);
                    foreach(var cuonsach in sach.CUONSACHes)
                        cuonsach.IsDeleted = true;
                    sach.IsDeleted = true;
                    sach.SoLuongCon = 0;
                    sach.SoLuong = 0;
                    await context.SaveChangesAsync();
                    return (true, "Xóa sách thành công");
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
            }
        }   
        public async Task<(bool, string)> UpdateSach(SACH s)
        {
            using (var context = new LibraryManagementEntities())
            {
                try
                {
                    var sach = await context.SACHes.FindAsync(s.id);
                    sach.NamXB = s.NamXB;
                    sach.NhaXB = s.NhaXB;
                    sach.SoLuong = s.SoLuong;
                    sach.SoLuongCon = s.SoLuongCon;
                    sach.TriGia = s.TriGia;
                    sach.IsDeleted = s.IsDeleted;
                    await context.SaveChangesAsync();
                    return (true, "Cập nhật sách thành công");
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
            }
        }
    }
}
