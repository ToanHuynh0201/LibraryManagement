using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DAL
{
    public class DauSachDAL
    {
        private static DauSachDAL instance;
        public static DauSachDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DauSachDAL();
                }
                return instance;
            }
        }
        public async Task<List<DAUSACH>> GetAllDauSach()
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.DAUSACHes.AsNoTracking().Include(ds => ds.THELOAI).Include(ds => ds.TACGIAs).ToListAsync();
            }
        }
        public async Task<DAUSACH> GetDauSachById(int id)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.DAUSACHes.Include(ds => ds.THELOAI).Include(ds => ds.TACGIAs).FirstOrDefaultAsync(ds => ds.id == id);
            }
        }
        public async Task<List<DAUSACH>> GetDauSachByMa(string madausach)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.DAUSACHes.AsNoTracking().Include(ds => ds.THELOAI).Include(ds => ds.TACGIAs)
                .Where(ds => ds.MaDauSach.Contains(madausach)).ToListAsync();
            }
        }
        public async Task<List<DAUSACH>> GetDauSachByTen(string tendausach)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.DAUSACHes.AsNoTracking().Include(ds => ds.THELOAI).Include(ds => ds.TACGIAs)
                .Where(ds => ds.TenDauSach.Contains(tendausach)).ToListAsync();
            }
        }
        public async Task<List<DAUSACH>> GetDauSachByMaTheLoai(int matheloai)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.DAUSACHes.AsNoTracking().Include(ds => ds.THELOAI).Include(ds => ds.TACGIAs)
                .Where(ds => ds.MaTheLoai == matheloai).ToListAsync();
            }
        }
        public async Task<List<DAUSACH>> GetDauSachByTacGia(List<TACGIA> dstg)
        {
            using (var context = new LibraryManagementEntities())
            {
                var dsds = new List<DAUSACH>();
                foreach(TACGIA TG in dstg)
                {
                    var DS = await context.DAUSACHes.AsNoTracking().Include(ds => ds.THELOAI).Include(ds => ds.TACGIAs)
                    .Where(ds => ds.TACGIAs.Any(tg => tg.id == TG.id)).ToListAsync();
                    dsds.AddRange(DS);
                }
                return dsds;
            }
        }
        public async Task<(bool, string)> AddDauSach(DAUSACH ds)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    var tacGiaIds = ds.TACGIAs.Select(tg => tg.id).ToList();

                    var tacgias = await context.TACGIAs.Where(tg => tacGiaIds.Contains(tg.id)).ToListAsync();

                    ds.TACGIAs.Clear();
                    foreach (var tg in tacgias)
                    {
                        ds.TACGIAs.Add(tg);
                    }
                    context.DAUSACHes.Add(ds);
                    await context.SaveChangesAsync();
                    return (true, "Thêm đầu sách thành công");
                }
                catch(Exception ex)
                {
                    return (false, ex.Message);
                }
        }
        public async Task<(bool, string)> UpdateDauSach(DAUSACH ds)
        {
            using (var context = new LibraryManagementEntities())
            {
                try
                {

                    var dausach = await context.DAUSACHes.Include(d => d.TACGIAs).FirstOrDefaultAsync(d => d.id == ds.id);

                    if (dausach == null)
                        return (false, "Không tìm thấy đầu sách");


                    dausach.TenDauSach = ds.TenDauSach;
                    dausach.MaTheLoai = ds.MaTheLoai;


                    dausach.TACGIAs.Clear();

                    var tacGiaIds = ds.TACGIAs.Select(t => t.id).ToList();

                    var tacgias = await context.TACGIAs
                        .Where(tg => tacGiaIds.Contains(tg.id))
                        .ToListAsync();

                    foreach (var tg in tacgias)
                    {
                        dausach.TACGIAs.Add(tg);
                    }

                    await context.SaveChangesAsync();
                    return (true, "Cập nhật đầu sách và tác giả thành công");
                }
                catch (Exception ex)
                {
                    return (false, $"Lỗi khi cập nhật: {ex.Message}");
                }
            }
        }
        public async Task<(bool, string)> DeleteDauSach(int id)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    var dausach = await context.DAUSACHes.FindAsync(id);
                    context.DAUSACHes.Remove(dausach);
                    await context.SaveChangesAsync();
                    return (true, "Xoá đầu sách thành công");
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
        }
    }
}
