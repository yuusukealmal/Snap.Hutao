﻿// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using CommunityToolkit.Common;

namespace Snap.Hutao.Service.Update;

internal sealed class UpdateStatus
{
    public UpdateStatus(string version, long bytesRead, long totalBytes)
    {
        Version = version;
        VersionDescription = SH.FormatServiceUpdateStatusVersionDescription(Version);
        BytesRead = bytesRead;
        TotalBytes = totalBytes;
        ProgressDescription = bytesRead != totalBytes
            ? $"{Converters.ToFileSizeString(bytesRead)}/{Converters.ToFileSizeString(totalBytes)}"
            : string.Empty;
    }

    public string? Version { get; set; }

    public string VersionDescription { get; }

    public double BytesRead { get; set; }

    public double TotalBytes { get; set; }

    public string ProgressDescription { get; }
}