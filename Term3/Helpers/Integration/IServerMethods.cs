using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TErm.Helpers.Integration
{
    interface IServerMethods
    {
        int addUser(string token, string userName);
    }
}
