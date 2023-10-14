﻿using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IAuthService
    {
        Task<AuthDto> Authenticate(LoginRequest request);

        Task<string> Register(RegisterRequest request);

        Task<AuthDto> RefreshToken(RefreshTokenRequest request);

        Task RevokeRefreshToken(string userId);

        Task RevokeAllRefreshToken();
    }
}