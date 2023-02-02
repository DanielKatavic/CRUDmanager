using CRUDmanager.Dal;
using CRUDmanager.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CRUDmanager.ViewModels
{
    public class UniversityViewModel
    {
        private readonly IRepository _repository = RepositoryFactory.GetRepository();

        public ObservableCollection<Subject> Subjects { get; }
        public ObservableCollection<Person> Persons { get; }

        public UniversityViewModel()
        {
            var students = new ObservableCollection<Person>(_repository.GetCollectionOfModel<Student>());
            var professors = new ObservableCollection<Person>(_repository.GetCollectionOfModel<Professor>());
            Subjects = new ObservableCollection<Subject>(_repository.GetCollectionOfModel<Subject>());
            Persons = new ObservableCollection<Person>(Enumerable.Concat(students, professors));
            Persons.CollectionChanged += Persons_CollectionChanged;
            Subjects.CollectionChanged += Subjects_CollectionChanged;
        }

        public ICollection<Person> GetStudentsForSubject(int id) 
            => new ObservableCollection<Person>(_repository.GetStudentsForSubject(id));

        private void Persons_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    _repository.AddOrUpdateItem(Persons[e.NewStartingIndex]);
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    _repository.RemoveItem(e.OldItems?.OfType<Person>().First()!);
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    _repository.AddOrUpdateItem(e.NewItems?.OfType<Person>().First()!);
                    break;
            }
        }

        private void Subjects_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    _repository.AddOrUpdateItem(Subjects[e.NewStartingIndex]);
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    _repository.RemoveItem(e.OldItems?.OfType<Subject>().First()!);
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    _repository.AddOrUpdateItem(e.NewItems?.OfType<Subject>().First()!);
                    break;
            }
        }

        internal void Update(Person person) => Persons[Persons.IndexOf(person)] = person;
    }
}
