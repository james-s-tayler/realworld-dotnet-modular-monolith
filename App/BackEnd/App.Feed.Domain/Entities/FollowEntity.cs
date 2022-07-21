namespace App.Feed.Domain.Entities
{
    internal class FollowEntity
    {
        public int UserId { get; set; }
        public int FollowingUserId { get; set; }
    }
}