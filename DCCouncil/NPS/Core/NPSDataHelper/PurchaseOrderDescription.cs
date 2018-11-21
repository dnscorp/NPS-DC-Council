using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRIFACT.PRIFACTBase.SQLHelpers;

namespace PRIFACT.DCCouncil.NPS.Core.NPSDataHelper
{
    public class PurchaseOrderDescription
    {
        public long? DescriptionID
        {
            get;
            set;
        }

        public string PONumber
        {
            get;
            set;
        }

        public string DescriptionText
        {
            get;
            set;
        }

        public DateTime? CreatedDate
        {
            get;
            set;
        }

        public DateTime? UpdatedDate
        {
            get;
            set;
        }

        internal static PurchaseOrderDescription Bind(SqlDataReader reader)
        {
            PurchaseOrderDescription objPurchaseOrderDescription = new PurchaseOrderDescription();
            objPurchaseOrderDescription.CreatedDate = BasicConverter.DbToNullableDateValue(reader["PurchaseOrderDescriptionCreatedDate"]);
            objPurchaseOrderDescription.DescriptionID = BasicConverter.DbToNullableLongValue(reader["PurchaseOrderDescriptionDescriptionID"]);
            objPurchaseOrderDescription.DescriptionText = BasicConverter.DbToStringValue(reader["PurchaseOrderDescriptionDescriptionText"]);
            objPurchaseOrderDescription.PONumber = BasicConverter.DbToStringValue(reader["PurchaseOrderDescriptionPONumber"]);
            objPurchaseOrderDescription.UpdatedDate = BasicConverter.DbToNullableDateValue(reader["PurchaseOrderDescriptionUpdatedDate"]);
            return objPurchaseOrderDescription;
        }

    }
}
