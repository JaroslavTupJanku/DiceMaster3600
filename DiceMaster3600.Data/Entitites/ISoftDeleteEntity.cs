using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceMaster3600.Data.Entitites
{
    public interface ISoftDeleteEntity
    {
        DateTime? DeletedDate { get; set; }
    }
}
