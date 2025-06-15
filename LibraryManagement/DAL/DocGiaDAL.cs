using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
                return await context.DOCGIAs.Include(dg => dg.LOAIDOCGIA).Where(dg => dg.IsDeleted == false).ToListAsync();
            }
        }
        public async Task<DOCGIA> GetDocGiaById(int id)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.DOCGIAs.Include(dg => dg.LOAIDOCGIA).Where(dg => dg.IsDeleted == false).FirstOrDefaultAsync(dg => dg.id == id);
            }
        }

        public async Task<List<DOCGIA>> GetDocGiaByMaDG(string madg)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.DOCGIAs.Include(dg => dg.LOAIDOCGIA).Where(dg => dg.MaDG.Contains(madg)).Include(dg => dg.LOAIDOCGIA).ToListAsync();
            }
        }
        public async Task<List<DOCGIA>> GetDocGiaByTenDG(string tendg)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.DOCGIAs.Include(dg => dg.LOAIDOCGIA).Where(dg => dg.IsDeleted == false).Where(dg => dg.TenDG.Contains(tendg)).Include(dg => dg.LOAIDOCGIA).ToListAsync();
            }
        }
        public async Task<DOCGIA> GetDocGiaByTenDangNhap(string tendangnhap)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.DOCGIAs.Include(dg => dg.LOAIDOCGIA).Where(dg => dg.IsDeleted == false).Include(dg => dg.LOAIDOCGIA).Where(dg => dg.IsDeleted == false).FirstOrDefaultAsync(dg => dg.TenDangNhap == tendangnhap);
            }
        }
        public async Task<(bool, string)> AddDocGia(DOCGIA dg)
        {
            using (var context = new LibraryManagementEntities())
            {
                try
                {
                    NGUOIDUNG nd = new NGUOIDUNG();
                    nd.TenDangNhap = dg.TenDangNhap;
                    nd.MatKhau = "123456";
                    nd.TenNguoiDung = dg.TenDG;
                    nd.MaNhom = 3;

                    if (dg.TongNo == null) dg.TongNo = 0;
                    dg.IsDeleted = false;
                    context.NGUOIDUNGs.Add(nd);
                    context.DOCGIAs.Add(dg);
                    await context.SaveChangesAsync();
                    return (true, "Thêm độc giả thành công, mật khẩu mặc định là 123456");
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
                    docgia.IsDeleted = true;
                    await context.SaveChangesAsync();
                    return (true, "Đã ẩn độc giả");
                }
                catch (Exception ex)
                {
                    return (false, "Ẩn độc giả thất bại: " + ex.ToString());
                }
            }
        }
    }
}
