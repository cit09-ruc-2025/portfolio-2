using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.DTOs;
using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.Models;
using WebServiceLayer.Utils;

namespace WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IReviewService _reviewService;
        protected readonly IMapper _mapper;

        public UserController(IUserService userService, IReviewService reviewService, LinkGenerator generator, IMapper mapper) : base(generator)
        {
            _userService = userService;
            _reviewService = reviewService;
            _mapper = mapper;
        }
        
        [HttpPost]
        public IActionResult CreateUser(CreateUserRequest user)
        {
            var validationErrors = new Dictionary<string, string>();

            bool emailExists = _userService.GetUserByEmail(user.Email) != null;

            if (emailExists)
            {
                validationErrors.Add("email", "EMAIL_EXISTS");
            }

            bool usernameExists = _userService.GetUserByUsername(user.Username) != null;

            if (usernameExists)
            {
                validationErrors.Add("username", "USERNAME_EXISTS");

            }

            if (validationErrors.Any())
            {
                return BadRequest(new { errors = validationErrors });
            }

            var PasswordHasher = new PasswordHasher();

            var newUser = new User
            {
                Email = user.Email,
                Username = user.Username,
                HashedPassword = PasswordHasher.HashPassword(user.Password),
                ProfileUrl = user.ProfileUrl
            };

            _userService.CreateUser(newUser);

            return Created();

        }

        [HttpGet("{userId}/reviews", Name = nameof(UserReviewList))]
        public IActionResult UserReviewList([FromRoute] string userId, [FromQuery] QueryParams queryParams)
        {

            if (!Guid.TryParse(userId, out Guid parsedUserId))
            {
                return BadRequest(new
                {
                    errors = new { userId = "INVALID_USER_ID" }
                });
            }

            var user = _userService.GetById(parsedUserId);

            if (user == null)
            {
                return NotFound(new
                {
                    errors = new
                    {
                        user = "USER_NOT_FOUND"
                    }
                });
            }

            var ratings = _reviewService
                .GetByUserId(parsedUserId, queryParams.Page, queryParams.PageSize);

            var mappedRatings = ratings.Items
            .Select(x => CreateRatingListModel(x));

            if (ratings.Items.Count < 1)
            {
                return NoContent();
            }

            var results = CreatePaging(nameof(UserReviewList), mappedRatings, ratings.Total, queryParams);

            return Ok(results);
        }

        private ReviewWithRating CreateRatingListModel(ReviewWithRating rating)
        {
            var model = _mapper.Map<ReviewWithRating>(rating);
            model.UserUrl = GetUrl(nameof(UserReviewList), new { id = rating.UserId });
            return model;
        }


    }
}