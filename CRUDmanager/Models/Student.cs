using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace CRUDmanager.Models
{
    public partial record Student(int Id, string FirstName, string LastName) : IDataReadable, IDataErrorInfo
    {
        public string Error => string.Empty;

        public string this[string columnName] => OnValidate(columnName);

        private string OnValidate(string columnName)
        {
            if (columnName == nameof(FirstName))
            {
                if (string.IsNullOrEmpty(FirstName))
                {
                    return $"{nameof(FirstName)} is mandatory";
                }
                else if (FirstName.Any(char.IsDigit))
                {
                    return "Should enter alphabets only!";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(LastName))
                {
                    return $"{nameof(LastName)} is mandatory";
                }
                else if (LastName.Any(char.IsDigit))
                {
                    return "Should enter alphabets only!";
                }
            }
            return string.Empty;
        }

        public static dynamic GetInstanceFromDataReader(SqlDataReader dr)
        {
            return new Student((int)dr[0],
                    dr[1].ToString() ?? string.Empty,
                    dr[2].ToString() ?? string.Empty);
        }

        [GeneratedRegex("^[a-zA-Z]+$")]
        private static partial Regex AlphabtetsRegex();
    }
}
