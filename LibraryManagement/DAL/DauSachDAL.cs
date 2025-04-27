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
                return await context.DAUSACHes.AsNoTracking().ToListAsync();
            }
        }
        public async Task<DAUSACH> GetDauSachById(int id)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.DAUSACHes.FindAsync(id);
            }
        }
        public async Task<List<DAUSACH>> GetDauSachByMa(string madausach)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.DAUSACHes.AsNoTracking()
                .Where(ds => ds.MaDauSach.Contains(madausach)).ToListAsync();
            }
        }
        public async Task<List<DAUSACH>> GetDauSachByTen(string tendausach)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.DAUSACHes.AsNoTracking()
                .Where(ds => ds.TenDauSach.Contains(tendausach)).ToListAsync();
            }
        }
        public async Task<List<DAUSACH>> GetDauSachByMaTheLoai(int matheloai)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.DAUSACHes.AsNoTracking()
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
                    var DS = await context.DAUSACHes.AsNoTracking()
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
                try
                {
                    var dausach = await context.DAUSACHes.FindAsync(ds.id);
                    dausach.TenDauSach = ds.TenDauSach;
                    dausach.MaTheLoai = ds.MaTheLoai;
                    dausach.TACGIAs = ds.TACGIAs;
                    await context.SaveChangesAsync();
                    return (true, "Cập nhật đầu sách thành công");
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
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
