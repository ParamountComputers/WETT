using System;
using System.Collections.Generic;

namespace WETT.Data;

public partial class InventoryTx
{
    public long InventoryTxId { get; set; }

    public string StockAdjCode { get; set; }

    public long InventoryTxTypeId { get; set; }

    public long? FromInventoryLocationId { get; set; }

    public long? ToInventoryLocationId { get; set; }

    public long? SupplierId { get; set; }

    public long? CustomerOrderId { get; set; }

    public long? TruckingCompanyId { get; set; }

    public long? ShippingLocationId { get; set; }

    public string PurchaseOrder { get; set; }

    public string Seal { get; set; }

    public string Probill { get; set; }

    public DateTime Date { get; set; }

    public string PortOfEntry { get; set; }

    public string TransactionNo { get; set; }

    public string PreviousTransactionNo { get; set; }

    public string Comments { get; set; }

    public string InsertUserId { get; set; }

    public DateTime InsertTimestamp { get; set; }

    public string UpdateUserId { get; set; }

    public DateTime UpdateTimestamp { get; set; }

    public virtual CustomerOrder CustomerOrder { get; set; }

    public virtual InventoryLocation FromInventoryLocation { get; set; }

    public virtual ICollection<InventoryTxDetail> InventoryTxDetails { get; } = new List<InventoryTxDetail>();

    public virtual InventoryTxType InventoryTxType { get; set; }

    public virtual ShippingLocation ShippingLocation { get; set; }

    public virtual Supplier Supplier { get; set; }

    public virtual InventoryLocation ToInventoryLocation { get; set; }

    public virtual TruckingCompany TruckingCompany { get; set; }
}
