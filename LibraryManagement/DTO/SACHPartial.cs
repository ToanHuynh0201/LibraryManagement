using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DTO
{
    public partial class SACH
    {
        public string DisplayName => $"{MaSach} ({DAUSACH.TenDauSach})";
        public override string ToString()
        {
            return DisplayName.ToString();
        }
    }
}
