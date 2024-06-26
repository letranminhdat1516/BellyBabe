using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class Payment
{
    public int PaymentId { get; set; }

    public string? PaymentContent { get; set; }

    public string? PaymentCurrency { get; set; }

    public string? PaymentRefId { get; set; }

    public decimal? RequiredAmount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public DateTime? ExpireDate { get; set; }

    public string? PaymentLanguage { get; set; }

    public string? MerchantId { get; set; }

    public string? PaymentDestinationId { get; set; }

    public decimal? PaidAmount { get; set; }

    public string? PaymentStatus { get; set; }

    public string? PaymentLastMessage { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? LastUpdatedBy { get; set; }

    public DateTime? LastUpdatedAt { get; set; }

    public DateTime? PayTime { get; set; }

    public decimal? Amount { get; set; }

    public string? ExternalTransactionCode { get; set; }

    public int? ProductId { get; set; }

    public int? OrderId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
