using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApplicationDbContext.ValidationHelpers
{
    public class EmailValidator
    {
        static public bool IsValid(string email)
        {
            Regex regex = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,14})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
            return regex.Match(email).Success;
        }
    }
}
