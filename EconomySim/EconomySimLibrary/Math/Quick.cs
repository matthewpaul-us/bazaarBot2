using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EconomySim
{    
    public static class Quick
    {
        //TODO: improve this design
        public static Random rnd = new Random();

        public static double AvgList(List<double> list)
        {
            double avg=0;
            for (int j = 0; j < list.Count; j++)
            {
                avg += list[j];
            }
            avg /= list.Count;
            return avg;
        }

        public static double MinArr(List<double> a, int window)
        {
            double min = 99999999;//Math.POSITIVE_INFINITY;
            if (window > a.Count) window = a.Count;
            for (int i = 0; i < window-1; i++)
            {
                var f = a[a.Count-1 - i];
                if (f < min) { min = f; }
            }

            return min;
        }

        public static double MaxArr(List<double> a, int window)
        {
            double max = -9999999;///Math.NEGATIVE_INFINITY;
            if (window > a.Count) window = a.Count;
            for (int i = 0; i < window - 1; i++)
            {
                var f = a[a.Count - 1 - i];
                if (f > max) { max = f; }
            }
            return max;
        }

        /**
         * Turns a number into a string with the specified number of decimal points
         * @param	num
         * @param	decimals
         * @return
         */
        public static string NumToStr(double num, int decimals)
        {
            string s = string.Format("{0:N"+decimals.ToString()+"}", num);
            return s;
        }

        public static double PositionInRange(double value, double min, double max, bool clamp = true)
        {
            value -= min;
            max -= min;
            min = 0;
            value = (value / (max - min));
            if (clamp) {
                if (value < 0) { value = 0; }
                if (value > 1) { value = 1; }
            }
            return value;
        }


        public static List<Offer> Shuffle(List<Offer>list)
        {
            /*
            To shuffle an array a of n elements (indices 0..n-1):
            for i from n − 1 downto 1 do
                j ← random integer with 0 ≤ j ≤ i
                exchange a[j] and a[i]
             */
            for (int i = list.Count - 1; i >= 0; i--)
            {
                int ii = (list.Count - 1) - i;
                if (ii > 1)
                {
                    int j = rnd.Next(ii);
                    var temp = list[j];
                    list[j] = list[ii];
                    list[ii] = temp;
                }
            }
            return list;
        }

        //TODO: convert these to refs
        public static int SortAgentAlpha(BasicAgent a, BasicAgent b)
        {
            return string.Compare(a.ClassName,b.ClassName);
        }

        public static int SortOfferAcending(Offer a, Offer b)
        {
            if (a.unitPrice < b.unitPrice) return -1;
            if (a.unitPrice > b.unitPrice) return 1;
            return 0;
        }
    }
}
