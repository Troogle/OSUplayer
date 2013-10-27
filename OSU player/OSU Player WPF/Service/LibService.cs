using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSU_Player_WPF.Service
{
    /// <summary>
    /// Service逻辑聚合器
    /// </summary>
    public class LibService
    {
        public GetInitialize GetInitialize()
        {
            return new GetInitialize();
        }
    }
}
