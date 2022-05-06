﻿namespace Brimborium.RowVersion.Entity;

public interface IEntityWithDataEntityVersion
    : IEntityWithVersion {

    // from IEntityWithVersion 
    // long EntityVersion { get; set; }

    string DataVersion { get; set; }

    DataEntitiyVersion DataEntityVersion { get; set; }
}