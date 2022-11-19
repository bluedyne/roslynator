// This code is originally from https://github.com/josefpihrt/orang. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Roslynator.Text.RegularExpressions;

internal class GroupItemCollection : ReadOnlyCollection<GroupItem>
{
    internal GroupItemCollection(IList<GroupItem> list)
        : base(list)
    {
    }
}
