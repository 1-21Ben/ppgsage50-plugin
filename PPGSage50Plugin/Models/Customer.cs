using System;
using System.Collections.Generic;

namespace PPGSage50Plugin.Models
{
    /// <summary>
    /// Modèle représentant un client dans le système PPG Live
    /// </summary>
    public class Customer
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string VatNumber { get; set; }
        public string PaymentTerms { get; set; }
        public string Currency { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public List<CustomerContact> Contacts { get; set; } = new List<CustomerContact>();
    }

    /// <summary>
    /// Contact associé à un client
    /// </summary>
    public class CustomerContact
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Position { get; set; }
        public bool IsPrimary { get; set; }
    }
}
