using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DTO
{
    public partial class DAUSACH
    {
        public string DisplayName => $"{TenDauSach} ({MaDauSach})";
        public override string ToString()
        {
            return DisplayName.ToString();
        }
        public string TacGiaDaiDien => TACGIAs?.FirstOrDefault()?.TenTG ?? "";

        public string TacGiaDisplay => TACGIAs == null? "" : string.Join(", ", TACGIAs.Select(t => t.TenTG));
    }
}
