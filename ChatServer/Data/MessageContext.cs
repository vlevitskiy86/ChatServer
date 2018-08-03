﻿using ChatApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApi.Data
{
    public class MessageContext : DbContext
    {
        public MessageContext(DbContextOptions<MessageContext> options) : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }
    }
}
