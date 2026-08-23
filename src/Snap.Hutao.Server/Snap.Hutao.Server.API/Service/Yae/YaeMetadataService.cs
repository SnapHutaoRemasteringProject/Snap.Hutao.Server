// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.API.Service.Yae;

public sealed class YaeMetadataService
{
    private static readonly string[] MetadataUrls =
    [
        "https://rin.holohat.work/schicksal/metadata",
        "https://ena-rin.holohat.work/schicksal/metadata",
        "https://cn-cd-1259389942.file.myqcloud.com/schicksal/metadata",
    ];

    private static readonly JsonSerializerOptions JsonOptions = new();

    private readonly HttpClient httpClient;

    public YaeMetadataService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    internal async Task<YaeMetadata> GetMetadataAsync(CancellationToken token)
    {
        Exception? lastException = null;
        foreach (string url in MetadataUrls)
        {
            try
            {
                byte[] data = await httpClient.GetByteArrayAsync(url, token).ConfigureAwait(false);
                return DecodeAchievementInfo(data);
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                if (token.IsCancellationRequested)
                {
                    throw;
                }

                lastException = ex;
            }
        }

        throw new InvalidOperationException("Failed to download Yae metadata from all mirrors.", lastException);
    }

    internal string BuildAchievementFieldIdJson(YaeMetadata metadata)
    {
        (MethodRvaConfig chinese, MethodRvaConfig oversea) = GetRegionalMethodRva(metadata);

        AchievementFieldId fieldId = new()
        {
            Id = (int)metadata.Id,
            Status = (int)metadata.Status,
            TotalProgress = (int)metadata.TotalProgress,
            CurrentProgress = (int)metadata.CurrentProgress,
            FinishTimestamp = (int)metadata.FinishTimestamp,
            NativeConfig = new NativeConfiguration
            {
                StoreCmdId = metadata.StoreCmdId,
                AchievementCmdId = metadata.AchievementCmdId,
                MethodRva = new MethodRvaWrapper
                {
                    Chinese = ToMethodRva(chinese),
                    Oversea = ToMethodRva(oversea),
                },
            },
        };

        return JsonSerializer.Serialize(fieldId, JsonOptions);
    }

    private static MethodRva ToMethodRva(MethodRvaConfig config)
    {
        return new MethodRva
        {
            DoCmd = config.DoCmd,
            UpdateNormalProp = config.UpdateNormalProp,
            NewString = config.NewString,
            FindGameObject = config.FindGameObject,
            EventSystemUpdate = config.EventSystemUpdate,
            SimulatePointerClick = config.SimulatePointerClick,
            ToInt32 = config.ToInt32,
            TcpStatePtr = config.TcpStatePtr,
            SharedInfoPtr = config.SharedInfoPtr,
            Decompress = config.Decompress,
        };
    }

    private static (MethodRvaConfig Chinese, MethodRvaConfig Oversea) GetRegionalMethodRva(YaeMetadata metadata)
    {
        if (metadata.MethodRva.Count < 2)
        {
            throw new InvalidDataException($"Expected at least 2 method_rva entries, got {metadata.MethodRva.Count}.");
        }

        List<uint> keys = metadata.MethodRva.Keys.OrderBy(key => key).ToList();
        return (metadata.MethodRva[keys[0]], metadata.MethodRva[keys[1]]);
    }

    private static YaeMetadata DecodeAchievementInfo(ReadOnlySpan<byte> data)
    {
        YaeMetadata metadata = new()
        {
            Version = string.Empty,
        };

        int offset = 0;
        while (offset < data.Length)
        {
            (int fieldNumber, int wireType) = ReadTag(data, ref offset);
            if (fieldNumber == 1 && wireType == 2)
            {
                metadata.Version = Encoding.UTF8.GetString(ReadLengthDelimited(data, ref offset));
            }
            else if (fieldNumber == 4 && wireType == 2)
            {
                DecodePbInfo(ReadLengthDelimited(data, ref offset), metadata);
            }
            else if (fieldNumber == 5 && wireType == 2)
            {
                DecodeNativeConfig(ReadLengthDelimited(data, ref offset), metadata);
            }
            else
            {
                SkipValue(data, ref offset, wireType);
            }
        }

        return metadata;
    }

    private static void DecodePbInfo(ReadOnlySpan<byte> data, YaeMetadata metadata)
    {
        int offset = 0;
        while (offset < data.Length)
        {
            (int fieldNumber, int wireType) = ReadTag(data, ref offset);
            if (wireType == 0)
            {
                uint value = (uint)ReadVarint(data, ref offset);
                switch (fieldNumber)
                {
                    case 1: metadata.Id = value; break;
                    case 2: metadata.Status = value; break;
                    case 3: metadata.TotalProgress = value; break;
                    case 4: metadata.CurrentProgress = value; break;
                    case 5: metadata.FinishTimestamp = value; break;
                }
            }
            else
            {
                SkipValue(data, ref offset, wireType);
            }
        }
    }

    private static void DecodeNativeConfig(ReadOnlySpan<byte> data, YaeMetadata metadata)
    {
        int offset = 0;
        while (offset < data.Length)
        {
            (int fieldNumber, int wireType) = ReadTag(data, ref offset);
            if (wireType == 0)
            {
                uint value = (uint)ReadVarint(data, ref offset);
                switch (fieldNumber)
                {
                    case 1: metadata.StoreCmdId = value; break;
                    case 2: metadata.AchievementCmdId = value; break;
                }
            }
            else if (fieldNumber == 10 && wireType == 2)
            {
                (uint key, MethodRvaConfig config) = DecodeMethodRvaMapEntry(ReadLengthDelimited(data, ref offset));
                metadata.MethodRva[key] = config;
            }
            else
            {
                SkipValue(data, ref offset, wireType);
            }
        }
    }

    private static (uint Key, MethodRvaConfig Value) DecodeMethodRvaMapEntry(ReadOnlySpan<byte> data)
    {
        uint key = 0;
        MethodRvaConfig config = new();
        int offset = 0;
        while (offset < data.Length)
        {
            (int fieldNumber, int wireType) = ReadTag(data, ref offset);
            if (fieldNumber == 1 && wireType == 0)
            {
                key = (uint)ReadVarint(data, ref offset);
            }
            else if (fieldNumber == 2 && wireType == 2)
            {
                config = DecodeMethodRvaConfig(ReadLengthDelimited(data, ref offset));
            }
            else
            {
                SkipValue(data, ref offset, wireType);
            }
        }

        return (key, config);
    }

    private static MethodRvaConfig DecodeMethodRvaConfig(ReadOnlySpan<byte> data)
    {
        MethodRvaConfig config = new();
        int offset = 0;
        while (offset < data.Length)
        {
            (int fieldNumber, int wireType) = ReadTag(data, ref offset);
            if (wireType == 0)
            {
                uint value = (uint)ReadVarint(data, ref offset);
                switch (fieldNumber)
                {
                    case 1: config.DoCmd = value; break;
                    case 3: config.UpdateNormalProp = value; break;
                    case 4: config.NewString = value; break;
                    case 5: config.FindGameObject = value; break;
                    case 6: config.EventSystemUpdate = value; break;
                    case 7: config.SimulatePointerClick = value; break;
                    case 8: config.ToInt32 = value; break;
                    case 9: config.TcpStatePtr = value; break;
                    case 10: config.SharedInfoPtr = value; break;
                    case 11: config.Decompress = value; break;
                }
            }
            else
            {
                SkipValue(data, ref offset, wireType);
            }
        }

        return config;
    }

    private static (int FieldNumber, int WireType) ReadTag(ReadOnlySpan<byte> data, ref int offset)
    {
        ulong tag = ReadVarint(data, ref offset);
        return ((int)(tag >> 3), (int)(tag & 7));
    }

    private static ulong ReadVarint(ReadOnlySpan<byte> data, ref int offset)
    {
        ulong result = 0;
        int shift = 0;
        while (true)
        {
            byte value = data[offset++];
            result |= (ulong)(value & 0x7F) << shift;
            if ((value & 0x80) == 0)
            {
                return result;
            }

            shift += 7;
        }
    }

    private static ReadOnlySpan<byte> ReadLengthDelimited(ReadOnlySpan<byte> data, ref int offset)
    {
        int length = (int)ReadVarint(data, ref offset);
        ReadOnlySpan<byte> result = data.Slice(offset, length);
        offset += length;
        return result;
    }

    private static void SkipValue(ReadOnlySpan<byte> data, ref int offset, int wireType)
    {
        switch (wireType)
        {
            case 0:
                ReadVarint(data, ref offset);
                break;
            case 1:
                offset += 8;
                break;
            case 2:
                ReadLengthDelimited(data, ref offset);
                break;
            case 5:
                offset += 4;
                break;
            default:
                throw new InvalidDataException($"Unsupported protobuf wire type: {wireType}");
        }
    }
}
