using System;
using System.Collections.Generic;
using System.Text;

namespace Atylos.Abstraction
{
    public interface IUnitBattleSelector
    {
        AtylosUnit Select(AtylosBattle battle,AtylosUnit self);
    }
}
