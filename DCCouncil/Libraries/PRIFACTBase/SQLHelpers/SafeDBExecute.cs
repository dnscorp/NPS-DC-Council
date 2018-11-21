using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIFACT.PRIFACTBase.SQLHelpers
{
    public class SafeDBExecute<T>
    {

        public delegate T Exec(DBContext context);

        readonly Exec _execObj = null;
        public SafeDBExecute(Exec execObj)
        {
            _execObj = execObj;
        }

        public T DoExecute(string strDbConnectionString)
        {
            DBContext dbContext = null;
            bool bException = false;
            Exception orginalException = null;
            try
            {
                dbContext = DBContext.GetDBContext(strDbConnectionString);
                return _execObj.Invoke(dbContext);
            }
            catch (Exception origEx)
            {
                bException = true;
                orginalException = origEx;
                throw;
            }
            finally
            {
                try
                {
                    if (bException)
                    {
                        if (dbContext != null)
                        {
                            dbContext.ReleaseDBContext(false);
                        }
                    }
                    else
                    {
                        dbContext.ReleaseDBContext(true);
                    }
                }
                catch (Exception cleanupExeception)
                {
                    if (dbContext != null)
                    {
                        dbContext.ReleaseDBContext(false);
                    }

                    if (orginalException != null)
                    {
                        throw new System.Exception("An error occured while rolling back a transcation  (See inner exception for original failed transaction) - [" + cleanupExeception.Message + "] " + cleanupExeception.StackTrace, orginalException);
                    }

                    throw;
                }
            }
        }
    }
}
