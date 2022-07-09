using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Entities.EtopUp 
{

    [Table("INV_EVC_TRANSACTION_LOG")]
    public class EvcTransactionLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("RECORD_ID")]
        public long RecordId { get; set; }
        [Column("TRANSACTION_ID")]
        public string transId { get; set; }

        [Column("SOURCE_EVC_ACCT")]
        public string SourceAccount { get; set; }
        [Column("DEST_EVC_ACCT")]
        public string DestAccount { get; set; }
        
        [Column("ORDER_NUMBER")]
        public string OrderNo { get; set; }
        //public string PartnerEvcAcctCode { get; set; }
        //public string SubscriberMsisdn { get; set; }
        [Column("TRANSACTION_AMT")]
        public decimal TranAmount { get; set; }

        [Column("TRAN_DATE")]
        public DateTime TranDate { get; set; }
        [Column("TRANSACTION_TYPE")]
        public string TranType { get; set; }
        [Column("IS_PROCESSED")]
        public int IsProcessed { get; set; }
        [Column("PROCESSED_DATE")]
        public DateTime? ProcessedDate { get; set; }
        [Column("COUNT_RETRIES")]
        public int CountRetries { get; set; }
        [Column("ERROR_CODE")]
        public string ErrorCode { get; set; }
        [Column("ERROR_DESC")]
        public string ErrorDesc { get; set; }

        [Column("TRANSACTION_DESC")]
        public string TransactionDesc { get; set; }
        [Column("TRANSACTION_CLASS")]
        public int TransactionClass { get; set; }




    }
}
