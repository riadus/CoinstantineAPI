using System;
using System.Collections.Generic;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Tests.Builders
{
    public class UserBuilder
    {
        public static ApiUser NewMoh()
        {
            return new ApiUser
            {
                Username = ApiUsers[0].Username,
                Email = ApiUsers[0].Email
            };
        }

        public static ApiUser NewHadj()
        {
            return new ApiUser
            {
                Username = ApiUsers[0].Username,
                Email = ApiUsers[0].Email
            };
        }

        private static List<ApiUser> _apiUsers;
        public static List<ApiUser> ApiUsers
        {
            get
            {
                return _apiUsers ?? (_apiUsers = new List<ApiUser>{
                new ApiUser
                {
                    Id = 1,
                    UniqueId = "123",
                    DeviceId = "Device123",
                    Phonenumber = "0123456789",
                    Username = "Moh",
                    Email = "moh@gmail.com",
                    BctProfile = new BitcoinTalkProfile
                    {
                        BctId = 1234,
                        Username = "Moh123",
                        Position = BitcoinTalkRank.HeroMember,
                        Activity = 12,
                        Age = 30,
                        Email = "moh@gmail.com",
                        LastActive = DateTime.Now,
                        Location = "0x123",
                        Aim = "AIM",
                        Icq = "ICQ",
                        Msn = "MSN",
                        Yim = "YIM",
                        Posts = 12,
                        RegistrationDate = DateTime.Now,
                        Trust = "Yes",
                        Website = "www.google.com",
                    },
                    TwitterProfile = new TwitterProfile {  TwitterId = 1, Validated = false, Username = "@Moh" },
                    Telegram = new TelegramProfile { TelegramId = 1, Validated = false, Username = "@Moh" },
                },
                new ApiUser
                {
                        Id = 2,
                    UniqueId = "1234",
                    DeviceId = "Device1234",
                    Phonenumber = "0299345678",
                    Username = "Hadj",
                    Email = "hadj@gmail.com",
                    BctProfile = new BitcoinTalkProfile
                    {
                        BctId=123,
                        Username = "Hadj123",
                        Position = BitcoinTalkRank.JrMember,
                        Activity = 12,
                        Age = 30,
                        Email = "hadj@gmail.com",
                        LastActive = DateTime.Now,
                        Location = "0x1234",
                        Aim = "AIM2",
                        Icq = "ICQ2",
                        Msn = "MSN2",
                        Yim = "YIM2",
                        Posts = 12,
                        RegistrationDate = DateTime.Now,
                        Trust = "Yes",
                        Website = "www.yahoo.com",
                    },
                    TwitterProfile = new TwitterProfile { TwitterId = 2, Validated = false, Username = "@Hadj" },
                    Telegram = new TelegramProfile { TelegramId = 2, Validated = false, Username = "@Hadj" },
                }
                });
            }
        }
        private static List<AccountCreationModel> _accountCreationModel;
        public static List<AccountCreationModel> AccountCreationModels
        {
            get
            {
                return _accountCreationModel ?? (_accountCreationModel = new List<AccountCreationModel>{
                    new AccountCreationModel{
                        Email = ApiUsers[0].Email,
                        Password = "PassW0rd$!",
                        Username = ApiUsers[0].Username
                    },
                    new AccountCreationModel{
                        Email = ApiUsers[1].Email,
                        Password = "PassW0rd$!",
                        Username = ApiUsers[1].Username
                    }
                });
            }
        }
    }
}

