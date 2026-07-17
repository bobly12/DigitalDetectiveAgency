using AutoMapper;
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Models.DTOs.Case;
using DigitalDetectiveAgency.Models.DTOs.CaseAttempt;
// ADD THESE TWO USINGS BELOW TO RESOLVE THE SYMBOLS:
using DigitalDetectiveAgency.Models.DTOs.Evidence; 
using DigitalDetectiveAgency.Models.DTOs.Witness;
using DigitalDetectiveAgency.Models.DTOs.Suspect;
using SuspectDto = DigitalDetectiveAgency.Models.DTOs.Case.SuspectDto;

namespace DigitalDetectiveAgency.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Case, CaseListDto>();
            
            CreateMap<Case, CaseDetailDto>()
                .ForMember(dest => dest.Suspects, opt => opt.MapFrom(src => src.Suspects))
                .ForMember(dest => dest.Evidences, opt => opt.MapFrom(src => src.Evidences))
                .ForMember(dest => dest.Witnesses, opt => opt.MapFrom(src => src.Witnesses));

            // These will resolve once the namespace usings above match your folder structure
            CreateMap<Evidence, EvidenceDto>();
            CreateMap<Witness, WitnessDto>();
            CreateMap<Suspect, SuspectDto>();
            CreateMap<CaseAttempt, CaseAttemptResultDto>();
        }
    }
}