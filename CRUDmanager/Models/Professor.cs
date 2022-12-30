namespace CRUDmanager.Models
{
    public sealed record Professor(Person Person) : Person(Person.Id, Person.FirstName, Person.LastName)
    {
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
