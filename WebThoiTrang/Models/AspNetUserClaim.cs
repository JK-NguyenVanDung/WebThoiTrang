//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebThoiTrang.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class AspNetUserClaim
    {
        [Required]

        public int Id { get; set; }
        [Required]

        public string UserId { get; set; }


        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }


        [Required]
        public virtual AspNetUser AspNetUser { get; set; }


    }
}
