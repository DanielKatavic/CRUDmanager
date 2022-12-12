using CRUDmanager.Dal;
using CRUDmanager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace CRUDmanager.ViewModels
{
    public class UniversityViewModel
    {
        private readonly IRepository _repository = RepositoryFactory.GetRepository();

        public ObservableCollection<Student> Students { get; set; }
        public ObservableCollection<Subject> Subjects { get; }
        public ObservableCollection<Professor> Professors { get; }

        public UniversityViewModel()
        {
            Students = new ObservableCollection<Student>(_repository.GetCollectionOfModel<Student>());
            Subjects = new ObservableCollection<Subject>(_repository.GetCollectionOfModel<Subject>());
            Professors = new ObservableCollection<Professor>(_repository.GetCollectionOfModel<Professor>());
            Students.CollectionChanged += Students_CollectionChanged;
            Subjects.CollectionChanged += Subjects_CollectionChanged;
            Professors.CollectionChanged += Professors_CollectionChanged;
        }

        public ICollection<Student> GetStudentsForSubject(int id)
        {
            Students = new ObservableCollection<Student>(_repository.GetStudentsForSubject(id));
            return Students;
        }

        private void Professors_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Subjects_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    _repository.AddStudent(Students[e.NewStartingIndex]);
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    _repository.RemoveStudent(e.OldItems?.OfType<Student>().First());
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    _repository.UpdateStudent(e.NewItems?.OfType<Student>().First());
                    break;
            }
        }

        private void Students_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    _repository.AddOrUpdatePerson(Students[e.NewStartingIndex]);
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    _repository.RemoveStudent(e.OldItems?.OfType<Student>().First());
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    _repository.UpdateStudent(e.NewItems?.OfType<Student>().First());
                    break;
            }
        }

        internal void Update(Student student)
        {
            throw new NotImplementedException();
        }
    }
}
