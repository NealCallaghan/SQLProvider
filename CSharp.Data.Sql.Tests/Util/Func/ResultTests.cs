namespace CSharp.Data.Sql.Tests.Util.Func
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using NUnit.Framework;
    using Sql.Util.Func;

    [TestFixture]
    public class ResultTests
    {
        [Test]
        public void Created_Success_Is_Success()
        {
            var result = Success<string>.Succeed("test");
            result.Should().BeOfType<Success<string>>();
        }

        [Test]
        public void Created_Success_Is_Assignable_To_Result()
        {
            var result = Success<string>.Succeed("test");
            result.Should().BeAssignableTo<Result<string>>();
        }

        [Test]
        public void Created_Failure_Is_Failure()
        {
            var testResultError = new TestResultError();
            var result = Failure<string, TestResultError>.Fail(testResultError);
            result.Should().BeOfType<Failure<string, TestResultError>>();
        }

        [Test]
        public void Created_Failure_Is_Assignable_To_Result()
        {
            var testResultError = new TestResultError();
            var result = Failure<string, TestResultError>.Fail(testResultError);
            result.Should().BeAssignableTo<Result<string>>();
        }

        [Test]
        public void Created_Success_Calls_Then_Function()
        {

            var successFunctionIsCalled = false;
            Result<int> NewResult(string s)
            {
                successFunctionIsCalled = true;
                return Success<int>.Succeed(int.Parse(s));
            }

            var _ = Success<string>.Succeed("1").Then(NewResult);

            successFunctionIsCalled.Should().BeTrue();
        }

        [Test]
        public void Created_Success_Then_Returns_Correct_Result()
        {
            static Result<int> NewResult(string s) =>
                Success<int>.Succeed(int.Parse(s));

            var result = Success<string>.Succeed("1")
                .Then(NewResult);

            result.Should().BeOfType<Success<int>>();
            result.Should().BeAssignableTo<Result<int>>();
        }

        [Test]
        public void Created_Failure_Does_Not_Call_Then_Function()
        {
            var successFunctionIsCalled = false;
            Result<int> NewResult(string s)
            {
                successFunctionIsCalled = true;
                return Success<int>.Succeed(int.Parse(s));
            }

            var _ = Failure<string, TestResultError>.Fail(new TestResultError())
                .Then(NewResult);

            successFunctionIsCalled.Should().BeFalse();
        }

        [Test]
        public void Created_Failure_Then_Returns_Correct_Result()
        {
            static Result<int> NewResult(string s) =>
                Success<int>.Succeed(int.Parse(s));

            var result = Failure<string, TestResultError>.Fail(new TestResultError())
                .Then(NewResult);

            result.Should().BeOfType<Failure<int, TestResultError>>();
            result.Should().BeAssignableTo<Result<int>>();
        }

        [Test]
        public void Created_Success_OnSuccess_Calls_Function()
        {
            var actionIsCalled = false;

            void ActionFunc(string s)
            {
                actionIsCalled = true;
            }

            Success<string>.Succeed("").OnSuccess(ActionFunc);

            actionIsCalled.Should().BeTrue();
        }

        [Test]
        public void Created_Success_OnSuccess_Calls_Async_Function()
        {
            var actionIsCalled = false;

            async Task ActionFunc(string s)
            {
                actionIsCalled = true;
                await Task.CompletedTask;
            }

            Success<string>.Succeed("").OnSuccess(ActionFunc);

            actionIsCalled.Should().BeTrue();
        }

        [Test]
        public void Created_Failure_OnSuccess_Does_Not_Call_Function()
        {
            var actionIsCalled = false;

            void ActionFunc(string s)
            {
                actionIsCalled = true;
            }

            Failure<string, TestResultError>.Fail(new TestResultError())
                .OnSuccess(ActionFunc);

            actionIsCalled.Should().BeFalse();
        }

        [Test]
        public void Created_Success_OnError_Does_Not_Call_Function()
        {
            var actionIsCalled = false;

            Success<string>.Succeed("").OnError((TestResultError _) =>
            {
                actionIsCalled = true;
            });

            actionIsCalled.Should().BeFalse();
        }

        [Test]
        public void Created_Failure_OnError_Does_Call_Function()
        {
            var actionIsCalled = false;

            Failure<string, TestResultError>.Fail(new TestResultError()).OnError((TestResultError _) =>
            {
                actionIsCalled = true;
            });

            actionIsCalled.Should().BeTrue();
        }

        [Test]
        public void Created_Failure_OnError_Calls_Correct_Function()
        {
            var actionIsCalled = false;
            var incorrectActionIsCalled = false;

            Failure<string, TestResultError>.Fail(new TestResultError())
                .OnError((DummyResultError _) =>
                {
                    incorrectActionIsCalled = true;
                })
                .OnError((TestResultError _) =>
                {
                    actionIsCalled = true;
                });

            actionIsCalled.Should().BeTrue();
            incorrectActionIsCalled.Should().BeFalse();
        }

        [Test]
        public async Task Result_Failure_Async_OnError_Calls_Correct_Function()
        {
            var actionIsCalled = false;
            var incorrectActionIsCalled = false;

            static Func<Task<Result>> TestMethod() => 
                async () =>
            {
                await Task.CompletedTask;
                return Failure<string, TestResultError>.Fail(new TestResultError());
            };


            var result = TestMethod();

            await result()
                .OnError((TestResultError _) =>
                {
                    actionIsCalled = true;
                })
                .OnError((DummyResultError _) =>
                {
                    incorrectActionIsCalled = true;
                });

            actionIsCalled.Should().BeTrue();
            incorrectActionIsCalled.Should().BeFalse();
        }

        private class DummyResultError : ResultError { }

        private class TestResultError : ResultError { }
    }
}