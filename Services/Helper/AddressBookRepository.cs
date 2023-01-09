using Contract.IHelper;
using Contract.Response;
using Entities;
using Entities.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helper
{
    public class AddressBookRepository : IAddressBookRepo
    {
        private readonly BookRepository _context;

        public AddressBookRepository(BookRepository context)
        {
            _context = context;
        }

        public void CreateAddressBook(AddressBookDto addressBookData)
        {
            var addressBook = new AddressBookDatabase
            {
                Id = addressBookData.Id,
                FirstName = addressBookData.FirstName,
                LastName = addressBookData.LastName,
                UserId = addressBookData.UserId,
            };

            _context.AddressBooks.Add(addressBook);

            _context.Emails.AddRange(addressBookData.Emails);

            _context.Addresses.AddRange(addressBookData.Addresses);

            _context.Phones.AddRange(addressBookData.Phones);
        }

        /*public AddressBookDatabase GetAddressBookByName(string firstName, string lastName)
        {
            return _context.AddressBooks.SingleOrDefault(addressBook => addressBook.FirstName == firstName && addressBook.LastName == lastName);
        }*/
        public AddressBookDatabase GetAddressBookByNames(string firstName, string lastName)
        {
            return _context.AddressBooks.SingleOrDefault(addressBook => addressBook.FirstName == firstName && addressBook.LastName == lastName);
        }


        public AddressBookDatabase GetAddressBookById(Guid AddressBookId)
        {
            return _context.AddressBooks.SingleOrDefault(addressBook => addressBook.Id == AddressBookId);
        }

        public int GetAddressBookCount()
        {
            return _context.Users.Count();
        }

        public void DeleteAddressBook(AddressBookDatabase addressBook)
        {
            _context.AddressBooks.Remove(addressBook);
        }

        public List<AddressBookDatabase> GetAddressBooks(Guid userId)
        {
            return _context.AddressBooks.Where(addressBook => addressBook.UserId == userId).ToList();
        }

        public void UpdateAddressBook(AddressBookDatabase addressBook, IEnumerable<Email> Emails, IEnumerable<Address> Addresses, IEnumerable<Phone> Phones)
        {

            _context.AddressBooks.Update(addressBook);

            _context.Emails.UpdateRange(Emails);

            _context.Addresses.UpdateRange(Addresses);

            _context.Phones.UpdateRange(Phones);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
