using System;
using System.Collections.Generic;
using System.Linq;

namespace ZeroWidthApi
{
    /// <summary>
    /// Main class for the encoding of the plaintext recieved from the site or app
    /// </summary>
    public class Coder
    {
        /// <summary>
        ///  !!!!! Needs input!
        /// </summary>
        /// <returns></returns>        
        private byte[] ConvertToByteArray(string str, System.Text.Encoding encoding)
        {
            return encoding.GetBytes(str);
        }
        private string ToBinary(Byte[] data)
        {
            return string.Join(" ", data.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0')));
        }
        private string ToBinaryString(string str)
        {
            string binaryString = ToBinary(ConvertToByteArray(str, System.Text.Encoding.UTF8));
            return binaryString;
        }
        
        private string BinaryToString(string binaryString)
        {
            string binNoSpace = binaryString.Replace(" ", "");
            string plainResult = "";

            while (binNoSpace.Length > 0)
            {
                var first8 = binNoSpace.Substring(0, 8);
                binNoSpace = binNoSpace.Substring(8);
                var number = Convert.ToInt32(first8, 2);
                plainResult += (char)number;
            }

            return plainResult;
        }

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
