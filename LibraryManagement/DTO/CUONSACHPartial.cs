using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DTO
{
    public partial class CUONSACH
    {
        public string DisplayTinhTrang
        {
            get
            {
                if (TinhTrang == false) return ("Chưa được mượn");
                return ("Đã được mượn");
            }
        }
    }
}
