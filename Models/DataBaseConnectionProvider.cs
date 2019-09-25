
using Microsoft.Extensions.Configuration;


namespace ELOTEC
{
    public class DataBaseConnectionProvider
    {
        // IConfiguration _iconfiguration;

        public static string _strconnection = string.Empty;
        public static string _strfilepath = string.Empty;

        public static string ConnectionString
        {
            set { _strconnection = value; }
            get { return _strconnection; }
        }

        public static string FilePath
        {
            set { _strfilepath = value; }
            get { return _strfilepath; }
        }

        public static string _strKeyTokenString = string.Empty;

        public static string KeyTokenString
        {
            set { _strKeyTokenString = value; }
            get { return _strKeyTokenString; }
        }


        public static string _strIssuerTokenString = string.Empty;

        public static string IssuerTokenString
        {
            set { _strIssuerTokenString = value; }
            get { return _strIssuerTokenString; }
        }


    }
}
