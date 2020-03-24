using APECA_Shared_Library;

namespace APECA_Client.Scripts
{
    //Model for saving user configurations
    public class SettingsConfig
    {
        public string userName { get; set; }
        public string serverIP { get; set; }
        public byte[] key { get; set; }

        public string getKeyString()
        {
            return SharedEncoding.decodeString(key);
        }
        public bool setKeyWithString(string input)
        {
            byte[] bytes = SharedEncoding.encodeString(input);
            if (bytes.Length == 16)
            {
                this.key = bytes;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
