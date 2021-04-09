using System.Collections.Generic;
using blogger.Models;
using blogger.Services;
using Microsoft.AspNetCore.Mvc;

namespace blogger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
  public class ProfilesController : ControllerBase
  {
    private readonly ProfilesService _pservice;
    private readonly BlogsService _blogservice;

    private readonly CommentsService _cservice;

        public ProfilesController(ProfilesService pservice, BlogsService blogservice, CommentsService cservice)
        {
            _pservice = pservice;
            _blogservice = blogservice;
            _cservice = cservice;
        }

    [HttpGet("{id}")]
    public ActionResult<Profile> Get(string id)
    {
      try
      {
        return Ok(_pservice.GetProfileById(id));
      }
      catch (System.Exception err)
      {
        return BadRequest(err.Message);
      }
    }


    [HttpGet("{id}/blogs")]
    public ActionResult<IEnumerable<Blog>> GetBlogs(string id)
    {
      try
      {
        return Ok(_blogservice.GetBlogsByProfileId(id));
      }
      catch (System.Exception err)
      {
        return BadRequest(err.Message);
      }
    }

    [HttpGet("{id}/comments")]
    public ActionResult<IEnumerable<Comment>> GetComments(string id){
      try
      {
          return Ok(_cservice.GetCommentsByProfileId(id));
      }
      catch (System.Exception err)
      {
          return BadRequest(err.Message);
      }
    }
  }
}