using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wojilu.Test {





    public class SpeedUtil {

        private const string stopwatchstring = "stopwatch";

        private static Stopwatch stopwatch = new Stopwatch();

        public static void Start() {
            stopwatch.Start();
            Console.WriteLine( "------------程序开始于{0}------------", DateTime.Now );
        }

        public static void Stop() {
            stopwatch.Stop();
            Console.WriteLine( "------------程序结束于于{0}------------", DateTime.Now );
            Console.WriteLine( "------------共耗时：{0} 毫秒------------", stopwatch.ElapsedMilliseconds );
        }

    }
}
