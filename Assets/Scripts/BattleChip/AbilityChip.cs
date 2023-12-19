using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class AbilityChip
{
    public AbilityChipList index;
    public bool isActivated;

    public AbilityChip(AbilityChipList index = AbilityChipList.None)
    {
        this.index = index;
        isActivated = false;
    }
}