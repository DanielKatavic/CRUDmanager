using Microsoft.Data.SqlClient;

namespace CRUDmanager.Models
{
    public interface IDataReadable
    {
        public abstract static dynamic GetInstanceFromDataReader(SqlDataReader dr);
    }
}