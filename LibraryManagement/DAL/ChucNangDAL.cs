using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DAL
{
    public class ChucNangDAL
    {
        private static ChucNangDAL instance;
        public static ChucNangDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ChucNangDAL();
                }
                return instance;
            }
        }
        public async Task<List<CHUCNANG>> GetAllChucNang()
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.CHUCNANGs.AsNoTracking().ToListAsync();
            }
        }
        public async Task<CHUCNANG> GetChucNangById(int id)
        {
            using (var context = new LibraryManagementEntities())
            {
                return await context.CHUCNANGs.FindAsync(id);
            }
        }
    }
}
