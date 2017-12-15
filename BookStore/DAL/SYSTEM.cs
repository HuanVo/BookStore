namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SYSTEMS")]
    public partial class SYSTEM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int system_id { get; set; }

        [StringLength(50)]
        public string config_value { get; set; }
    }
}
