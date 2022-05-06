namespace Brimborium.RowVersion.Extensions;

public static class AggregationRowVersionExtension {
    public static AggregationRowVersion ToAggregationRowVersion<T>(this IEnumerable<T> that) where T : IEntityWithVersion {
        var result = new AggregationRowVersion();
        foreach (var item in that) {
            item.SerialVersion.MaxRowVersion(ref result);
        }
        return result;
    }

    public static string CalculateArguments<T>(T args) {
        var b = JsonSerializer.SerializeToUtf8Bytes(args);
        return MD5Extension.GetMD5HashFromByteArray(b);
    }

    public static string ToCacheKey(this AggregationRowVersion that) {
        return $"{that.RowVersion.ToString("x16")}:{that.CountVersion.ToString("x8")}:{that.Arguments}";
    }

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void MaxRowVersion(this long rowVersionNext, ref long rowVersion) {
        if (rowVersion < rowVersionNext) {
            rowVersion = rowVersionNext;
        }
    }

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void MaxRowVersion(this long rowVersionNext, ref AggregationRowVersion aggregationRowVersion) {
        if (aggregationRowVersion.RowVersion < rowVersionNext) {
            aggregationRowVersion.RowVersion = rowVersionNext;
        }
        aggregationRowVersion.CountVersion++;
    }
}
