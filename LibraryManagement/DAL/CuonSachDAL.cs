﻿using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace LibraryManagement.DAL
{
    public class CuonSachDAL
    {
        private static CuonSachDAL instance;
        public static CuonSachDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CuonSachDAL();
                }
                return instance;
            }
        }
        public async Task<List<CUONSACH>> GetAllCuonSach()
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.CUONSACHes.AsNoTracking().Where(cs => cs.IsDeleted == false).ToListAsync();
            }
        }
        public async Task<CUONSACH> GetCuonSachById(int id)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.CUONSACHes.AsNoTracking()
                .FirstOrDefaultAsync(cs => cs.id == id && cs.IsDeleted == false);
            }
        }
        public async Task<List<CUONSACH>> GetCuonSachByMaSach(int masach)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.CUONSACHes.AsNoTracking()
                .Where(cs => cs.MaSach == masach && cs.IsDeleted == false).ToListAsync();
            }
        }
        public async Task<List<CUONSACH>> GetCuonSachChuaDuocMuonByMaSach(int masach)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.CUONSACHes.AsNoTracking()
                .Where(cs => cs.MaSach == masach && cs.TinhTrang == false
                && cs.IsDeleted == false).ToListAsync();
            }
        }
        public async Task<(bool, string)> UpdateCuonSach(CUONSACH cs)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    var cuonsach = await context.CUONSACHes.FindAsync(cs.id);
                    cuonsach.TinhTrang = cs.TinhTrang;
                    await context.SaveChangesAsync();
                    return (true, "Cập nhật trạng thái cuốn sách thành công.");
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
        }
        public async Task<(bool, string)> DeleteCuonSach(int id)
        {
            using (var context = new LibraryManagementEntities())
                try
                {
                    var cuonsach = await context.CUONSACHes.FindAsync(id);
                    var sach = await context.SACHes.FindAsync(cuonsach.MaSach);
                    cuonsach.IsDeleted = true;
                    sach.SoLuong--;
                    sach.SoLuongCon--;
                    await context.SaveChangesAsync();
                    return (true, "Đã xoá cuốn sách.");
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
        }
    }
}
