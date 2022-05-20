using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    public interface IMatch
    {
        string RemainingText();

        bool Success();
    }
}
