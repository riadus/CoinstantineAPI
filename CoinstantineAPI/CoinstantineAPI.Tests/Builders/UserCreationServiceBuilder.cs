using CoinstantineAPI.Core.Blockchain;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.DataProviders;
using CoinstantineAPI.Core.Email;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Users;
using FakeItEasy;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.Tests.Builders
{
    public class UserCreationServiceBuilder
    {
        private IContextProvider _contextProvider = A.Fake<IContextProvider>();
        private IUsersService _usersService = A.Fake<IUsersService>();
        private IBlockchainService _blockchainService = A.Fake<IBlockchainService>();
        private ILoggerFactory _loggerFactory = A.Dummy<ILoggerFactory>();
        private IEmailService _emailService = A.Dummy<IEmailService>();
        private IPasswordService _passwordService = A.Fake<IPasswordService>();
        private ICountriesProvider _countriesProvider = A.Dummy<ICountriesProvider>();
        private IReferralService _referralService = A.Dummy<IReferralService>();
        private string _confirmationCode;

        public IUserCreationService Build()
        {
            var codeGenerator = A.Fake<ICodeGenerator>();
            A.CallTo(() => codeGenerator.GenerateCode()).Returns(_confirmationCode);
            A.CallTo(() => _passwordService.CreatePasswordHash(A<string>.Ignored)).Returns(("", ""));
            A.CallTo(() => _blockchainService.CreateUser(A<string>.Ignored)).Returns("");
            return new UserCreationService(_usersService, _referralService, codeGenerator, _emailService, _passwordService, _contextProvider, _blockchainService, _countriesProvider, _loggerFactory);
        }

        public UserCreationServiceBuilder WithUsersService(IUsersService usersService)
        {
            _usersService = usersService;
            return this;
        }

        public UserCreationServiceBuilder WithBlockchainService(IBlockchainService blockchainService)
        {
            _blockchainService = blockchainService;
            return this;
        }

        public UserCreationServiceBuilder WithContextProvider(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
            return this;
        }

        public UserCreationServiceBuilder WithConfirmationCode(string confirmationCode)
        {
            _confirmationCode = confirmationCode;
            return this;
        }
    }
}
