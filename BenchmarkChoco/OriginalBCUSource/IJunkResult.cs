﻿using System.Security.Permissions;

namespace BenchmarkChoco
{
    public interface IJunkResult
    {
        /// <summary>
        ///     Confidence that this entry is safe to remove
        /// </summary>
        ConfidenceCollection Confidence { get; }

        /// <summary>
        /// Create this item's backup inside of the supplied directory
        /// </summary>
        void Backup(string backupDirectory);

        /// <summary>
        ///     Delete this entry permanently
        /// </summary>
        void Delete();

        /// <summary>
        ///     Origin of this junk
        /// </summary>
        IJunkCreator Source { get; }

        /// <summary>
        ///     Uninstaller this entry belongs to
        /// </summary>
        ApplicationUninstallerEntry Application { get; }

        string GetDisplayName();

        /// <summary>
        ///     Preview item in an external application
        /// </summary>
        void Open();

        /// <summary>
        ///     Get extended information with overall confidence information.
        /// </summary>
        string ToLongString();
    }
}
