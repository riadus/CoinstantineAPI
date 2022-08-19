using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CoinstantineAPI.Data
{
    public class UserIdentity : Entity
    {
        [ForeignKey("ApiUserId")]
        public ApiUser RelatedUser { get; set; }
        public string Username { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string EmailAddress { get; set; }
        public string Source { get; set; }
        public string Model { get; set; }
        public string OsVersion { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }

        public string UserId { get; set; }
        public UserRole Role { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        public string PasswordSalt { get; set; }

        [JsonIgnore]
        public List<RefreshTokens> RefreshTokens { get; set; }

        public DateTime DoB { get; set; }
        public string Address { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public string ConfirmationCode { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public bool EmailConfirmed { get; set; }
        public string ReferralCode { get; set; }
        public override bool Equals(object obj)
        {
            return obj is UserIdentity identity &&
                   UserId == identity.UserId;
        }

        public override int GetHashCode()
        {
            return 2139390487 + EqualityComparer<string>.Default.GetHashCode(UserId);
        }
    }

    public class RefreshTokens : Entity
    {
        public string RefreshToken { get; set; }
        public string RefreshTokenSalt { get; set; }
        public Application Application { get; set; }
        public UserIdentity User { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime GenerationDate { get; set; }
    }

    public class Application : Entity
    {
        public string ApplicationId { get; set; }
        public string ApplicationSecret { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Document : Entity
    {
        public string Path { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime DownloadDate { get; set; }
        public int Version { get; set; }
        public string Filename { get; set; }
        public DateTime LastCheck { get; set; }
        public bool DocumentAvailableOnline { get; set; }
        public string AzureFilename { get; set; }
    }

    public enum UserRole
    {
        Member,
        Admin,
        Manager
    }
}
