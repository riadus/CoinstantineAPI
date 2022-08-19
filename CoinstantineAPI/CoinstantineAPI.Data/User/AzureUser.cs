using System;

namespace CoinstantineAPI.Data
{
    public class AzureUser
    {
        public string UserId { get; set; }
        public string EmailAddress { get; set; }
        public UserRole Role { get; set; }
    }

    public class AccountCorrect
    {
        public bool UsernameAvailable { get; set; }
        public bool EmailAvailable { get; set; }
        public bool PasswordCorrect { get; set; }
        public bool AllGood => UsernameAvailable && EmailAvailable && PasswordCorrect;
    }

    public class AccountCreationModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string ReferralCode { get; set; }
        public string Source { get; set; }
        public string Model { get; set; }
        public string OsVersion { get; set; }
        public string Language { get; set; }
    }

    public class AccountChangeModel
    {
        public string UserId { get; set; }
        public string ConfirmationCode { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
