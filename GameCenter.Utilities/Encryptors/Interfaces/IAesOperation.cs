namespace SGTC.Utilities.Encryptors.Interfaces
{
    public interface IAesOperation
    {
        string Encrypt(string plainText);
        T Decrypt<T>(string cipherText);
        string Decrypt(string cipherText);
    }
}
