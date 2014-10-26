using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParallelMSpecRunner.Utils;
using Machine.Specifications;

namespace ParallelMSpecRunner.Tests
{
    public class When_MultiThreadedWorker_works
    {
        static MultiThreadedWorker<int, int> Worker;
        static IEnumerable<int> Result;
        static Dictionary<int, int> CallsTracker;

        Establish context = () => {
            Worker = new MultiThreadedWorker<int, int>(new[] { 1, 2, 3, 4 }, WorkerAction, threads: 3);
            CallsTracker = new Dictionary<int, int>() {
                { 1, 0 },
                { 2, 0 },
                { 3, 0 },
                { 4, 0 },
            };
        };

        private static int WorkerAction(int value)
        {
            switch (value) {
                case 1:
                    CallsTracker[1] = CallsTracker[1] + 1;
                    return 11;
                case 2:
                    CallsTracker[2] = CallsTracker[2] + 1;
                    return 22;
                case 3:
                    CallsTracker[3] = CallsTracker[3] + 1;
                    return 33;
                case 4:
                    CallsTracker[4] = CallsTracker[4] + 1;
                    return 44;
                default:
                    throw new InvalidOperationException("Should have never been reached");
            }
        }

        Because of = () => {
            Result = Worker.Run().Result;
        };
        
        It should_proccess_all_items_only_once = () => {
            CallsTracker.Values.All(called => called == 1).ShouldBeTrue();
        };

        It should_aggregate_all_results = () => {
            Result.Count().ShouldEqual(4);
            Result.ShouldContain(11);
            Result.ShouldContain(22);
            Result.ShouldContain(33);
            Result.ShouldContain(44);
        };
    }
}
