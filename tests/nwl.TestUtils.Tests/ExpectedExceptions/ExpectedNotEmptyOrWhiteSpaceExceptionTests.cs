﻿using System;
using nwl.TestingUtilities.ExpectedExceptions;
using Xunit;

namespace nwl.TestingUtilities.Tests.ExpectedExceptions
{
    public class ExpectedNotEmptyOrWhiteSpaceExceptionTests : ExpectedExceptionBaseTests<ExpectedNotEmptyOrWhiteSpaceException>
    {
        protected override object GetInvalidParameterValueExpectedValue()
        {
            return ExpectedNotEmptyOrWhiteSpaceException.DefaultInvalidValue;
        }

        [Fact]
        [Trait("Category",
               "Unit")]
        public void ReturnsExpectedInvalidValue()
        {
            Assert.Equal(ExpectedNotEmptyOrWhiteSpaceException.DefaultInvalidValue, _sut.GetInvalidParameterValue(MethodsHolder.GetStringParameterInfo(), null));
        }

        [Fact]
        [Trait("Category",
               "Unit")]
        public void GetInvalidParameterValueThrowIfInputParametersAreInvalid()
        {
            var validator = ArgumentsValidatorHelper.GetMethodArgumentsValidator(new ExpectedNotEmptyOrWhiteSpaceException(), nameof(ExpectedNotEmptyOrWhiteSpaceException.GetInvalidParameterValue), methodArguments: new object[] { MethodsHolder.GetStringParameterInfo(), null });

            validator.SetupParameter("param", ExpectedExceptionRules.NotNull)
                     .SetupParameter("defaultValue", ExpectedExceptionRules.None)
                     .Validate();
        }

        [Fact]
        [Trait("Category",
               "Unit")]
        public void EvaluateReturnsFalseIfExceptionIsNull()
        {
            var result = _sut.Evaluate("paramName",
                                       null,
                                       out var additionalMessage);
            Assert.False(result);
            Assert.Equal(ExpectedExceptionBase.NoExceptionMessage,
                         additionalMessage);
        }

        [Fact]
        [Trait("Category",
               "Unit")]
        public void EvaluateReturnsFalseIfExceptionIsUnknownType()
        {
            var result = _sut.Evaluate("paramName",
                                       new NotSupportedException(),
                                       out var additionalMessage);
            Assert.False(result);
            Assert.Null(additionalMessage);
        }

        [Fact]
        [Trait("Category",
               "Unit")]
        public void EvaluateReturnsFalseIfParamNameIsNotTheExpectedOne()
        {
            var result = _sut.Evaluate("paramName",
                                       new ArgumentNullException("NotParamName"),
                                       out var additionalMessage);
            Assert.False(result);
            Assert.Equal("An ArgumentException for the parameter 'paramName' was expected however the exception is for parameter 'NotParamName'",
                         additionalMessage);
        }

        [Fact]
        [Trait("Category",
               "Unit")]
        public void EvaluateReturnsTrueForExpectedException()
        {
            var result = _sut.Evaluate("paramName",
                                       new ArgumentNullException("paramName"),
                                       out var additionalMessage);
            Assert.True(result);
            Assert.Null(additionalMessage);
        }
    }
}
