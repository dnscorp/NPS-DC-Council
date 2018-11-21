using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRIFACT.PRIFACTBase.SQLHelpers;

namespace PRIFACT.DCCouncil.NPS.Core.NPSDataHelper
{
    public class ExpenditureCategoryAttributeLookup
    {
        public long ExpenditureCategoryAttributeLookupID
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

        internal static ExpenditureCategoryAttributeLookup Bind(System.Data.SqlClient.SqlDataReader reader)
        {
            ExpenditureCategoryAttributeLookup objLookup = new ExpenditureCategoryAttributeLookup();
            objLookup.CreatedDate = BasicConverter.DbToDateValue(reader["ExpenditureCategoryAttributeLookupCreatedDate"]);
            objLookup.ExpenditureCategoryAttributeLookupID = BasicConverter.DbToLongValue(reader["ExpenditureCategoryAttributeLookupID"]);
            objLookup.Name = BasicConverter.DbToStringValue(reader["ExpenditureCategoryAttributeLookupName"]);
            objLookup.Description = BasicConverter.DbToStringValue(reader["ExpenditureCategoryAttributeLookupDescription"]);
            objLookup.UpdatedDate = BasicConverter.DbToNullableDateValue(reader["ExpenditureCategoryAttributeLookupUpdatedDate"]);
            return objLookup;
        }
    }
}
