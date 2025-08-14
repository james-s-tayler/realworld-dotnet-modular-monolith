using System.Data.Common;
using System.Threading.Tasks;
using App.Core.DataAccess;
using App.Feed.Domain.Entities;
using App.Feed.Domain.Setup.Module;
using Dapper;
using JetBrains.Annotations;

namespace App.Feed.Domain.Infrastructure.Repositories
{
    internal class FollowRepository : IFollowRepository
    {
        private readonly DbConnection _connection;

        public FollowRepository([NotNull] ModuleDbConnectionWrapper<FeedModule> connectionWrapper)
        {
            _connection = connectionWrapper.Connection;
        }

        public Task<bool> IsFollowing(int userId, int followingUserId)
        {
            var sql = "SELECT EXISTS (SELECT 1 FROM follows WHERE user_id=@user_id AND following_user_id=@following_user_id)";
            var arguments = new
            {
                user_id = userId,
                following_user_id = followingUserId
            };
            var isFollowing = _connection.ExecuteScalar<bool>(sql, arguments);
            return Task.FromResult(isFollowing);
        }

        public Task<FollowEntity> Follow(FollowEntity follow)
        {
            var sql = "INSERT INTO follows (user_id, following_user_id) VALUES (@user_id, @following_user_id) RETURNING *";

            var arguments = new
            {
                user_id = follow.UserId,
                following_user_id = follow.FollowingUserId
            };

            var insertedFollower = _connection.QuerySingle<FollowEntity>(sql, arguments);

            return Task.FromResult(insertedFollower);
        }

        public Task Unfollow(FollowEntity follow)
        {
            var sql = "DELETE FROM follows WHERE user_id=@user_id AND following_user_id=@following_user_id";

            var arguments = new
            {
                user_id = follow.UserId,
                following_user_id = follow.FollowingUserId
            };

            _connection.Execute(sql, arguments);
            return Task.CompletedTask;
        }
    }
}