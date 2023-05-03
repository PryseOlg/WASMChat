﻿using Microsoft.EntityFrameworkCore;
using WASMChat.Shared.Messages;

namespace WASMChat.Server.Data.Repositories;

public class MessageRepository : RepositoryBase<Message>
{
    public MessageRepository(DbContext ctx) : base(ctx)
    { }
    

}