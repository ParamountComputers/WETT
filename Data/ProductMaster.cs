using System;
using System.Collections.Generic;

namespace WETT.Data;

public partial class ProductMaster
{
    public long ProductId { get; set; }

    public long SupplierId { get; set; }

    public string LobCode { get; set; }

    public bool ActiveFlag { get; set; }

    public string InsertUserId { get; set; }

    public DateTime InsertTimestamp { get; set; }

    public string UpdateUserId { get; set; }

    public DateTime UpdateTimestamp { get; set; }

    public virtual ICollection<CustomerOrderDetail> CustomerOrderDetails { get; } = new List<CustomerOrderDetail>();

    public virtual ICollection<Inventory> Inventories { get; } = new List<Inventory>();

    public virtual ICollection<ProductRegulatorCan> ProductRegulatorCans { get; } = new List<ProductRegulatorCan>();

    public virtual ICollection<ProductRegulatorLiq> ProductRegulatorLiqs { get; } = new List<ProductRegulatorLiq>();
}
