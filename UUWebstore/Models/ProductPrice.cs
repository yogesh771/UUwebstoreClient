//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UUWebstore.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductPrice
    {
        public long productPriceId { get; set; }
        public Nullable<long> productId { get; set; }
        public Nullable<double> price { get; set; }
        public Nullable<System.DateTime> lastDatePrice { get; set; }
        public Nullable<double> tax { get; set; }
        public long createdBy { get; set; }
        public System.DateTime createdDate { get; set; }
        public long modifiedBy { get; set; }
        public System.DateTime modifiedDate { get; set; }
        public bool isActive { get; set; }
        public bool isDelete { get; set; }
    }
}