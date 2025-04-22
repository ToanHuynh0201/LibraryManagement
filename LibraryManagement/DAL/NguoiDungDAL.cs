using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DAL
{
    public class NguoiDungDAL
    {
        private static NguoiDungDAL instance;
        public static NguoiDungDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NguoiDungDAL();
                }
                return instance;
            }
        }

        public List<NGUOIDUNG> GetAllUsers()
        {
            using (var context = new LibraryManagementEntities())
            {
                return context.NGUOIDUNGs.ToList();
            }
        }

    }
}
