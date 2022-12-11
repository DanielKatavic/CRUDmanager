using CRUDmanager.Models;
using System.Collections.Generic;

namespace CRUDmanager.Dal
{
    public interface IRepository
    {
        void AddStudent(Student? student);
        public ICollection<T> GetCollectionOfModel<T>();
        public Professor GetProfessorById(int id);
        public ICollection<Student> GetStudentsForSubject(int subjectId);
        void RemoveStudent(Student? student);
        void UpdateStudent(Student? student);
    }
}
