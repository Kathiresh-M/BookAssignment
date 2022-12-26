using Entities;
using Entities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.IHelper
{
    public interface IAddressBookRepo
    {
        void CreateAddressBook(AddressBookDto addressBookData);
        AddressBookDatabase GetAddressBookByName(string firstName, string lastName);

        AddressBookDatabase GetAddressBookById(Guid AddressBookId);

        //int GetAddressBookCount(Guid userId);

        int GetAddressBookCount();

        void DeleteAddressBook(AddressBookDatabase addressBook);

        public List<AddressBookDatabase> GetAddressBooks(Guid userId);

        void UpdateAddressBook(AddressBookDatabase addressBook, IEnumerable<Email> Emails, IEnumerable<Address> Addresses, IEnumerable<Phone> Phones);
        void Save();
    }
}
