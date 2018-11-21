using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIFACT.PRIFACTBase.SQLHelpers
{
    public class DBContext
    {
        public class InternalContext
        {
            public SqlCommand m_ContextTransactionCommand;
            public SqlConnection m_ContextTransactionConnection;
            public SqlTransaction m_ContextTransaction;
            public bool m_bContextTransactionInProgress;
            public int m_ActiveTransactionOpenCount;
            public System.Collections.Generic.List<DBContext> m_activeDbContextList;
        }


        [ThreadStatic]
        private static Dictionary<string, InternalContext> allInternalContextsInThread;

        private static InternalContext _GetC(string dbRef)
        {
            if (allInternalContextsInThread == null)
            {
                allInternalContextsInThread = new Dictionary<string, InternalContext>();
            }

            InternalContext c = null;
            if (allInternalContextsInThread.ContainsKey(dbRef))
            {
                c = allInternalContextsInThread[dbRef];
            }

            if (c == null)
            {
                allInternalContextsInThread.Add(dbRef, new InternalContext());
                c = allInternalContextsInThread[dbRef];
            }
            return c;
        }


        //Member variables
        private readonly bool m_bStartedNewTransaction = false;
        private bool m_bReleased = false;
        private readonly string m_strStackOnCreation = "";

        private void _Log(string str)
        {
            //Logger.DBLogger.LogInfo("[DBCONTEXT] " + str + " Count=" + m_ActiveTransactionOpenCount + " Threadid = " + System.Threading.Thread.CurrentThread.ManagedThreadId);
        }

        public string m_dbRef = null;

        public static DBContext GetDBContext()
        {
            return GetDBContext(ConfigHelpers.ConfigReader.GetValue("DBConnectionString"));
        }

        /// <summary>
        /// Starts a new transation or get current transation.
        /// </summary>
        /// <returns></returns>
        public static DBContext GetDBContext(string dbConnectionString)
        {
            DBContext objDBContext = null;
            objDBContext = _GetDBContext(dbConnectionString);
            return objDBContext;
        }

        private static DBContext _GetDBContext(string dbConnectionString)
        {
            if (string.IsNullOrEmpty(dbConnectionString))
                dbConnectionString = ConfigHelpers.ConfigReader.GetValue("DBConnectionString");

            InternalContext c = _GetC(dbConnectionString);

            if (c.m_bContextTransactionInProgress)
            {
                return new DBContext(false, dbConnectionString);
            }

            try
            {
                c.m_ContextTransactionConnection = new SqlConnection(dbConnectionString);
                c.m_ContextTransactionConnection.Open();
                c.m_ContextTransaction = c.m_ContextTransactionConnection.BeginTransaction();
                c.m_ContextTransactionCommand = new SqlCommand();
                c.m_ContextTransactionCommand.CommandTimeout = 600;
                c.m_ContextTransactionCommand.Connection = c.m_ContextTransactionConnection;
                c.m_ContextTransactionCommand.Transaction = c.m_ContextTransaction;
                c.m_bContextTransactionInProgress = true;
            }
            catch
            {
                //Reset the flag that we just set (to default)
                c.m_bContextTransactionInProgress = false;

                if (c.m_ContextTransaction != null)
                {
                    try
                    {
                        c.m_ContextTransaction.Dispose();
                        c.m_ContextTransaction = null;
                    }
                    catch
                    {
                        //Ignore any erros when we try to dispose transaction object
                    }
                }

                if (c.m_ContextTransactionConnection != null)
                {
                    try
                    {
                        c.m_ContextTransactionConnection.Close();
                        c.m_ContextTransactionConnection.Dispose();
                        c.m_ContextTransactionConnection = null;
                    }
                    catch
                    {
                        //Ignore any exceptions when we try to close 
                        //since, an exception here means that the connection was 
                        //not in open state
                    }
                }

                if (c.m_ContextTransactionCommand != null)
                {
                    try
                    {
                        c.m_ContextTransactionCommand.Dispose();
                        c.m_ContextTransactionCommand = null;
                    }
                    catch
                    {
                        //Ignore any errors disposing the command object
                    }
                }

                //IMPORTANT: While we ignore excecptions that arise when we attempt
                //to cleanup, never ignore the original exeception. Throw the original exception 
                //back to caller anyway

                throw;
            }
            return new DBContext(true, dbConnectionString);
        }

        public static void CloseReader(SqlDataReader reader)
        {
            try
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch
            {
                //Ignore any erros when we try to dispose/close
            }
        }

        public static int TrasactionOpenCount
        {
            get
            {
                int total = 0;

                foreach (string dbRef in allInternalContextsInThread.Keys)
                {
                    total += allInternalContextsInThread[dbRef].m_ActiveTransactionOpenCount;
                }

                return total;
            }

        }

        public static System.Collections.Generic.List<DBContext> ActiveContextList
        {
            get
            {
                return allInternalContextsInThread.Values.SelectMany(x => x.m_activeDbContextList).ToList();
            }
        }

        private DBContext(bool bStartedNewTransaction, string dbRefString)
        {
            InternalContext c = _GetC(dbRefString);

            try
            {
                m_dbRef = dbRefString;

                c.m_ActiveTransactionOpenCount++;

                m_strStackOnCreation = new StackTrace().ToString();

                if (bStartedNewTransaction)
                {
                    //_Log("Starting new transaction");
                }
                else
                {
                    //_Log("Appending to existing transaction");
                }

                m_bStartedNewTransaction = bStartedNewTransaction;
                ResetForNextQuery();

                if (c.m_activeDbContextList == null)
                    c.m_activeDbContextList = new System.Collections.Generic.List<DBContext>();

                c.m_activeDbContextList.Add(this);
            }
            catch
            {
                //Any exceptions here is cleaned up here and the exception passed to caller
                try
                {
                    c.m_ActiveTransactionOpenCount--;
                    if (c.m_activeDbContextList != null)
                    {
                        c.m_activeDbContextList.Remove(this);
                    }
                }
                catch
                {
                    //Ignore any exceptions here since these doenst have any memory or handles
                }

                m_bReleased = true;
                throw;
            }
        }

        public SqlCommand ContextSqlCommand
        {
            get
            {
                InternalContext c = _GetC(this.m_dbRef);
                return c.m_ContextTransactionCommand;
            }
        }

        public string InstanceStackTrace
        {
            get
            {
                return m_strStackOnCreation;
            }

        }

        /// <summary>
        /// Commits or rollbacks current transaction depends on the input.
        /// </summary>
        /// <param name="bCommit"></param>
        public void ReleaseDBContext(bool bCommit)
        {
            InternalContext c = _GetC(this.m_dbRef);

            if (m_bReleased)
                return;

            m_bReleased = true;

            c.m_ActiveTransactionOpenCount--;

            if (c.m_activeDbContextList != null)
                c.m_activeDbContextList.Remove(this);

            if (c.m_bContextTransactionInProgress)
            {
                if (!m_bStartedNewTransaction)  //If we dint start a new trans, we dont do commit..
                    return;

                try
                {
                    if (bCommit)
                    {
                        c.m_ContextTransaction.Commit();
                    }
                    else
                    {
                        c.m_ContextTransaction.Rollback();
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {

                    //Attempt to cleanup Transaction object
                    try
                    {
                        c.m_ContextTransaction.Dispose();

                    }
                    catch { }


                    //Attempt to cleanup Connection object
                    try
                    {
                        c.m_ContextTransactionConnection.Close();
                        c.m_ContextTransactionConnection.Dispose();
                    }
                    catch { }

                    //Attempt to cleanup Command object
                    try
                    {
                        c.m_ContextTransactionCommand.Dispose();
                    }
                    catch { }


                    c.m_ContextTransactionConnection = null;
                    c.m_ContextTransactionCommand = null;
                    c.m_ContextTransaction = null;
                    c.m_bContextTransactionInProgress = false;
                }

            }
            else
            {
                //Unexpected!  
                throw new System.Exception("There is no context transaction in progress");
            }


            return;
        }

        public void ResetForNextQuery()
        {
            InternalContext c = _GetC(this.m_dbRef);
            if (c.m_bContextTransactionInProgress)
            {
                c.m_ContextTransactionCommand.CommandText = "";
                c.m_ContextTransactionCommand.CommandType = CommandType.Text;
                c.m_ContextTransactionCommand.Parameters.Clear();
                return;
            }
            throw new System.Exception("There is no context transaction in progress");
        }
    }
}
