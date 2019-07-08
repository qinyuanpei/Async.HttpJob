using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Common;
using Hangfire.Logging;
using Hangfire.States;
using Hangfire.Storage;

namespace AsyncHttpJob.Extends {
    public class HttpJobFilter : JobFilterAttribute, IApplyStateFilter {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger ();

        public void OnStateApplied (ApplyStateContext context, IWriteOnlyTransaction transaction) {
            if (context.NewState is FailedState) {
                var failedState = context.NewState as FailedState;
                if (failedState != null) {
                    Logger.ErrorException (
                        String.Format ("Background Job #{0} 执行失败。", context.BackgroundJob.Id),
                        failedState.Exception);
                }
            } else {
                Logger.InfoFormat (
                    "当前执行的Job为：#{0}, 状态为：{1}。",
                    context.BackgroundJob.Id,
                    context.NewState.Name
                );
            }
        }

        public void OnStateUnapplied (ApplyStateContext context, IWriteOnlyTransaction transaction) {

        }
    }
}