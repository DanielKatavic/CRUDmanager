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
                            var method = typeof(T).GetInterfaceMap(typeof(IDataReadable)).TargetMethods.First();
                            var obj = method?.Invoke(null, new object[] { dr });

                            if (obj is Person person)
                            {
                                var tempObj = Activator.CreateInstance(typeof(T), new object[] { person });
                                models.Add((T)tempObj!);
                            }
                            else
                            {
                                models.Add((T)obj!);
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
                            var person = Person.GetInstanceFromDataReader(dr);
                            return new Professor(person);
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
                            var person = Person.GetInstanceFromDataReader(dr);
                            var student = new Student(person);
                            students.Add(student);
                        }
                    }
                }
            }
            return students;
        }

        public void RemoveItem(dynamic item)
        {
            using (SqlConnection con = new(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = $"Remove{item?.GetType().Name}";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int));
                    cmd.Parameters[0].Value = item?.Id;
                }
            }
        }

        public void AddOrUpdateItem(dynamic item)
        {
            using (SqlConnection con = new(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = $"{(item?.Id == -1 ? "Add" : "Update")}{item?.GetType().Name}";
                    cmd.CommandType = CommandType.StoredProcedure;

                    PropertyInfo[]? propertyInfos = item?.GetType().GetProperties();
                    ICollection<SqlParameter> parameters = new List<SqlParameter>();

                    foreach (var propertyInfo in propertyInfos?.Take(propertyInfos.Length - 2) ?? Enumerable.Empty<PropertyInfo>())
                    {
                        parameters.Add(new SqlParameter($"@{propertyInfo.Name}", propertyInfo.PropertyType == typeof(int) ? SqlDbType.Int : SqlDbType.NVarChar, 50)
                        {
                            Value = propertyInfo.GetValue(item)
                        });
                    }

                    cmd.Parameters.AddRange(parameters.ToArray());
                }
            }
        }
    }
}
