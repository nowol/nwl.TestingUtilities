﻿using System;
using System.Collections.Generic;

namespace nwl.TestingUtilities
{
    public interface IObjectCreator
    {
        bool CanHandle(Type type);

        object Create(Type type, ICollection<IObjectCreator> objectCreators);
    }
}