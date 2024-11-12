using System;

namespace DiceMaster3600.Data.Entitites
{
    public interface ISoftDeleteEntity
    {
        DateTime? DeletedDate { get; set; }
    }
}
