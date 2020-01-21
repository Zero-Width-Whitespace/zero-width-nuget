using System;
using System.Linq;
using System.Text;

namespace ZeroWidthApi
{
    /// <summary>
    /// Main class for the encoding and decoding of the plaintext and zero width whitespace recieved from the site or app
    /// </summary>
    public class Coder
    {
        /// <summary>
        ///  Converts a plaintext string to a byte array.
        /// </summary>
        /// <returns> A byte array derived from a plaintext string </returns>        
        private byte[] ConvertToByteArray(string str, System.Text.Encoding encoding)
        {
            return encoding.GetBytes(str);
        }

        /// <summary>
        /// The function takes a byte array and converts it to a binary string.
        /// </summary>
        /// <param name="data"></param>
        /// <returns> A string containing the binary text. </returns>
        private string ToBinary(Byte[] data)
        {
            return string.Join(" ", data.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0')));
        }

        /// <summary>
        /// The function takes a plaintext string and converts it to a binary string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns> A string containing binary </returns>
        private string ToBinaryString(string str)
        {
            string binaryString = ToBinary(ConvertToByteArray(str, System.Text.Encoding.UTF8));
            return binaryString;
        }

        /// <summary>
        /// Takes the binary string output and converts it to regular plaintext.
        /// </summary>
        /// <param name="binaryString"></param>
        /// <returns> A plaintext string containing the decoded text </returns>
        private string BinaryToString(string binaryString)
        {
            string binNoSpace = binaryString.Replace(" ", "");
            StringBuilder plainResult = new StringBuilder();

            while (binNoSpace.Length > 0)
            {
                var first8 = binNoSpace.Substring(0, 8);
                binNoSpace = binNoSpace.Substring(8);
                var number = Convert.ToInt32(first8, 2);
                plainResult.Append((char)number);
            }

            return plainResult.ToString();
        }

        /// <summary>
        /// Recieves the plaintext and converts it to binary, and is then converted to zero width whitespace characters.
        /// </summary>
        /// <param name="str"></param>
        /// <returns> A string containing zero width characters </returns>
        public string Encode(string str)
        {
            string binString = ToBinaryString(str);

            string encodeText = new StringBuilder(binString)
                .Replace('\u0030', '\u200B')
                .Replace('\u0031', '\u200C')
                .Replace('\u0020', '\u200D')
                .ToString();

            return encodeText;
        }

        /// <summary>
        /// Decodes zero width text while ignoring plaintext.
        /// </summary>
        /// <param name="zeroWidthText"></param>
        /// <returns> The decoded plaintext</returns>
        public string Decode(string zeroWidthText)
        {
            StringBuilder filtered = new StringBuilder();
            
            foreach(char c in zeroWidthText)
            {
                if (c == '\u200B' || c == '\u200C' || c == '\u200D')
                {
                    filtered.Append(c);
                }
            }

            string filterString = filtered.ToString();
            string decodeBinText = new StringBuilder(filterString)
                .Replace('\u200B', '\u0030')
                .Replace('\u200C', '\u0031')
                .Replace('\u200D', '\u0020')
                .ToString();

            return BinaryToString(decodeBinText);           
        }

    }        
}
