﻿using System;
using Castle.DynamicProxy;
using Moq;
using NoWoL.TestingUtilities.ExpectedExceptions;
using NoWoL.TestingUtilities.ObjectCreators;
using Xunit;

namespace NoWoL.TestingUtilities.Tests.ObjectCreators
{
    public class ProxyInterfaceCreatorTests
    {
        private readonly ProxyInterfaceCreator _sut = new();

        [Theory]
        [Trait("Category",
               "Unit")]
        [InlineData(typeof(ComplexTestClass))]
        [InlineData(typeof(int))]
        [InlineData(typeof(double))]
        public void UnhandledTypes(Type type)
        {
            Assert.False(_sut.CanHandle(type));
        }

        [Theory]
        [Trait("Category",
               "Unit")]
        [InlineData(typeof(ComplexTestClass))]
        [InlineData(typeof(int))]
        [InlineData(typeof(double))]
        public void CreateThrowsExceptionUnhandledTypes(Type type)
        {
            var ex = Assert.Throws<UnsupportedTypeException>(() => _sut.Create(type,
                                                                               null));
#pragma warning disable CA1062 // Validate arguments of public methods
            Assert.Equal("Expecting an interface however received " + type.FullName,
                         ex.Message);
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        private class TestClass
        {
            private readonly ISomeInterface _expected;

            public TestClass(ISomeInterface expected)
            {
                _expected = expected;
            }

            public string MethodToValidate(ISomeInterface paramO)
            {
                if (paramO == _expected)
                {
                    throw new ArgumentNullException(nameof(paramO));
                }

                return null;
            }
        }

        [Fact]
        [Trait("Category",
               "Unit")]
        public void CanHandleInterfaces()
        {
            Assert.True(_sut.CanHandle(typeof(ISomeInterface)));
        }

        [Fact]
        [Trait("Category",
               "Unit")]
        public void CanHandleThrowIfInputParametersAreInvalid()
        {
            var validator = ParametersValidatorHelper.GetMethodParametersValidator(new ProxyInterfaceCreator(),
                                                                                   nameof(ProxyInterfaceCreator.CanHandle),
                                                                                   new object[] { null });

            validator.SetupParameter("type",
                                     ExpectedExceptionRules.NotNull)
                     .Validate();
        }

        [Fact]
        [Trait("Category",
               "Unit")]
        public void CreateCastleProxyOfInterface()
        {
            var result = _sut.Create(typeof(ISomeInterface),
                                     null) as ISomeInterface;

            Assert.True(ProxyUtil.IsProxy(result));
        }

        [Fact]
        [Trait("Category",
               "Unit")]
        public void CreateThrowIfInputParametersAreInvalid()
        {
            var validator = ParametersValidatorHelper.GetMethodParametersValidator(new ProxyInterfaceCreator(),
                                                                                   nameof(ProxyInterfaceCreator.Create),
                                                                                   new object[] { typeof(ISomeInterface), null });

            validator.SetupParameter("type",
                                     ExpectedExceptionRules.NotNull)
                     .SetupParameter("objectCreators",
                                     ExpectedExceptionRules.None)
                     .Validate();
        }

        [Fact]
        [Trait("Category",
               "Unit")]
        public void ValidateWithException()
        {
            var obj = new TestClass(null);

            var validator = ParametersValidatorHelper.GetMethodParametersValidator(obj,
                                                                                   nameof(TestClass.MethodToValidate));
            validator.SetupParameter("paramO",
                                     ExpectedExceptionRules.NotNull)
                     .Validate();
        }

        [Fact]
        [Trait("Category",
               "Unit")]
        public void ValidateWithoutException()
        {
            var obj = new TestClass(new Mock<ISomeInterface>().Object);

            var validator = ParametersValidatorHelper.GetMethodParametersValidator(obj,
                                                                                   nameof(TestClass.MethodToValidate));
            validator.SetupParameter("paramO",
                                     ExpectedExceptionRules.None)
                     .Validate();
        }
    }
}