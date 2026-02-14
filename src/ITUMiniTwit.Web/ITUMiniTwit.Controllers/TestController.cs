using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using ITUMiniTwit.Core;
using ITUMiniTwit.Core.Models;
using ITUMiniTwit.Infrastructure.ITUMiniTwit.Repositories;
using ITUMiniTwit.Infrastructure.ITUMiniTwit.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;


public static class LatestState
{
    public static int? state = null;
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
        if (!Authorization())
        {
            return StatusCode(403, new { status = 0, error_msg = "Not authorized" });
        }

        List<string> follows = await getFollowingUsernames(username);
        if (latest != null)
        {
            LatestState.state = (int)latest;
        }
        return Ok(new { follows });

    }

    [HttpPost("{username}")]
    public async Task<IActionResult> Post(string username, [FromBody] FollowAction body, [FromQuery] int? latest)
    {
        try { _AuthServ.GetAuthorByName(username); } catch { return NotFound(); }
        if (!Authorization())
        {
            return StatusCode(403, new { status = 0, error_msg = "Not authorized" });
        }

        var user = User.Identity!.Name!;

        if (body.Follow != null)
        {
            await _AuthServ.Follow(user, username);
        }
        if (body.Unfollow != null)
        {
            await _AuthServ.Unfollow(user, username);
        }
        if (latest != null)
        {
            LatestState.state = (int)latest;
        }

        return NoContent();
    }

    public bool Authorization()
    {
        string? authHeader = Request.Headers["Authorization"];
        if (authHeader == null || !authHeader.StartsWith("Basic"))
        {
            return false;
        }

        string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
        if (encodedUsernamePassword == "c2ltdWxhdG9yOnN1cGVyX3NhZmUh")
        {
            return true;
        }else{
            return false;}
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
        public string? Follow { get; set; }
        public string? Unfollow { get; set; }
    }
}

[ApiController]
[Route("latest")]
public class latestController : ControllerBase
{
    [HttpGet("")]
    public IActionResult Get()
    {
        try
        {
            return Ok(new { latest = LatestState.state });
        }
        catch
        {
            return StatusCode(500, new { status = 0, error_msg = "Unexpected Error" });
        }
    }
}

[ApiController]
[Route("msgs")]
public class messageController : ControllerBase
{
    CheepService _CheepServ;
    public messageController(CheepService CheepServ)
    {
        _CheepServ = CheepServ;
    }

    [HttpGet("")]
    public IActionResult Get([FromQuery(Name = "no")] int? num, [FromQuery(Name = "latest")] int? latest)
    {
        if (!Authorization())
        { return StatusCode(403, new { status = 0, error_msg = "Not authorized" }); }

        List<CheepDto> list = new List<CheepDto>();
        if (num != null)
        {
            list = _CheepServ.GetCheeps(1, (int)num);
        }
        else
        {
            list = _CheepServ.GetCheeps(1, 100);
        }

        List<messageInfo> messageInfos = new List<messageInfo>();
        foreach (CheepDto Dto in list)
        {
            messageInfos.Add(new messageInfo
            {
                content = Dto.Text,
                pub_date = Dto.TimeStamp,
                user = Dto.Author
            });
        }

        if (latest != null)
        { LatestState.state = (int)latest; }

        return Ok(messageInfos);
    }

    [HttpGet("{username}")]
    public IActionResult GetWithUsername()
    {
        return Ok(null);
    }

    [HttpPost("{username}")]
    public IActionResult Post()
    {
        return Ok(null);
    }

    public class messageInfo
    {
        public string? content { get; set; }
        public string? pub_date { get; set; }
        public string? user { get; set; }
    }

    public bool Authorization()
    {
        string? authHeader = Request.Headers["Authorization"];
        if (authHeader == null || !authHeader.StartsWith("Basic"))
        {
            return false;
        }

        string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
        if (encodedUsernamePassword == "c2ltdWxhdG9yOnN1cGVyX3NhZmUh")
        {
            return true;
        }else{
            return false;}
    }
}

[ApiController]
[Route("register")]
public class registerController : ControllerBase
{

}
