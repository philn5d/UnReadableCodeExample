namespace UnReadableCodeDataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Quote
    {
        [Key]
        [Column("quote_id")]
        public int Id { get; set; }

        [Column("item_id")]
        [ForeignKey("QuoteToItem")]
        public int ItemId { get; set; }

        [Association("QuoteToItem", "ItemId", "Id")]
        public Item Item { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column(TypeName = "money")]
        public decimal Cost { get; set; }

        [Column("account_id")]
        public int AccountId { get; set; }
    }
}
