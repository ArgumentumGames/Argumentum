using System;
using System.Globalization;
using AutoMapper;

namespace Argumentum.AssetConverter.Entities;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<ArgumentVirtue, Fallacy>()
			.ForMember(dest => dest.PK, opt => opt.MapFrom(src => src.Pk))
			.ForMember(dest => dest.Path, opt => opt.MapFrom(src => src.Path))
			.ForMember(dest => dest.Depth, opt => opt.MapFrom(src => src.Depth))
			.ForMember(dest => dest.Famille, opt => opt.MapFrom(src => src.FamilyFr))
			.ForMember(dest => dest.SousFamille, opt => opt.MapFrom(src => src.SubfamilyFr))
			.ForMember(dest => dest.Soussousfamille, opt => opt.MapFrom(src => src.SubsubfamilyFr))
			.ForMember(dest => dest.TextFr, opt => opt.MapFrom(src => src.TitleFr))
			.ForMember(dest => dest.DescFr, opt => opt.MapFrom(src => src.DescriptionFr))
			.ForMember(dest => dest.ExampleFr, opt => opt.MapFrom(src => src.RemarkFr))
			.ForMember(dest => dest.LinkFr,
				opt => opt.MapFrom(src =>
					src.LinkFr)) // or LinkFr, LinkFrFallback is used for null checking in the original code
			.ForMember(dest => dest.FamilleCamelCase, opt => opt.MapFrom(src => src.FamilyFrCamelcase))
			.ForMember(dest => dest.Carte, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Card) ? (int?)null : int.Parse(src.Card)))
			.ForMember(dest => dest.DecimalPath, opt => opt.MapFrom(src =>  Decimal.Parse(src.DecimalPathPadded).ToString(CultureInfo.InvariantCulture)))
			// Belt-and-suspenders for GHSA-rvv3-g6hj-g44x (DoS via uncontrolled recursion). This map is
			// flat and acyclic — only scalar ForMember projections — so the vulnerable recursion path is
			// never taken on AutoMapper 14.0.0 (the last free MIT version). MaxDepth(1) makes that bound
			// explicit on both directions, mirroring the vendor's MaxDepth-default fix, so the NU1903
			// audit suppression in the .csproj is defensible rather than blind.
			.MaxDepth(1)
			.ReverseMap()
			.MaxDepth(1);
	}
}