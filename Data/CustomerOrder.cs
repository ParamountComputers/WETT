using System;
using System.Collections.Generic;

namespace WETT.Data;

public partial class CustomerOrder
{
    public long CustomerOrderId { get; set; }

    public long CustomerId { get; set; }

    public long OrderSourceId { get; set; }

    public string LobCode { get; set; }

    public string RegulatorCode { get; set; }

    public long? SupplierId { get; set; }

    public string OrderNumber { get; set; }

    public DateTime DateOrdered { get; set; }

    public DateTime DateReceived { get; set; }

    public DateTime DateShipped { get; set; }

    public long CustomerOrderStatusId { get; set; }

    public long CarrierId { get; set; }

    public string Driver { get; set; }

    public string DsSlipNumber { get; set; }

    public DateTime DeliveryReqDate { get; set; }

    public string SpecialInstructions { get; set; }

    public string InsertUserId { get; set; }

    public DateTime InsertTimestamp { get; set; }

    public string UpdateUserId { get; set; }

    public DateTime UpdateTimestamp { get; set; }

    public virtual Carrier Carrier { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual ICollection<CustomerOrderDetail> CustomerOrderDetails { get; } = new List<CustomerOrderDetail>();

    public virtual CustomerOrderStatus CustomerOrderStatus { get; set; }

    public virtual ICollection<InventoryTx> InventoryTxes { get; } = new List<InventoryTx>();

    public virtual OrderSource OrderSource { get; set; }

    public virtual Supplier Supplier { get; set; }
}
