using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PRIFACT.PRIFACTBase.SQLHelpers
{
    public class BasicConverter
    {
        public static char DbToCharValue(object obj)
        {

            if (obj == DBNull.Value)
            {
                throw new System.Exception("Cannot convert DBNull to char value");
            }

            if (obj.ToString().Length != 1)
            {
                throw new System.Exception("Invalid length - Unable to convert to char value");
            }

            return obj.ToString()[0];
        }

        public static object CharToDbValue(char c)
        {
            return c.ToString();
        }

        public static char[] DbToNCharValue(object obj, int numChars)
        {

            if (obj == DBNull.Value)
            {
                return null;
            }

            if (obj.ToString().Length != numChars)
            {
                throw new System.Exception("Invalid length - Unable to convert to char value");
            }

            return obj.ToString().ToCharArray();
        }



        public static string DbToStringValue(object obj)
        {
            if (obj == DBNull.Value)
            {
                return null;
            }
            return obj.ToString();
        }

        public static XElement DbToXElementValue(object obj)
        {
            if (obj == DBNull.Value)
            {
                return null;
            }
            return XElement.Parse(obj.ToString());
        }

        public static Guid DbToGuidValue(object obj)
        {
            if (obj == DBNull.Value)
            {
                throw new System.Exception("Cannot convert DBNull to guid value");
            }
            return new Guid(obj.ToString());
        }

        public static bool DbToBoolValue(object obj)
        {
            if (obj == DBNull.Value)
            {
                return false;
            }

            string str = obj.ToString();
            try
            {
                return (Convert.ToInt32(obj.ToString()) == 0) ? false : true;
            }
            catch (FormatException)
            {
                return Convert.ToBoolean(str);
            }
        }

        public static bool? DbToNullableBoolValue(object obj)
        {
            if (obj == DBNull.Value)
            {
                return null;
            }

            string str = obj.ToString();
            try
            {
                return (Convert.ToInt32(obj.ToString()) == 0) ? false : true;
            }
            catch (FormatException)
            {
                return Convert.ToBoolean(str);
            }
        }

        public static Guid? DbToNullableGuidValue(object obj)
        {
            if (obj == DBNull.Value)
            {
                return null;
            }
            return new Guid(obj.ToString());
        }

        public static int DbToIntValue(object obj)
        {
            if (obj == DBNull.Value)
            {
                return 0;
            }

            if (obj.ToString().Length == 0)
            {
                return 0;
            }

            return Convert.ToInt32(obj.ToString());
        }

        public static short DbToShortValue(object obj)
        {
            if (obj == DBNull.Value)
            {
                return 0;
            }

            if (obj.ToString().Length == 0)
            {
                return 0;
            }

            return Convert.ToInt16(obj.ToString());
        }

        public static int? DbToNullableIntValue(object obj)
        {
            if (obj == DBNull.Value)
            {
                return null;
            }

            if (obj.ToString().Length == 0)
            {
                return null;
            }

            return Convert.ToInt32(obj.ToString());
        }

        public static long DbToLongValue(object obj)
        {
            if (obj == DBNull.Value)
            {
                return 0;
            }

            if (obj.ToString().Length == 0)
            {
                return 0;
            }

            return Convert.ToInt64(obj.ToString());
        }

        public static long? DbToNullableLongValue(object obj)
        {
            if (obj == DBNull.Value)
            {
                return null;
            }

            if (obj.ToString().Length == 0)
            {
                return null;
            }

            return Convert.ToInt64(obj.ToString());
        }

        public static ulong DbToULongValue(object obj)
        {
            if (obj == DBNull.Value)
            {
                return 0;
            }

            if (obj.ToString().Length == 0)
            {
                return 0;
            }

            return Convert.ToUInt64(obj.ToString());
        }

        public static double DbToDoubleValue(object obj)
        {
            if (obj == DBNull.Value)
            {
                return 0;
            }

            if (obj.ToString().Length == 0)
            {
                return 0;
            }

            return Convert.ToDouble(obj.ToString());
        }

        public static double? DbToNullableDoubleValue(object obj)
        {
            if (obj == DBNull.Value)
            {
                return null;
            }

            if (obj.ToString().Length == 0)
            {
                return 0;
            }

            return Convert.ToDouble(obj.ToString());
        }

        public static DateTime DbToDateValue(object obj)
        {
            if (obj == DBNull.Value)
            {
                return new DateTime();
            }

            if (obj.ToString().Length == 0)
            {
                return new DateTime();
            }

            return Convert.ToDateTime(obj.ToString());
        }

        public static DateTime? DbToNullableDateValue(object obj)
        {
            if (obj == DBNull.Value)
            {
                return null;
            }

            if (obj.ToString().Length == 0)
            {
                return null;
            }

            return Convert.ToDateTime(obj.ToString());
        }


        public static object NCharToDbValue(char[] c)
        {
            if (c == null)
            {
                return DBNull.Value;
            }

            //Our db is designed for the below criteria
            if (c.Length == 0)
            {
                return DBNull.Value;
            }

            return c;

        }

        public static object StringToDbValue(string str)
        {
            if (str == null)
            {
                return DBNull.Value;
            }

            str = str.Trim();

            //Our db is designed for the below criteria
            if (str.Length == 0)
            {
                return DBNull.Value;
            }

            return str;
        }

        public static object IntToDbValue(int val)
        {
            if (val == 0)
                return DBNull.Value;
            return val;
        }

        public static object NullableIntToDbValue(int? val)
        {
            if (!val.HasValue)
                return DBNull.Value;
            return val.Value;
        }

        public static object LongToDbValue(long val)
        {
            if (val == 0)
                return DBNull.Value;
            return val;
        }

        public static object NullableLongToDbValue(long? val)
        {
            if (!val.HasValue)
                return DBNull.Value;
            return val.Value;
        }

        public static object DoubleToDbValue(double val)
        {
            if (val == 0)
                return DBNull.Value;
            return val;
        }

        public static object NullableDoubleToDbValue(double? val)
        {
            if (!val.HasValue)
                return DBNull.Value;
            return val;
        }

        public static object ULongToDbValue(ulong val)
        {
            if (val == 0)
                return DBNull.Value;
            return val;
        }

        public static int BoolToDbValue(bool bVal)
        {
            return bVal ? 1 : 0;
        }

        public static object NullableBoolToDbValue(bool? bVal)
        {
            if (!bVal.HasValue)
                return DBNull.Value;

            return bVal.Value ? 1 : 0;
        }

        public static object NullableGuidToDbValue(Guid? gVal)
        {
            if (!gVal.HasValue)
                return DBNull.Value;

            return gVal.Value;
        }

        public static char? DbToNullableCharValue(object obj)
        {
            if (obj == DBNull.Value)
            {
                return null;
            }

            if (obj.ToString().Length == 0)
            {
                return null;
            }

            return Convert.ToChar(obj);
        }

        public static object NullableCharToDBValue(char? val)
        {
            if (val.HasValue)
            {
                return val.Value;
            }
            else
            {
                return DBNull.Value;
            }
        }

        public static object DateToDbValue(DateTime dt)
        {
            if (dt == DateTime.MinValue)
                return DBNull.Value;
            return dt;
        }

        public static object NullableDateToDbValue(DateTime? dt)
        {
            if (!dt.HasValue)
                return DBNull.Value;
            return dt.Value;
        }

        public static object GuidToDbValue(Guid val)
        {
            return val;
        }

        public static string NStr(object obj)
        {
            return NStr(obj, "");
        }


        public static string NStr(object obj, string replacementStringIfNull)
        {
            if (obj == null)
                return replacementStringIfNull;

            return obj.ToString();
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}
