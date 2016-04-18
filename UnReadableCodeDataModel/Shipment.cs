namespace UnReadableCodeDataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Shipment
    {
        [Key]
        public int shipment_id { get; set; }

        public int quote_id { get; set; }

        public int itemid { get; set; }

        public DateTime date { get; set; }
    }
}
