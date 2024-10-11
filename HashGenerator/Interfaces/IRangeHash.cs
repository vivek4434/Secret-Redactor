using Secret.Redactor.Hash.Generator.Enums;

namespace Secret.Redactor.Hash.Generator.Interfaces
{
    public interface IRangeHash
    {
        long GetRangeHash(int start, int end, HashType hashType);
    }
}
