using CRUDmanager.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace CRUDmanager.Dal
{
    public class SqlRepository : IRepository
    {
        private const string MethodName = nameof(IDataReadable.GetInstanceFromDataReader);
        private static readonly string? connectionString = App.Configuration?.GetConnectionString(nameof(connectionString));

        public ICollection<T> GetCollectionOfModel<T>()
        {
            ICollection<T> models = new HashSet<T>();
            using (SqlConnection con = new(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = $"Get{typeof(T).Name}s";
                    cmd.CommandType = CommandType.StoredProcedure;

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
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int));
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
            throw new ArgumentNullException(nameof(id));
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
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@subjectId", SqlDbType.Int));
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

        public void RemovePerson(Person? person)
        {
            using (SqlConnection con = new(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = $"Remove{person?.GetType().Name}";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int));
                    cmd.Parameters[0].Value = person?.Id;
                }
            }
        }

        public void AddOrUpdatePerson(Person? person)
        {
            using (SqlConnection con = new(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = $"{(person?.Id == -1 ? "Add" : "Update")}{person?.GetType().Name}";
                    cmd.CommandType = CommandType.StoredProcedure;

                    PropertyInfo[]? propertyInfos = person?.GetType().GetProperties();
                    ICollection<SqlParameter> parameters = new List<SqlParameter>();

                    foreach (var propertyInfo in propertyInfos?.Take(propertyInfos.Length - 2) ?? Enumerable.Empty<PropertyInfo>())
                    {
                        parameters.Add(new SqlParameter($"@{propertyInfo.Name}", propertyInfo.PropertyType == typeof(int) ? SqlDbType.Int : SqlDbType.NVarChar, 50)
                        {
                            Value = propertyInfo.GetValue(person)
                        });
                    }

                    cmd.Parameters.AddRange(parameters.ToArray());
                }
            }
        }
    }
}
