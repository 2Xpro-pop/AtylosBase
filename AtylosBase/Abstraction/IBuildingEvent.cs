using System;
using System.Collections.Generic;
using System.Text;

namespace AtylosBase.Abstraction
{
    public interface IBuildingEvent
    {
        void OnBuilding(AtylosMatch atylos);
    }
}
