using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wifi.PlayListEditor.Types
{
    public interface IStorageDescription
    {
        string StorageLocation { get; }
        string Description { get; }
    }
}
