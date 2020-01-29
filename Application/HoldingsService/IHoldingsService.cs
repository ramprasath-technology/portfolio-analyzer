using System;
using System.Collections.Generic;
using System.Text;
using Domain;

namespace Application.HoldingsService
{
    public interface IHoldingsService
    {
        Holdings GetHoldingsForUser(ulong userId);
    }
}
