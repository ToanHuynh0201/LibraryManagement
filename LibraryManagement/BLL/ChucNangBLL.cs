using LibraryManagement.DAL;
using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.BLL
{
    public class ChucNangBLL
    {
        private static ChucNangBLL instance;
        public static ChucNangBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ChucNangBLL();
                }
                return instance;
            }
        }
        public async Task<List<CHUCNANG>> GetAllChucNang()
        {
            return await ChucNangDAL.Instance.GetAllChucNang();
        }
        public async Task<CHUCNANG> GetChucNangById(int id)
        {
            return await ChucNangDAL.Instance.GetChucNangById(id);
        }
    }
}
