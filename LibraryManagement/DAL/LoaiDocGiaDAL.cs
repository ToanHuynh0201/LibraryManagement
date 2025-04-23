using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DAL
{
    public class LoaiDocGiaDAL
    {
        private static LoaiDocGiaDAL instance;
        public static LoaiDocGiaDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LoaiDocGiaDAL();
                }
                return instance;
            }
        }
        public async Task<List<LOAIDOCGIA>> GetAllLoaiDocGia()
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.LOAIDOCGIAs.ToListAsync();
            }
        }
        public async Task<LOAIDOCGIA> GetLoaiDocGiaById(int id)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.LOAIDOCGIAs.FindAsync(id);
            }
        }
        public async Task<List<LOAIDOCGIA>> GetLoaiDocGiaByTen(string tenldg)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.LOAIDOCGIAs.Where(ldg => ldg.TenLoaiDG.Contains(tenldg)).ToListAsync();
            }
        }
        public async Task<(bool, string)> AddLoaiDocGia(LOAIDOCGIA ldg)
        {
            using (var context = new LibraryManagementEntities())
            {
                try
                {
                    context.LOAIDOCGIAs.Add(ldg);
                    await context.SaveChangesAsync();
                    return (true, "Thêm loại độc giả thành công");
                }
                catch (Exception ex)
                {
                    return (false, "Thêm loại độc giả thất bại: " + ex.Message);
                }
            }
        }
        public async Task<(bool, string)> UpdateLoaiDocGia(LOAIDOCGIA ldg)
        {
            using (var context = new LibraryManagementEntities())
            {
                try
                {
                    LOAIDOCGIA loaidocgia = await context.LOAIDOCGIAs.FindAsync(ldg.id);
                    loaidocgia.TenLoaiDG = ldg.TenLoaiDG;
                    await context.SaveChangesAsync();
                    return (true, "Cập nhật loại độc giả thành công");
                }
                catch (Exception ex)
                {
                    return (false, "Cập nhật loại độc giả thất bại: " + ex.Message);
                }
            }
        }
        public async Task<(bool, string)> DeleteLoaiDocGia(int id)
        {
            using (var context = new LibraryManagementEntities())
            {
                try
                {
                    var ldg = await context.LOAIDOCGIAs.FindAsync(id);
                    context.LOAIDOCGIAs.Remove(ldg);
                    await context.SaveChangesAsync();
                    return (true, "Xóa loại độc giả thành công");
                }
                catch (Exception ex)
                {
                    return (false, "Xóa loại độc giả thất bại: " + ex.Message);
                }
            }
        }
    }
}
