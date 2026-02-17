using System.Text;
using ITUMiniTwit.Core;
using ITUMiniTwit.Core.Models;
using ITUMiniTwit.Infrastructure.ITUMiniTwit.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


public static class LatestState
{
    public static int? state = null;
}

[ApiController]
[Route("fllws")]
public class fllwsController : ControllerBase
{
    private readonly IAuthorService _AuthorServ;
    public fllwsController(IAuthorService AuthorServ)
    {
        _AuthorServ = AuthorServ;
    }


    [HttpGet("{username}")]
    public async Task<IActionResult> Get(string username, [FromQuery(Name = "no")] int? num, [FromQuery] int? latest)
    {
        try { _AuthorServ.GetAuthorByName(username); } catch { return NotFound(); }
        if (!Authorization())
        {
            return StatusCode(403, new { status = 0, error_msg = "Not authorized" });
        }

        List<string> follows = await getFollowingUsernames(username, num);
        if (latest != null)
        {
            LatestState.state = (int)latest;
        }
        return Ok(new { follows });

    }

    [HttpPost("{username}")]
    public async Task<IActionResult> Post(string username, [FromBody] FollowAction body, [FromQuery] int? latest)
    {
        try { _AuthorServ.GetAuthorByName(username); } catch { return NotFound(); }
        if (!Authorization())
        {
            return StatusCode(403, new { status = 0, error_msg = "Not authorized" });
        }
    
        if (body.Follow != null)
        {
            await _AuthorServ.Follow(username, body.Follow);
        }
        if (body.Unfollow != null)
        {
            await _AuthorServ.Unfollow(username, body.Unfollow);
        }
        if (latest != null)
        {
            LatestState.state = (int)latest;
        }

        return NoContent();
    }

    private bool Authorization()
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
        }
        else
        {
            return false;
        }
    }

    private string? GetUserThroughAuth()
    {
        string? authHeader = Request.Headers["Authorization"];
        if (authHeader == null || !authHeader.StartsWith("Basic"))
        {
            return null;
        }

        string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
        Encoding encoding = Encoding.GetEncoding("UTF8");
        string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
        string username = usernamePassword.Split(":")[0];
        return username;
    }

    private async Task<List<string>> getFollowingUsernames(string user, int? num)
    {
        List<Author> list = await _AuthorServ.GetFollowing(user);
        List<string> follows = new List<string>();
        if (num != null)
        {
            for (int i = 0; i < Math.Min((int)num, list.Count); i++)
            {
                follows.Add(list[i].UserName ?? "");
            }
        }
        else
        {
            foreach (Author x in list)
            {
                follows.Add(x.UserName ?? "");
            }
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
    ICheepService _CheepServ;
    IAuthorService _AuthorServ;
    public messageController(ICheepService CheepServ, IAuthorService AuthorServ)
    {
        _CheepServ = CheepServ;
        _AuthorServ = AuthorServ;
    }

    [HttpGet("")]
    public IActionResult Get([FromQuery(Name = "no")] int? num, [FromQuery(Name = "latest")] int? latest)
    {
        if (!Authorization())
        { return StatusCode(403, new { status = 0, error_msg = "Not authorized" }); }

        List<CheepDto> list;
        List<messageInfo> messageInfos = new List<messageInfo>();
        
        try{
        if (num != null){ 
            list = _CheepServ.GetCheeps(1, (int)num); }
        else {
            list = _CheepServ.GetCheeps(1, 100);}

        }
        catch(Exception e)
        {
            return StatusCode(500, e.Message);
        }

       
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
    public IActionResult GetWithUsername(string username, [FromQuery(Name = "no")] int? num, [FromQuery(Name = "latest")] int? latest)
    {
        if (!Authorization()){ 
            return StatusCode(403, new { status = 0, error_msg = "Not authorized" });}
        try { _AuthorServ.GetAuthorByName(username); } catch { return NotFound(); }

        List<CheepDto> list;
        List<messageInfo> messageInfos = new List<messageInfo>();

        if(num != null) {
            list = _CheepServ.GetCheepsFromAuthor(username, 1, (int) num);}
        else{
           list = _CheepServ.GetCheepsFromAuthor(username, 1, 100);}


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

    [HttpPost("{username}")]
    public IActionResult Post(string username, [FromBody] messageContent message, [FromQuery(Name ="latest")] int? latest)
    {
        if (!Authorization())
        { return StatusCode(403, new { status = 0, error_msg = "Not authorized" }); }
        _CheepServ.AddCheep(username, message.content);
       
        if(latest != null){ LatestState.state = latest; }

        return NoContent();
    }

    public class messageInfo
    {
        public string? content { get; set; }
        public string? pub_date { get; set; }
        public string? user { get; set; }
    }

    public class messageContent
    {
        public required string content {get;set;}
    }

    public bool Authorization()
    {
        string? authHeader = Request.Headers["Authorization"];
        if (authHeader == null || !authHeader.StartsWith("Basic"))
        {
            return false;
        }

        string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
        if (encodedUsernamePassword == "c2ltdWxhdG9yOnN1cGVyX3NhZmUh"){
            return true;
        } else{
            return false;
        }
    }
}




[ApiController]
[Route("register")]
public class registerController : ControllerBase
{
    UserManager<Author> _userManager;
    public registerController(UserManager<Author> userManager)
    {
        _userManager = userManager;
    }
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] RegisterInfo registerInfo, [FromQuery(Name ="latest")] int? latest)
    {
        Author newUser = new Author
        {
            UserName = registerInfo.username,
            Email = registerInfo.email
        };
        
        var result = await _userManager.CreateAsync(newUser, registerInfo.pwd);
        if (!result.Succeeded){
            return StatusCode(400, new {status = 0, error_msg = "Bad Request. Either email/username is already taken or, following are missing: username, email or password."});
        }

        if(latest != null){
            LatestState.state = latest;
        }
        return NoContent();
    }


    public class RegisterInfo()
    {
        public required string username {get; set;}
        public required string email {get; set;}
        public required string pwd {get; set;}
    }
}
