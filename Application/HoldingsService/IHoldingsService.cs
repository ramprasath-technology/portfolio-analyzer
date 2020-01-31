using System;
using System.Collections.Generic;
using System.Text;
using Domain;

namespace Application.HoldingsService
{
    public interface IHoldingsService
    {
        List<Holdings> GetHoldingsForUser(ulong userId);
    }
}
