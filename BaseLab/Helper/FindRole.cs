﻿using BaseLab.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLab.Helper
{
    internal class FindRole
    {
        int id;
        public FindRole(int id)
        {
            this.id = id;
        }
        public bool RolePredicate(Role role)
        {
            return role.Id == id;
        }
    }
}
