using LibraryManagement.DAL;
using LibraryManagement.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.BLL
{
    public class LoginBLL
    {
        private static LoginBLL instance;
        public static LoginBLL Instance
        {
            get
            {
                if (instance == null)
                    instance = new LoginBLL();
                return instance;
            }
        }
        public async Task<(bool, string)> CheckLogin(string username, string password)
        {
            List<NGUOIDUNG> dsnd = await NguoiDungDAL.Instance.GetAllUsers();
            foreach (var nd in dsnd)
            {
                if (nd.TenDangNhap == username)
                    if (nd.MatKhau == password)
                        return (true, "Đăng nhập thành công.");
            }
            return (false, "Đăng nhập thất bại.");
        }
    }
}
