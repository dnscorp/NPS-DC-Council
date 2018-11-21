using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums
{
    public enum TelephoneChargesStatusBeforeImport
    {
        Valid = 0,
        InvalidTotalCurrentCharges = 1,
        InvalidTransactionDate = 2,
        InvalidWirelessNumber=3
    }
}
