﻿using Alexa.NET.Request;
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
                id = "-1";
            }
            return id;
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