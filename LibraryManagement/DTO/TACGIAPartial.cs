﻿//------------------------------------------------------------------------------
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

    public partial class TACGIA
    {
        public string DisplayName => $"{TenTG} ({MaTG})";
        public override string ToString()
        {
            return DisplayName.ToString();
        }
        public override bool Equals(object obj)
        {
            if (obj is TACGIA other)
                return this.id == other.id;
            return false;
        }
        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

    }
}
