using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.SortFields
{
    public enum ExpenditureSortFields
    {
        DateOfTransaction = 0,
        VendorName = 1,
        Description = 2,
        OBJCode = 3,
        IndexCode = 4,
        PCA = 5,
        Amount = 6,        
        Comments = 8
    }

    public enum RecurringSortFields
    {
        VendorName = 0,
        Description = 1,
        CategoryType = 2,
        Amount = 3,
        Comments = 4
    }
}
