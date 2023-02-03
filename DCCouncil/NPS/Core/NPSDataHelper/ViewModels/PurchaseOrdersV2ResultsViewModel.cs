using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.ViewModels
{
    public class PurchaseOrdersV2ResultsViewModel
    {
        public int RowCount {get;set;}
        public List<PurchaseOrdersV2ViewModel> Items {get;set;}
    }
}
