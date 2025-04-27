using LibraryManagement.DTO;
using Microsoft.Xaml.Behaviors.Media;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DAL
{
    public class TacGiaDAL
    {
        private static TacGiaDAL instance;
        public static TacGiaDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TacGiaDAL();
                }
                return instance;
            }
        }

        public async Task<List<TACGIA>> GetAllTacGia()
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.TACGIAs.AsNoTracking().ToListAsync();
            }
        }

        public async Task<TACGIA> GetTacGiaById(int id)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.TACGIAs.AsNoTracking().FirstOrDefaultAsync(tg => tg.id == id);
            }
        }

        public async Task<List<TACGIA>> GetTacGiaByMa(string matg)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.TACGIAs.AsNoTracking().Where(tg => tg.MaTG.Contains(matg)).ToListAsync();
            }
        }
        public async Task<List<TACGIA>> GetTacGiaByTen(string tentg)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.TACGIAs.AsNoTracking().Where(tg => tg.TenTG.Contains(tentg)).ToListAsync();
            }
        }
        public async Task<(bool, string)> AddTacGia(TACGIA tg)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    context.TACGIAs.Add(tg);
                    await context.SaveChangesAsync();
                    return (true, "Thêm tác giả thành công");
                }
                catch(Exception ex)
                {
                    return(false, ex.Message);
                }
        }
        public async Task<(bool, string)> UpdateTacGia(TACGIA tg)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    var tacgia = await context.TACGIAs.FindAsync(tg.id);
                    tacgia.TenTG = tg.TenTG;
                    await context.SaveChangesAsync();
                    return (true, "Cập nhật tác giả thành công");
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
        }
        public async Task<(bool, string)> DeleteTacGia(int id)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    var tacgia = await context.TACGIAs.FindAsync(id);
                    context.TACGIAs.Remove(tacgia);
                    await context.SaveChangesAsync();
                    return (true, "Xoá tác giả thành công");
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
        }
    }
}
