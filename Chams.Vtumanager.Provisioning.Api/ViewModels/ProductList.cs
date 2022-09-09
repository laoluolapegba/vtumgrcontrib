using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Api.ViewModels
{
    public class ProductList
    {
        /// <summary>
        /// Unique Product identifier
        /// </summary>
        
        public string ProductCode { get; set; }
        /// <summary>
        /// Product name
        /// </summary>
        
        public string ProductDescription { get; set; }
        /// <summary>
        /// 1= VTU, 2= DataBundle
        /// </summary>
        
        public string ProductType { get; set; }
        /// <summary>
        /// Price in Naira
        /// </summary>
        [Column("price")]
        public decimal Price { get; set; }


        /// <summary>
        /// validity in hours
        /// </summary>
        
        public int Validity { get; set; }
        public string ServiceProviderName { get; set; }
    }
}
