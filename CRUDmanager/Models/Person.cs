using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Linq;

namespace CRUDmanager.Models
{
    public abstract record Person(int Id, string FirstName, string LastName) : IDataErrorInfo, IDataReadable
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
            throw new System.NotImplementedException();
        }
    }
}
