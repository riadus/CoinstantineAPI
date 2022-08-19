using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Blockchain;
using CoinstantineAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoinstantineAPI.WebApi.Controllers
{
	[Authorize]
    public class IntegrationTestController : Controller
    {
        private readonly ICoinstantineService _airdropService;
        private readonly IBlockchainService _blockchainService;

        public IntegrationTestController(ICoinstantineService airdropService,
                                         IBlockchainService blockchainService)
        {
            _airdropService = airdropService;
            _blockchainService = blockchainService;
        }

        public async Task<IActionResult> Post([FromBody]Scenario scenario)
        {
            return BadRequest();
            //return Ok(await Start(scenario));
        }

        private async Task<Result> Start(Scenario scenario)
        {
            var result = new Result
            {
                Scenario = scenario
            };
            try
            {
                var now = DateTime.Now;
                Debug.WriteLine($"Airdrop creation {now.ToString("T")}");
                var airdropId = await _airdropService.CreateAirdrop(scenario.Airdrop);
                result.AirdropId = airdropId;
                var delta = DateTime.Now - now;
                Debug.WriteLine($"Airdrop created in {delta.ToString("c")}");

                now = DateTime.Now;
                Debug.WriteLine($"Starting deposit {now.ToString("T")}");
                var depositDone = await _airdropService.Deposit(airdropId, scenario.Airdrop.Amount);
                result.DepositDone = depositDone;
                delta = DateTime.Now - now;
                Debug.WriteLine($"Deposit done {delta.ToString("c")}");

                now = DateTime.Now;
                Debug.WriteLine($"Checking deposit {now.ToString("T")}");
                var checkedDeposit = await _airdropService.CheckDeposit(airdropId);
                result.CheckedDeposit = checkedDeposit;
                delta = DateTime.Now - now;
                Debug.WriteLine($"Checking done ({checkedDeposit}) in {delta.ToString("c")}");

                now = DateTime.Now;
                Debug.WriteLine($"Creating users {now.ToString("T")}");
                var users = new List<BlockchainUser>();
                for (var i = 0; i < scenario.NumberOfUsers; i++)
                {
                    var user = new BlockchainUser();
                    var address = await _blockchainService.CreateUser($"User {users.Count() + 1}");
                    user.Address = address;
                    users.Add(user);
                }
                result.Users = users;
                delta = DateTime.Now - now;
                Debug.WriteLine($"Users created in {delta.ToString("c")}");

                now = DateTime.Now;
                Debug.WriteLine($"Subscribing {now.ToString("T")}");
                foreach (var user in users)
                {
                    await _airdropService.Subscribe(user.Username, airdropId);
                }
                var subscribers = await _airdropService.Subscribers(airdropId);
                result.Subscribers = subscribers;
                delta = DateTime.Now - now;
                Debug.WriteLine($"Subscribing done in {delta.ToString("c")}");

                now = DateTime.Now;
                Debug.WriteLine($"Starting distribution {now.ToString("T")}");
                await _airdropService.StartDistribution(airdropId);
                delta = DateTime.Now - now;
                Debug.WriteLine($"Starting distribution done ({checkedDeposit}) in {delta.ToString("c")}");


                now = DateTime.Now;
                Debug.WriteLine($"Withdrawing {now.ToString("T")}");
                foreach (var user in users)
                {
                    await _airdropService.Withdraw(airdropId, user.Username);
                }
                delta = DateTime.Now - now;
                Debug.WriteLine($"Withdrawing done in {delta.ToString("c")}");

                var withdrawn = await _airdropService.Subscribers(airdropId);
                result.Withdrawn = withdrawn;

                now = DateTime.Now;
                Debug.WriteLine($"Closing airdrop {now.ToString("T")}");
                var closed = await _airdropService.CloseAirdrop(airdropId);
                delta = DateTime.Now - now;
                Debug.WriteLine($"airdrop closed ({closed}) in {delta.ToString("c")}");
            }
            catch { }
            return result;
        }

        public class Scenario
        {
            public Airdrop Airdrop { get; set; }
            public int AmountPerPerson { get; set; }
            public int NumberOfUsers { get; set; }
        }

        public class Result
        {
            public Scenario Scenario { get; set; }
            public string AirdropId { get; set; }
            public bool DepositDone { get; set; }
            public string CheckedDeposit { get; set; }
            public IEnumerable<BlockchainUser> Users { get; set; }
            public IEnumerable<Subscriber> Subscribers { get; set; }
            public int NumberOfValidated { get; set; }
            public int NumberOfDistributed { get; set; }
            public IEnumerable<Subscriber> Withdrawn { get; set; }
        }
    }
}
