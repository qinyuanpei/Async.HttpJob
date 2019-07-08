using Hangfire.Common;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Storage;
using Hangfire.Logging;

namespace AsyncHttpJob.Extends
{
    public class HttpJobFilter: JobFilterAttribute, IApplyStateFilter
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            var failedState = context.NewState as FailedState;
            if (failedState != null)
            {
                Logger.ErrorException(
                    String.Format("Background Job #{0} 执行失败。", context.JobId),
                    failedState.Exception);
            }
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            
        }
    }
}
