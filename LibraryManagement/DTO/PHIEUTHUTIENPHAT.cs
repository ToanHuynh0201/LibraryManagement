//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibraryManagement.DTO
{
    using System;
    using System.Collections.Generic;
    
    public partial class PHIEUTHUTIENPHAT
    {
        public int id { get; set; }
        public string SoPhieuThu { get; set; }
        public int MaDG { get; set; }
        public System.DateTime NgayThu { get; set; }
        public int SoTienThu { get; set; }
    
        public virtual DOCGIA DOCGIA { get; set; }
    }
}
