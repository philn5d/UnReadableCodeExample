namespace UnReadableCodeDataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ITEMS")]
    public partial class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("item_id")]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Column("item_name")]
        public string Name { get; set; }
    }
}
