using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CoinstantineAPI.Core;
using CoinstantineAPI.Core.Blockchain;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.DataProviders;
using CoinstantineAPI.Core.Email;
using CoinstantineAPI.Core.Exceptions;
using CoinstantineAPI.Core.Extensions;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.Users
{
    public class UserCreationService : IUserCreationService
    {
        private readonly IUsersService _userService;
        private readonly IReferralService _referralService;
        private readonly ICodeGenerator _codeGenerator;
        private readonly IEmailService _emailService;
        private readonly IPasswordService _passwordService;
        private readonly IContextProvider _contextProvider;
        private readonly IBlockchainService _blockchainService;
        private readonly ICountriesProvider _countriesProvider;
        private readonly ILogger _logger;

        public UserCreationService(IUsersService userService,
                                   IReferralService referralService,
                                   ICodeGenerator codeGenerator,
                                   IEmailService emailService,
                                   IPasswordService passwordService,
                                   IContextProvider contextProvider,
                                   IBlockchainService blockchainService,
                                   ICountriesProvider countriesProvider,
                                   ILoggerFactory loggerFactory)
        {
            _userService = userService;
            _referralService = referralService;
            _codeGenerator = codeGenerator;
            _emailService = emailService;
            _passwordService = passwordService;
            _contextProvider = contextProvider;
            _blockchainService = blockchainService;
            _countriesProvider = countriesProvider;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task<UserIdentity> GetUser(string email)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    var userIdentity = await context.UserIdentities.Include(u => u.RelatedUser)
                                              .FirstOrDefaultAsync(x => x.EmailAddress == email);
                    if (userIdentity == null)
                    {
                        throw new UserDoesNotExistException();
                    }
                    return userIdentity;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetUser()", email);
                throw ex;
            }
        }

        public async Task<bool> SubscribeUser(AccountCreationModel accountCreationModel)
        {
            try
            {
                var randomCode = _codeGenerator.GenerateCode();
                var (hash, salt) = _passwordService.CreatePasswordHash(accountCreationModel.Password);
                var parsed = DateTime.TryParse(accountCreationModel.DateOfBirth, out var dob);
                var country = await _countriesProvider.GetCountryForStore(accountCreationModel.Country);
                var userIdentity = new UserIdentity
                {
                    UserId = Guid.NewGuid().ToString(),
                    Address = accountCreationModel.Address,
                    FirstName = accountCreationModel.FirstName,
                    DoB = parsed ? dob : new DateTime(),
                    LastName = accountCreationModel.LastName,
                    Password = hash,
                    PasswordSalt = salt,
                    EmailAddress = accountCreationModel.Email,
                    City = accountCreationModel.City,
                    Country = country?.Name,
                    PostalCode = accountCreationModel.PostalCode,
                    Username = accountCreationModel.Username,
                    ConfirmationCode = randomCode,
                    Role = UserRole.Member,
                    SubscriptionDate = DateTime.Now,
                    ReferralCode = accountCreationModel.ReferralCode,
                    Source = accountCreationModel.Source,
                    Model = accountCreationModel.Model,
                    OsVersion = accountCreationModel.OsVersion,
                    Language = accountCreationModel.Language
                };

                await _emailService.SendConfirmationEmail(userIdentity);

                using (var context = _contextProvider.CoinstantineContext)
                {
                    await context.UserIdentities.AddAsync(userIdentity);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erorr in SubscribeUser()", accountCreationModel);
                return false;
            }
        }

        public async Task CreateAdmin()
        {
            var userIdentity = new UserIdentity
            {
                UserId = Guid.NewGuid().ToString(),
                EmailAddress = Constants.AdminEmail,
                Role = UserRole.Admin,
                SubscriptionDate = DateTime.Now,
                EmailConfirmed = true,
                Username = "Admin"
            };

            using (var context = _contextProvider.CoinstantineContext)
            {
                if (await context.UserIdentities.AnyAsync(x => x.EmailAddress == Constants.AdminEmail))
                {
                    return;
                }
                await context.UserIdentities.AddAsync(userIdentity);
                await context.SaveChangesAsync();
            }
            await CreateApiUser(userIdentity);
            await ResetPassword(Constants.AdminEmail);
        }

        public async Task<UserIdentity> GetUserFromConfirmationCode(string confirmationCode)
        {
            using (var context = _contextProvider.CoinstantineContext)
            {
                return await context.UserIdentities.FirstOrDefaultAsync(x => x.ConfirmationCode == confirmationCode);
            }
        }

        public async Task<(ApiUser, bool)> CreateApiUser(AzureUser azureUser, string username)
        {
            UserIdentity userIdentity = null;
            using (var context = _contextProvider.CoinstantineContext)
            {
                userIdentity = await context.UserIdentities.FirstOrDefaultAsync(x => x.EmailAddress == azureUser.EmailAddress);
            }
            return await CreateApiUser(userIdentity, username);
        }

        public async Task<bool> CreateApiUser(UserIdentity userIdentity)
        {
            try
            {
                if (await _userService.ApiUserExists(userIdentity.Username))
                {
                    return false;
                }
                var apiUser = new ApiUser { Username = userIdentity.Username, Email = userIdentity.EmailAddress };

                var newAddress = await _blockchainService.CreateUser(userIdentity.Username);

                apiUser.BlockchainInfo = new BlockchainInfo
                {
                    Coinstantine = 0,
                    Address = newAddress,
                    Ether = 0
                };
                userIdentity.RelatedUser = apiUser;
                userIdentity.ConfirmationDate = DateTime.Now;
                userIdentity.EmailConfirmed = true;
                using (var context = _contextProvider.CoinstantineContext)
                {
                    await context.UserIdentities.AddOrUpdateAsync(userIdentity);
                    await context.SaveChangesAsync();
                }

                await _referralService.SetReferral(apiUser, userIdentity.ReferralCode);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateApiUser()", userIdentity);
                return false;
            }
        }

        public async Task<(ApiUser, bool)> CreateApiUser(UserIdentity userIdentity, string username)
        {
            try
            {
                var usernameUsed = await _userService.IsUsernameUsed(username);
                if (usernameUsed)
                {
                    return (null, false);
                }

                userIdentity.Username = username;
                var apiUser = new ApiUser { Username = username, Email = userIdentity.EmailAddress };

                var newAddress = await _blockchainService.CreateUser(username);

                apiUser.BlockchainInfo = new BlockchainInfo
                {
                    Coinstantine = 0,
                    Address = newAddress,
                    Ether = 0
                };
                userIdentity.RelatedUser = apiUser;
                using (var context = _contextProvider.CoinstantineContext)
                {
                    await context.UserIdentities.AddOrUpdateAsync(userIdentity);
                    await context.SaveChangesAsync();
                    return (apiUser, true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateApiUser()", userIdentity, username);
                return (null, false);
            }
        }

        public async Task<AccountCorrect> IsAccountCorrect(AccountCreationModel accountCreationModel)
        {
            using (var context = _contextProvider.CoinstantineContext)
            {
                var usernameUsed = await context.ApiUsers.AnyAsync(a => a.Username.ToLower() == accountCreationModel.Username.ToLower());
                var emailUsed = await context.UserIdentities.AnyAsync(a => a.EmailAddress.ToLower() == accountCreationModel.Email.ToLower());
                var correctPassword = CorrectPassword(accountCreationModel.Password);
                return new AccountCorrect
                {
                    UsernameAvailable = !usernameUsed,
                    EmailAvailable = !emailUsed,
                    PasswordCorrect = correctPassword
                };
            }
        }

        private const string PasswordRegexPattern = "((?=.*\\d)(?=.*[A-Z]).{8,})";
        public bool CorrectPassword(string password)
        {
            return Regex.IsMatch(password, PasswordRegexPattern);
        }

        public async Task<bool> SendUsername(string email)
        {
            var user = await _userService.GetUserIdentityFromEmail(email);
            if (user != null)
            {
                await _emailService.SendUsername(user);
                return true;
            }
            return false;
        }

        public async Task<bool> ResetPassword(string email)
        {
            var user = await _userService.GetUserIdentityFromEmail(email);
            if (user != null)
            {
                user.ConfirmationCode = _codeGenerator.GenerateCode();
                await _emailService.SendResetPassword(user);
                using (var context = _contextProvider.CoinstantineContext)
                {
                    context.UserIdentities.Update(user);
                    await context.SaveChangesAsync();
                }
                return true;
            }
            return false;
        }

        public async Task<bool> ChangePassword(AccountChangeModel accountChangeModel)
        {
            using (var context = _contextProvider.CoinstantineContext)
            {
                var user = await context.UserIdentities.FirstOrDefaultAsync(a => a.UserId.ToLower() == accountChangeModel.UserId.ToLower());
                if (user != null)
                {
                    if (user.ConfirmationCode == accountChangeModel.ConfirmationCode)
                    {
                        var (hash, salt) = _passwordService.CreatePasswordHash(accountChangeModel.Password);
                        user.Password = hash;
                        user.PasswordSalt = salt;
                        user.ConfirmationCode = _codeGenerator.GenerateCode();
                        {
                            context.UserIdentities.Update(user);
                            await context.SaveChangesAsync();
                        }
                        return true;
                    }
                    return false;
                }
                throw new ArgumentException();
            }
        }
    }
}
