using CRUDmanager.Dal;
using Microsoft.Data.SqlClient;

namespace CRUDmanager.Models
{
    public record Subject(int Id, int Ects, string Name, Professor Professor) : IDataReadable
    {
        public static dynamic GetInstanceFromDataReader(SqlDataReader dr)
        {
            return new Subject((int)dr[0], (int)dr[1], dr[2].ToString() ?? string.Empty, PrepareProffesor((int)dr[3]));
        }

        private static Professor PrepareProffesor(int professorId)
        {
            return RepositoryFactory.GetRepository().GetProfessorById(professorId);
        }

        public override string ToString() => Name;
    }
}