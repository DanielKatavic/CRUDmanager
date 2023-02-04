using CRUDmanager.Dal;
using CRUDmanager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;

namespace CRUDmanager.ViewModels
{
    public class UniversityViewModel
    {
        private readonly IRepository _repository = RepositoryFactory.GetRepository();

        public ObservableCollection<Subject> Subjects { get; set; }
        public ObservableCollection<Person> Persons { get; set; }

        public UniversityViewModel()
        {
            var students = _repository.GetCollectionOfModel<Student>();
            var professors = _repository.GetCollectionOfModel<Professor>();
            Subjects = new ObservableCollection<Subject>(_repository.GetCollectionOfModel<Subject>());
            Persons = new ObservableCollection<Person>(Enumerable.Concat<Person>(students, professors));
            Persons.CollectionChanged += Persons_CollectionChanged;
            Subjects.CollectionChanged += Subjects_CollectionChanged;
        }

        public ICollection<Person> GetStudentsForSubject(int id)
            => new ObservableCollection<Person>(_repository.GetStudentsForSubject(id));

        private void Persons_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
            => HandleCollectionChanged<Person>(e);

        private void Subjects_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
            => HandleCollectionChanged<Subject>(e);

        private void HandleCollectionChanged<T>(NotifyCollectionChangedEventArgs e)
        {
            dynamic? list = GetType().GetProperties().FirstOrDefault(p => p.PropertyType == typeof(ObservableCollection<T>));
            if (list is null)
            {
                return;
            }
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var collection = list?.GetMethod?.Invoke(this, null);
                    if (collection is ObservableCollection<T> col)
                    {
                        _repository.AddOrUpdateItem(col[e.NewStartingIndex]!);
                    }
                    return;
                case NotifyCollectionChangedAction.Remove:
                    _repository.RemoveItem(e.OldItems!.OfType<T>().First()!);
                    return;
                case NotifyCollectionChangedAction.Replace:
                    _repository.AddOrUpdateItem(e.NewItems!.OfType<T>().First()!);
                    return;
            }
        }

        internal void UpdateModel<T>(T model)
        {
            var list = GetType().GetProperties().FirstOrDefault(p => p.PropertyType == typeof(ObservableCollection<T>));
            var collection = list?.GetMethod?.Invoke(this, null);
            if (collection is ObservableCollection<T> col)
            {
                col[col.IndexOf(model)] = model;
            }
        }
    }
}
