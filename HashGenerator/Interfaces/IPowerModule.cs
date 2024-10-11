namespace Secret.Redactor.Hash.Generator.Interfaces
{
    using Secret.Redactor.Hash.Generator.Enums;

    public interface IPowerModule
    {
        long GetPower(int power, HashType type);

        long GetInvPower(int power, HashType type);
    }
}
