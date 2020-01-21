using System;
using System.Collections.Generic;
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
            char[] binCharArr = binString.ToCharArray();
            List<char> zeroWidthList = new List<char>();

            foreach (char ch in binCharArr)
            {
                
                switch (ch)
                {
                    case '\u0030':
                        zeroWidthList.Add('\u200B');
                        break;

                    case '\u0031':
                        zeroWidthList.Add('\u200C');
                        break;

                    case '\u0020':
                        zeroWidthList.Add('\u200D');
                        break;
                }
            }

            return string.Join("", zeroWidthList);
        }

        /// <summary>
        /// Decodes zero width text while ignoring plaintext.
        /// </summary>
        /// <param name="zeroWidthText"></param>
        /// <returns> The decoded plaintext</returns>
        public string Decode(string zeroWidthText)
        {
            List<char> filtered = new List<char>();
            List<char> result = new List<char>();

            foreach(char c in zeroWidthText)
            {
                if (c == '\u200B' || c == '\u200C' || c == '\u200D')
                {
                    filtered.Add(c);
                }
            }

            foreach(char ch in filtered)
            {

                switch (ch)
                {
                    case '\u200B':
                        result.Add('\u0030');
                        break;

                    case '\u200C':
                        result.Add('\u0031');
                        break;

                    case '\u200D':
                        result.Add('\u0020');
                        break;
                }
            }

            return BinaryToString(string.Join("", result));           
        }

    }        
}
