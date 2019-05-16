using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Account
    {
        public string Id
        {
            get;
            set;
        }
        public string Pw;
        public UserPermission Permission
        {
            get;
            set;
        }
        public Account(string m_id , string m_pw , UserPermission m_permission)
        {
            Id = m_id;
            Pw = m_pw;
            Permission = m_permission;
        }
    }
}
