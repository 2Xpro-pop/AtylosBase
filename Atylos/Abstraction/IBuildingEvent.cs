using System;
using System.Collections.Generic;
using System.Text;

namespace Atylos.Abstraction
{
    public interface IBuildingEvent
    {
        void OnBuilding(AtylosMatch atylos);
    }
}
