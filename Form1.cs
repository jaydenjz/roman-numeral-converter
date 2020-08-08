using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RomanNumeralConverter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(int.TryParse(input.Text, out int textInput))
            {
                result.Text = NumberToRoman(textInput);
                return;
            }
            result.Text = RomanToNumber(input.Text);
            input.Text = input.Text.ToUpper();
        }

        // convert number to roman
        private string NumberToRoman(int number)
        {
            string roman = "";
            if(number < 0 || number > 3999)
            {
                return "Please enter a value between 0 to 3999";
            }
            foreach(var pair in GetRomanNumeralPair())
            {
                while(number >= pair.Value)
                {
                    number -= pair.Value;
                    roman += pair.Key;
                }
            }
            return roman;
        }

        // convert roman to number
        private string RomanToNumber(string roman)
        {
            int number = 0;
            int temp = 0;
            int lesserCount = 0;
            int previous = 0;

            if(roman == null)
            {
                return "Input can not be null.";
            }

            // standardize the roman input
            roman = roman.ToUpper().Trim();

            if(!IsValidRoman(roman))
            {
                return "Invalid Roman Numeral";
            }

            // loop trough the roman input by each character
            for(int i = 0; i < roman.Length; i++)
            {
                int current = GetNumberByRoman(roman[i]);

                // to check if left side lower than right
                if(previous < current)
                {
                    lesserCount++;
                }

                // each roman lesser than previous more than 2 time subsequently 
                if(lesserCount > 2)
                {
                    return "Invalid Roman Numeral";
                }

                // prevent input like IVI, CMC, CMCD
                if(temp*-1 == current)
                {
                    return "Invalid Roman Numeral";
                }

                // not end of string; current number less than next number (eg.IV);
                if(i + 1 < roman.Length && current < GetNumberByRoman(roman[i + 1]))
                {
                    // current is one of I,X,C; next number is 10x larger than current; current does not appear more than twice
                    if("IXC".IndexOf(roman[i]) == -1 || GetNumberByRoman(roman[i + 1]) > (current * 10) || roman.Split(roman[i]).Length > 3)
                    {
                        return "**Invalid Roman Numeral**";
                    }
                    number -= current;
                    temp = number;
                }
                else
                {
                    number += current;
                    lesserCount--;
                }
                previous = current;
            }

            return number.ToString();
        }

        private int GetNumberByRoman(char roman)
        {
            if(GetRomanNumeralPair().TryGetValue(roman.ToString(), out int number))
            {
                return number;
            }
            return 0;
        }

        private bool IsValidRoman(string input)
        {
            int count = 1;
            char temp = 'Z';
            foreach(char numeral in input)
            {
                // check if numeral is valid
                if(!GetRomanNumeralPair().TryGetValue(numeral.ToString(), out _))
                {
                    Console.WriteLine("Invalid roman character(s).");
                    return false;
                }

                // check if numeral duplicated (eg. IIII is not valid)
                if(numeral == temp)
                {
                    count++;
                    if(count == 4)
                    {
                        Console.WriteLine("Roman character(s) repeated more than 3 times.");
                        return false;
                    }
                }
                else
                {
                    count = 1;
                    temp = numeral;
                }
            }

            // invalid combination check
            if(input.Split('V').Length > 2 || input.Split('L').Length > 2 || input.Split('D').Length > 2)
            {
                Console.WriteLine("Invalid roman combination.");
                return false;
            }

            return true;
        }

        // a list of roman and its relative numeral
        private Dictionary<string, int> GetRomanNumeralPair()
        {
            return new Dictionary<string, int>
            {
                { "M", 1000 },
                { "CM", 900},
                { "D", 500 },
                { "CD", 400},
                { "C", 100 },
                { "XC", 90 },
                { "L", 50 },
                { "XL", 40 },
                { "X", 10 },
                { "IX", 9 },
                { "V", 5 },
                { "IV", 4 },
                { "I", 1 }
            };
        }
    }
}
