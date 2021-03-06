using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using blogger.Models;
using blogger.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CodeWorks.Auth0Provider;

namespace blogger.Controllers
{
    [ApiController]
    [Route("[controller]")]

    // REVIEW this tag enforces the user must be logged in
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly ProfilesService _pservice;
        private readonly BlogsService _blogservice;

        private readonly CommentsService _cservice;

        public AccountController(ProfilesService pservice, BlogsService blogservice, CommentsService cservice)
        {
            _pservice = pservice;
            _blogservice = blogservice;
            _cservice = cservice;
        }

        [HttpGet]
        // REVIEW async calls must return a System.Threading.Tasks, this is equivalent to a promise in JS
        public async Task<ActionResult<Profile>> GetAsync()
        {
            try
            {
                // REVIEW how to get the user info from the request token
                // same as to req.userInfo
                //MAKE SURE TO BRING IN codeworks.auth0provider
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                return Ok(_pservice.GetOrCreateProfile(userInfo));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("blogs")]
        public async Task<ActionResult<IEnumerable<Blog>>> GetBlogs()
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                return Ok(_blogservice.GetBlogsByProfileId(userInfo.Id));
            }
            catch (System.Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpGet("comments")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                return Ok(_cservice.GetCommentsByProfileId(userInfo.Id));
            }
            catch (System.Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<Comment>> EditAccount([FromBody] Profile newProfile)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                newProfile.Id = userInfo.Id;
                return Ok(_pservice.EditAccount(newProfile));
            }
            catch (System.Exception err)
            {
                return BadRequest(err.Message);
            }
        }

    }
}