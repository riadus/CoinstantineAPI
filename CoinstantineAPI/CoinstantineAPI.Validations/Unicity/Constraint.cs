using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Users.Unicity
{
    public class Constraint<T>
    {
        public Func<ApiUser, object> ContraintToCheck { get; set; }
        public Expression<Func<ApiUser, T>> Navigation { get; set; }
        public UniqueKey UniqueKey { get; set; }
    }

    public class ContextConstraints<T>
    {
        public Func<T, object> ContraintToCheck { get; set; }
        public Expression<Func<IContext, T>> Navigation { get; set; }
        public UniqueKey UniqueKey { get; set; }
    }
}
