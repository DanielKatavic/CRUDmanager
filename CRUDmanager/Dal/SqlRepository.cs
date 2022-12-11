using CRUDmanager.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CRUDmanager.Dal
{
    public class SqlRepository : IRepository
    {
        private const string MethodName = nameof(IDataReadable.GetInstanceFromDataReader);
        private static readonly string? connectionString = App.Configuration?.GetConnectionString(nameof(connectionString));

        public void AddStudent(Student? student)
        {
            using (SqlConnection con = new(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@FirstName", System.Data.SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@LastName", System.Data.SqlDbType.NVarChar));
                    cmd.Parameters[0].Value = student?.FirstName;
                    cmd.Parameters[1].Value = student?.LastName;
                }
            }
        }

        public ICollection<T> GetCollectionOfModel<T>()
        {
            ICollection<T> models = new HashSet<T>();
            using (SqlConnection con = new(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = $"Get{typeof(T).Name}s";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            MethodInfo? methodInfo = typeof(T).GetMethods().FirstOrDefault(m => m.Name == MethodName);
                            var obj = methodInfo?.Invoke(null, new object[] { dr });
                            if (obj is not null)
                            {
                                models.Add((T)obj);
                            }
                        }
                    }
                }
            }
            return models;
        }

        public Professor GetProfessorById(int id)
        {
            using (SqlConnection con = new(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.Int));
                    cmd.Parameters[0].Value = id;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            return new Professor((int)dr[0],
                                dr[1].ToString() ?? string.Empty,
                                dr[2].ToString() ?? string.Empty);
                        }
                    }
                }
            }
            throw new System.ArgumentNullException();
        }

        public ICollection<Student> GetStudentsForSubject(int subjectId)
        {
            ICollection<Student> students = new HashSet<Student>();
            using (SqlConnection con = new(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@subjectId", System.Data.SqlDbType.Int));
                    cmd.Parameters[0].Value = subjectId;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            students.Add(Student.GetInstanceFromDataReader(dr));
                        }
                    }
                }
            }
            return students;
        }

        public void RemoveStudent(Student? student)
        {
            using (SqlConnection con = new(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.Int));
                    cmd.Parameters[0].Value = student?.Id;
                }
            }
        }

        public void UpdateStudent(Student? student)
        {
            using (SqlConnection con = new(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@FirstName", System.Data.SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@LastName", System.Data.SqlDbType.NVarChar));
                    cmd.Parameters[0].Value = student?.Id;
                    cmd.Parameters[1].Value = student?.FirstName;
                    cmd.Parameters[2].Value = student?.LastName;
                }
            }
        }
    }
}
