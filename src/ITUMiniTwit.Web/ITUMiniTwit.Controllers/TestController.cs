using System.Text.Json.Nodes;
using ITUMiniTwit.Core.Models;
using ITUMiniTwit.Infrastructure.ITUMiniTwit.Repositories;
using ITUMiniTwit.Infrastructure.ITUMiniTwit.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;

[ApiController]
[Route("fllws")]
public class TestController : ControllerBase
{
    private readonly AuthorService _AuthServ;
    public TestController(AuthorService AuthServ)
    {
        _AuthServ = AuthServ;
    }


    [HttpGet("{username}")]
    public async Task<IActionResult> Get(string username)
    {
        try { _AuthServ.GetAuthorByName(username); } catch { return NotFound(); }
        if (!Authorization()){ 
            return StatusCode(403, new {status = 0, error_msg = "Not authorized"});
            }

        List<string> follows = await getFollowingUsernames(username);

        return Ok(new { follows });

    }

    [HttpPost("{username}")]
    public async Task<IActionResult> Post(string username, [FromBody] FollowAction body)
    {
        try { _AuthServ.GetAuthorByName(username); } catch { return NotFound(); }
        if (!Authorization())
        {
            return StatusCode(403, new {status = 0, error_msg = "Not authorized"});
        }

        var user = User.Identity!.Name!;
        
        if(body.Follow != null)
        {
            await _AuthServ.Follow(user, username);
        }
        if(body.Unfollow != null)
        {
            await _AuthServ.Unfollow(user,username);
        }

        return NoContent();
    }

    public bool Authorization()
    {
        string? value = Request.Headers["c2ltdWxhdG9yOnN1cGVyX3NhZmUh"];
        if(value == null)
        {
            return false;
        } else {
            return true;
        }
    }

    public async Task<List<string>> getFollowingUsernames(string user)
    {
        List<Author> list = await _AuthServ.GetFollowing(user);
        List<string> follows = new List<string>();
        foreach (Author x in list)
        {
            follows.Add(x.UserName ?? "");
        }

        return follows;
        
    }

    public class FollowAction()
    {
        public string? Follow {get; set;}
        public string? Unfollow {get; set;}
    }
}