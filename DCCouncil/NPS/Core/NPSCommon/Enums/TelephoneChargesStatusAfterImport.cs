using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums
{
    public enum TelephoneChargesStatusAfterImport
    {
        NotImported = 0,
        Success = 1,
        UserDoesNotExist = 2,
        MoreThanOneUserExists = 3,
        DateOutsideCurrentFiscalYear = 4,
        BudgetNotSpecified = 5,
        ImportedDateNotMonthEnd = 6
    }
}
