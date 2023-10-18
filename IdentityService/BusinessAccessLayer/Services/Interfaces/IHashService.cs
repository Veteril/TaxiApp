using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public interface IHashService
    {
        List<string> CreateHash(string password);

        string ComputeHash(string password, string salt);
    }
}