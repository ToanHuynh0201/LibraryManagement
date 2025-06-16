using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DAL
{
    public class NhomNguoiDungDAL
    {
        private static NhomNguoiDungDAL instance;
        public static NhomNguoiDungDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NhomNguoiDungDAL();
                }
                return instance;
            }
        }
        public async Task<List<NHOMNGUOIDUNG>> GetAllNhomNguoiDung()
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.NHOMNGUOIDUNGs.Include(nnd => nnd.CHUCNANGs).AsNoTracking().ToListAsync();
            }
        }
        public async Task<NHOMNGUOIDUNG> GetNhomNguoiDungById(int id)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.NHOMNGUOIDUNGs.AsNoTracking().Include(nnd => nnd.CHUCNANGs).FirstOrDefaultAsync(nnd => nnd.id == id);
            }
        }
        public async Task<List<NHOMNGUOIDUNG>> GetNhomNguoiDungByMa(string manhom)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.NHOMNGUOIDUNGs.AsNoTracking().Include(nnd => nnd.CHUCNANGs).
                             Where(nnd => nnd.MaNhom.Contains(manhom)).ToListAsync();
            }
        }
        public async Task<List<NHOMNGUOIDUNG>> GetNhomNguoiDungByTen(string tennhom)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.NHOMNGUOIDUNGs.AsNoTracking().Include(nnd => nnd.CHUCNANGs).
                             Where(nnd => nnd.TenNhom.Contains(tennhom)).ToListAsync();
            }
        }
        public async Task<(bool, string)> AddNhomNguoiDung(NHOMNGUOIDUNG nnd)
        {
            using (var context = new LibraryManagementEntities())
            {
                try
                {
                    var dsCN = new List<CHUCNANG>();
                    foreach (var cn in nnd.CHUCNANGs)
                    {
                        var chucnang = await context.CHUCNANGs.FindAsync(cn.id);
                        if (dsCN != null) dsCN.Add(chucnang);
                    }

                    nnd.CHUCNANGs = dsCN;

                    context.NHOMNGUOIDUNGs.Add(nnd);
                    await context.SaveChangesAsync();

                    return (true, "Thêm nhóm người dùng thành công");
                }
                catch (Exception ex)
                {
                    return (false, "Thêm nhóm người dùng thất bại: " + ex.Message);
                }
            }
        }


        public async Task<(bool, string)> UpdateNhomNguoiDung(NHOMNGUOIDUNG nnd)
        {
            using (var context = new LibraryManagementEntities())
            {
                try
                {
                    var NND = await context.NHOMNGUOIDUNGs.Include(nd => nd.CHUCNANGs)
                                    .FirstOrDefaultAsync(n => n.id == nnd.id);

                    if (NND == null)
                        return (false, "Nhóm người dùng không tồn tại");

                    NND.TenNhom = nnd.TenNhom;

                    NND.CHUCNANGs.Clear();

                    foreach (var cn in nnd.CHUCNANGs)
                    {
                        var chucnang = await context.CHUCNANGs.FindAsync(cn.id);
                        if (chucnang != null)
                            NND.CHUCNANGs.Add(chucnang);
                    }

                    await context.SaveChangesAsync();
                    return (true, "Cập nhật nhóm người dùng thành công");
                }
                catch (Exception ex)
                {
                    return (false, "Cập nhật nhóm người dùng thất bại: " + ex.Message);
                }
            }
        }


        public async Task<(bool, string)> DeleteNhomNguoiDung(int id)
        {
            using (var context = new LibraryManagementEntities())
            {
                try
                {
                    var NND = await context.NHOMNGUOIDUNGs.FindAsync(id);
                    if (NND == null)
                        return (false, "Nhóm người dùng không tồn tại");

                    var dsND = context.NGUOIDUNGs.Where(nd => nd.MaNhom == id).ToList();
                    context.NGUOIDUNGs.RemoveRange(dsND);
                    context.NHOMNGUOIDUNGs.Remove(NND);
                    await context.SaveChangesAsync();
                    return (true, "Xoá nhóm người dùng thành công");
                }
                catch (Exception ex)
                {
                    return (false, "Xoá nhóm người dùng thất bại: " + ex.Message);
                }
            }
        }

    }
}
