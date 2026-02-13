using System.Text.Json.Nodes;
using ITUMiniTwit.Core.Models;
using ITUMiniTwit.Infrastructure.ITUMiniTwit.Repositories;
using ITUMiniTwit.Infrastructure.ITUMiniTwit.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;

[ApiController]
[Route("fllws")]
public class TestController: ControllerBase
{
    private readonly AuthorService _AuthServ;


    public TestController(AuthorService AuthServ)
    {
        _AuthServ = AuthServ;
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> Get(string username)
    {
        List<Author> list = await _AuthServ.GetFollowing(username);
        List<string> follows = new List<string>(); 
        foreach (Author x in list)
        {
            follows.Add(x.UserName ?? "");
        }
        return Ok(new {follows});
    }

    [HttpPost]
    public IActionResult Post(string username)
    {
        return Ok(new {message = "hello"});
    }
}