﻿using mp.ce.fdid.Domain.Entities;
using mp.ce.fdid.Domain.Interfaces.Repositories.Common;

namespace mp.ce.fdid.Domain.Interfaces
{
    public interface IPrazoRepository : IRepositoryBase<Prazo>
    {
        bool Prazo();
    }
}
