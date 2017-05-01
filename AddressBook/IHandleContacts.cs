using System.Collections.Generic;

namespace AddressBook
{
    public interface IHandleContacts
    {
        int Count { get; }

        void CreateCompany(string name, string phoneNumber);
        void CreatePerson(string firstName, string lastName, string phoneNumber);
        List<Contact> GetAllContacts();
        void ReplaceAllContacts(List<Contact> listToSave);
    }
}