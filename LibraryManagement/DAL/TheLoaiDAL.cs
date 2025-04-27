using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DAL
{
    public class TheLoaiDAL
    {
        private static TheLoaiDAL instance;
        public static TheLoaiDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TheLoaiDAL();
                }
                return instance;
            }
        }

        public async Task<List<THELOAI>> GetAllTheLoai()
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.THELOAIs.AsNoTracking().ToListAsync();
            }
        }

        public async Task<THELOAI> GetTheLoaiById(int id)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.THELOAIs.FindAsync(id);
            }
        }

        public async Task<List<THELOAI>> GetTheLoaiByMa(string matheloai)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.THELOAIs.AsNoTracking().Where(tl => tl.MaTheLoai.Contains(matheloai)).ToListAsync();
            }
        }
        public async Task<List<THELOAI>> GetTheLoaiByTen(string tentheloai)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.THELOAIs.AsNoTracking().Where(tl => tl.TenTheLoai.Contains(tentheloai)).ToListAsync();
            }
        }

        public async Task<(bool, string)> AddTheLoai(THELOAI tl)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    context.THELOAIs.Add(tl);
                    await context.SaveChangesAsync();
                    return (true, "Thêm thể loại thành công");
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
        }
        public async Task<(bool, string)> DeleteTheLoai(int id)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    var theloai = await context.THELOAIs.FindAsync(id);
                    context.THELOAIs.Remove(theloai);
                    await context.SaveChangesAsync();
                    return (true, "Xóa thể loại thành công");
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
        }
        public async Task<(bool, string)> UpdateTheLoai(THELOAI tl)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    var theloai = await context.THELOAIs.FindAsync(tl.id);
                    theloai.TenTheLoai = tl.TenTheLoai;
                    await context.SaveChangesAsync();
                    return (true, "Cập nhật thể loại thành công");
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
        }
    }
}
