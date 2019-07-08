using AsyncHttpJob.Extends;
using AsyncHttpJob.Models;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace AsyncHttpJob.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HttpJobsController : ControllerBase
    {
        /// <summary>
        /// 添加一个任务到队列并立即执行
        /// </summary>
        /// <param name="jobDescriptor"></param>
        /// <returns></returns>
        [HttpPost("AddEnqueue")]
        public JsonResult Enqueue(HttpJobDescriptor jobDescriptor)
        {
            try
            {
                var jobId = string.Empty;
                jobId = BackgroundJob.Enqueue(() => HttpJobExecutor.DoRequest(jobDescriptor));
                return new JsonResult(new { Flag = true, Message = $"Job:#{jobId}-{jobDescriptor.JobName}已加入队列" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Flag = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// 添加一个延迟任务到队列
        /// </summary>
        /// <param name="jobDescriptor"></param>
        /// <returns></returns>
        [HttpPost("AddSchedule")]
        public JsonResult Schedule([FromBody]HttpJobDescriptor jobDescriptor)
        {
            try
            {
                var jobId = string.Empty;
                jobId = BackgroundJob.Schedule(() => HttpJobExecutor.DoRequest(jobDescriptor), TimeSpan.FromMinutes((double)jobDescriptor.DelayInMinute));
                return new JsonResult(new { Flag = true, Message = $"Job:#{jobId}-{jobDescriptor.JobName}已加入队列" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Flag = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// 添加一个定时任务
        /// </summary>
        /// <param name="jobDestriptor"></param>
        /// <returns></returns>
        [HttpPost("AddRecurring")]
        public JsonResult Recurring([FromBody]HttpJobDescriptor jobDescriptor)
        {
            try
            {
                var jobId = string.Empty;
                RecurringJob.AddOrUpdate(jobDescriptor.JobName, () => HttpJobExecutor.DoRequest(jobDescriptor), jobDescriptor.Corn, TimeZoneInfo.Local);
                return new JsonResult(new { Flag = true, Message = $"Job:{jobDescriptor.JobName}已加入队列" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Flag = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// 删除一个定时任务
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        [HttpDelete("DeleteRecurring")]
        public JsonResult Delete(string jobName)
        {
            try
            {
                RecurringJob.RemoveIfExists(jobName);
                return new JsonResult(new { Flag = true, Message = $"Job:{jobName}已删除" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Flag = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// 触发一个定时任务
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        [HttpGet("TriggerRecurring")]
        public JsonResult Trigger(string jobName)
        {
            try
            {
                RecurringJob.Trigger(jobName);
                return new JsonResult(new { Flag = true, Message = $"Job:{jobName}已触发执行" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Flag = false, Message = ex.Message });
            }

        }

        /// <summary>
        /// 健康检查
        /// </summary>
        /// <returns></returns>
        [HttpGet("HealthCheck")]
        public IActionResult HealthCheck()
        {
            var serviceUrl = Request.Host;
            return new JsonResult(new { Flag = true, Message = "All is Well!", ServiceUrl = serviceUrl, CurrentTime = DateTime.Now });
        }
    }
}