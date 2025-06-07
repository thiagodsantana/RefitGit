using Refit;
using RefitGit.Models;
using System.Threading.Tasks;

namespace RefitGit.Interfaces
{
    public interface IGitHubApi
    {
        [Get("/users/{username}")]
        Task<GitHubUser> GetUserAsync(string username);

        [Get("/users/{username}/repos")]
        Task<List<GitHubRepo>> GetUserReposAsync(string username);

        [Get("/repos/{owner}/{repo}")]
        Task<GitHubRepo> GetRepoAsync(string owner, string repo);

        [Get("/users/{username}/followers")]
        Task<List<GitHubUser>> GetFollowersAsync(string username);

        [Get("/users/{username}/following")]
        Task<List<GitHubUser>> GetFollowingAsync(string username);
    }

}
