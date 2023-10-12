using AutoMapper;
using green_craze_be_v1.Application.Common.Enums;
using green_craze_be_v1.Application.Common.Exceptions;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.User;
using green_craze_be_v1.Application.Specification.User;
using green_craze_be_v1.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace green_craze_be_v1.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICurrentUserService _currentUserService;

        public UserService(UserManager<AppUser> userManager, IMapper mapper, IUploadService uploadService, IUnitOfWork unitOfWork, IDateTimeService dateTimeService, ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _uploadService = uploadService;
            _unitOfWork = unitOfWork;
            _dateTimeService = dateTimeService;
            _currentUserService = currentUserService;
        }

        public async Task<bool> ChangePassword(ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId)
                ?? throw new NotFoundException("Cannot find this user");

            if (request.NewPassword != request.ConfirmPassword)
                throw new ValidationException("Confirm password does not match");

            var res = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (!res.Succeeded)
                throw new ValidationException("Cannot change your password, your OldPassword may be incorrect");

            return true;
        }

        public async Task<string> CreateStaff(CreateStaffRequest request)
        {
            var user = _mapper.Map<AppUser>(request);
            user.UserName = Regex.Replace(request.Email, "[^A-Za-z0-9 -]", "");
            user.CreatedAt = _dateTimeService.Current;
            user.CreatedBy = _currentUserService.UserId;
            if (request.Avatar != null)
            {
                var url = await _uploadService.UploadFile(request.Avatar);
                user.Avatar = url;
            }
            var staff = new Staff()
            {
                Type = request.Type,
                Code = request.Code,
            };
            user.Staff = staff;
            user.Cart = new Cart();
            var res = await _userManager.CreateAsync(user, request.Password);

            if (res.Succeeded)
            {
                List<string> roles = new()
                    {
                       USER_ROLE.STAFF,
                       USER_ROLE.USER
                    };
                await _userManager.AddToRolesAsync(user, roles);

                return user.Id;
            }

            string error = "";
            res.Errors.ToList().ForEach(x => error += (x.Description + "/n"));
            throw new Exception(error);
        }

        public async Task<PaginatedResult<StaffDto>> GetListStaff(GetUserPagingRequest request)
        {
            var staffSpec = new StaffSpecification(request, isPaging: true);

            var countSpec = new StaffSpecification(request);

            var staffList = await _unitOfWork.Repository<Staff>().ListAsync(staffSpec);
            var count = await _unitOfWork.Repository<Staff>().CountAsync(countSpec);
            var listDto = new List<StaffDto>();
            foreach (var staff in staffList)
            {
                var staffDto = _mapper.Map<StaffDto>(staff);
                staffDto.User.Roles = (await _userManager.GetRolesAsync(staff.User)).ToList();

                listDto.Add(staffDto);
            }

            return new PaginatedResult<StaffDto>(listDto, request.PageIndex, count, request.PageSize);
        }

        public async Task<PaginatedResult<UserDto>> GetListUser(GetUserPagingRequest request)
        {
            var userSpec = new UserSpecification(request, isPaging: true);

            var countSpec = new UserSpecification(request);

            var userList = await _unitOfWork.Repository<AppUser>().ListAsync(userSpec);
            var count = await _unitOfWork.Repository<AppUser>().CountAsync(countSpec);
            var listDto = new List<UserDto>();
            foreach (var user in userList)
            {
                var userRoles = (await _userManager.GetRolesAsync(user)).ToList();
                var userDto = _mapper.Map<UserDto>(user);
                userDto.Roles = userRoles;
                listDto.Add(userDto);
            }

            return new PaginatedResult<UserDto>(listDto, request.PageIndex, count, request.PageSize);
        }

        public async Task<UserDto> GetUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new NotFoundException("Cannot find this user");
            var userRoles = (await _userManager.GetRolesAsync(user)).ToList();
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = userRoles;

            return userDto;
        }

        public async Task<bool> ToggleUserStatus(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new NotFoundException("Cannot find this user");
            user.Status = user.Status == 1 ? 0 : 1;
            user.UpdatedAt = _dateTimeService.Current;
            user.UpdatedBy = _currentUserService.UserId;
            var res = await _userManager.UpdateAsync(user);
            if (!res.Succeeded)
                throw new Exception("Cannot toggle status of user");

            return true;
        }

        public async Task<bool> UpdateStaff(UpdateStaffRequest request)
        {
            var staff = await _unitOfWork.Repository<Domain.Entities.Staff>().GetEntityWithSpec(new StaffSpecification(request.Id))
                ?? throw new NotFoundException("Cannot find this staff");

            await UpdateProperty(request, staff.User);
            staff.Type = request.Type;
            staff.Code = request.Code;
            staff.UpdatedAt = _dateTimeService.Current;
            staff.UpdatedBy = _currentUserService.UserId;
            if (!string.IsNullOrEmpty(request.Password))
            {
                staff.User.PasswordHash = _userManager.PasswordHasher.HashPassword(staff.User, request.Password);
            }
            _unitOfWork.Repository<Domain.Entities.Staff>().Update(staff);
            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
                throw new Exception("Cannot update staff information");

            return true;
        }

        private async Task UpdateProperty(UpdateUserRequest request, AppUser user)
        {
            var url = user.Avatar;
            _mapper.Map(request, user);
            if (request.Avatar != null)
            {
                var imageUrl = user.Avatar;
                user.Avatar = await _uploadService.UploadFile(request.Avatar);
                await _uploadService.DeleteFile(imageUrl);
            }
            else
            {
                user.Avatar = url;
            }
        }

        public async Task<bool> UpdateUser(UpdateUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId)
                ?? throw new NotFoundException("Cannot find this user");
            await UpdateProperty(request, user);

            user.UpdatedAt = _dateTimeService.Current;
            user.UpdatedBy = _currentUserService.UserId;
            var res = await _userManager.UpdateAsync(user);
            if (!res.Succeeded)
                throw new ValidationException("Cannot update of user");

            return true;
        }

        public async Task<StaffDto> GetStaff(long staffId)
        {
            var staff = await _unitOfWork.Repository<Domain.Entities.Staff>().GetEntityWithSpec(new StaffSpecification(staffId))
                ?? throw new NotFoundException("Cannot find this staff");

            var staffRoles = (await _userManager.GetRolesAsync(staff.User)).ToList();
            var staffDto = _mapper.Map<StaffDto>(staff);
            staffDto.User.Roles = staffRoles;

            return staffDto;
        }

        public async Task<bool> DisableListUserStatus(List<string> userIds)
        {
            foreach (var userId in userIds)
            {
                var user = await _userManager.FindByIdAsync(userId)
                    ?? throw new NotFoundException("Cannot find this user");
                user.Status = 0;
                user.UpdatedAt = _dateTimeService.Current;
                user.UpdatedBy = _currentUserService.UserId;
                var res = await _userManager.UpdateAsync(user);
                if (!res.Succeeded)
                    throw new Exception("Cannot toggle status of user");
            }

            return true;
        }
    }
}