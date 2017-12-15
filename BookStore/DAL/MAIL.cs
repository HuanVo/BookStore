namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MAIL")]
    public partial class MAIL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int mail_id { get; set; }

        [StringLength(30)]
        public string from_address { get; set; }

        [StringLength(100)]
        public string subjects { get; set; }

        [StringLength(500)]
        public string body { get; set; }
    }
}
