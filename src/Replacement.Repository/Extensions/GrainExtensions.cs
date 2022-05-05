namespace Replacement.Repository.Extensions;

public static class GrainExtensions {

    public static Orleans.Core.IGrainIdentity? GetGrainId(this IAddressable grain) {
        if (grain is GrainReference grainReference) {
            return grainReference.GrainIdentity;
        }
        if (grain is IGrain grain2) {
            return grain2.GetGrainIdentity();
        }
        return default;
    }
}
