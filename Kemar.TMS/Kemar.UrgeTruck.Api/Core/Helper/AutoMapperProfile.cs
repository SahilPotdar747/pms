using AutoMapper;
using Kemar.TMS.Domain.DTOs;
using Kemar.TMS.Domain.RequestModel;
using Kemar.TMS.Domain.ResponseModel;
using Kemar.TMS.Repository.Entities;
using Kemar.UrgeTruck.Domain.DTOs;
using Kemar.UrgeTruck.Domain.RequestModel;
using Kemar.UrgeTruck.Domain.ResponseModel;
using Kemar.UrgeTruck.Repository.Entities;

namespace Kemar.UrgeTruck.Api.Core.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterUserRequest, UserManager>().ReverseMap();
            CreateMap<UserManager, AuthenticateResponse>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.RoleMaster.RoleName));
            CreateMap<RoleMaster, RoleMasterResponse>();
            CreateMap<RoleMasterResponse, RoleMasterRequest>();
            CreateMap<RoleMasterRequest, RoleMaster>().ReverseMap();
            CreateMap<UserScreenMasterRequest, UserScreenMaster>();
            CreateMap<UserScreenMaster, UserScreenMasterResponse>();
            CreateMap<UserAccessManagerRequest, UserAccessManager>();
            CreateMap<UserAccessManagerResponse, UserAccessManagerRequest>();
            CreateMap<UserAccessManager, UserAccessManagerResponse>()
                .ForMember(dest => dest.ScreenName, opt => opt.MapFrom(src => src.UserScreenMaster.ScreenName))
                .ForMember(dest => dest.ScreenCode, opt => opt.MapFrom(src => src.UserScreenMaster.ScreenCode))
                  .ForMember(dest => dest.MenuIcon, opt => opt.MapFrom(src => src.UserScreenMaster.MenuIcon))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleMaster.RoleName))
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.UserScreenMaster.ParentId));

            CreateMap<UserManager, UserTokenDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleMaster.RoleName))
                .ForMember(dest => dest.UserAccess, opt => opt.MapFrom(src => src.RoleMaster.UserAccessManager));

            CreateMap<UserAccessManager, UserAccessDto>()
                .ForMember(dest => dest.ScreenCode, opt => opt.MapFrom(src => src.UserScreenMaster.ScreenCode))
                 .ForMember(dest => dest.CanRead, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<RoleMaster, UserControlResponse>()
          .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
          .ForMember(dest => dest.UserAccessManagerResonse, opt => opt.MapFrom(src => src.UserAccessManager));
            CreateMap<ApplicationConfigMaster, ApplicationConfigurationResponse>();
            CreateMap<ApplicationConfigurationRequest, ApplicationConfigMaster>();
            //Master
            CreateMap<DepartmentMaster, DepartmentMasterResponse>();
            CreateMap<DepartmentMasterResponse, DepartmentMasterRequest>();
            CreateMap<DepartmentMasterRequest, DepartmentMaster>().ReverseMap();

            CreateMap<DesignationMaster, DesignationMasterResponse>();
            CreateMap<DesignationMasterResponse, DesignationMasterRequest>();
            CreateMap<DesignationMasterRequest, DesignationMaster>().ReverseMap();

            CreateMap<ProjectMaster, ProjectMasterResponse>();
            CreateMap<ProjectMasterResponse, ProjectMasterRequest>();
            CreateMap<ProjectMasterRequest, ProjectMaster>().ReverseMap();

            CreateMap<TaskTypeMaster, TaskTypeMasterResponse>();
            CreateMap<TaskTypeMasterResponse, TaskTypeMasterRequest>();
            CreateMap<TaskTypeMasterRequest, TaskTypeMaster>().ReverseMap();

            CreateMap<TaskTransaction, TaskTransactionResponse>();
            CreateMap<TaskTransaction, TaskTransactionResponseForDownload>()
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.ProjectMaster.ProjectName))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.DepartmentMaster.DepartmentName))
                .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.UserManager.FirstName + " " + src.UserManager.LastName))
                .ForMember(dest => dest.TaskTypeName, opt => opt.MapFrom(src => src.TaskTypeMaster.TaskName));
            CreateMap<DelegateHistory, DelegateHistoryResponse>();

            CreateMap<UserManager, UserForTaskResponse>();
            CreateMap<NotificationRequest, Notification>();
            CreateMap<Notification, NotificationResponse>();
            CreateMap<UserManager, UserResponse>();
            CreateMap<RegisterUserRequest, UserManager>();

            CreateMap<TaskTransaction, TaskHistory>();
            CreateMap<TaskTransaction, TaskTransactionResponse>(); 

            CreateMap<ProjectWiseTaskCountReport, ProjectWiseTaskCountReportResponse>();
            CreateMap<ProjectWiseTaskCountReport, ProjectWiseTaskCountReportResponseForDownload>();
            CreateMap<UserWiseTaskDataCountReport, UserWiseTaskDataCountReportResponse>();
            CreateMap<UserWiseTaskDataCountReport, UserWiseTaskDataCountReportResponseToDownload>();

            CreateMap<UserManager, UserResponseNew>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.DepartmentMaster.DepartmentName))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleMaster.RoleName))
                .ForMember(dest => dest.DesignationName, opt => opt.MapFrom(src => src.DesignationMaster.DesignationName));
        }
    }
}
