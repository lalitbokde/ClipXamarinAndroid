using System.Collections.Generic;

namespace WeClip.Core.Model
{
    public class ContactsModel
    {

        public string Phone { get; set; }

        public string Address { get; set; }

        public string PhotoURL { get; set; }

        public List<Address> Addresses { get; set; }

        public string DisplayName { get; set; }

        public List<Email> Emails { get; set; }

        public string EmailID { get; set; }

        public string FirstName { get; set; }

        public string Id { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string NickName { get; set; }

        public string ProfilePic { get; set; }
                      
    }

    public class Address
    {
        public string City { get; set; }

        public string Country { get; set; }

        public string Label { get; set; }

        public string PostalCode { get; set; }

        public string Region { get; set; }
    }

    public class Email
    {
        public string Address { get; set; }

        public string Lable { get; set; }
    }
}
