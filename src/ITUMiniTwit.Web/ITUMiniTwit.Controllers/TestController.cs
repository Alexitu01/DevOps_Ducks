using System.Net;
using System.Text.Json.Nodes;
using ITUMiniTwit.Core.Models;
using ITUMiniTwit.Infrastructure.ITUMiniTwit.Repositories;
using ITUMiniTwit.Infrastructure.ITUMiniTwit.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;


public static class LatestState
{
    public static int state = 0;
}

[ApiController]
[Route("fllws")]
public class fllwsController : ControllerBase
{
    private readonly AuthorService _AuthServ;
    public fllwsController(AuthorService AuthServ)
    {
        _AuthServ = AuthServ;
    }


    [HttpGet("{username}")]
    public async Task<IActionResult> Get(string username, [FromQuery] int? latest)
    {
        try { _AuthServ.GetAuthorByName(username); } catch { return NotFound(); }
        if (!Authorization()){ 
            return StatusCode(403, new {status = 0, error_msg = "Not authorized"});
            }

        List<string> follows = await getFollowingUsernames(username);
        if(latest != null)
        {
            LatestState.state = (int) latest; 
        }
        return Ok(new { follows });

    }

    [HttpPost("{username}")]
    public async Task<IActionResult> Post(string username, [FromBody] FollowAction body, [FromQuery] int? latest)
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
        if(latest != null)
        {
            LatestState.state = (int) latest; 
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

[ApiController]
[Route("latest")]
public class latestController : ControllerBase
{
    AuthorService _AuthServ;

    public latestController(AuthorService AuthServ)
    {
        _AuthServ = AuthServ;
    }

    [HttpGet("")]
    public IActionResult Get()
    {
        return Ok(new {latest = LatestState.state});
    }
    
}

[ApiController]
[Route("msgs")]
public class messageController : ControllerBase
{  
}

[ApiController]
[Route("register")]
public class registerController : ControllerBase
{
    
}
