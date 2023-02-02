using CRUDmanager.Models;
using System.Collections.Generic;

namespace CRUDmanager.Dal
{
    public interface IRepository
    {
        public ICollection<T> GetCollectionOfModel<T>();
        public Professor GetProfessorById(int id);
        public ICollection<Student> GetStudentsForSubject(int subjectId);
        public void RemoveItem(dynamic item);
        public void AddOrUpdateItem(dynamic item);
    }
}
