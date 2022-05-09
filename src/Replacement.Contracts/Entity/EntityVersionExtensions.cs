namespace Replacement.Contracts.Entity;

public static class EntityVersionExtensions {
#warning TODO: TEST
    public static bool EntityVersionDoesMatch(this long currentEntityVersion, long newEntityVersion) {
        if (0 == newEntityVersion) {
            return true;
        }
        //if (currentSerialVersion == newSerialVersion) {
        //    return true;
        //}
        return currentEntityVersion == newEntityVersion;
    }
}

