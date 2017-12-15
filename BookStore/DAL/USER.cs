namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("USERS")]
    public partial class USER
    {
        [Key]
        [StringLength(100)]
        public string login_id { get; set; }

        [StringLength(500)]
        public string login_pass { get; set; }

        public int? cnt_login_error { get; set; }

        public DateTime? date_login_error { get; set; }

        public int? dest_flg { get; set; }

        public long? code_cst { get; set; }

        [StringLength(500)]
        public string loginkey { get; set; }

        public int? use_flg { get; set; }

        [StringLength(30)]
        public string mail { get; set; }

        public DateTime? date_created { get; set; }

        public DateTime? date_modify { get; set; }

        [StringLength(50)]
        public string member_modify { get; set; }

        public virtual CUSTOMER CUSTOMER { get; set; }
    }
}
