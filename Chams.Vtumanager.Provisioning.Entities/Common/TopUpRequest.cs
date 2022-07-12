﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.Common
{
    [Table("topup_transaction_log")]
    public class TopUpTransactionLog
    {
        

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("idtopup_transaction_log")]
        public long RecordId { get; set; }
        [Column("tran_date")]
        public DateTime tran_date { get; set; }
        [Column("transref")]
        public string transref { get; set; }
        [Column("serviceproviderid")]
        public int serviceproviderid { get; set; }
        [Column("serviceprovidername")]
        public string serviceprovidername { get; set; }
        [Column("transtype")]
        public int transtype { get; set; }
        [Column("productid")]
        public string productid { get; set; }
        [Column("msisdn")]
        public string msisdn { get; set; }
        [Column("transamount")]
        public decimal transamount { get; set; }
        [Column("channelid")]
        public int channelid { get; set; }
        [Column("sourcesystem")]
        public string sourcesystem { get; set; }
        [Column("is_processed")]
        public int IsProcessed { get; set; }
        [Column("count_retries")]
        public int CountRetries { get; set; }
        [Column("processed_date")]
        public DateTime? ProcessedDate { get; set; }
        [Column("error_code")]
        public string ErrorCode { get; set; }
        [Column("error_desc")]
        public string ErrorDesc { get; set; }
    }
}