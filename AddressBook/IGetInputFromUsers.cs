namespace AddressBook
{
    public interface IGetInputFromUsers
    {
        int GetNumber();
        string GetNonEmptyString();
        void WaitForEnterKey();
    }
}