using Microsoft.Data.SqlClient;

namespace CRUDmanager.Models
{
    public record Professor(int Id, string FirstName, string LastName) : IDataReadable
    {
        public static dynamic GetInstanceFromDataReader(SqlDataReader dr)
        {
            return new Professor((int)dr[0],
                    dr[1].ToString() ?? string.Empty,
                    dr[2].ToString() ?? string.Empty);
        }

        public override string ToString() => $"{FirstName} {LastName}";
    }
}
