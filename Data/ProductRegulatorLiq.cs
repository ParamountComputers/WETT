﻿using System;
using System.Collections.Generic;

namespace WETT.Data;

public partial class ProductRegulatorLiq
{
    public long ProductId { get; set; }

    public string RegulatorCode { get; set; }

    public string ProvinceCode { get; set; }

    public string Sku { get; set; }

    public string Description { get; set; }

    public string Description2 { get; set; }

    public decimal SingleWeight { get; set; }

    public decimal ContainerWeight { get; set; }

    public decimal CaseWeight { get; set; }

    public int PackSize { get; set; }

    public string HlSingle { get; set; }

    public decimal HlContainer { get; set; }

    public decimal HlCase { get; set; }

    public bool ActiveFlag { get; set; }

    public string InsertUserId { get; set; }

    public virtual ProductMaster Product { get; set; }
}
