﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpCompress.Common.Rar;
using SharpCompress.Common.Rar.Headers;
using SharpCompress.IO;
using SharpCompress.Readers;

namespace SharpCompress.Archives.Rar
{
    /// <summary>
    /// A rar part based on a FileInfo object
    /// </summary>
    public class FileInfoRarArchiveVolume : RarVolume
    {
        internal FileInfoRarArchiveVolume(FileInfo fileInfo, ReaderOptions options)
            : base(StreamingMode.Seekable, new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1024), FixOptions(options))
        {
            FileInfo = fileInfo;
            FileParts = GetVolumeFileParts().ToArray().ToReadOnly();
        }

        private static ReaderOptions FixOptions(ReaderOptions options)
        {
            //make sure we're closing streams with fileinfo
            options.LeaveStreamOpen = false;
            return options;
        }

        internal ReadOnlyCollection<RarFilePart> FileParts { get; }

        internal FileInfo FileInfo { get; }

        internal override RarFilePart CreateFilePart(MarkHeader markHeader, FileHeader fileHeader)
        {
            return new FileInfoRarFilePart(this, ReaderOptions.Password, markHeader, fileHeader, FileInfo);
        }

        internal override IEnumerable<RarFilePart> ReadFileParts()
        {
            return FileParts;
        }
    }
}
