using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using API_Advanced.Models;
using API_Advanced.Models.DTO;
using AutoMapper;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using API_Advanced.Data;

namespace API_Advanced.Controllers
{
    [Authorize(Roles = Role.Admin)]
    public class RoleController : BaseApiController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<IdentityRole>>> GetRoles()
        {
            var list = await _roleManager.Roles.ToListAsync();
            return Ok(list);
        }

        [HttpGet("{RoleName}")]

        public async Task<ActionResult<List<DisplayUserDto>>> GetUsersFromOneRole(string RoleName)
        {

            var users = await _userManager.GetUsersInRoleAsync(RoleName);

            return _mapper.Map<List<AppUser>, List<DisplayUserDto>>(users.ToList());
        }

        [HttpPut("{UserId}")]

        public async Task<IActionResult> ChangeUserRole(string UserId, RoleChangeDto roleChangeDto)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            var checkRoleExists = await _userManager.IsInRoleAsync(user, roleChangeDto.RoleName.ToLower());
            IdentityResult result = null;
            //Delete the role from the user
            if (checkRoleExists && roleChangeDto.Assign == false)
            {
                result = await _userManager.RemoveFromRoleAsync(user, roleChangeDto.RoleName);
            }
            //Add the role to the user
            if (!checkRoleExists && roleChangeDto.Assign == true)
            {
                result = await _userManager.AddToRoleAsync(user, roleChangeDto.RoleName);
            }
            if (result == null) return BadRequest("Nothing is changed");

            if (!result.Succeeded) return BadRequest("Unable to update the user role");

            return NoContent();
        }
    }
}
