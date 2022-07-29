using AutoMapper;
using com.tweetapp.application.Response;
using com.tweetapp.domain.DAOEntities;
using com.tweetapp.domain.Models;
using com.tweetapp.infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace com.tweetapp.application.Queries
{
    public interface IUserRegisterationQuery
    {
        Task<ApiResponse<string>> UserRegistartion(UserRegisterDAO user);
        Task<ApiResponse<string>> UserLogin(UserLoginDAO user);
        Task<ApiResponse<string>> ForgotPassword(ForgotPasswordDAO email);
        Task<ApiResponse<string>> ResetPassword(string email, string password);
        Task<ApiResponse<string>> Logout(string email);
        Task<ApiResponse<IEnumerable<UserDAO>>> GetAllUsers();
        Task<ApiResponse<IEnumerable<UserDAO>>> GetSearchedUser(string username);

    }
    public class UserRegisterationQuery : IUserRegisterationQuery
    {
        protected readonly IUserRepository  _userRepository;
        protected readonly IMapper _mapper;
        public  IConfiguration Configuration { get; }

        public UserRegisterationQuery(IUserRepository userRepository, IConfiguration configuration, IMapper mapper)
        {
            _userRepository = userRepository;
            Configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> UserRegistartion(UserRegisterDAO user)
        {
            if (user.Email==null || user.FirstName=="" || user.Gender.Equals(null)|| user.Password == "")
            {
                return new ApiResponse<string>()
                {
                    Message = "Please Fill all the Mandatory Fields",
                    StatusCode = 500,
                    Success = false,
                    Data = null
                };
            }
            else
            {
                var userDetails = await _userRepository.CheckEmailAndUserNameExists(user.UserName,user.Email);
                if (userDetails)
                {
                    var date = user.DateOfBirth.ToString().Split('-');
                    user.Password = GenerateHashPassword(user.Password, Configuration["JWT:Secret"]);
                    User user1 = new User()
                    {
                        FirstName = user.FirstName,
                        DateOfBirth = new DateTime(int.Parse(date[2]),int.Parse(date[1]),int.Parse(date[0])),
                        Email = user.Email,
                        Gender = user.Gender,
                        IsActive = true,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        PhoneNumber = user.PhoneNumber,
                        Password = user.Password
                    };
                   var result = await _userRepository.RegisterUser(user1);
                    var userDetail = await _userRepository.GetUserByEmail(user.Email);
                    if (result)
                    {
                        return new ApiResponse<string>()
                        {
                            Message = "User Registeration successful",
                            StatusCode = 201,
                            Success = true,
                            Data = GenerateToken(user1.Email, userDetail.Id,user1.UserName)
                        };
                    }
                        return new ApiResponse<string>()
                        {
                            Message = "Something Went wrong! please check again....",
                            StatusCode = 401,
                            Success = false,
                            Data = null
                        };
                    }
                    else
                    {
                        return new ApiResponse<string>()
                        {
                            Message = "User Name or Email Already Exists",
                            StatusCode = 300,
                            Success = false,
                            Data = null
                        };
                    }
               
            }
           
        }

        public async Task<ApiResponse<string>> UserLogin(UserLoginDAO user)
        {
            if(user==null || user.UserName==null || user.Password == null)
            {
                return new ApiResponse<string>()
                {
                    Message = "Please Fill all the Mandatory Fields",
                    StatusCode = 500,
                    Success = false,
                    Data = null
                };
            }
            else
            {
                var userDetails = await _userRepository.GetUserByUserName(user.UserName);
                if (userDetails == null)
                {
                    return new ApiResponse<string>()
                    {
                        Message = "User Name doesn't exists",
                        StatusCode = 300,
                        Success = false,
                        Data = null
                    };
                }
                else
                {
                    var checkPassword = GenerateHashPassword(user.Password,Configuration["JWT:Secret"]);
                    if(checkPassword != userDetails.Password)
                    {
                        return new ApiResponse<string>()
                        {
                            Message = "User Name or Password doesn't match",
                            StatusCode = 300,
                            Success = false,
                            Data = null
                        };
                    }
                    else
                    {
                        var token = GenerateToken(userDetails.Email, userDetails.Id,user.UserName);
                        return new ApiResponse<string>()
                        {
                            Message = "User Login successful",
                            StatusCode = 201,
                            Success = true,
                            Data = token
                        };
                    }
                }
            }
        }

        public async Task<ApiResponse<string>> ForgotPassword(ForgotPasswordDAO forgotPassword)
        {
            if (forgotPassword.Email == "" || forgotPassword.DateOfBirth == null || forgotPassword.Password==null)
            {
                return new ApiResponse<string>()
                {
                    Message = "Please Fill all the Mandatory Fields",
                    StatusCode = 500,
                    Success = false,
                    Data = null
                };
            }
            else
            {
                var user = await _userRepository.GetUserByEmail(forgotPassword.Email);
                if (user == null)
                {
                    return new ApiResponse<string>()
                    {
                        Message = "Email doesn't Exists",
                        StatusCode = 300,
                        Success = false,
                        Data = null
                    };
                }
                else
                {
                    Regex regex = new Regex(@"^([0]?[1-9]|[1|2][0-9]|[3][0|1])[-]([0]?[1-9]|[1][0-2])[-]([0-9]{4}|[0-9]{2})$");
                    Match match = regex.Match(forgotPassword.DateOfBirth);
                    DateTime dt = DateTime.ParseExact(forgotPassword.DateOfBirth, "dd-MM-yyyy", CultureInfo.CurrentCulture);
                    //Verify whether date entered in dd/MM/yyyy format.
                    if (match.Success && dt.Date == user.DateOfBirth.Value.Date)
                    {
                        user.Password = GenerateHashPassword(forgotPassword.Password, Configuration["JWT:Secret"]);
                        var isForgotPasswordChanged = await _userRepository.ForgotPassword(user);
                        if (isForgotPasswordChanged)
                        {
                            return new ApiResponse<string>()
                            {
                                Message = "Password Updated successfully",
                                StatusCode = 200,
                                Success = true,
                                Data = null
                            };
                        }

                        return new ApiResponse<string>()
                        {
                            Message = "Something went wrong",
                            StatusCode = 400,
                            Success = false,
                            Data = null
                        };
                    }
                    else if (!match.Success)
                    {
                        return new ApiResponse<string>()
                        {
                            Message = "Date format is not correct, Date format should be dd/MM/yyyy",
                            StatusCode = 300,
                            Success = false,
                            Data = null
                        };
                    }
                    else
                    {
                        return new ApiResponse<string>()
                        {
                            Message = "Incorrect Date of birth",
                            StatusCode = 300,
                            Success = false,
                            Data = null
                        };
                    }
                }
            }
        }

        public async Task<ApiResponse<string>> ResetPassword(string email,string password)
        {
            if(password==null || email == null)
            {
                return new ApiResponse<string>()
                {
                    Message = "Please Fill all the Mandatory Fields",
                    StatusCode = 500,
                    Success = false,
                    Data = null
                };
            }
            else
            {
                var user =await _userRepository.GetUserByEmail(email);
                if (user == null)
                {
                    return new ApiResponse<string>()
                    {
                        Message = "Email doesn't exists",
                        StatusCode = 400,
                        Success = false,
                        Data = null
                    };
                }
                else
                {
                    user.Password = GenerateHashPassword(password, Configuration["JWT:Secret"]);
                   var isChanged =  await _userRepository.ResetPassword(user);
                    if (isChanged)
                    {
                    return new ApiResponse<string>()
                    {
                        Message = "Reset Password Successful",
                        Success = true,
                        StatusCode = 200,
                        Data = null
                    };
                    }
                    else
                    {
                        return new ApiResponse<string>()
                        {
                            Message = "Something Went Wrong",
                            Success = false,
                            StatusCode = 500,
                            Data = null
                        };
                    }
                }
            }

        }

        public async Task<ApiResponse<IEnumerable<UserDAO>>> GetAllUsers()
        {
           var users= await _userRepository.GetAllUsers();
            ICollection<UserDAO> userDAOs = new List<UserDAO>();
            foreach (var item in users)
            {
                UserDAO u = new UserDAO();
                u.Email = item.Email;
                u.UserName = item.UserName;
                u.FirstName = item.FirstName;
                u.Gender = Enum.GetName(typeof(Gender), item.Gender);
                u.LastName = item.LastName;
                u.PhoneNumber = item.PhoneNumber;
                u.LastSeen = item.LastSeen;
                userDAOs.Add(u);
            }
            return new ApiResponse<IEnumerable<UserDAO>> ()
            {
                Message = "Users Retried Successful.",
                Success = true,
                StatusCode = 200,
                Data = userDAOs
            };
        }


        public async Task<ApiResponse<string>> Logout(string email)
        {
            if (email == null)
            {
                return new ApiResponse<string>()
                {
                    Message = "Please Fill all the Mandatory Fields",
                    StatusCode = 500,
                    Success = false,
                    Data = null
                };
            }
            else
            {
                var user = await _userRepository.GetUserByEmail(email);
                if (user == null)
                {
                    return new ApiResponse<string>()
                    {
                        Message = "Email doesn't exists",
                        StatusCode = 400,
                        Success = false,
                        Data = null
                    };
                }
                else
                {
                   var changes= await _userRepository.Logout(user);
                    if (changes)
                    {
                        return new ApiResponse<string>()
                        {
                            Message = "User logged out successful",
                            Success = true,
                            StatusCode = 200,
                            Data = null
                        };
                    }
                    return new ApiResponse<string>()
                    {
                        Message = "Something went wrong",
                        StatusCode = 500,
                        Success = false,
                        Data = null
                    };
                }
            }
        }



        //public static string EncodePasswordToBase64(string password)
        //{
        //    try
        //    {
        //        byte[] encData_byte = new byte[password.Length];
        //        encData_byte = Encoding.UTF8.GetBytes(password);
        //        string encodedData = Convert.ToBase64String(encData_byte);
        //        return encodedData;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error in base64Encode" + ex.Message);
        //    }
        //}

        private static string GenerateHashPassword(string password, string salt)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(salt + password);
            data = System.Security.Cryptography.MD5.Create().ComputeHash(data);
            return Convert.ToBase64String(data);
        }

        private  string GenerateToken(string email, string id,string userName)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var secToken = new JwtSecurityToken(
                signingCredentials: credentials,
                issuer: Configuration["JWT:ValidIssuer"],
                audience: Configuration["JWT:ValidAudience"],
                claims: new[]
                {
                     new Claim(ClaimTypes.Email, email),
                     new Claim(ClaimTypes.NameIdentifier,id+""),
                     new Claim(ClaimTypes.Name,userName)
                },
                expires: DateTime.UtcNow.AddMinutes(120));
            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(secToken);
        }

        public async Task<ApiResponse<IEnumerable<UserDAO>>> GetSearchedUser(string username)
        {
            if (username == null)
            {
                return new ApiResponse<IEnumerable<UserDAO>> ()
                {
                    Message = "No Users Found",
                    StatusCode = 200,
                    Success = true,
                    Data = null
                };
            }
            else
            {
                var results =await _userRepository.GetSearchedUser(username);
                var userTweet = _mapper.Map<IEnumerable<UserDAO>>(results);
                return new ApiResponse<IEnumerable<UserDAO>>()
                {
                    Message = "Users searched successfully.",
                    StatusCode = 200,
                    Success = true,
                    Data = userTweet
                };
            }
        }
    }
}
