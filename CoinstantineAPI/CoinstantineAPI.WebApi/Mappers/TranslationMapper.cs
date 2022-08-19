using CoinstantineAPI.Core.Services;
using CoinstantineAPI.Data;
using CoinstantineAPI.WebApi.DTO;

namespace CoinstantineAPI.WebApi.Mappers
{
    public class TranslationMapper : IMapper<Translation, TranslationDTO>
    {
		public TranslationDTO Map(Translation source)
		{
			return new TranslationDTO
			{
				Key = source.Key,
				Text = source.Text,
				Language = source.Language,
				Comments = source.Comments
			};
		}
	}
}
