﻿using System;

namespace Puppet
{
    public enum ServerEnvironment
    {
        Dev,
        QA,
        Production
    }

    public interface IServerMode
    {
        string GetBaseUrl();

        string Port { get; }

        string Domain { get; }

        string GetPath(string path);
    }
}
