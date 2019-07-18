using System;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Principal;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

using MongoDB_Identity.Models;

namespace MongoDB_Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register-user")]
        public async Task<bool> RegisterUser([FromBody] UserModel user)
        {
            var new_user = new ApplicationUser { UserName = user.Email, Email = user.Email };
            var result = await _userManager.CreateAsync(new_user, user.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(new_user, "Admin");
                return true;
            }

            return false;
        }

        [HttpPost("verify-roles")]
        [AllowAnonymous]
        public async Task<List<bool>> VerifyRoles([FromBody] UserModel user)
        {
            List<bool> roles = new List<bool>();

            await _signInManager.PasswordSignInAsync(user.Email, user.Password, false, lockoutOnFailure: false);
            var logged = await _userManager.FindByEmailAsync(user.Email);
            roles.Add(await _userManager.IsInRoleAsync(logged, "Admin"));
            roles.Add(await _userManager.IsInRoleAsync(logged, "Manager"));
            roles.Add(await _userManager.IsInRoleAsync(logged, "Member"));

            return roles;
        }

        [HttpPost("add-role")]
        public async Task<bool> AddRole([FromBody] AddRoleModel model)
        {
            var usr = await _userManager.FindByEmailAsync(model.Email);
            var result = await _userManager.AddToRoleAsync(usr, model.Role);
            if (result.Succeeded)
                return true;

            return false;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<object> Post([FromBody]UserModel usuario, [FromServices]SigningConfigurations signingConfigurations, [FromServices]TokenConfigurations tokenConfigurations)
        {
            try
            {
                bool credenciaisValidas = false;
                if (usuario != null && !string.IsNullOrWhiteSpace(usuario.Email))
                {
                    // Retrieve User
                    var userIdentity = await _userManager.FindByEmailAsync(usuario.Email);
                    if (userIdentity != null)
                    {
                        // LogIn User
                        var resultadoLogin = _signInManager
                            .CheckPasswordSignInAsync(userIdentity, usuario.Password, false)
                            .Result;
                        if (resultadoLogin.Succeeded)
                        {
                            // Verify permission
                            credenciaisValidas = _userManager.IsInRoleAsync(
                                userIdentity, Roles.ROLE_API_DOSSIER).Result;
                        }
                    }
                }

                if (credenciaisValidas)
                {
                    ClaimsIdentity identity = new ClaimsIdentity(
                        new GenericIdentity(usuario.Email, "Login"),
                        new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Email),
                        new Claim("roles", "Member"),
                        new Claim("roles", "Admin")
                        }
                    );

                    DateTime dataCriacao = DateTime.Now;
                    DateTime dataExpiracao = dataCriacao +
                        TimeSpan.FromSeconds(tokenConfigurations.Seconds);

                    var handler = new JwtSecurityTokenHandler();
                    var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                    {
                        Issuer = tokenConfigurations.Issuer,
                        Audience = tokenConfigurations.Audience,
                        SigningCredentials = signingConfigurations.SigningCredentials,
                        Subject = identity,
                        NotBefore = dataCriacao,
                        Expires = dataExpiracao
                    });
                    var token = handler.WriteToken(securityToken);

                    return new
                    {
                        authenticated = true,
                        created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                        expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                        accessToken = token,
                        message = "OK"
                    };
                }
                else
                {
                    return new
                    {
                        authenticated = false,
                        message = "Falha ao autenticar"
                    };
                }
            }
            catch (Exception ex)
            {
                return new
                {
                    authenticaded = false,
                    message = "Um erro ocorreu: " + ex.Message
                };
            }
            
        }
    }
}