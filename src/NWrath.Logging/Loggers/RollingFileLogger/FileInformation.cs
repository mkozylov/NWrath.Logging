using System;
using System.IO;

namespace NWrath.Logging
{
    public class FileInformation
    {
        public virtual DateTime CreationTime => _fileInfo.CreationTime;

        public virtual string FullName => _fileInfo.FullName;

        public virtual bool Exists { get { _fileInfo.Refresh(); return _fileInfo.Exists; } }

        private FileInfo _fileInfo;

        public FileInformation(string path)
        {
            _fileInfo = new FileInfo(path);
        }

        public virtual void Delete()
        {
            if (Exists)
            {
                _fileInfo.Delete();
            }
        }

        public static implicit operator FileInformation(FileInfo info)
        {
            if (info == null)
            {
                return null;
            }

            return new FileInformation(info.FullName);
        }
    }
}