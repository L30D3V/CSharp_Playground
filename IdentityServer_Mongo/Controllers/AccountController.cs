using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Principal;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

using IdentityServer_Mongo.Helpers;
using IdentityServer_Mongo.Entities;

namespace IdentityServer_Mongo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<ResultResponse<LoginResponse>>> Login(
            [FromBody] UserModel usuario,
            [FromServices]SigningConfigurations signingConfigurations,
            [FromServices]TokenConfigurations tokenConfigurations)
        {
            Stopwatch _stopwatch = Stopwatch.StartNew();

            var response = new ResultResponse<LoginResponse>();
            response.QueryId = HttpContext.TraceIdentifier;

            try
            {
                ApplicationUser user;
                LoginResponse loginResponse = new LoginResponse();

                if (usuario != null && !string.IsNullOrWhiteSpace(usuario.Email))
                {
                    // Retrieve User
                    user = await _userManager.FindByEmailAsync(usuario.Email);

                    // Login User
                    if (user != null)
                        loginResponse.Authenticated = _signInManager.CheckPasswordSignInAsync(user, usuario.Password, false).Result.Succeeded;
                    else
                        throw new BusinessException("Email informado não cadastrado no sistema.");

                    if (loginResponse.Authenticated)
                    {
                        ClaimsIdentity identity = new ClaimsIdentity(
                            new ClaimsIdentity(new GenericIdentity(usuario.Email, "Login")),
                            new[] {
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Email),
                            }
                        );

                        var roles = await _userManager.GetRolesAsync(user);
                        foreach (var role in roles)
                        {
                            identity.AddClaim(new Claim("roles", role));
                        }

                        DateTime dataCriacao = DateTime.Now;
                        DateTime dataExpiracao = dataCriacao + TimeSpan.FromSeconds(tokenConfigurations.Seconds);

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

                        loginResponse.AccessToken = handler.WriteToken(securityToken);
                        loginResponse.Created = dataCriacao.ToString("dd-MM-yyyy HH:mm:ss");
                        loginResponse.Expiration = dataExpiracao.ToString("dd-MM-yyyy HH:mm:ss");

                        Console.WriteLine($"[{DateTime.Now} | Login] - Usuário autenticado. " + user.UserName + " : " + loginResponse.AccessToken);

                        response.Result = loginResponse;
                        response.Status = new ResultStatus() { Code = 200, Message = "Ok" };
                        response.ElapsedMilliseconds = _stopwatch.ElapsedMilliseconds;

                        _stopwatch.Stop();

                        return Ok(response);
                    }
                    else
                        throw new BusinessException("Usuário ou senha incorretos.");
                }
                else
                    throw new BusinessException("Dados obrigatórios não informados. Verifique sua requisição e tente novamente.");
            }
            catch (BusinessException ex)
            {
                Console.WriteLine($"[{DateTime.Now} | Login] - Falha no login: " + ex.Message);

                response.Result = new LoginResponse();
                response.Status = new ResultStatus() { Code = 400, Message = ex.Message };

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now} | Login] - Falha no login: " + ex.Message);

                response.Result = new LoginResponse();
                response.Status = new ResultStatus() { Code = 500, Message = "Erro ao autenticar." };

                return Ok(response);
            }
        }

        [HttpPost("register-user")]
        public async Task<ActionResult<ResultResponse<bool>>> RegisterUser([FromBody] UserModel model)
        {
            Stopwatch _stopwatch = Stopwatch.StartNew();

            var response = new ResultResponse<bool>();
            response.QueryId = HttpContext.TraceIdentifier;

            try
            {
                if (string.IsNullOrEmpty(model.Email))
                    throw new BusinessException("Email do usuário não informado.");
                if (string.IsNullOrEmpty(model.Password))
                    throw new BusinessException("Senha do usuário não informada.");
                if (string.IsNullOrEmpty(model.ConfirmPassword))
                    throw new BusinessException("Confirmação de senha não informada.");
                if (model.Password != model.ConfirmPassword)
                    throw new BusinessException("As senhas não são iguais. Tente novamente.");

                ApplicationUser new_user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(new_user, model.Password);
                if (result.Succeeded)
                {
                    response.Result = true;
                    response.Status = new ResultStatus() { Code = 200, Message = "Usuário registrado com sucesso." };
                    response.ElapsedMilliseconds = _stopwatch.ElapsedMilliseconds;

                    _stopwatch.Stop();

                    return response;
                }
                else
                {
                    string errors = "";
                    foreach (var error in result.Errors)
                    {
                        errors += "Erro: " + error.Description;
                    }

                    throw new Exception(errors);
                }
            }
            catch (BusinessException ex)
            {
                Console.WriteLine($"[{DateTime.Now} | Cadastrar Usuário] - Falha no cadastro: " + ex.Message);

                response.Result = false;
                response.Status = new ResultStatus() { Code = 400, Message = ex.Message };
                response.ElapsedMilliseconds = _stopwatch.ElapsedMilliseconds;

                _stopwatch.Stop();

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now} | Cadastrar Usuário] - Falha no cadastro: " + ex.Message);

                response.Result = false;
                response.Status = new ResultStatus() { Code = 500, Message = "Falha ao registrar o usuário. " + ex.Message };
                response.ElapsedMilliseconds = _stopwatch.ElapsedMilliseconds;

                _stopwatch.Stop();

                return response;
            }
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
    }
}