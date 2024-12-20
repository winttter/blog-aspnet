﻿namespace ASP.NET.ModelsDTO.Community
{
    public class CommunityDto
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsClosed { get; set; }
        public int SubscribersCount { get; set; }
    }
}
