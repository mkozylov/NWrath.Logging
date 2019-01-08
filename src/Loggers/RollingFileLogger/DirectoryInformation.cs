using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NWrath.Logging
{
    public class DirectoryInformation
    {
        public virtual DateTime CreationTime => _directoryInfo.CreationTime;

        public virtual string FullName => _directoryInfo.FullName;

        public virtual bool Exists { get { _directoryInfo.Refresh(); return _directoryInfo.Exists; } }

        private DirectoryInfo _directoryInfo;

        public DirectoryInformation(string path)
        {
            _directoryInfo = new DirectoryInfo(path);
        }

        public virtual void Create()
        {
            if (!Exists)
            {
                _directoryInfo.Create();
            }
        }

        public virtual IEnumerable<FileInformation> EnumerateFiles()
        {
            return _directoryInfo.EnumerateFiles()
                                 .Select(x => new FileInformation(x.FullName));
        }

        public virtual FileInformation[] GetFiles()
        {
            return EnumerateFiles().ToArray();
        }

        public static implicit operator DirectoryInformation(DirectoryInfo info)
        {
            if (info == null)
            {
                return null;
            }

            return new DirectoryInformation(info.FullName);
        }
    }
}