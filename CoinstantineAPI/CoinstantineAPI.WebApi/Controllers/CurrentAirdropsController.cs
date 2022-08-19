using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoinstantineAPI.Core.Airdrops;
using CoinstantineAPI.Core.Extensions;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;
using CoinstantineAPI.WebApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoinstantineAPI.WebApi.Controllers
{
    [Route("api/airdrops/current")]
    [Authorize]
    public class CurrentAirdropsController : BaseController
    {
        private readonly IAirdropService _airdropService;
        private readonly IMapper _mapper;

        public CurrentAirdropsController(IAirdropService airdropService,
                                         IMapper mapper,
                                         IUsersService usersService,
                                         ILoggerFactory loggerFactory,
                                         IUserResolverService userResolverService) : base(usersService, loggerFactory, userResolverService)
        {
            _airdropService = airdropService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentAirdrops()
        {
            var currentAirdrops = await _airdropService.GetCurrentAirdrops();
            if (!currentAirdrops?.Any(x => x.AirdropDefinition.AirdropType == AirdropType.Airdrop) ?? true)
            {
                var airdrops = GetAirdropDefinitions();
                var successfullyAdded = await _airdropService.CreateAirdrop(airdrops.ElementAt(0));
                if (successfullyAdded)
                {
                    successfullyAdded &= await _airdropService.CreateAirdrop(airdrops.ElementAt(1));
                }
                if (successfullyAdded)
                {
                    return await GetCurrentAirdrops();
                }
                else
                {
                    return BadRequest();
                }
            }
            var response = _mapper.Map<IEnumerable<AirdropSubscriptionResponse>>(currentAirdrops.Where(x => x.AirdropDefinition.AirdropType == AirdropType.Airdrop));
            return Ok(response);
        }

        [HttpGet("games")]
        public async Task<IActionResult> GetCurrentGames()
        {
            var currentGame = await _airdropService.GetCurrentGames();
            if (!currentGame?.Any() ?? true)
            {
                var games = GetGames();
                var successfullyAdded = await _airdropService.CreateGame(games.ElementAt(0));
                if (successfullyAdded)
                {
                    return await GetCurrentGames();
                }
                else
                {
                    return BadRequest();
                }
            }
            var response = _mapper.Map<IEnumerable<GameResponse>>(currentGame);
            return Ok(response);
        }

        [HttpGet("bounties")]
        public async Task<IActionResult> GetBounties()
        {
            var currentGames = await _airdropService.GetCurrentGames();
            var currentBounties = currentGames?.Where(x => x.AirdropDefinition?.AirdropType == AirdropType.BountyProgram);
            if (!currentBounties?.Any() ?? true)
            {
                var bounties = GetBountyProgram();
                var successfullyAdded = await _airdropService.CreateGame(bounties.ElementAt(0));
                if (successfullyAdded)
                {
                    return await GetBounties();
                }
                else
                {
                    return BadRequest();
                }
            }
            var response = _mapper.Map<IEnumerable<GameResponse>>(currentBounties);
            return Ok(response);
        }

        [HttpGet]
        [Route("mine")]
        public async Task<IActionResult> GetUserAirdrops()
        {
            return Ok(await _airdropService.GetUserAidrops(await GetApiUser()));
        }

        [HttpPost]
        [Route("{airdropId}")]
        public async Task<IActionResult> SubscribeToAidrop(int airdropId)
        {
            var user = await GetApiUser();
            var result = await _airdropService.SubscribeToAirdrop(user, airdropId);
            if(result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("{airdropId}")]
        public async Task<IActionResult> GetAirdrop(int airdropId)
        {
            var airdrop = await _airdropService.GetAirdropSubscription(airdropId);
            return Ok(_mapper.Map<AirdropSubscriptionResponse>(airdrop));
        }

        [HttpGet]
        [Route("bounty")]
        public async Task<IActionResult> GetGame([FromQuery] int id)
        {
            var user = await GetApiUser();
            var game = await _airdropService.GetGame(id, user);
            return Ok(_mapper.Map<GameResponse>(game));
        }

        private IEnumerable<Game> GetGames()
        {
            return new List<Game>
            {
                new Game
                {
                    AirdropDefinition = new AirdropDefinition
                                    {
                                        AirdropName = "Coinstantine on Discord",
                                        TokenName = "Coinstantine",
                                        OtherInfoToDisplay = "Games",
                                        AirdropType = AirdropType.Game,
                                        MaxLimit = -1,
                                        DiscordAirdropRequirement = new DiscordAirdropRequirement
                                        {
                                            NeedsToJoinServer = true,
                                            ServerName = "Coinstantine",
                                            ServerUrl = "https://discord.gg/mTwwdc"
                                        }
                                    },
                    Achievements = new List<Achievements>()
                }
            };
        }

        private IEnumerable<Game> GetBountyProgram()
        {
            return new List<Game>
            {
                new Game
                {
                    AirdropDefinition = new AirdropDefinition
                                    {
                                        AirdropName = "Coinstantine Referral",
                                        TokenName = "Coinstantine",
                                        OtherInfoToDisplay = "Coinstantine Referral",
                                        AirdropType = AirdropType.BountyProgram,
                                        MaxLimit = -1
                                    },
                    Achievements = new List<Achievements>()
                }
            };
        }

        private IEnumerable<AirdropDefinition> GetAirdropDefinitions()
        {
            var twitterRequirement1 = new TwitterAirdropRequirement
            {
                HasAccount = true,
                MinimumFollowers = 10,
                MinimumCreationDate = new DateTime(2018, 1, 1),
            };
            var bctRequirement1 = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                MinimumRank = BitcoinTalkRank.Newbie,
                MinimumCreationDate = new DateTime(2018, 1, 1)
            };
            var telegramRequirement1 = new TelegramAirdropRequirement
            {
                HasAccount = true
            };
            var bctRequirement2 = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                MinimumRank = BitcoinTalkRank.SrMember,
                MinimumCreationDate = new DateTime(2018, 1, 1)
            };
            var twitterRequirement2 = new TwitterAirdropRequirement
            {
                HasAccount = true,
                MinimumFollowers = 10,
                MinimumCreationDate = new DateTime(2018, 1, 1),
            };
            var telegramRequirement2 = new TelegramAirdropRequirement
            {
                HasAccount = true
            };

            var airdropDefinition = new AirdropDefinition
            {
                AirdropName = "Coinstantine Airdrop #1",
                BitcoinTalkAirdropRequirement = bctRequirement1,
                TelegramAirdropRequirement = telegramRequirement1,
                TwitterAirdropRequirement = twitterRequirement1,
                StartDate = DateTime.Now,
                ExpirationDate = new DateTime(2019, 10, 1),
                MaxLimit = 1000,
                TokenName = "Coinstantine",
                OtherInfoToDisplay = "Welcome pack",
                Amount = 100,
                AirdropType = AirdropType.Airdrop
            };

            var airdropDefinition2 = new AirdropDefinition
            {
                AirdropName = "Coinstantine Airdrop #2",
                BitcoinTalkAirdropRequirement = bctRequirement2,
                TelegramAirdropRequirement = telegramRequirement2,
                TwitterAirdropRequirement = twitterRequirement2,
                MaxLimit = 100,
                StartDate = DateTime.Now,
                ExpirationDate = new DateTime(2019, 10, 1),
                TokenName = "Coinstantine",
                OtherInfoToDisplay = "Welcome pack for Sr. Members, minimum",
                Amount = 50,
                AirdropType = AirdropType.Airdrop
            };

            return new List<AirdropDefinition> { airdropDefinition, airdropDefinition2 };
        }
    }
}
