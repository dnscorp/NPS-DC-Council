using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIFACT.DCCouncil.NPS.Core.NPSApi.SessionHelper
{
    [Serializable]
    public class NPSSessionData
    {
        private readonly Guid _npsSessionGuid;

        public object LoggedInUser
        {
            get;
            set;
        }
        
        //For serialization support
        private NPSSessionData()
        {

        }

        public NPSSessionData(Guid guid)
        {
            _npsSessionGuid = guid;
        }

        public Guid GetSessionUniqueKey()
        {
            return _npsSessionGuid;
        }
    }
}
