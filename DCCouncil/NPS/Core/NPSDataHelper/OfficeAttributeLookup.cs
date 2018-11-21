using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRIFACT.PRIFACTBase.SQLHelpers;

namespace PRIFACT.DCCouncil.NPS.Core.NPSDataHelper
{
    public class OfficeAttributeLookup
    {
        public long OfficeAttributeLookupID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public DateTime CreatedDate
        {
            get;
            set;
        }

        public DateTime? UpdatedDate
        {
            get;
            set;
        }


        internal static OfficeAttributeLookup Bind(System.Data.SqlClient.SqlDataReader reader)
        {
            OfficeAttributeLookup objLookup = new OfficeAttributeLookup();
            objLookup.CreatedDate = BasicConverter.DbToDateValue(reader["OfficeAttributeLookupCreatedDate"]);
            objLookup.OfficeAttributeLookupID = BasicConverter.DbToLongValue(reader["OfficeAttributeLookupID"]);
            objLookup.Name = BasicConverter.DbToStringValue(reader["OfficeAttributeLookupName"]);
            objLookup.Description = BasicConverter.DbToStringValue(reader["OfficeAttributeLookupDescription"]);
            objLookup.UpdatedDate = BasicConverter.DbToNullableDateValue(reader["OfficeAttributeLookupUpdatedDate"]);
            return objLookup;
        }
    }
}
