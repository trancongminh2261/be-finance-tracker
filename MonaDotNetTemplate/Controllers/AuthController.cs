using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MonaDotNetTemplate.Entities.Models;
using MonaDotNetTemplate.Entities.PostRequests;
using MonaDotNetTemplate.Services.Interfaces.Auth;
using MonaDotNetTemplate.Utilities;
using System.Net;
using System.Reflection;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MonaDotNetTemplate.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        protected readonly IAuthenticationService authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public virtual async Task<AppDataDomainResult> LoginAsync([FromForm] LoginPostRequest loginRequest)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException();
            return new()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Data = await authenticationService.LoginAsync(loginRequest)
            };
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public virtual async Task<AppDataDomainResult> RegisterAsync([FromForm] RegisterPostRequest  registserRequest)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException();
            return new()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Data = await authenticationService.RegisterAsync(registserRequest)
            };
        }

        [AllowAnonymous]
        [HttpPost("change-password")]
        public virtual async Task<AppDataDomainResult> ChangePasswordAsync([FromForm] ChangePasswordPostRequest changePasswordRequest)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException();
            return new()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Data = await authenticationService.ChangePasswordAsync(changePasswordRequest)
            };
        }

        [AllowAnonymous]
        [HttpPost("refesh")]
        public virtual async Task<AppDataDomainResult> RefeshAsync([FromForm] string refesh)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException();
            return new()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Data = await authenticationService.RefreshAsync(refesh)
            };
        }

        [AllowAnonymous]
        [HttpGet("controller-method")]
        public virtual async Task<AppDataDomainResult> GetControllerAsync()
        {
            if (!ModelState.IsValid)
                throw new ArgumentException();
            Assembly asm = Assembly.GetExecutingAssembly();
            var controllerTypes = asm.GetTypes().Where(type => typeof(ControllerBase).IsAssignableFrom(type)).ToList();
            var listMethodModel = new List<MethodModel>();
            for (int i = 0; i < controllerTypes.Count;)
            {
                if (controllerTypes[i].Name.Contains("Base"))
                {
                    controllerTypes.RemoveAt(i);
                }
                else
                {
                    var methodModel = new MethodModel();
                    methodModel.Name = controllerTypes[i].Name;
                    methodModel.Method = new List<string>();
                    foreach(var method in controllerTypes[i].GetMethods())
                    {
                        bool isAPI = false;
                        string route = string.Empty;
                        foreach (var attribute in method.CustomAttributes)
                        {
                            if (attribute.AttributeType.Name.Contains("Http"))
                            {
                                route = attribute.ConstructorArguments.FirstOrDefault().ToString()?? "";
                                isAPI = true;
                                break;
                            }
                        }
                        if (isAPI)
                        {
                            methodModel.Method.Add(method.Name);
                        }
                    }
                    listMethodModel.Add(methodModel);                    
                    i++;
                }
            }
            return new()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Data = listMethodModel
            };
        }

        private class MethodModel
        {
            public string Name { get; set; }
            public List<string> Method { get; set; }
        }
    }
}
