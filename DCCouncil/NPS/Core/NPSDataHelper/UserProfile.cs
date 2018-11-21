using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRIFACT.PRIFACTBase.SQLHelpers;

namespace PRIFACT.DCCouncil.NPS.Core.NPSDataHelper
{
    [Serializable]
    public class UserProfile
    {
        public long UserProfileID
        {
            get;
            set;
        }
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public string FirstName
        {
            get;
            set;
        }

        public string LastName
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


        public static UserProfile Bind(System.Data.SqlClient.SqlDataReader reader)
        {
            UserProfile objUserProfile = new UserProfile();
            objUserProfile.CreatedDate = BasicConverter.DbToDateValue(reader["UserProfileCreatedDate"]);
            objUserProfile.FirstName = BasicConverter.DbToStringValue(reader["UserProfileFirstName"]);
            objUserProfile.LastName = BasicConverter.DbToStringValue(reader["UserProfileLastName"]);
            objUserProfile.UpdatedDate = BasicConverter.DbToNullableDateValue(reader["UserProfileUpdatedDate"]);
            objUserProfile.UserProfileID = BasicConverter.DbToLongValue(reader["UserProfileID"]);
            return objUserProfile;
        }
    }
}
