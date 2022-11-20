using System;
using System.Collections.Generic;
using System.Text;

namespace Atylos.ScopableServiceProvider
{
    public interface IScopableServiceProvider: IServiceProvider
    {
        IDisposable ActivateScope(Enum scope);
    }
}
