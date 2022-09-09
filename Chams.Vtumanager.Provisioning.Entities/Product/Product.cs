using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.Product
{
    /// <summary>
    /// 
    /// </summary>
    [Table("vtu_telco_products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        /// <summary>
        /// Unique Product identifier
        /// </summary>
        [Column("product_id")]
        public string ProductCode { get; set; }
        /// <summary>
        /// Product name
        /// </summary>
        [Column("product_name")] 
        public string ProductDescription { get; set; }
        /// <summary>
        /// 1= VTU, 2= DataBundle
        /// </summary>
        [Column("product_type")]
        public string ProductType { get; set; }
        /// <summary>
        /// Price in Naira
        /// </summary>
        [Column("price")] 
        public string Price { get; set; }

        
        [Column("service_provider_id")] 
        public int ServiceProviderId { get; set; }
        /// <summary>
        /// validity in hours
        /// </summary>
        [Column("validity")]
        public int Validity { get; set; }
    }
}
