using QuizTool.Logic;
using QuizTool.Logic.Migrations;
using QuizTool.Logic.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace QuizTool.UI
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {    
        //public void CallManagerFinish()
        //{
        //    var thread = new Thread(new ThreadStart()) { IsBackground = true };
        //    thread.Start();
        //}

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            Manager.DefaultManager.WorkFinished += Current.Shutdown;

            Question currentQuestion = Manager.DefaultManager.SearchQuestionInDB();

            if (currentQuestion == null)
            {
                currentQuestion = Manager.DefaultManager.GetQuestionForToday();
            }            
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            Manager.DefaultManager.FinishWork?.Invoke();
            
        }
    }
}
