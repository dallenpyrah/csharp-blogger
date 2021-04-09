using System.Collections.Generic;
using System.Threading.Tasks;
using blogger.Models;
using blogger.Services;
using CodeWorks.Auth0Provider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blogger.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BlogsController : ControllerBase
    {

        private readonly BlogsService _service;
        private readonly CommentsService _cservice;

        public BlogsController(BlogsService service, CommentsService cservice)
        {
            _service = service;
            _cservice = cservice;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Blog>> GetAll()
        {
            try
            {
                return Ok(_service.GetAll());
            }
            catch (System.Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpGet("{id}/comments")]
        public ActionResult<IEnumerable<Comment>> GetCommentsByBlogId(int id)
        {
            try
            {
                return Ok(_cservice.GetCommentsByBlogId(id));
            }
            catch (System.Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Blog> GetOneById(int id)
        {
            try
            {
                return Ok(_service.GetOneById(id));
            }
            catch (System.Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Blog>> CreateBlogAsync([FromBody] Blog newBlog)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                newBlog.CreatorId = userInfo.Id;
                Blog created = _service.CreateBlog(newBlog);
                created.Creator = userInfo;
                return created;
            }
            catch (System.Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Blog>> EditBlog(int id, [FromBody] Blog editedBlog)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                editedBlog.Id = id;
                userInfo.Id = editedBlog.CreatorId;
                Blog edited = _service.EditBlog(editedBlog);
                edited.Creator = userInfo;
                return edited;
            }
            catch (System.Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Blog>> DeleteBlog(int id)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                return Ok(_service.DeleteBlog(id, userInfo.Id));
            }
            catch (System.Exception err)
            {
                return BadRequest(err.Message);
            }
        }

    }
}