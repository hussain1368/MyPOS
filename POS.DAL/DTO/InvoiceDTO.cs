﻿using System;
using System.Collections.Generic;

namespace POS.DAL.DTO
{
    public class InvoiceDTO
    {
        public int Id { get; set; }
        public string SerialNum { get; set; }
        public byte InvoiceType { get; set; }
        public int WarehouseId { get; set; }
        public int TreasuryId { get; set; }
        public int? AccountId { get; set; }
        public int CurrencyId { get; set; }
        public double CurrencyRate { get; set; }
        public DateTime IssueDate { get; set; }
        public byte PaymentType { get; set; }
        public string Note { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public IList<InvoiceItemDTO> Items { get; set; }

        public InvoiceDTO() => Items = new List<InvoiceItemDTO>();
    }
}
