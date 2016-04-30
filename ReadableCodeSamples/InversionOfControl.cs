using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ReadableCodeSamples
{
    public interface ITrivialOperations
    {
        string ConvertToString(int i);
        void NoOp();
    }


    [TestClass]
    public class InversionOfControl
    {
        LoggingInvoker _log = GetLoggingMock();
        ITrivialOperations _sut = GetSystemUnderTest();

        [TestMethod]
        public void LogsToString()
        {
            _sut.ConvertToString(1);

            Assert.IsNotNull(_log.BeforeInvokeLog);
            Assert.IsNotNull(_log.AfterInvokeLog);
            Assert.AreEqual("Resulted in 1", _log.AfterInvokeLog);
        }

        [TestMethod]
        public void ShouldSetAfterInvokingToNullForVoidMethods()
        {
            _sut.NoOp();

            Assert.IsNotNull(_log.BeforeInvokeLog);
            Assert.IsNull(_log.AfterInvokeLog);
        }
        
        private static LoggingInvoker GetLoggingMock()
        {
            return Container.GetFromContainer<LoggingInvoker>();
        }

        private static ITrivialOperations GetSystemUnderTest()
        {
            return Container.GetFromContainer<ITrivialOperations>();
        }
    }
    
    public class TrivialClass : ITrivialOperations
    {
        [DebugLogging]
        public string ConvertToString(int i)
        {
            return i.ToString();
        }

        [DebugLogging]
        public void NoOp() { }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    sealed class DebugLoggingAttribute : HandlerAttribute
    {
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return Container.GetFromContainer<LoggingInvoker>();
        }
    }

    public class LoggingInvoker : Invoker
    {
        public string BeforeInvokeLog { get; private set; }
        public string AfterInvokeLog { get; private set; }

        protected override void Before(IMethodInvocation methodInvocation)
        {
            BeforeInvokeLog = string.Format("Calling {0} on {1}", methodInvocation.MethodBase.Name, methodInvocation.Target.GetType().Name);
        }
        protected override void After(IMethodInvocation methodInvocation, IMethodReturn response)
        {
            if(response == null || response.ReturnValue == null)
            {
                AfterInvokeLog = null;
                return;
            }
            AfterInvokeLog = string.Format("Resulted in {0}", response.ReturnValue.ToString());
        }
    }

    public abstract class Invoker : ICallHandler
    {
        public int Order { get; set; }

        protected abstract void Before(IMethodInvocation methodInvocation);
        protected abstract void After(IMethodInvocation methodInvocation, IMethodReturn response);
        
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            Before(input);
            IMethodReturn response = getNext()(input, getNext);
            After(input, response);
            return response;
        }
    }

    public static class Container
    {
        private static IUnityContainer _container;
        static Container()
        {
            _container = new UnityContainer();
            _container
                .RegisterType<LoggingInvoker>(new ContainerControlledLifetimeManager())
                .RegisterType<ITrivialOperations, TrivialClass>();

            _container
                .AddNewExtension<Interception>()
                .Configure<Interception>()
                .SetInterceptorFor(typeof(ITrivialOperations), new InterfaceInterceptor());
        }

        public static T GetFromContainer<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
