namespace Replacement.Contracts.API;

public static class SerialVersionExtensions {
    public static bool SerialVersionDoesMatch(this long currentSerialVersion, long newSerialVersion) {
        if (0 == newSerialVersion) {
            return true;
        }
        //if (currentSerialVersion == newSerialVersion) {
        //    return true;
        //}
        return (currentSerialVersion == newSerialVersion);
    }
}

