using AutoMapper;

namespace Argumentum.AssetConverter.Entities;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Fallacy, ArgumentVirtue>()
			.ForMember(dest => dest.Pk, opt => opt.MapFrom(src => src.PK))
			.ForMember(dest => dest.Path, opt => opt.MapFrom(src => src.Path))
			.ForMember(dest => dest.Depth, opt => opt.MapFrom(src => src.Depth))
			.ForMember(dest => dest.FamilyFr, opt => opt.MapFrom(src => src.Famille))
			.ForMember(dest => dest.SubfamilyFr, opt => opt.MapFrom(src => src.SousFamille))
			.ForMember(dest => dest.SubsubfamilyFr, opt => opt.MapFrom(src => src.Soussousfamille))
			.ForMember(dest => dest.TitleFr, opt => opt.MapFrom(src => src.TextFr))
			.ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.DescFr))
			.ForMember(dest => dest.RemarkFr, opt => opt.MapFrom(src => src.ExampleFr))
			.ForMember(dest => dest.LinkFr, opt => opt.MapFrom(src => src.LinkFr)) // or LinkFr, LinkFrFallback is used for null checking in the original code
			.ForMember(dest => dest.FamilyFrCamelcase, opt => opt.MapFrom(src => src.FamilleCamelCase))
			.ForMember(dest => dest.DepthMax4, opt => opt.Ignore()) // No clear mapping
			.ForMember(dest => dest.Card, opt => opt.MapFrom(src => src.Carte.HasValue ? src.Carte.Value.ToString() : ""))
			.ForMember(dest => dest.Update, opt => opt.Ignore()) // No clear mapping
			.ForMember(dest => dest.Locked, opt => opt.Ignore()) // No clear mapping
			.ReverseMap(); // This is optional, to enable mapping from Argument to Fallacy
	}
}