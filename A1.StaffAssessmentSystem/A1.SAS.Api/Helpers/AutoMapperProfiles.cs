using A1.SAS.Api.Dtos;
using A1.SAS.Domain.Entities;
using AutoMapper;

namespace A1.SAS.Api.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            #region Account
            CreateMap<TblAccount, AccountDto>().ReverseMap();
            #endregion

            #region Employee
            CreateMap<TblEmployee, EmployeeDto>().ReverseMap();
            CreateMap<TblEmployee, PostEmployeeDto>().ReverseMap();
            #endregion

            #region Range
            CreateMap<TblRange, RangeDto>().ReverseMap();
            #endregion

            #region Assessment
            CreateMap<TblAssessment, AssessmentDto>().ReverseMap();
            #endregion
        }
    }
}
