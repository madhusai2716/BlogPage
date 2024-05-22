using BloggingCode.API.Models.DTO;
using BloggingCode.API.Repo.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace BloggingCode.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthContoller : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepo tokenRepo;
        public AuthContoller(UserManager<IdentityUser> userManager, ITokenRepo tokenRepo)
        {
            this.userManager = userManager;
            this.tokenRepo = tokenRepo;
            
        }
        
        //Post : {apibasedUrl}/api/auth/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequest)
        {
           var identityUser  = await userManager.FindByEmailAsync(loginRequest.Email);
            if(identityUser is not null)
            {
                //Check Password
                var checkPassword =await userManager.CheckPasswordAsync(identityUser, loginRequest.Password);
                if (checkPassword)
                {
                    var roles = await userManager.GetRolesAsync(identityUser);
                    //create a token
                    var jwtToken = tokenRepo.CreateJwtToken(identityUser, roles.ToList());
                    var response = new LoginResponseDto()
                    {
                        Email = loginRequest.Email,
                        Roles = roles.ToList(),
                        Token = jwtToken
                    };
                    return Ok(response);


                }
                else
                {
                    return BadRequest("Password is incorrect.");
                   // ModelState.AddModelError("", "Email or password is incorrect.");
                    //return ValidationProblem(ModelState);
                }
            }
            //ModelState.AddModelError("", "Email or password is Incorrect");

            // return ValidationProblem(ModelState);
            else
            {
                //ModelState.AddModelError("", "User does not exist.");
                //return ValidationProblem(ModelState);
                return BadRequest("User does not exist.");
            }

        }



        //Post: {apibasedUrl}/api/auth/register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request) 
        {
            if (!IsValidEmail(request.Email))
            {
                return BadRequest("Invalid email address format.");
            }
            //perform additional custom password validation
            else if(!IsPasswordValid(request.Password))
            {
                return BadRequest("Password must be atleast 6 characters and atleast one special character.");
            }
            
            //Create the identity user object
            var user = new IdentityUser
            {
                UserName = request.Email?.Trim(),
                Email = request.Email?.Trim()
            };
            //Create User
            var identityResult = await userManager.CreateAsync(user, request.Password);
            if(identityResult.Succeeded)
            {
                //add role to user so that perform that action only
                identityResult = await userManager.AddToRoleAsync(user, "Reader");
                if(identityResult.Succeeded)
                {
                    return Ok();
                }
            }
            else
            {
                if(identityResult.Errors.Any())
                {
                    foreach(var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return ValidationProblem(ModelState);

        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

       

        private bool IsPasswordValid(string password)
        {
        // Password length should be greater than 6
        if (password.Length < 6)
        {
            return false;
        }

        // Check if the password contains at least one capital letter
        bool containsCapital = Regex.IsMatch(password, @"[A-Z]");

        // Check if the password contains at least one special character
        bool containsSpecialChar = Regex.IsMatch(password, @"[^a-zA-Z0-9]");

        // Return true if all conditions are met
        return containsCapital && containsSpecialChar;
         }


}
}

