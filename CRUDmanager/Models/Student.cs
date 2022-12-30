namespace CRUDmanager.Models
{
    public sealed record Student(Person Person) : Person(Person.Id, Person.FirstName, Person.LastName);
}
