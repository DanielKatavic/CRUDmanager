using Microsoft.Data.SqlClient;

namespace CRUDmanager.Models
{
    public record Student(int Id, string FirstName, string LastName) : Person(Id, FirstName, LastName)
    {
        public static new dynamic GetInstanceFromDataReader(SqlDataReader dr)
        {
            return new Student((int)dr[0],
                    dr[1].ToString() ?? string.Empty,
                    dr[2].ToString() ?? string.Empty);
        }
    }
}
