using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.DTO;
namespace LibraryManagement.DAL
{
    public class DocGiaDAL
    {
        private static DocGiaDAL instance;
        public static DocGiaDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DocGiaDAL();
                }
                return instance;
            }
        }
        public async Task<List<DOCGIA>> GetAllDocGia()
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.DOCGIAs.ToListAsync();
            }
        }
        public async Task<DOCGIA> GetDocGiaById(int id)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.DOCGIAs.FindAsync(id);
            }
        }
        public async Task<List<DOCGIA>> GetDocGiaByMaDG(string madg)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.DOCGIAs.Where(dg => dg.MaDG.Contains(madg)).ToListAsync();
            }
        }
        public async Task<List<DOCGIA>> GetDocGiaByTenDG(string tendg)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.DOCGIAs.Where(dg => dg.MaDG.Contains(tendg)).ToListAsync();
            }
        }
        public async Task<DOCGIA> GetDocGiaByTenDangNhap(string tendangnhap)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.DOCGIAs.FirstOrDefaultAsync(dg => dg.TenDangNhap == tendangnhap);
            }
        }
        public async Task<(bool, string)> AddDocGia(DOCGIA dg)
        {
            using (var context = new LibraryManagementEntities())
            {
                try
                {
                    context.DOCGIAs.Add(dg);
                    await context.SaveChangesAsync();
                    return (true, "Thêm độc giả thành công");
                }
                catch (Exception ex)
                {
                    return (false, "Thêm độc giả thất bại: " + ex.Message);
                }
            }
        }
        public async Task<(bool, string)> UpdateDocGia(DOCGIA dg)
        {
            using (var context = new LibraryManagementEntities())
            {
                try
                {
                    DOCGIA docgia = await context.DOCGIAs.FindAsync(dg.id);
                    docgia.MaLoaiDG = dg.MaLoaiDG;
                    docgia.TenDG = dg.TenDG;
                    docgia.NgaySinhDG = dg.NgaySinhDG;
                    docgia.DiaChiDG = dg.DiaChiDG;
                    docgia.Email = dg.Email;
                    docgia.NgayLapThe = dg.NgayLapThe;
                    docgia.NgayHetHan = dg.NgayHetHan;
                    docgia.TongNo = dg.TongNo;
                    await context.SaveChangesAsync();
                    return (true, "Cập nhật độc giả thành công");
                }
                catch (Exception ex)
                {
                    return (false, "Cập nhật độc giả thất bại: " + ex.Message);
                }
            }
        }
        public async Task<(bool, string)> DeleteDocGia(int id)
        {
            using (var context = new LibraryManagementEntities())
            {
                try
                {
                    DOCGIA docgia = await context.DOCGIAs.FindAsync(id);
                    context.DOCGIAs.Remove(docgia);
                    await context.SaveChangesAsync();
                    return (true, "Xóa độc giả thành công");
                }
                catch (Exception ex)
                {
                    return (false, "Xóa độc giả thất bại: " + ex.Message);
                }
            }
        }
    }
}
