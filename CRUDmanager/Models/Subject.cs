using CRUDmanager.Dal;
using Microsoft.Data.SqlClient;

namespace CRUDmanager.Models
{
    public record Subject(int Id, string Name, int Ects, Professor Professor) : IDataReadable
    {
        public static dynamic GetInstanceFromDataReader(SqlDataReader dr) 
            => new Subject((int)dr[0], dr[1].ToString() ?? string.Empty, (int)dr[2], PrepareProffesor((int)dr[3]));

        private static Professor PrepareProffesor(int professorId) 
            => RepositoryFactory.GetRepository().GetProfessorById(professorId);

        public override string ToString() => Name;
    }
}