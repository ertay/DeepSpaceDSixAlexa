using Alexa.NET.Request;
using DeepSpaceDSixAlexa.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeepSpaceDSixAlexa.Extensions
{
    public static class Extensions
    {

        public static string GetSlotId(this Slot slot)
        {
            string id;
            try
            {
                id = slot.Resolution.Authorities.First().Values.First().Value.Id;
            }
            catch(Exception ex)
            {
                id = string.Empty;
            }
            return id;
        }

        public static int ExtractNumber(this Slot slot)
        {
            int result = -1;
            try
            {
                result = Convert.ToInt16(slot.Value);
            }
            catch (Exception ex)
            {
                // invalid number
            }

            return result;
        }

        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
        /// <summary>
        /// Shuffle extension for IList lists.
        /// Grabbed from https://stackoverflow.com/a/1262619/3646421
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
