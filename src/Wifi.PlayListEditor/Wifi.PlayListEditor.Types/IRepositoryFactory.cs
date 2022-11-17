using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wifi.PlayListEditor.Types
{
    public interface IRepositoryFactory
    {
        IEnumerable<IFileDescription> AvailableTypes { get; }
        IRepository Create(string itemPath);
    }
}
