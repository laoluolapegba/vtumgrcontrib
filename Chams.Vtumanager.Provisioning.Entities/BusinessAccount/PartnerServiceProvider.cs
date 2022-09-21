using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.BusinessAccount
{
    [Table("partner_service_provider")]
    public class PartnerServiceProvider
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("partner_id")]
        public int PartnerId { get; set; }
        [Column("provider_id")]
        public int ServiceProviderId { get; set; }
        [Column("product_category_id")]
        public int ProductCategoryid { get; set; }
        [Column("commission_percentage")]
        public decimal? CommissionPct { get; set; }

    }
}
