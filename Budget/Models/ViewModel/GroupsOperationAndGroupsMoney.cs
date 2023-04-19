using Budget.Models.Database;
using System.Collections;
using System.Collections.Generic;

namespace Budget.Models.ViewModel;

public class GroupsOperationAndGroupsMoney
{
    public IEnumerable<GroupOperation> GroupsOperation { get; set; } = null!;
    public IEnumerable<GroupMoney> GroupsMoney { get; set; } = null!;
} 
