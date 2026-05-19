using ProjectManagement.Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.IServices
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> LoginAsync(LoginDTO loginDTO);

        Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDTO);
    }
}
