using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrwgTronics.Uatu.Models;

namespace DrwgTronics.Uatu.Components
{
    public interface IBulkLoader
    {
        int Load(string directory, string filter, IFolderModel model);
    }
}
