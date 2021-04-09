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
    public class CommentsController : ControllerBase
    {

        private readonly CommentsService _service;

        public CommentsController(CommentsService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Comment>> GetAll()
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

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Comment>> CreateCommentAsync([FromBody] Comment newComment)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                newComment.CreatorId = userInfo.Id;
                _service.CreateComment(newComment);
                newComment.Creator = userInfo;
                return Ok(newComment);
            }
            catch (System.Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Comment>> EditCommentAsync(int id, [FromBody] Comment editedComment)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                editedComment.Id = id;
                userInfo.Id = editedComment.CreatorId;
                _service.EditComment(editedComment);
                editedComment.Creator = userInfo;
                return Ok(editedComment);
            }
            catch (System.Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Comment>> DeleteCommentAsync(int id)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                return Ok(_service.DeleteComment(id, userInfo.Id));
            }
            catch (System.Exception err)
            {
                return BadRequest(err.Message);
            }
        }
    }
}