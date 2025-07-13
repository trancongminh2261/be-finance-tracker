﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MonaDotNetTemplate.Services.Interfaces.BackgroundServices;

namespace NhapHangV2.Service.Services.BackgroundServices
{
    public class RuntimeBackgroundService : BackgroundService
    {
        public IBackgroundTaskQueue TaskQueue { get; }
        public static IServiceProvider ServiceProvider { get; set; }
        public static void SetService(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public RuntimeBackgroundService(IBackgroundTaskQueue taskQueue, IServiceProvider serviceProvider)
        {
            TaskQueue = taskQueue;
            SetService(serviceProvider);
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await TaskQueue.DequeueAsync(stoppingToken);
                try
                {
                    await workItem(stoppingToken);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[ex count_stack]" + ex.Message);
                }
            }
        }
    }
}
