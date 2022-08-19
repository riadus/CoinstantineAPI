using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CoinstantineAPI.Core.DataProvider;
using CoinstantineAPI.Core.Services;
using CoinstantineAPI.Data;
using HtmlAgilityPack;

namespace CoinstantineAPI.DataProvider.BitcoinTalkProvider
{
    public class BitcoinTalkPublicProfileProvider : IBitcoinTalkPublicProfileProvider
    {
        private HttpClient _httpClient;
        private Dictionary<string, Action<string, BitcoinTalkUserDTO>> _userBuilderDictionary;
        private readonly IMapper<BitcoinTalkUserDTO, BitcoinTalkProfile> _mapper;
        public BitcoinTalkPublicProfileProvider(IMapper<BitcoinTalkUserDTO, BitcoinTalkProfile> mapper)
        {
            _mapper = mapper;
            BuildDictionary();
        }

        public async Task<BitcoinTalkProfile> GetUser(int id)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri($"https://bitcointalk.org/index.php?action=profile;u={id}")
            };
            var html = await _httpClient.GetStringAsync("");
            var htmlDoc = new HtmlDocument();
            var fixedHtml = html.Replace("</td>\n\t\t\t\t\t</td>\n\t\t\t\t</tr><tr>\n\t\t\t\t\t<td colspan=\"2\"><hr size=\"1\" width=\"100%\" class=\"hrcolor\" /></td>\n\t\t\t\t</tr>", "</td>\n\t\t\t\t</tr><tr>\n\t\t\t\t\t<td colspan=\"2\"><hr size=\"1\" width=\"100%\" class=\"hrcolor\" /></td>\n\t\t\t\t</tr>");
            htmlDoc.LoadHtml(fixedHtml);
            var bodyareaDiv = htmlDoc.GetElementbyId("bodyarea");
            var user = new BitcoinTalkUserDTO
            {
                BctId = id
            };
            var table = bodyareaDiv.SelectNodes("//table/tr/td/table/tr[2]/td/table");
            if (!table?.Any() ?? true || table[0]?.ChildNodes == null)
            {
                return null;
            }
            foreach (var child in table[0].ChildNodes)
            {
                try
                {
                    if (child.FirstChild == null) { continue; }
                    var node = child.SelectNodes("td");
                    if (node?.Count >= 2)
                    {
                        BuildUser(node[0].InnerText, node[1].InnerText, user);
                    }
                }
                catch
                {
                    continue;
                }
            }
            return _mapper.Map(user);
        }

        private void BuildDictionary()
        {
            _userBuilderDictionary = new Dictionary<string, Action<string, BitcoinTalkUserDTO>>{
                {"Name:", (value, user) => user.Name = value},
                {"Posts:", (value, user) => user.Posts = value},
                {"Activity:", (value, user) => user.Activity = value},
                {"Position:", (value, user) => user.Position = value},
                {"DateRegistered:", (value, user) => user.RegistrationDate = value},
                {"LastActive::", (value, user) => user.LastActive = value},
                {"ICQ:", (value, user) => user.Icq = value},
                {"AIM:", (value, user) => user.Aim = value},
                {"MSN:", (value, user) => user.Msn = value},
                {"YIM:", (value, user) => user.Yim = value},
                {"Age:", (value, user) => user.Age = value},
                {"Location:", (value, user) => user.Location = value},
                {"Trust:", (value, user) => user.Trust = value},
            };

        }

        private void BuildUser(string key, string value, BitcoinTalkUserDTO user)
        {
            var trimmedKey = key.Replace(" ", "");
            if (_userBuilderDictionary.ContainsKey(trimmedKey))
            {
                _userBuilderDictionary[trimmedKey].Invoke(value, user);
            }
        }
    }
}
