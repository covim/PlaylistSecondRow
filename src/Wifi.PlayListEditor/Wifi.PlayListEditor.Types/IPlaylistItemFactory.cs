﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wifi.PlayListEditor.Types
{
    public interface IPlaylistItemFactory
    {
        IEnumerable<IFileDescription> AvailableTypes { get; }
        IPlaylistItem Create(string itemPath);
    }
}